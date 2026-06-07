using System.Reflection;

namespace DustInTheWind.ConsoleTools.Commando;

public static class MethodInfoExtensions
{
    public static async Task<object> InvokeAsync(this MethodInfo methodInfo, object obj, params object[] parameters)
    {
        Task task = (Task)methodInfo.Invoke(obj, parameters);

        await task.ConfigureAwait(false);

        PropertyInfo resultProperty = task.GetType().GetProperty("Result");
        return resultProperty.GetValue(task);
    }
}