namespace Common;

public static class F
{
    public static T Run<T>(Func<T> func)
    {
        return func.Invoke();
    }
}