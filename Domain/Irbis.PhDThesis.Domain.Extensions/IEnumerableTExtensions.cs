namespace Irbis.PhDThesis.Domain.Extensions;

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
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="firstEnumerable"></param>
    /// <param name="secondEnumerable"></param>
    /// <typeparam name="T1"></typeparam>
    /// <typeparam name="T2"></typeparam>
    /// <returns></returns>
    public static IEnumerable<(T1, T2)> CartesianProduct<T1, T2>(
        this IEnumerable<T1> firstEnumerable,
        IEnumerable<T2> secondEnumerable)
    {
        foreach (var firstItem in firstEnumerable)
        {
            foreach (var secondItem in secondEnumerable)
            {
                yield return (firstItem, secondItem);
            }
        }
    }

    public static IEnumerable<(T1, T2)> CartesianProduct<T1, T2>(
        this IEnumerable<T1> firstEnumerable,
        IEnumerable<T2> secondEnumerable,
        Func<T1, T2, bool> predicate)
    {
        return firstEnumerable
            .CartesianProduct(secondEnumerable)
            .Where(x => predicate(x.Item1, x.Item2));
    }

}