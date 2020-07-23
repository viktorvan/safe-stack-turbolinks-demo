module Client.Navbar

open Browser

let toggleNavbarMenu () =
    document.getElementById("chickencheck-navbar-burger")
    |> Option.ofObj
    |> Option.iter (HtmlHelper.toggleClass "is-active")
    document.getElementById("chickencheck-navbar-menu")
    |> Option.ofObj
    |> Option.iter (HtmlHelper.toggleClass "is-active")

