module Client.CompositionRoot

open Shared
open Fable.Remoting.Client


let api : IChickensApi =
    Remoting.createApi()
    |> Remoting.withRouteBuilder Route.builder
    |> Remoting.buildProxy<IChickensApi>

let scrollPositionService = ScrollPositionService()
let getCurrentDate() = HtmlHelper.DataAttributes.parseCurrentDate()

// Workflows
let toggleNavbarMenu() = Navbar.toggleNavbarMenu()
let addEgg chickenId = Chickens.addEgg api chickenId (getCurrentDate())
let removeEgg chickenId = Chickens.removeEgg api chickenId (getCurrentDate())
let initDatepicker() = Datepicker.init (getCurrentDate())
