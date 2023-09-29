# How to use? (Dependency Injection Approach)

Commando offers support for three dependency injection frameworks:

- [Autofac](https://autofac.org/)
- [Ninject](http://www.ninject.org/)
- Microsoft Dependency Injection

Regardless of the one you choose, the approach is similar. Read ahead.

## With Autofac

1. Include `ConsoleTools.Commando.DependencyInjection.Autofac` nuget package.

   It will automatically include the packages:

   -  `ConsoleTools.Commando`
   - `Autofac`

2. Register `Commando` into Autofac's container.

   ```csharp
   Assembly presentationAssembly = typeof(SomeCommand).Assembly; // The assembly containing your commands.
   containerBuilder.RegisterCommando(presentationAssembly);
   ```

3. Instantiate and run the `Application`.

   ```csharp
   Application application = container.Resolve<Application>();
   await application.Run(args);
   ```

4. Create your commands.

   ```c#
   [NamedCommand("read", Description = "Display the content of a text file.")]
   internal class ReadCommand : ICommand
   {
       [NamedParameter("file", ShortName = 'f', Description = "The full path of the file.")]
       public string FilePath { get; set; }
       
   	public Task Execute()
   	{
   		...
   	}
   }
   ```

## With Ninject

1. Include `ConsoleTools.Commando.DependencyInjection.Ninject` nuget package.

   It will automatically include the packages:

   -  `ConsoleTools.Commando`
   - `Autofac`

2. Register `Commando` into Ninject's conatiner.

   ```csharp
   Assembly presentationAssembly = typeof(SomeCommand).Assembly; // The assembly containing your commands.
   kernel.RegisterCommando(presentationAssembly);
   ```

3. Instantiate the `Application`.

   ```csharp
   Application application = container.Get<Application>();
   await application.Run(args);
   ```

4. Create your commands.

   ```c#
   [NamedCommand("read", Description = "Display the content of a text file.")]
   internal class ReadCommand : ICommand
   {
       [NamedParameter("file", ShortName = 'f', Description = "The full path of the file.")]
       public string FilePath { get; set; }
       
   	public Task Execute()
   	{
   		...
   	}
   }
   ```

## With Microsoft Dependency Injection

1. Include `ConsoleTools.Commando.DependencyInjection.Microsoft` nuget package.

   It will automatically include the packages:

   -  `ConsoleTools.Commando`
   - `Autofac`

2. Register `Commando` into Microsoft's dependency container.

   ```csharp
   Assembly presentationAssembly = typeof(SomeCommand).Assembly; // The assembly containing your commands.
   kernel.AddCommando(presentationAssembly);
   ```

3. Instantiate the `Application`.

   ```csharp
   Application application = serviceProvider.GetService<Application>();
   await application.Run(args);
   ```

4. Create your commands.

   ```c#
   [NamedCommand("read", Description = "Display the content of a text file.")]
   internal class ReadCommand : ICommand
   {
       [NamedParameter("file", ShortName = 'f', Description = "The full path of the file.")]
       public string FilePath { get; set; }
       
   	public Task Execute()
   	{
   		...
   	}
   }
   ```

   



