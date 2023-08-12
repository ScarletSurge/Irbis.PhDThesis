using System.Collections;
using System.Numerics;

using PhDThesis.Domain.Extensions;
using PhDThesis.Domain.Helpers.Guarding;

namespace PhDThesis.Math.Domain;

/// <summary>
/// 
/// </summary>
public sealed class HomogenousHypergraph:
    IEnumerable<HyperEdge>
{

    #region Fields

    /// <summary>
    /// 
    /// </summary>
    private int _verticesCount;

    /// <summary>
    /// 
    /// </summary>
    private readonly BitArray _simplices;

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
        Guardant.Instance
            .ThrowIfGreaterThan(simplicesDimension, verticesCount);
        
        VerticesCount = verticesCount;
        SimplicesDimension = simplicesDimension;
        var simplicesMaxCount = _simplicesMaxCount = BigIntegerExtensions.CombinationsCount(
            VerticesCount, SimplicesDimension);

        _simplices = new BitArray((uint)simplicesMaxCount);
        
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
            Guardant.Instance
                .ThrowIfNull(hyperEdge)
                .ThrowIfNotEqual(hyperEdge.VerticesCount, SimplicesDimension);
            
            return _simplices[SimplexToBitIndex(hyperEdge)];
        }
        
        set
        {
            Guardant.Instance
                .ThrowIfNull(hyperEdge)
                .ThrowIfNotEqual(hyperEdge.VerticesCount, SimplicesDimension);
            
            _simplices[SimplexToBitIndex(hyperEdge)] = value;
        }
    }
    
    /// <summary>
    /// 
    /// </summary>
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
            Guardant.Instance
                .ThrowIfLowerThanOrEqualTo(value, 0);

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
            Guardant.Instance
                .ThrowIfLowerThanOrEqualTo(value, 1);

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
        Guardant.Instance
            .ThrowIfNull(hyperEdge);
        
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
        Guardant.Instance
            .ThrowIfLowerThan(simplexBitIndex, 0)
            .ThrowIfGreaterThanOrEqualTo(simplexBitIndex, SimplicesMaxCount);

        return _simplices[simplexBitIndex];
    }
    
    #endregion
    
    #region Mapping
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="hyperEdge"></param>
    /// <param name="simplicesDimension"></param>
    /// <param name="verticesCount"></param>
    /// <param name="simplicesMaxCount"></param>
    /// <returns></returns>
    public static int SimplexToBitIndex(
        HyperEdge hyperEdge,
        int simplicesDimension,
        int verticesCount,
        BigInteger simplicesMaxCount)
    {
        Guardant.Instance
            .ThrowIfNull(hyperEdge);

        var result = BigInteger.Zero;
        using var enumerator = hyperEdge.GetEnumerator();
        for (var i = 0; i < simplicesDimension; i++)
        {
            if (!enumerator.MoveNext())
            {
                // TODO: ?!
            }
            
            var value = enumerator.Current;

            result += BigIntegerExtensions.CombinationsCount(verticesCount - value - 1, simplicesDimension - i);
        }

        return (int)(simplicesMaxCount - 1 - result);
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="hyperEdge"></param>
    /// <returns></returns>
    public int SimplexToBitIndex(
        HyperEdge hyperEdge)
    {
        return SimplexToBitIndex(hyperEdge, SimplicesDimension, VerticesCount, SimplicesMaxCount);
    }
    
    /// <summary>
    /// https://math.stackexchange.com/questions/1227409/indexing-all-combinations-without-making-list
    /// </summary>
    /// <param name="simplexBitIndex"></param>
    /// <param name="simplicesDimension"></param>
    /// <param name="verticesCount"></param>
    /// <param name="simplicesMaxCount"></param>
    /// <returns></returns>
    public static HyperEdge BitIndexToSimplex(
        int simplexBitIndex,
        int simplicesDimension,
        int verticesCount,
        BigInteger simplicesMaxCount)
    {
        Guardant.Instance
            .ThrowIfLowerThan(simplexBitIndex, 0)
            .ThrowIfGreaterThanOrEqualTo(simplexBitIndex, simplicesMaxCount);
        
        var result = new uint[simplicesDimension];
        var r = (BigInteger)simplexBitIndex;
        var j = 0u;
        
        for (var i = 0; i < simplicesDimension; i++)
        {
            uint cs = j + 1;
            BigInteger cc;
            
            while (r - (cc = BigIntegerExtensions.CombinationsCount(verticesCount - cs, simplicesDimension - i - 1)) >= 0)
            {
                r -= cc;
                cs++;
            }

            result[i] = cs - 1;
            j = cs;
        }
        
        return new HyperEdge(result);
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="simplexBitIndex"></param>
    /// <returns></returns>
    public HyperEdge BitIndexToSimplex(
        int simplexBitIndex)
    {
        return BitIndexToSimplex(simplexBitIndex, SimplicesDimension, VerticesCount, SimplicesMaxCount);
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