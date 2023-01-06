namespace PhDThesis.Domain.Extensions;

/// <summary>
/// 
/// </summary>
public static class IEnumerableTExtensions
{
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="enumerable"></param>
    /// <param name="action"></param>
    /// <typeparam name="T"></typeparam>
    public static void ForEach<T>(
        this IEnumerable<T> enumerable,
        Action<T, int> action)
    {
        var i = 0;
        foreach (var item in enumerable)
        {
            action(item, i++);
        }
    }
    
}