using System.Collections;
using System.Numerics;

using PhDThesis.Domain.Extensions;
using PhDThesis.Domain.Helpers.Guarding;

namespace PhDThesis.Math.Domain;

/// <summary>
/// 
/// </summary>
public sealed class HomogenousHypergraph : IEnumerable<HyperEdge>
{

    #region Fields

    /// <summary>
    /// 
    /// </summary>
    private int _verticesCount;

    /// <summary>
    /// 
    /// </summary>
    private readonly byte[] _simplices;

    /// <summary>
    /// 
    /// </summary>
    private int _simplicesDimension;
    
    /// <summary>
    /// 
    /// </summary>
    private readonly BigInteger _simplicesMaxCount;

    #endregion

    #region Constructors

    /// <summary>
    /// 
    /// </summary>
    /// <param name="verticesCount"></param>
    /// <param name="simplicesDimension"></param>
    /// <param name="simplicesVertices"></param>
    public HomogenousHypergraph(
        int verticesCount,
        int simplicesDimension,
        params HyperEdge[]? simplicesVertices)
    {
        Guard.ThrowIfGreaterThan(simplicesDimension, verticesCount);
        
        VerticesCount = verticesCount;
        SimplicesDimension = simplicesDimension;
        var simplicesMaxCount = _simplicesMaxCount = BigIntegerExtensions.CombinationsCount(
            VerticesCount, SimplicesDimension);
        
        if (simplicesMaxCount % 8 != 0)
        {
            simplicesMaxCount += 8 - simplicesMaxCount % 8;
        }
        
        _simplices = new byte[(ulong)simplicesMaxCount / 8];
        
        foreach (var simplexVertex in simplicesVertices ?? Enumerable.Empty<HyperEdge>())
        {
            this[simplexVertex] = true;
        }
    }

    #endregion

    #region Properties
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="hyperEdge"></param>
    private bool this[
        HyperEdge hyperEdge]
    {
        get
        {
            Guard.ThrowIfNull(hyperEdge);
            var bitIndex = SimplexToBitIndex(hyperEdge);
            return ((_simplices[bitIndex >> 3] >> (bitIndex & 7)) & 1) == 1;
        }
        
        set
        {
            Guard.ThrowIfNull(hyperEdge);
            var bitIndex = SimplexToBitIndex(hyperEdge);
            _simplices[bitIndex >> 3] |= (byte)((value ? 1 : 0) << (bitIndex & 7));
        }
    }

    public int SimplicesMaxCount =>
        (int)_simplicesMaxCount;

    /// <summary>
    /// 
    /// </summary>
    /// <exception cref="ArgumentException"></exception>
    public int VerticesCount
    {
        get =>
            _verticesCount;

        private set
        {
            Guard.ThrowIfLowerThanOrEqualTo(value, 0);

            _verticesCount = value;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <exception cref="ArgumentException"></exception>
    public int SimplicesDimension
    {
        get =>
            _simplicesDimension;

        private set
        {
            Guard.ThrowIfLowerThanOrEqualTo(value, 1);

            _simplicesDimension = value;
        }
    }
    
    #endregion
    
    #region Methods
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="hyperEdge"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public bool ContainsSimplex(
        HyperEdge hyperEdge)
    {
        Guard.ThrowIfNull(hyperEdge);
        
        return this[hyperEdge];
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="simplexBitIndex"></param>
    /// <returns></returns>
    public bool ContainsSimplex(
        int simplexBitIndex)
    {
        Guard.ThrowIfLowerThan(simplexBitIndex, 0);
        Guard.ThrowIfGreaterThanOrEqualTo(simplexBitIndex, SimplicesMaxCount);
        
        return ((_simplices[simplexBitIndex >> 3] >> (simplexBitIndex & 7)) & 1) == 1;
    }
    
    #endregion
    
    #region Mapping
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="hyperEdge"></param>
    /// <returns></returns>
    private int SimplexToBitIndex(
        HyperEdge hyperEdge)
    {
        Guard.ThrowIfNull(hyperEdge);

        var result = BigInteger.Zero;
        using var enumerator = hyperEdge.GetEnumerator();
        for (var i = 0; i < SimplicesDimension; i++)
        {
            if (!enumerator.MoveNext())
            {
                // TODO: ?!
            }
            
            var value = enumerator.Current;

            result += BigIntegerExtensions.CombinationsCount(VerticesCount - value - 1, SimplicesDimension - i);
        }

        return (int)(_simplicesMaxCount - 1 - result);
    }
    
    /// <summary>
    /// https://math.stackexchange.com/questions/1227409/indexing-all-combinations-without-making-list
    /// </summary>
    /// <param name="simplexBitIndex"></param>
    /// <returns></returns>
    public HyperEdge BitIndexToSimplex(
        int simplexBitIndex)
    {
        Guard.ThrowIfLowerThan(simplexBitIndex, 0);
        Guard.ThrowIfGreaterThanOrEqualTo(simplexBitIndex, SimplicesMaxCount);
        
        var result = new uint[SimplicesDimension];
        var r = (BigInteger)simplexBitIndex;
        var j = (uint)0;
        
        for (var i = 0; i < SimplicesDimension; i++)
        {
            var cs = j + 1;
            BigInteger cc;
            
            while (r - (cc = BigIntegerExtensions.CombinationsCount(VerticesCount - cs, SimplicesDimension - i - 1)) >= 0)
            {
                r -= cc;
                cs++;
            }

            result[i] = cs - 1;
            j = cs;
        }
        
        return new HyperEdge(result);
    }

    #endregion
    
    #region System.Collections.IEnumerator implementation
    
    /// <inheritdoc cref="IEnumerable.GetEnumerator" />
    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
    
    #endregion
    
    #region System.Collections.Generic.IEnumerable<out T> implementation
    
    /// <inheritdoc cref="IEnumerable{T}.GetEnumerator" />
    public IEnumerator<HyperEdge> GetEnumerator()
    {
        for (var i = 0; i < SimplicesMaxCount; i++)
        {
            if (!ContainsSimplex(i))
            {
                continue;
            }

            yield return BitIndexToSimplex(i);
        }
    }
    
    #endregion
    
}