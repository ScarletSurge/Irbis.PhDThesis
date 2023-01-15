namespace PhDThesis.Domain.Helpers.Guarding;

// TODO: unstatic?!
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
    /// <param name="value"></param>
    /// <typeparam name="T"></typeparam>
    public static void ThrowIfNullOrEmpty<T>(
        IEnumerable<T>? value)
    {
        ThrowIfNull(value);
        ThrowIfEmpty(value);
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="initialValue"></param>
    /// <param name="valueToCompareWith"></param>
    /// <typeparam name="T"></typeparam>
    /// <exception cref="GuardException"></exception>
    public static void ThrowIfEqual<T>(
        T initialValue,
        T valueToCompareWith)
        where T : IEquatable<T>
    {
        if (!initialValue.Equals(valueToCompareWith))
        {
            return;
        }
        
        throw new GuardException("Initial value equals to other value by inner equality comparison.");
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="initialValue"></param>
    /// <param name="valueToCompareWith"></param>
    /// <param name="comparer"></param>
    /// <typeparam name="T"></typeparam>
    /// <exception cref="GuardException"></exception>
    public static void ThrowIfEqual<T>(
        T initialValue,
        T valueToCompareWith,
        IEqualityComparer<T> comparer)
    {
        if (!comparer.Equals(initialValue, valueToCompareWith))
        {
            return;
        }
        
        throw new GuardException("Initial value equals to other value by outer equality comparison.");
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="initialValue"></param>
    /// <param name="valueToCompareWith"></param>
    /// <typeparam name="T"></typeparam>
    /// <exception cref="GuardException"></exception>
    public static void ThrowIfNotEqual<T>(
        T initialValue,
        T valueToCompareWith)
        where T : IEquatable<T>
    {
        if (initialValue.Equals(valueToCompareWith))
        {
            return;
        }
        
        throw new GuardException("Initial value not equals to other value by inner equality comparison.");
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="initialValue"></param>
    /// <param name="valueToCompareWith"></param>
    /// <param name="comparer"></param>
    /// <typeparam name="T"></typeparam>
    /// <exception cref="GuardException"></exception>
    public static void ThrowIfNotEqual<T>(
        T initialValue,
        T valueToCompareWith,
        IEqualityComparer<T> comparer)
    {
        if (comparer.Equals(initialValue, valueToCompareWith))
        {
            return;
        }
        
        throw new GuardException("Initial value not equals to other value by outer equality comparison.");
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
        where T : IComparable<T>
    {
        if (initialValue.CompareTo(valueToCompareWith) >= 0)
        {
            return;
        }

        throw new GuardException("Initial value is LT other value by inner comparison.");
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="initialValue"></param>
    /// <param name="valueToCompareWith"></param>
    /// <param name="comparer"></param>
    /// <typeparam name="T"></typeparam>
    /// <exception cref="GuardException"></exception>
    public static void ThrowIfLowerThan<T>(
        T initialValue,
        T valueToCompareWith,
        IComparer<T> comparer)
    {
        if (comparer.Compare(initialValue, valueToCompareWith) >= 0)
        {
            return;
        }

        throw new GuardException("Initial value is LT other value by outer comparison.");
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

        throw new GuardException("Initial value is LT or EQ to other value by inner comparison.");
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="initialValue"></param>
    /// <param name="valueToCompareWith"></param>
    /// <param name="comparer"></param>
    /// <typeparam name="T"></typeparam>
    /// <exception cref="GuardException"></exception>
    public static void ThrowIfLowerThanOrEqualTo<T>(
        T initialValue,
        T valueToCompareWith,
        IComparer<T> comparer)
    {
        if (comparer.Compare(initialValue, valueToCompareWith) > 0)
        {
            return;
        }

        throw new GuardException("Initial value is LT or EQ to other value by outer comparison.");
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

        throw new GuardException("Initial value is GT other value by inner comparison.");
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="initialValue"></param>
    /// <param name="valueToCompareWith"></param>
    /// <param name="comparer"></param>
    /// <typeparam name="T"></typeparam>
    /// <exception cref="GuardException"></exception>
    public static void ThrowIfGreaterThan<T>(
        T initialValue,
        T valueToCompareWith,
        IComparer<T> comparer)
    {
        if (comparer.Compare(initialValue, valueToCompareWith) <= 0)
        {
            return;
        }

        throw new GuardException("Initial value is GT other value by outer comparison.");
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

        throw new GuardException("Initial value is GT or EQ to other value by inner comparison.");
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="initialValue"></param>
    /// <param name="valueToCompareWith"></param>
    /// <param name="comparer"></param>
    /// <typeparam name="T"></typeparam>
    /// <exception cref="GuardException"></exception>
    public static void ThrowIfGreaterThanOrEqualTo<T>(
        T initialValue,
        T valueToCompareWith,
        IComparer<T> comparer)
    {
        if (comparer.Compare(initialValue, valueToCompareWith) < 0)
        {
            return;
        }

        throw new GuardException("Initial value is GT or EQ to other value by outer comparison.");
    }
    
}