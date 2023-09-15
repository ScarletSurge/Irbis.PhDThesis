namespace PhDThesis.Domain.Helpers.Guarding;

/// <summary>
/// 
/// </summary>
public sealed class Guardant
{
    
    /// <summary>
    /// 
    /// </summary>
    private static Guardant _instance = new ();

    public static Guardant Instance =>
        _instance;
    
    /// <summary>
    /// 
    /// </summary>
    private Guardant()
    {
        
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="value"></param>
    /// <param name="predicate"></param>
    /// <param name="exceptionMessage"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    /// <exception cref="GuardantException"></exception>
    public Guardant ThrowIf<T>(
        T? value,
        Predicate<T?> predicate,
        string exceptionMessage)
    {
        if (predicate(value))
        {
            throw new GuardantException(exceptionMessage);
        }

        return this;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="value"></param>
    /// <typeparam name="T"></typeparam>
    public Guardant ThrowIfNull<T>(
        T? value)
        where T : class
    {
        return ThrowIf(value, passedValue => passedValue is null, "Value is null.");
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="value"></param>
    /// <typeparam name="T"></typeparam>
    public Guardant ThrowIfEmpty<T>(
        IEnumerable<T> value)
    {
        //return ThrowIf(value, passedValue => !passedValue.Any())
        if (value.Any())
        {
            return this;
        }
        
        throw new GuardantException("Value is empty enumerable.");
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="value"></param>
    /// <typeparam name="T"></typeparam>
    public Guardant ThrowIfNullOrEmpty<T>(
        IEnumerable<T>? value)
    {
        return ThrowIfNull(value).ThrowIfEmpty(value);
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="initialValue"></param>
    /// <param name="valueToCompareWith"></param>
    /// <typeparam name="T"></typeparam>
    /// <exception cref="GuardantException"></exception>
    public Guardant ThrowIfEqual<T>(
        T initialValue,
        T valueToCompareWith)
        where T : IEquatable<T>
    {
        if (!initialValue.Equals(valueToCompareWith))
        {
            return this;
        }
        
        throw new GuardantException("Initial value equals to other value by inner equality comparison.");
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="initialValue"></param>
    /// <param name="valueToCompareWith"></param>
    /// <param name="comparer"></param>
    /// <typeparam name="T"></typeparam>
    /// <exception cref="GuardantException"></exception>
    public Guardant ThrowIfEqual<T>(
        T initialValue,
        T valueToCompareWith,
        IEqualityComparer<T> comparer)
    {
        if (!comparer.Equals(initialValue, valueToCompareWith))
        {
            return this;
        }
        
        throw new GuardantException("Initial value equals to other value by outer equality comparison.");
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="initialValue"></param>
    /// <param name="valueToCompareWith"></param>
    /// <typeparam name="T"></typeparam>
    /// <exception cref="GuardantException"></exception>
    public Guardant ThrowIfNotEqual<T>(
        T initialValue,
        T valueToCompareWith)
        where T : IEquatable<T>
    {
        if (initialValue.Equals(valueToCompareWith))
        {
            return this;
        }
        
        throw new GuardantException("Initial value not equals to other value by inner equality comparison.");
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="initialValue"></param>
    /// <param name="valueToCompareWith"></param>
    /// <param name="comparer"></param>
    /// <typeparam name="T"></typeparam>
    /// <exception cref="GuardantException"></exception>
    public Guardant ThrowIfNotEqual<T>(
        T initialValue,
        T valueToCompareWith,
        IEqualityComparer<T> comparer)
    {
        if (comparer.Equals(initialValue, valueToCompareWith))
        {
            return this;
        }
        
        throw new GuardantException("Initial value not equals to other value by outer equality comparison.");
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="initialValue"></param>
    /// <param name="valueToCompareWith"></param>
    /// <typeparam name="T"></typeparam>
    /// <exception cref="GuardantException"></exception>
    public Guardant ThrowIfLowerThan<T>(
        T initialValue,
        T valueToCompareWith)
        where T : IComparable<T>
    {
        if (initialValue.CompareTo(valueToCompareWith) >= 0)
        {
            return this;
        }

        throw new GuardantException("Initial value is LT other value by inner comparison.");
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="initialValue"></param>
    /// <param name="valueToCompareWith"></param>
    /// <param name="comparer"></param>
    /// <typeparam name="T"></typeparam>
    /// <exception cref="GuardantException"></exception>
    public Guardant ThrowIfLowerThan<T>(
        T initialValue,
        T valueToCompareWith,
        IComparer<T> comparer)
    {
        if (comparer.Compare(initialValue, valueToCompareWith) >= 0)
        {
            return this;
        }

        throw new GuardantException("Initial value is LT other value by outer comparison.");
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="initialValue"></param>
    /// <param name="valueToCompareWith"></param>
    /// <typeparam name="T"></typeparam>
    /// <exception cref="GuardantException"></exception>
    public Guardant ThrowIfLowerThanOrEqualTo<T>(
        T initialValue,
        T valueToCompareWith)
        where T: IComparable<T>
    {
        if (initialValue.CompareTo(valueToCompareWith) > 0)
        {
            return this;
        }

        throw new GuardantException("Initial value is LT or EQ to other value by inner comparison.");
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="initialValue"></param>
    /// <param name="valueToCompareWith"></param>
    /// <param name="comparer"></param>
    /// <typeparam name="T"></typeparam>
    /// <exception cref="GuardantException"></exception>
    public Guardant ThrowIfLowerThanOrEqualTo<T>(
        T initialValue,
        T valueToCompareWith,
        IComparer<T> comparer)
    {
        if (comparer.Compare(initialValue, valueToCompareWith) > 0)
        {
            return this;
        }

        throw new GuardantException("Initial value is LT or EQ to other value by outer comparison.");
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="initialValue"></param>
    /// <param name="valueToCompareWith"></param>
    /// <typeparam name="T"></typeparam>
    /// <exception cref="GuardantException"></exception>
    public Guardant ThrowIfGreaterThan<T>(
        T initialValue,
        T valueToCompareWith)
        where T: IComparable<T>
    {
        if (initialValue.CompareTo(valueToCompareWith) <= 0)
        {
            return this;
        }

        throw new GuardantException("Initial value is GT other value by inner comparison.");
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="initialValue"></param>
    /// <param name="valueToCompareWith"></param>
    /// <param name="comparer"></param>
    /// <typeparam name="T"></typeparam>
    /// <exception cref="GuardantException"></exception>
    public Guardant ThrowIfGreaterThan<T>(
        T initialValue,
        T valueToCompareWith,
        IComparer<T> comparer)
    {
        if (comparer.Compare(initialValue, valueToCompareWith) <= 0)
        {
            return this;
        }

        throw new GuardantException("Initial value is GT other value by outer comparison.");
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="initialValue"></param>
    /// <param name="valueToCompareWith"></param>
    /// <typeparam name="T"></typeparam>
    /// <exception cref="GuardantException"></exception>
    public Guardant ThrowIfGreaterThanOrEqualTo<T>(
        T initialValue,
        T valueToCompareWith)
        where T: IComparable<T>
    {
        if (initialValue.CompareTo(valueToCompareWith) < 0)
        {
            return this;
        }

        throw new GuardantException("Initial value is GT or EQ to other value by inner comparison.");
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="initialValue"></param>
    /// <param name="valueToCompareWith"></param>
    /// <param name="comparer"></param>
    /// <typeparam name="T"></typeparam>
    /// <exception cref="GuardantException"></exception>
    public Guardant ThrowIfGreaterThanOrEqualTo<T>(
        T initialValue,
        T valueToCompareWith,
        IComparer<T> comparer)
    {
        if (comparer.Compare(initialValue, valueToCompareWith) < 0)
        {
            return this;
        }

        throw new GuardantException("Initial value is GT or EQ to other value by outer comparison.");
    }
    
}