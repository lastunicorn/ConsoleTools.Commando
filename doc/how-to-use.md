# How to use?

1. Setup Commando.
2. Create your commands (`ICommand` classes).

## 1) Setup Commando

### Basic

Commando is provided as a nuget package:

- `DustInTheWind.ConsoleTools.Commando`

Manually setting up Commando is moderately difficult. It involves around 10 or so classes that need to be registered into the dependency container of your choice. More details can be found in the [How to Create a Setup Component](how-to-create-setup-component.md) tutorial.

So, don't do it yourself; use one of the already existing Setup components that use Autofac or Ninject.

### With Autofac

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

### With Ninject

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

