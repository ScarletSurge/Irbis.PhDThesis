using System.Numerics;

using PhDThesis.Domain.Helpers.Guarding;

namespace PhDThesis.Math.Domain.Fraction;

/// <summary>
/// 
/// </summary>
public readonly struct Fraction:
    IEquatable<Fraction>,
    IComparable,
    IComparable<Fraction>
{
    
    #region Constructors
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="numerator"></param>
    /// <param name="denominator"></param>
    public Fraction(
        BigInteger numerator,
        BigInteger denominator)
    {
        Guardant.Instance
            .ThrowIfEqual(denominator, BigInteger.Zero);
        
        (Numerator, Denominator) = Reduct(numerator, denominator);
    }
    
    #endregion
    
    #region Properties
    
    /// <summary>
    /// 
    /// </summary>
    public BigInteger Numerator
    {
        get;
    }
    
    /// <summary>
    /// 
    /// </summary>
    public BigInteger Denominator
    {
        get;
    }
    
    #endregion
    
    #region methods

    /// <summary>
    /// 
    /// </summary>
    private static (BigInteger, BigInteger) Reduct(
        BigInteger initialNumerator,
        BigInteger initialDenominator)
    {
        if (initialNumerator.IsZero)
        {
            return (BigInteger.Zero, BigInteger.One);
        }

        var reductedNumerator = BigInteger.Abs(initialNumerator);
        var reductedDenominator = BigInteger.Abs(initialDenominator);
        var sign = initialNumerator.Sign * initialDenominator.Sign;
        var gcd = BigInteger.GreatestCommonDivisor(reductedNumerator, reductedDenominator);
        reductedNumerator /= gcd;
        reductedDenominator /= gcd;
        if (sign == -1)
        {
            reductedNumerator = BigInteger.Negate(reductedNumerator);
        }
        
        return (reductedNumerator, reductedDenominator);
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="numerator"></param>
    /// <param name="denominator"></param>
    public void Deconstruct(
        out BigInteger numerator,
        out BigInteger denominator)
    {
        numerator = Numerator;
        denominator = Denominator;
    }
    
    #region Arithmetic
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="leftSummand"></param>
    /// <param name="rightSummand"></param>
    /// <returns></returns>
    public static Fraction Sum(
        Fraction leftSummand,
        Fraction rightSummand)
    {
        // TODO: unmoq
        return new Fraction();
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="leftSummand"></param>
    /// <param name="rightSummand"></param>
    /// <returns></returns>
    public static Fraction operator +(
        Fraction leftSummand,
        Fraction rightSummand)
    {
        return Sum(leftSummand, rightSummand);
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="minuend"></param>
    /// <param name="subtrahend"></param>
    /// <returns></returns>
    public static Fraction Subtract(
        Fraction minuend,
        Fraction subtrahend)
    {
        // TODO: unmoq
        return new Fraction();
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="minuend"></param>
    /// <param name="subtrahend"></param>
    /// <returns></returns>
    public static Fraction operator -(
        Fraction minuend,
        Fraction subtrahend)
    {
        return Subtract(minuend, subtrahend);
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="leftMultiplier"></param>
    /// <param name="rightMultiplier"></param>
    /// <returns></returns>
    public static Fraction Multiply(
        Fraction leftMultiplier,
        Fraction rightMultiplier)
    {
        // TODO: unmoq
        return new Fraction();  
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="leftMultiplier"></param>
    /// <param name="rightMultiplier"></param>
    /// <returns></returns>
    public static Fraction operator *(
        Fraction leftMultiplier,
        Fraction rightMultiplier)
    {
        return Multiply(leftMultiplier, rightMultiplier);
    }
    
    #endregion
    
    #endregion
    
    #region System.Object overrides
    
    /// <inheritdoc cref="object.ToString" />
    public override string ToString()
    {
        return $"[{Numerator}/{Denominator}]";
    }
    
    /// <inheritdoc cref="object.GetHashCode" />
    public override int GetHashCode()
    {
        return HashCode.Combine(Numerator, Denominator);
    }
    
    /// <inheritdoc cref="object.Equals(object?)" />
    public override bool Equals(
        object? obj)
    {
        if (obj is null)
        {
            return false;
        }

        if (obj is Fraction fraction)
        {
            return Equals(fraction);
        }

        return false;
    }
    
    #endregion
    
    #region System.IEquatable<PhDThesis.Math.Domain.Fraction.Fraction> implementation
    
    /// <inheritdoc cref="IEquatable{T}.Equals(T?)" />
    public bool Equals(
        Fraction fraction)
    {
        return Numerator.Equals(fraction.Numerator) &&
               Denominator.Equals(fraction.Denominator);
    }
    
    #endregion
    
    #region Equality operators implementation
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="leftOperand"></param>
    /// <param name="rightOperand"></param>
    /// <returns></returns>
    public static bool operator ==(
        Fraction leftOperand,
        Fraction rightOperand)
    {
        return leftOperand.Equals(rightOperand);
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="leftOperand"></param>
    /// <param name="rightOperand"></param>
    /// <returns></returns>
    public static bool operator !=(
        Fraction leftOperand,
        Fraction rightOperand)
    {
        return !leftOperand.Equals(rightOperand);
    }
    
    #endregion
    
    #region System.IComparable implementation
    
    /// <inheritdoc cref="IComparable.CompareTo" />
    public int CompareTo(
        object? obj)
    {
        if (obj is null)
        {
            throw new ArgumentNullException(nameof(obj));
        }

        if (obj is Fraction fraction)
        {
            return CompareTo(fraction);
        }

        throw new ArgumentException("Can't compare objects", nameof(obj));
    }
    
    #endregion
    
    #region System.IComparable<in T> implementation
    
    /// <inheritdoc cref="IComparable{T}.CompareTo" />
    public int CompareTo(
        Fraction fraction)
    {
        return (Numerator * fraction.Denominator).CompareTo(Denominator * fraction.Numerator);
    }
    
    #endregion
    
    #region Comparison operators implementation
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="leftOperand"></param>
    /// <param name="rightOperand"></param>
    /// <returns></returns>
    public static bool operator <(
        Fraction leftOperand,
        Fraction rightOperand)
    {
        return leftOperand.CompareTo(rightOperand) < 0;
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="leftOperand"></param>
    /// <param name="rightOperand"></param>
    /// <returns></returns>
    public static bool operator <=(
        Fraction leftOperand,
        Fraction rightOperand)
    {
        return leftOperand.CompareTo(rightOperand) <= 0;
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="leftOperand"></param>
    /// <param name="rightOperand"></param>
    /// <returns></returns>
    public static bool operator >(
        Fraction leftOperand,
        Fraction rightOperand)
    {
        return leftOperand.CompareTo(rightOperand) > 0;
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="leftOperand"></param>
    /// <param name="rightOperand"></param>
    /// <returns></returns>
    public static bool operator >=(
        Fraction leftOperand,
        Fraction rightOperand)
    {
        return leftOperand.CompareTo(rightOperand) >= 0;
    }
    
    #endregion

}