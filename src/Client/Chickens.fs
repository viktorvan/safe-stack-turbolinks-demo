module Client.Chickens

open Client.HtmlHelper
open Shared
open Browser


let private showEggLoader (id: ChickenId) =
    let selector = sprintf ".egg-icon-loader[%s]" (DataAttributes.chickenIdStr id)
    document.querySelector selector
    |> Option.ofObj
    |> Option.iter (HtmlHelper.toggleClass "is-hidden")

let private hideEggIcon (id: ChickenId) =
    let selector = sprintf ".egg-icon[%s]" (DataAttributes.chickenIdStr id)
    document.querySelector selector
    |> Option.ofObj
    |> Option.iter (HtmlHelper.toggleClass "is-hidden")

let addEgg (api: IChickensApi) =
    fun chickenId date ->
        async {
            try
                window.event.stopPropagation()
                showEggLoader chickenId
                do! api.AddEgg(chickenId, date)
                Turbolinks.reset()
            with exn ->
                eprintf "addEgg failed: %s" exn.Message
        }
        |> Async.StartImmediate

let removeEgg (api: IChickensApi) =
    fun chickenId date ->
        async {
            try
                window.event.stopPropagation()
                hideEggIcon chickenId
                showEggLoader chickenId
                do! api.RemoveEgg(chickenId, date)
                Turbolinks.reset()
            with exn ->
                eprintf "removeEgg failed: %s" exn.Message
        }
        |> Async.StartImmediate
