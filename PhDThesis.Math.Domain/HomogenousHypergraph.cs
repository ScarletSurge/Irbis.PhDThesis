using System.Numerics;
using PhDThesis.Domain.Extensions;

namespace PhDThesis.Math.Domain;

/// <summary>
/// 
/// </summary>
public sealed class HomogenousHypergraph
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
        params IOrderedEnumerable<int>[] simplicesVertices)
    {
        VerticesCount = verticesCount;
        SimplicesDimension = simplicesDimension;
        var simplicesMaxCount = _simplicesMaxCount = BigIntegerExtensions.CombinationsCount(
            VerticesCount, SimplicesDimension);
        
        if (simplicesMaxCount % 8 != 0)
        {
            simplicesMaxCount += 8 - simplicesMaxCount % 8;
        }
        
        _simplices = new byte[(ulong)simplicesMaxCount / 8];
        
        foreach (var simplexVertex in simplicesVertices ?? Enumerable.Empty<IOrderedEnumerable<int>>())
        {
            this[simplexVertex] = true;
        }
    }

    #endregion

    #region Properties

    private bool this[IOrderedEnumerable<int> simplexVertices]
    {
        // TODO: guard args
        
        get
        {
            var bitIndex = SimplexToBitIndex(simplexVertices);
            return ((_simplices[bitIndex >> 3] >> (bitIndex & 7)) & 1) == 1;
        }
        
        set
        {
            var bitIndex = SimplexToBitIndex(simplexVertices);
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
            if (value < 0)
            {
                throw new ArgumentException("Vertices count must be GT 0", nameof(value));
            }

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
            if (value < 2)
            {
                throw new ArgumentException("Simplex dimension must be GT 2", nameof(value));
            }

            _simplicesDimension = value;
        }
    }
    
    #endregion
    
    #region Methods
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="vertices"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public bool ContainsSimplex(
        IOrderedEnumerable<int> vertices)
    {
        // TODO: guard
        
        return this[vertices];
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="simplexBitIndex"></param>
    /// <returns></returns>
    public bool ContainsSimplex(
        int simplexBitIndex)
    {
        // TODO: guard
        
        return ((_simplices[simplexBitIndex >> 3] >> (simplexBitIndex & 7)) & 1) == 1;
    }
    
    #endregion
    
    #region Mapping
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="simplexVertices"></param>
    /// <returns></returns>
    private int SimplexToBitIndex(
        IOrderedEnumerable<int> simplexVertices)
    {
        // TODO: guard args

        var result = BigInteger.Zero;
        using var enumerator = simplexVertices.GetEnumerator();
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
    /// <param name="index"></param>
    /// <returns></returns>
    public IOrderedEnumerable<int> BitIndexToSimplex(
        int index)
    {
        var result = new int[SimplicesDimension];
        var r = (BigInteger)index;
        var j = 0;
        
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
        
        return result.OrderBy(x => x);
    }

    #endregion

}