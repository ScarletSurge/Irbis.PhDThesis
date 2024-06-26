﻿namespace Irbis.PhDThesis.Domain.Helpers.Guarding;

/// <summary>
/// 
/// </summary>
public sealed class Guardant
{
    
    #region Singleton

    /// <summary>
    /// 
    /// </summary>
    public static Guardant Instance
    {
        get;
    } = new ();

    #endregion
    
    #region Constructors
    
    /// <summary>
    /// 
    /// </summary>
    private Guardant()
    {
        
    }
    
    #endregion
    
    #region Methods
    
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
    /// <param name="predicate"></param>
    /// <param name="exceptionMessage"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    /// <exception cref="GuardantException"></exception>
    public Guardant ThrowIfAny<T>(
        IEnumerable<T?> value,
        Predicate<T?> predicate,
        string exceptionMessage)
    {
        if (value.Any(new Func<T?, bool>(predicate)))
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
        if (!value.Any())
        {
            throw new GuardantException("Value is empty enumerable.");
        }
        
        return this;
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="value"></param>
    /// <typeparam name="T"></typeparam>
    public Guardant ThrowIfNullOrEmpty<T>(
        IEnumerable<T>? value)
    {
        return ThrowIfNull(value).ThrowIfEmpty(value!);
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
        if (initialValue.Equals(valueToCompareWith))
        {
            throw new GuardantException("Initial value is equal to other value by inner equality comparison.");
        }

        return this;
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
        if (comparer.Equals(initialValue, valueToCompareWith))
        {
            throw new GuardantException("Initial value is equal to other value by outer equality comparison.");
        }

        return this;
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
        if (!initialValue.Equals(valueToCompareWith))
        {
            throw new GuardantException("Initial value is not equal to other value by inner equality comparison.");
        }
        
        return this;
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
        if (!comparer.Equals(initialValue, valueToCompareWith))
        {
            throw new GuardantException("Initial value is not equal to other value by outer equality comparison.");
        }

        return this;
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
        if (initialValue.CompareTo(valueToCompareWith) < 0)
        {
            throw new GuardantException("Initial value is LT other value by inner comparison.");
        }

        return this;
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
        if (comparer.Compare(initialValue, valueToCompareWith) < 0)
        {
            throw new GuardantException("Initial value is LT other value by outer comparison.");
        }

        return this;
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
        if (initialValue.CompareTo(valueToCompareWith) <= 0)
        {
            throw new GuardantException("Initial value is LT or EQ to other value by inner comparison.");
        }

        return this;
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
        if (comparer.Compare(initialValue, valueToCompareWith) <= 0)
        {
            throw new GuardantException("Initial value is LT or EQ to other value by outer comparison.");
        }

        return this;
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
        if (initialValue.CompareTo(valueToCompareWith) > 0)
        {
            throw new GuardantException("Initial value is GT other value by inner comparison.");
        }

        return this;
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
        if (comparer.Compare(initialValue, valueToCompareWith) > 0)
        {
            throw new GuardantException("Initial value is GT other value by outer comparison.");
        }

        return this;
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
        if (initialValue.CompareTo(valueToCompareWith) >= 0)
        {
            throw new GuardantException("Initial value is GT or EQ to other value by inner comparison.");
        }
        
        return this;
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
        if (comparer.Compare(initialValue, valueToCompareWith) >= 0)
        {
            throw new GuardantException("Initial value is GT or EQ to other value by outer comparison.");
        }

        return this;
    }
    
    #endregion
    
}