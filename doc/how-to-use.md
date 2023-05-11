# How to use?

## Using Autofac

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

## Using Ninject

1. Include `ConsoleTools.Commando.Ninject.DependencyInjection` nuget package.

   It will automatically include the `ConsoleTools.Commando` package.

2. Register `Commando` into `Ninject`.

   ```csharp
   Assembly presentationAssembly = typeof(SomeCommand).Assembly;
   kernel.RegisterCommando(presentationAssembly);
   ```

   The `RegisterCommando(...)` method needs as parameters the assemblies where your commands are located. It will identify the command classes using reflection.

3. Instantiate the `Application`.

   ```csharp
   Application application = container.Get<Application>();
   ```

4. Run the application.

   ```csharp
   await application.Run(args);
   ```

