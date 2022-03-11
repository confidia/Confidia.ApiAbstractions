using System.Reflection;

namespace TimCodes.ApiAbstractions.Extensions;

public static class TaskExtensions
{
    public static async Task<object?> InvokeAsync(this MethodInfo method, object obj, params object[] parameters)
    {
        var task = (Task?)method.Invoke(obj, parameters);
        if (task is null) return null;
        await task.ConfigureAwait(false);
        PropertyInfo? resultProperty = task.GetType().GetProperty("Result");
        return resultProperty?.GetValue(task, null);
    }
}
