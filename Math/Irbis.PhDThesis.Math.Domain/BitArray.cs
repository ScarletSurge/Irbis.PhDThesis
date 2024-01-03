using System.Collections;

using Irbis.PhDThesis.Domain.Helpers.Guarding;

namespace Irbis.PhDThesis.Math.Domain;

/// <summary>
/// 
/// </summary>
public sealed class BitArray:
    IEnumerable<bool>
{
    
    #region Fields
    
    /// <summary>
    /// 
    /// </summary>
    private readonly byte[] _bits;
    
    /// <summary>
    /// 
    /// </summary>
    private readonly byte _lastByteAffectedBits;
    
    #endregion
    
    #region Constructors
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="bitsCount"></param>
    public BitArray(
        uint bitsCount)
    {
        Guardant.Instance
            .ThrowIfLowerThan(bitsCount, 0U);

        if ((_lastByteAffectedBits = (byte)(bitsCount & 7)) != 0)
        {
            bitsCount += 8U - _lastByteAffectedBits;
        }

        _bits = new byte[bitsCount >> 3];
        if (_bits.Length != 0 && _lastByteAffectedBits == 0)
        {
            _lastByteAffectedBits = 8;
        }
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="bits"></param>
    public BitArray(
        IEnumerable<bool> bits)
    {
        Guardant.Instance
            .ThrowIfNull(bits);

        var packedBitsList = new List<byte>();
        var packedBits = default(byte);
        var iteration = 0;
        
        foreach (var bit in bits)
        {
            packedBits |= Convert.ToByte(Convert.ToByte(bit) << iteration);
            
            if (++iteration == 8)
            {
                iteration = 0;
                packedBitsList.Add(packedBits);
                packedBits = 0;
            }
        }

        if (iteration != 0)
        {
            packedBitsList.Add(packedBits);
            _lastByteAffectedBits = Convert.ToByte(iteration);
        }
        else
        {
            _lastByteAffectedBits = 8;
        }

        _bits = packedBitsList.ToArray();
    }
    
    #endregion
    
    #region Properties
    
    /// <summary>
    /// 
    /// </summary>
    public int BitsCount =>
        _bits.Length == 0
            ? 0
            : ((_bits.Length - 1) << 3) + _lastByteAffectedBits;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="bitIndex"></param>
    public bool this[
        int bitIndex]
    {
        get =>
            ((_bits[bitIndex >> 3] >> (bitIndex & 7)) & 1) == 1;

        set
        {
            if (value)
            {
                _bits[bitIndex >> 3] |= (byte)(1 << (bitIndex & 7));
            }
            else
            {
                _bits[bitIndex >> 3] &= (byte)~(1 << bitIndex & 7);
            }
        }
    }
    
    #endregion
    
    #region System.Collections.IEnumerable implementation
    
    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    #endregion
    
    #region System.Collections.Generic.IEnumerable<bool> implementation

    public IEnumerator<bool> GetEnumerator()
    {
        for (var i = 0; i < ((_bits.Length - 1) << 3) + _lastByteAffectedBits; i++)
        {
            yield return this[i];
        }
    }
    
    #endregion
    
}