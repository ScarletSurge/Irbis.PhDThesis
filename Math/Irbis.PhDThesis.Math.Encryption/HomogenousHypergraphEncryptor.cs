using Irbis.Crypto.Domain;

using Irbis.PhDThesis.Domain.Helpers.Guarding;
using Irbis.PhDThesis.Math.Domain;

namespace Irbis.PhDThesis.Math.Encryption;

/// <summary>
/// 
/// </summary>
public sealed class HomogenousHypergraphEncryptor:
    ISymmetricCipherAlgorithm
{
    
    #region Fields
    
    /// <summary>
    /// 
    /// </summary>
    private readonly HomogenousHypergraph _key;
    
    /// <summary>
    /// 
    /// </summary>
    private readonly int _smallBlockSize;
    
    /// <summary>
    /// 
    /// </summary>
    private readonly int[][] _homogenousHypergraphVerticesIncidence;
    
    #endregion
    
    #region Constructors
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="key"></param>
    /// <param name="smallBlockSize"></param>
    /// <exception cref="ArgumentNullException"></exception>
    public HomogenousHypergraphEncryptor(
        HomogenousHypergraph key,
        int smallBlockSize)
    {
        Guardant.Instance
            .ThrowIfNull(key)
            .ThrowIfLowerThanOrEqualTo(smallBlockSize, 0);

        _key = key;
        _smallBlockSize = smallBlockSize;
        _homogenousHypergraphVerticesIncidence = new int[key.VerticesCount - key.SimplicesDimension + 1][];
        for (var i = 0; i < _homogenousHypergraphVerticesIncidence.Length; ++i)
        {
            _homogenousHypergraphVerticesIncidence[i] = key.GetIncidentVerticesIndicesFor(i)
                .Where(incidentVertexIndex => incidentVertexIndex > i)
                .ToArray();
        }
    }

    #endregion
    
    #region Methods
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="into"></param>
    /// <param name="intoStartIndex"></param>
    /// <param name="from"></param>
    /// <param name="fromStartIndex"></param>
    /// <param name="count"></param>
    private void Xor(
        byte[] into,
        int intoStartIndex,
        byte[] from,
        int fromStartIndex,
        int count)
    {
        for (var i = 0; i != count; ++i)
        {
            into[intoStartIndex + i] ^= from[fromStartIndex + i];
        }
    }

    #endregion

    #region Irbis.Crypto.Domain.ISymmetricCipherAlgorithm implementation

    /// <inheritdoc cref="ICipherAlgorithm{TData,TCiphertext}.Encrypt" />
    public void Encrypt(
        byte[] input,
        ref byte[] output)
    {
        Guardant.Instance
            .ThrowIfNull(input)
            .ThrowIfNotEqual(input.Length, BlockSize);

        output ??= new byte[BlockSize];

        Guardant.Instance
            .ThrowIfNotEqual(input.Length, BlockSize);

        if (!ReferenceEquals(input, output))
        {
            Array.Copy(input, output, BlockSize);
        }
        
        var Y = new byte[_smallBlockSize];
        
        for (var j = 0; j < _homogenousHypergraphVerticesIncidence.Length; ++j)
        {
            var jCopy = j;
            
            Array.Clear(Y, 0, Y.Length);
            
            foreach (var incidentVertexIndex in _homogenousHypergraphVerticesIncidence[jCopy])
            {
                Xor(Y, 0, output, _smallBlockSize * incidentVertexIndex, _smallBlockSize);
            }

            if ((_homogenousHypergraphVerticesIncidence[jCopy].Length & 1) == 1)
            {
                Xor(Y, 0, output, _smallBlockSize * jCopy, _smallBlockSize);
            }
            
            Xor(output, _smallBlockSize * jCopy, Y, 0, _smallBlockSize);
            
            foreach (var incidentVertexIndex in _homogenousHypergraphVerticesIncidence[jCopy])
            {
                Xor(output, _smallBlockSize * incidentVertexIndex, Y, 0, _smallBlockSize);
            }
        }
    }
    
    /// <inheritdoc cref="ICipherAlgorithm{TData,TCiphertext}.Decrypt" />
    public void Decrypt(
        byte[] input,
        ref byte[] output)
    {
        Guardant.Instance
            .ThrowIfNull(input)
            .ThrowIfNotEqual(input.Length, BlockSize);
        
        output ??= new byte[input.Length];

        Guardant.Instance
            .ThrowIfNotEqual(input.Length, BlockSize);

        if (!ReferenceEquals(input, output))
        {
            Array.Copy(input, output, BlockSize);
        }
        
        var Y = new byte[_smallBlockSize];
        
        for (var j = _homogenousHypergraphVerticesIncidence.Length - 1; j >= 0; --j)
        {
            var jCopy = j;
            
            Array.Clear(Y, 0, Y.Length);
            foreach (var incidentVertexIndex in _homogenousHypergraphVerticesIncidence[jCopy])
            {
                Xor(Y, 0, output, _smallBlockSize * incidentVertexIndex, _smallBlockSize);
            }
            
            if ((_homogenousHypergraphVerticesIncidence[jCopy].Length & 1) == 1)
            {
                Xor(Y, 0, output, _smallBlockSize * jCopy, _smallBlockSize);
            }
            
            Xor(output, _smallBlockSize * jCopy, Y, 0, _smallBlockSize);
                
            foreach (var incidentVertexIndex in _homogenousHypergraphVerticesIncidence[jCopy])
            {
                Xor(output, _smallBlockSize * incidentVertexIndex, Y, 0, _smallBlockSize);
            }
        }
    }

    /// <inheritdoc cref="ISymmetricCipherAlgorithm.BlockSize" />
    public int BlockSize =>
        _key.VerticesCount * _smallBlockSize;

    #endregion

}