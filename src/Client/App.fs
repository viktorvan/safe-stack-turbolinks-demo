module Chickens

open Browser.Types
open Client
open Browser
open HtmlHelper.EventTargets

Turbolinks.start()

document.onpointerdown <-
    fun ev ->
        match ev.target :?> Element with
        | ChickenCard chickenId -> CompositionRoot.addEgg chickenId
        | EggIcon chickenId -> CompositionRoot.removeEgg chickenId
        | NavbarBurger -> CompositionRoot.toggleNavbarMenu()
        | _ -> ()

document.addEventListener("turbolinks:before-cache", fun _ ->
    CompositionRoot.scrollPositionService.Save())

document.addEventListener("turbolinks:load", fun _ ->
    CompositionRoot.scrollPositionService.Recall()
    CompositionRoot.initDatepicker())
