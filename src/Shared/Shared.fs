namespace Shared

open System

// types

type ChickenId = ChickenId of Guid
type ImageUrl = ImageUrl of string
type Chicken =
    { Id: ChickenId
      Name : string
      ImageUrl : ImageUrl option
      Breed : string }
type EggCount = EggCount of int
type NotFutureDate =
    { Year: int
      Month: int
      Day: int }
    with override this.ToString() = sprintf "%i-%02i-%02i" this.Year this.Month this.Day

type ChickenWithEggCount =
    { Chicken: Chicken
      Count: NotFutureDate * EggCount
      TotalCount: EggCount }

type IChickensApi =
    { AddEgg: ChickenId * NotFutureDate -> Async<unit>
      RemoveEgg: ChickenId * NotFutureDate -> Async<unit> }

// domain - implementation
module ChickenId =
    let create guid =
        if guid = Guid.Empty then ("ChickenId", "empty guid") ||> invalidArg |> raise
        else ChickenId guid

    let parse str = (str: string) |> Guid.Parse |> create

    let value (ChickenId guid) = guid

module ImageUrl =
    let create (str: string) =
        if String.IsNullOrWhiteSpace(str) then
            Error "Not a valid url"
        else
            ImageUrl str |> Ok
    let value (ImageUrl str) = str

module EggCount =
    let zero = EggCount 0
    let create num =
        if num < 1 then zero else EggCount num

    let increase (EggCount num) =
        EggCount (num + 1)

    let decrease (EggCount num) =
        if num < 1 then zero
        else EggCount (num - 1)

    let value (EggCount num) = num

    let toString (EggCount num) = num.ToString()

module NotFutureDate =
    let tryCreate (date: DateTime) =
        let tomorrow = DateTime.Today.AddDays(1.)
        if (date >= tomorrow) then
            Error "cannot be in the future"
        else
            Ok
                { Year = date.Year
                  Month = date.Month
                  Day = date.Day }

    let create (date: DateTime) =
        match tryCreate date with
        | Ok d -> d
        | Error err -> invalidArg "date" err

    let tryParse (dateStr: string) =
        match DateTime.TryParse dateStr with
        | true, date -> Some date
        | false,_ -> None
        |> Option.map create

    let parse dateStr =
        dateStr
        |> tryParse
        |> function
            | Some d -> d
            | None ->
                let msg = sprintf "Invalid date: %s" dateStr
                invalidArg "date" msg

    let toDateTime { Year = year; Month = month; Day = day } = DateTime(year, month, day)
    let today() = create DateTime.Today
    let tryAddDays days date =
        date
        |> toDateTime
        |> (fun dt -> dt.AddDays(float days))
        |> tryCreate

    let addDays days date =
        match tryAddDays days date with
        | Ok d -> d
        | Error err -> invalidArg "date" err

// Extensions
type ChickenId with
    member this.Value = this |> ChickenId.value
type ImageUrl with
    member this.Value = ImageUrl.value this

type EggCount with
    member this.Value = this |> EggCount.value
    member this.Increase() = this |> EggCount.increase
    member this.Decrease() = this |> EggCount.decrease

type NotFutureDate with
    member this.ToDateTime() = NotFutureDate.toDateTime this

module Route =
    let builder typeName methodName =
        sprintf "/api/%s/%s" typeName methodName

module DataAttributes =
    let [<Literal>] ChickenId = "data-chicken-id"
    let [<Literal>] CurrentDate = "data-current-date"
    let chickenIdStr (id: ChickenId) = sprintf "%s=\"%s\"" ChickenId (id.Value.ToString())

module Async =
    let retn x = async { return x }

module String =
    let notNullOrEmpty str =
        if System.String.IsNullOrEmpty str then invalidArg "" "String cannot be empty"
        else str

module List =
    let batchesOf size input =
        // Inner function that does the actual work.
        // 'input' is the remaining part of the list, 'num' is the number of elements
        // in a current batch, which is stored in 'batch'. Finally, 'acc' is a list of
        // batches (in a reverse order)
        let rec loop input num batch acc =
            match input with
            | [] ->
                // We've reached the end - add current batch to the list of all
                // batches if it is not empty and return batch (in the right order)
                if batch <> [] then (List.rev batch)::acc else acc
                |> List.rev
            | x::xs when num = size - 1 ->
                // We've reached the end of the batch - add the last element
                // and add batch to the list of batches.
                loop xs 0 [] ((List.rev (x::batch))::acc)
            | x::xs ->
                // Take one element from the input and add it to the current batch
                loop xs (num + 1) (x::batch) acc
        loop input 0 [] []

module Map =
    let keys map = map |> Map.toList |> List.map fst
    let values map = map |> Map.toList |> List.map snd

    let change key f map =
        Map.tryFind key map
        |> f
        |> function
        | Some v -> Map.add key v map
        | None -> Map.remove key map

    let tryFindWithDefault defaultValue key map =
        map
        |> Map.tryFind key
        |> Option.defaultValue defaultValue

module Option =
    let ofResult r =
        match r with
        | Ok v -> Some v
        | Error _ -> None
