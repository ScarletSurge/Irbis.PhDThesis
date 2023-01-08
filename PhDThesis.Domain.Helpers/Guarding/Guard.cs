namespace PhDThesis.Domain.Helpers.Guarding;

/// <summary>
/// 
/// </summary>
public static class Guard
{
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="value"></param>
    /// <typeparam name="T"></typeparam>
    public static void ThrowIfNull<T>(
        T? value)
        where T : class
    {
        if (value is not null)
        {
            return;
        }
        
        throw new GuardException("Value is null.");
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="value"></param>
    /// <typeparam name="T"></typeparam>
    public static void ThrowIfEmpty<T>(
        IEnumerable<T> value)
    {
        if (value.Any())
        {
            return;
        }
        
        throw new GuardException("Value is empty enumerable.");
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="initialValue"></param>
    /// <param name="valueToCompareWith"></param>
    /// <typeparam name="T"></typeparam>
    /// <exception cref="GuardException"></exception>
    public static void ThrowIfLowerThan<T>(
        T initialValue,
        T valueToCompareWith)
        where T: IComparable<T>
    {
        if (initialValue.CompareTo(valueToCompareWith) >= 0)
        {
            return;
        }

        throw new GuardException("Initial value is LT other value.");
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="initialValue"></param>
    /// <param name="valueToCompareWith"></param>
    /// <typeparam name="T"></typeparam>
    /// <exception cref="GuardException"></exception>
    public static void ThrowIfLowerThanOrEqualTo<T>(
        T initialValue,
        T valueToCompareWith)
        where T: IComparable<T>
    {
        if (initialValue.CompareTo(valueToCompareWith) > 0)
        {
            return;
        }

        throw new GuardException("Initial value is LT or EQ to other value.");
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="initialValue"></param>
    /// <param name="valueToCompareWith"></param>
    /// <typeparam name="T"></typeparam>
    /// <exception cref="GuardException"></exception>
    public static void ThrowIfGreaterThan<T>(
        T initialValue,
        T valueToCompareWith)
        where T: IComparable<T>
    {
        if (initialValue.CompareTo(valueToCompareWith) <= 0)
        {
            return;
        }

        throw new GuardException("Initial value is GT other value.");
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="initialValue"></param>
    /// <param name="valueToCompareWith"></param>
    /// <typeparam name="T"></typeparam>
    /// <exception cref="GuardException"></exception>
    public static void ThrowIfGreaterThanOrEqualTo<T>(
        T initialValue,
        T valueToCompareWith)
        where T: IComparable<T>
    {
        if (initialValue.CompareTo(valueToCompareWith) < 0)
        {
            return;
        }

        throw new GuardException("Initial value is GT or EQ to other value.");
    }
    
}