module Server.Bundle

open Feliz.ViewEngine

let bundle =
    [
        Html.script [ prop.src "vendors~app.js"; prop.defer true ]
        Html.script [ prop.src "app.js"; prop.defer true ]
    ]
