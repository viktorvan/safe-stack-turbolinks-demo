namespace Client

open Browser

type ScrollPositionService() =
    let mutable scrollPosition = None
    member this.Save() =
        scrollPosition <- Some {| X = window.scrollX; Y = window.scrollY |}
    member this.Recall() =
        scrollPosition
        |> Option.iter (fun p -> window.scrollTo(p.X, p.Y))
        scrollPosition <- None

