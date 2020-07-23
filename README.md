## F# SAFE web stack using Turbolinks

This repository demonstrates how [Turbolinks](https://github.com/turbolinks/turbolinks) can be used with the [F# SAFE web stack](https://safe-stack.github.io/).

I have written a companion blog post that goes into more detail on the setup.

## Turbolinks

Turbolinks can be a good compromise when only a little interactivity is needed on the frontend, and you don't want to setup a full single page application. It gives the user the feel a single-page application, but most of the business logic can be kept server-side.

From the Turbolinks documentation:

>Turbolinks® makes navigating your web application faster. Get the performance benefits of a single-page application without the added complexity of a client-side JavaScript framework. Use HTML to render your views on the server side and link to pages as usual. When you follow a link, Turbolinks automatically fetches the page, swaps in its \<body>, and merges its \<head>, all without incurring the cost of a full page load.


## Install pre-requisites

You'll need to install the following pre-requisites in order to build SAFE applications

* The [.NET Core SDK](https://www.microsoft.com/net/download)
* The [NPM](https://www.npmjs.com/) package manager.
* [Node LTS](https://nodejs.org/en/download/) installed for the front end components.
* If you're running on OSX or Linux, you'll also need to install [Mono](https://www.mono-project.com/docs/getting-started/install/).

## Work with the application

Before you run the project **for the first time only** you should install its local tools with this command:

```bash
dotnet tool restore
```

To run the application in watch mode use the following command. (There is no hot module reloading, since we are not using webpack-dev-server. This is a server-side web application):

```bash
dotnet fake build -t run
```

## Documentation

You will find more documentation about the used components at the following places:

* [Saturn](https://saturnframework.org/docs/)
* [Fable](https://fable.io/docs/)
* [Elmish](https://elmish.github.io/elmish/)
* [Fable.Remoting](https://zaid-ajaj.github.io/Fable.Remoting/)
* [Turbolinks](https://github.com/turbolinks/turbolinks)

