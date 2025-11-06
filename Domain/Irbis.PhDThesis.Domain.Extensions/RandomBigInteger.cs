using System.Numerics;

namespace Irbis.PhDThesis.Domain.Extensions;

/// <summary>
/// https://gist.github.com/rharkanson/50fe61655e80488fcfec7d2ee8eff568
/// </summary>
public sealed class RandomBigInteger:
    Random
{
    
    #region Constructors
    
    /// <summary>
    /// 
    /// </summary>
    public RandomBigInteger()
    {
        
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="seed"></param>
    public RandomBigInteger(int seed):
        base(seed)
    {
        
    }
    
    #endregion
    
    #region Methods
    
    /// <summary>
    /// Generates a random positive BigInteger between 0 and 2^bitLength (non-inclusive).
    /// </summary>
    /// <param name="bitLength">The number of random bits to generate.</param>
    /// <returns>A random positive BigInteger between 0 and 2^bitLength (non-inclusive).</returns>
    public BigInteger NextBigInteger(int bitLength)
    {
        if (bitLength < 1)
        {
            return BigInteger.Zero;
        }
        
        var bytes = bitLength / 8;
        var bits = bitLength % 8;
        
        // Generates enough random bytes to cover our bits.
        var bs = new byte[bytes + 1];
        NextBytes(bs);
        
        // Mask out the unnecessary bits.
        var mask = (byte)(0xFF >> (8 - bits));
        bs[^1] &= mask;
        
        return new BigInteger(bs);
    }
    
    /// <summary>
    /// Generates a random BigInteger between start and end (non-inclusive).
    /// </summary>
    /// <param name="start">The lower bound.</param>
    /// <param name="end">The upper bound (non-inclusive).</param>
    /// <returns>A random BigInteger between start and end (non-inclusive)</returns>
    public BigInteger NextBigInteger(BigInteger start, BigInteger end)
    {
        if (start == end)
        {
            return start;
        }
        
        var res = end;
        
        // Swap start and end if given in reverse order.
        if (start > end)
        {
            end = start;
            start = res;
            res = end - start;
        }
        else
        {
            // The distance between start and end to generate a random BigIntger between 0 and (end-start) (non-inclusive).
            res -= start;
        }

        var bs = res.ToByteArray();
        // Count the number of bits necessary for res.
        var bits = 8;
        var mask = 0x7F;
        while ((bs[bs.Length - 1] & mask) == bs[bs.Length - 1])
        {
            bits--;
            mask >>= 1;
        }
        bits += 8 * bs.Length;
        
        // Generate a random BigInteger that is the first power of 2 larger than res, 
        // then scale the range down to the size of res,
        // finally add start back on to shift back to the desired range and return.
        return ((NextBigInteger(bits + 1) * res) / BigInteger.Pow(2, bits + 1)) + start;
    }
    
    #endregion
    
}