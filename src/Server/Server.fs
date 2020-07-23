module Server.Startup

open System
open Giraffe
open Microsoft.AspNetCore.Http
open Saturn
open Shared
open FSharp.Control.Tasks.V2.ContextInsensitive
open FsToolkit.ErrorHandling
open Microsoft.Extensions.Logging
open Fable.Remoting.Server
open Fable.Remoting.Giraffe
open Server.Views
open Server.Views.Chickens

let chickensView : HttpHandler =
    fun _next (ctx: HttpContext) ->
        task {
            let maybeDate = ctx.TryGetQueryStringValue "date"
            let date = maybeDate |> Option.bind NotFutureDate.tryParse |> Option.defaultValue (NotFutureDate.today())
            let! chickensWithEggCounts = CompositionRoot.getAllChickens date
            let model =
                chickensWithEggCounts
                |> List.map (fun c ->
                    c.Chicken.Id,
                    { Id = c.Chicken.Id
                      Name = c.Chicken.Name
                      ImageUrl = c.Chicken.ImageUrl
                      Breed = c.Chicken.Breed
                      TotalEggCount = c.TotalCount
                      EggCountOnDate = snd c.Count })
               |> Map.ofList
           return! ctx.WriteHtmlStringAsync (Chickens.layout model date |> App.layout)
        }
let setTurbolinksLocationHeader : HttpHandler =
    let isTurbolink (ctx: HttpContext) =
        ctx.Request.Headers.ContainsKey "Turbolinks-Referrer"

    fun next ctx ->
        task {
            if isTurbolink ctx then
                ctx.SetHttpHeader "Turbolinks-Location" (ctx.Request.Path + ctx.Request.QueryString)
            return! next ctx
        }

let endpointPipe = pipeline {
    plug putSecureBrowserHeaders
    plug head
    plug setTurbolinksLocationHeader
}

let defaultRoute() = sprintf "/chickens?date=%s" (NotFutureDate.today().ToString())
let browser = router {
    get "/" (redirectTo false (defaultRoute()))
    get "/chickens" chickensView
}

let apiErrorHandler (ex: Exception) (routeInfo: RouteInfo<HttpContext>) =
    // do some logging
    let logger = routeInfo.httpContext.GetService<ILogger<IChickensApi>>()
    let msg = sprintf "Error at %s on method %s" routeInfo.path routeInfo.methodName
    logger.LogError(ex, msg)
    // decide whether or not you want to propagate the error to the client
    Ignore

let api : HttpHandler =
    Remoting.createApi()
    |> Remoting.withRouteBuilder Route.builder
    |> Remoting.fromValue CompositionRoot.api
    #if DEBUG
    |> Remoting.withDiagnosticsLogger (printfn "%s")
    #endif
    |> Remoting.withErrorHandler apiErrorHandler
    |> Remoting.buildHttpHandler

let webApp =
    choose [
        api
        browser
    ]

OptionHandler.register()

let errorHandler : ErrorHandler =
    fun exn logger _next ctx ->
        let msg = sprintf "Exception for %s%s" ctx.Request.Path.Value ctx.Request.QueryString.Value
        logger.LogError(exn, msg)
        match exn with
        | :? ArgumentException as a ->
            Response.badRequest ctx a.Message
        | _ ->
            Response.internalError ctx ()


let app = application {
    error_handler errorHandler
    pipe_through endpointPipe
    url ("http://*:8085/")
    use_router webApp
    use_static "public"
    use_gzip
    logging ignore
}

run app
