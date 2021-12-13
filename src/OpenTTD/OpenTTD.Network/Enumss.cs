using System.Runtime.CompilerServices;
using System.Runtime.Serialization;

namespace OpenTTD.Network;

public static class Enumss
{
    public static T[] ToArray<T>()
        where T : Enum => Enum.GetValues(typeof(T)).Cast<T>().ToArray();
}

public static class TaskHelper
{
    public static async Task<bool> WaitUntil(Func<bool> condition, TimeSpan delayBetweenChecks, TimeSpan duration)
    {
        var startTime = DateTime.Now;
        while ((DateTime.Now - startTime) < duration)
        {
            if (condition())
                return true;

            await Task.Delay(delayBetweenChecks);
        }

        return false;
    }

    public static Task WaitMax(this Task task, [CallerMemberName]string callerName = null) => task.WaitMax(TimeSpan.FromSeconds(10), callerName);
    public static async Task WaitMax(this Task task, TimeSpan waitTime, [CallerMemberName]string callerName = null)
    {
        Task delayTask = Task.Delay(waitTime);

        await Task.WhenAny(task, delayTask);

        if(delayTask.IsCompleted)
        {
            throw new TaskWaitException($"Inside {callerName} there was task timeout. Refer to exception to find more details.");
        }
    }

    public static Task<T> WaitMax<T>(this Task<T> task, [CallerMemberName]string callerName = null) => task.WaitMax(TimeSpan.FromSeconds(10), callerName);


    public static async Task<T> WaitMax<T>(this Task<T> task, TimeSpan waitTime, [CallerMemberName]string callerName = null)
    {
        Task delayTask = Task.Delay(waitTime);

        await Task.WhenAny(task, delayTask);

        if (delayTask.IsCompleted)
        {
            throw new TaskWaitException($"Inside {callerName} there was task timeout. Refer to exception to find more details.");
        }

        return await task;
    }

}

public class TaskWaitException : Exception
{
    public TaskWaitException()
    {
    }

    public TaskWaitException(string message) : base(message)
    {
    }

    public TaskWaitException(string message, Exception innerException) : base(message, innerException)
    {
    }

    protected TaskWaitException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
}