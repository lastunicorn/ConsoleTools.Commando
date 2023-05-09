# Console Tools Commando

It is a presentation layer framework using MVVM that helps to implement a CLI (command line interface).

## How to use (with Autofac)

1. Include `ConsoleTools.Commando.Autofac.DependencyInjection` nuget package.

   It will automatically include the `ConsoleTools.Commando` package.

2. Register `Commando` into `Autofac`.

   ```csharp
   Assembly presentationAssembly = typeof(SomeCommand).Assembly;
   containerBuilder.RegisterCommando(presentationAssembly);
   ```

   The `RegisterCommando(...)` method needs as parameters the assemblies where your commands are located. 

3. Instantiate the `Application`.
   ```csharp
   Application application = container.Resolve<Application>();
   ```

4. Run the application.

   ```csharp
   await application.Run(args);
   ```

## Discussions and Suggestions

https://github.com/lastunicorn/ConsoleTools.Commando/discussions

I appreciate any opinion or suggestion:

- Did you feel the need for a specific feature?
- Did you like or dislike something?
- Do you have questions?
- etc...

I'm looking forward to hearing from you.

# Donations

If you like my work and want to support me, you can buy me a coffee:

[![ko-fi](https://www.ko-fi.com/img/githubbutton_sm.svg)](https://ko-fi.com/Y8Y62EZ8H)

