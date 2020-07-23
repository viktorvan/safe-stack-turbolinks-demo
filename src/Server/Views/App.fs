module Server.Views.App

open Server
open Feliz.ViewEngine
open Feliz.Bulma.ViewEngine
open Shared

let versionInfo =
    Bulma.navbarEnd.div
        [ prop.children [ Bulma.navbarItem.div [ prop.text "Version 0.0.1" ] ] ]

let layout content =
    Html.html [
        Html.head [
            Html.meta [
                prop.charset.utf8
            ]
            Html.title "ChickenCheck"

            // <link rel="shortcut icon" type="image/png" href="/favicon.png"/>
            Html.link [
                prop.rel "shortcut icon"
                prop.type'.image
                prop.href "/favicon.ico"
            ]
            Html.link [
                prop.rel "stylesheet"
                prop.href "https://fonts.googleapis.com/css?family=Roboto:300,400,500,700&display=swap"
            ]
            yield! Bundle.bundle
        ]
        Html.body [
            Bulma.navbar [
                prop.id "chickencheck-navbar"
                prop.custom ("data-turbolinks-permanent", "")
                color.isInfo
                prop.children [
                    Bulma.navbarBrand.div [
                        Bulma.navbarItem.a [
                            prop.href "/"
                            prop.text "ChickenCheck"
                        ]
                        Bulma.navbarBurger [
                            prop.id "chickencheck-navbar-burger"
                            navbarItem.hasDropdown
                            prop.children [ yield! List.replicate 3 (Html.span []) ]
                        ]
                    ]
                    Bulma.navbarMenu [
                        prop.id "chickencheck-navbar-menu"
                        prop.children [
                            versionInfo
                        ]
                    ]
                ]
            ]
            content
        ]
    ]
    |> Render.htmlDocument

