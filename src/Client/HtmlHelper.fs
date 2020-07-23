module Client.HtmlHelper

open Browser
open Browser.Types
open Fable.Core.JsInterop
open Shared

let toggleClass className (e: Element) =
    let newClasses =
        if e.className.Contains(className) then
            e.className.Replace(className, "")
        else
            e.className + " " + className
    e.className <- newClasses

module DataAttributes =

    let parseChickenId (element: Element) =
        element?dataset?chickenId
        |> ChickenId.parse

    let parseCurrentDate() =
        document.querySelector(sprintf "[%s]" DataAttributes.CurrentDate)?dataset?currentDate
        |> NotFutureDate.parse

type Element with
    member this.ChickenId = DataAttributes.parseChickenId(this)

module EventTargets =

    let private tryGetEggIconElement (target: Element) =
        target.closest(".egg-icon")

    let private tryGetChickenCardElement (target: Element) =
        if (tryGetEggIconElement target |> Option.isSome) then None
        else
            target.closest(".chicken-card")

    let (|ChickenCard|_|) (target: Element) =
        target
        |> tryGetChickenCardElement
        |> Option.map (fun e -> ChickenCard e.ChickenId)

    let (|EggIcon|_|) (target: Element) =
        target
        |> tryGetEggIconElement
        |> Option.map (fun e -> EggIcon e.ChickenId)

    let (|NavbarBurger|_|) (target: Element) =
        if target.closest(".navbar-burger").IsSome then Some NavbarBurger else None

