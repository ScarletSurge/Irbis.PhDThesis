namespace Irbis.PhDThesis.Domain.Extensions;

/// <summary>
/// 
/// </summary>
public static class IAsyncEnumerableTExtensions
{
    
    #region Methods
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="source"></param>
    /// <param name="count"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public static async IAsyncEnumerable<T> TakeAsync<T>(
        this IAsyncEnumerable<T> source,
        int count,
        CancellationToken cancellationToken = default)
    {
        var enumerator = source.GetAsyncEnumerator(cancellationToken);
        while (count > 0)
        {
            cancellationToken.ThrowIfCancellationRequested();
            
            if (!await enumerator.MoveNextAsync())
            {
                yield break;
            }
            
            yield return enumerator.Current;
            --count;
        }
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="source"></param>
    /// <param name="count"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public static async IAsyncEnumerable<T> SkipAsync<T>(
        this IAsyncEnumerable<T> source,
        int count,
        CancellationToken cancellationToken = default)
    {
        var enumerator = source.GetAsyncEnumerator(cancellationToken);
        while (true)
        {
            cancellationToken.ThrowIfCancellationRequested();
            
            if (!await enumerator.MoveNextAsync())
            {
                yield break;
            }

            if (count != 0)
            {
                --count;
            }

            if (count == 0)
            {
                yield return enumerator.Current;
            }
        }
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="source"></param>
    /// <param name="cancellationToken"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static async Task<T[]> ToArrayAsync<T>(
        this IAsyncEnumerable<T> source,
        CancellationToken cancellationToken = default)
    {
        var resultList = new List<T>();

        await foreach (var sourceItem in source.WithCancellation(cancellationToken))
        {
            resultList.Add(sourceItem);
        }

        return resultList.ToArray();
    }
    
    #endregion
    
}