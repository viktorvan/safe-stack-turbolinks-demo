module Client.Turbolinks

open Fable.Core.JsInterop

type private ITurbolinksLib =
    abstract start : unit -> unit
    abstract setProgressBarDelay : int -> unit
    abstract clearCache : unit -> unit
    abstract visit : string -> unit

let private turbolinks : ITurbolinksLib = importDefault "turbolinks"

let visit url = turbolinks.visit url
let reset() =
    turbolinks.clearCache()
    turbolinks.visit ""

let start() = turbolinks.start()