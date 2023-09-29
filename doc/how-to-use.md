# How to use?

Commando offers two approaches for the setup process:

1. Builder Approach (starting with version 2.0.0)
2. Dependency Injection Approach (before version 2.0.0)

## 1) Builder Approach (from v2.0.0)

This setup approach is the preferred one starting with version 2.0.0.

In this approach you must do the following steps:

1. Use an `ApplicationBuilder` to create and configure an `Application` instance.
   - Needs the assembly containing your commands.
   - Uses a clean fluent syntax.
2. Run the `Application`  instance.

This approach is still using dependency injection and instantiates all the objects from the Dependency Injection Approach, but everything is done by the builder object.

For more details, please refer to this page:

- [How to Use - Builder Approach](how-to-use-builder-approach.md)

## 2) Dependency Injection Approach (before v2.0.0)

This setup approach is available for the older versions, but is also kept for the 2.0.0 version. It may become obsolete in the future.

In this approach you must do the following steps:

1. Create a dependency container. Three frameworks are supported:
   - [Autofac](https://autofac.org/)
   - [Ninject](http://www.ninject.org/)
   - Microsoft Dependency Injection

2. Register Commando into the dependency container, using the provided helper method.
   - Here you provide the assembly containing your commands.
3. Create an instance of the `Application` class.
4. Run the `Application` instance.

For more details, please refer to this page:

- [How to Use - Dependency Injection Approach](how-to-use-di-approach.md)