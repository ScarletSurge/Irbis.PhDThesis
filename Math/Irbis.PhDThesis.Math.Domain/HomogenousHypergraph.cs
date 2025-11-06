using System.Collections;
using System.Numerics;

using Irbis.PhDThesis.Domain.Extensions;
using Irbis.PhDThesis.Domain.Helpers.Guarding;

namespace Irbis.PhDThesis.Math.Domain;

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
    /// <param name="simplices"></param>
    public HomogenousHypergraph(
        int verticesCount,
        int simplicesDimension,
        params HyperEdge[]? simplices)
    {
        Guardant.Instance
            .ThrowIfGreaterThan(simplicesDimension, verticesCount);
        
        VerticesCount = verticesCount;
        SimplicesDimension = simplicesDimension;
        var simplicesMaxCount = _simplicesMaxCount = BigIntegerExtensions.CombinationsCount(
            VerticesCount, SimplicesDimension);

        _simplices = new BitArray((int)simplicesMaxCount);
        
        foreach (var simplex in simplices ?? Enumerable.Empty<HyperEdge>())
        {
            this[simplex] = true;
        }
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="verticesCount"></param>
    /// <param name="simplicesDimension"></param>
    /// <param name="simplicesIndices"></param>
    public HomogenousHypergraph(
        int verticesCount,
        int simplicesDimension,
        params int[]? simplicesIndices)
    {
        Guardant.Instance
            .ThrowIfGreaterThan(simplicesDimension, verticesCount);
        
        VerticesCount = verticesCount;
        SimplicesDimension = simplicesDimension;
        var simplicesMaxCount = _simplicesMaxCount = BigIntegerExtensions.CombinationsCount(
            VerticesCount, SimplicesDimension);

        _simplices = new BitArray((int)simplicesMaxCount);
        
        foreach (var simplex in simplicesIndices ?? Enumerable.Empty<int>())
        {
            this[simplex] = true;
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
    /// <param name="simplexBitIndex"></param>
    public bool this[
        int simplexBitIndex]
    {
        get
        {
            Guardant.Instance
                .ThrowIfLowerThan(simplexBitIndex, 0);

            return _simplices[simplexBitIndex];
        }

        set
        {
            Guardant.Instance
                .ThrowIfLowerThan(simplexBitIndex, 0);

            _simplices[simplexBitIndex] = value;
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

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public VerticesDegreesVector ToVerticesDegreesVector()
    {
        var verticesDegreesVector = new int[VerticesCount];

        for (var simplexBitIndex = 0; simplexBitIndex < _simplices.BitsCount; ++simplexBitIndex)
        {
            if (!ContainsSimplex(simplexBitIndex))
            {
                continue;
            }

            var containedSimplex = BitIndexToSimplex(simplexBitIndex);
            foreach (var vertexIndex in containedSimplex)
            {
                ++verticesDegreesVector[vertexIndex];
            }
        }

        return new VerticesDegreesVector(verticesDegreesVector);
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="vertexIndex"></param>
    /// <returns></returns>
    public IEnumerable<int> GetSimplicesIncidentToVertexIndices(
        int vertexIndex)
    {
        IEnumerable<HyperEdge> GenerateSimplices(
            int lastAddedVertexIndex,
            HyperEdge toFill)
        {
            if (toFill.VerticesCount == _simplicesDimension)
            {
                yield return toFill;
            }
            else
            {
                for (var i = lastAddedVertexIndex + 1; i < VerticesCount; ++i)
                {
                    if (toFill._vertices.Add(i))
                    {
                        foreach (var generatedSimplex in GenerateSimplices(i, toFill))
                        {
                            yield return generatedSimplex;
                        }

                        toFill._vertices.Remove(i);
                    }
                }
            }
        }

        Guardant.Instance
            .ThrowIfLowerThan(vertexIndex, 0)
            .ThrowIfGreaterThanOrEqualTo(vertexIndex, _verticesCount);

        var simplex = new HyperEdge(vertexIndex);

        foreach (var candidateSimplex in GenerateSimplices(-1, simplex))
        {
            var simplexIndex = SimplexToBitIndex(candidateSimplex);
            if (ContainsSimplex(simplexIndex))
            {
                yield return simplexIndex;
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="vertexIndex"></param>
    /// <returns></returns>
    public IEnumerable<HyperEdge> GetSimplicesIncidentToVertex(
        int vertexIndex)
    {
        return GetSimplicesIncidentToVertexIndices(vertexIndex)
            .Select(BitIndexToSimplex);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="vertexIndex"></param>
    /// <returns></returns>
    public IEnumerable<int> GetIncidentVerticesIndicesFor(
        int vertexIndex)
    {
        var result = new HashSet<int>();

        foreach (var simplex in GetSimplicesIncidentToVertex(vertexIndex))
        {
            foreach (var incidentVertexIndex in simplex)
            {
                if (incidentVertexIndex == vertexIndex)
                {
                    continue;
                }
                
                if (result.Add(incidentVertexIndex))
                {
                    yield return incidentVertexIndex;
                }
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public bool CheckDijkstraConnectivity()
    {
        bool CheckDijkstraConnectivityInner(
            int lastAddedVertexIndex,
            HashSet<int> verticesIndicesSet)
        {
            var notVisitedVerticesIndices = new Lazy<List<int>>(() => new List<int>(VerticesCount));
            foreach (var incidentVertexForLastAdded in GetIncidentVerticesIndicesFor(lastAddedVertexIndex))
            {
                if (verticesIndicesSet.Add(incidentVertexForLastAdded))
                {
                    if (verticesIndicesSet.Count == VerticesCount)
                    {
                        return true;
                    }
                    
                    notVisitedVerticesIndices.Value.Add(incidentVertexForLastAdded);
                }
            }

            if (!notVisitedVerticesIndices.IsValueCreated)
            {
                return false;
            }
            
            foreach (var notVisitedVertexIndex in notVisitedVerticesIndices.Value)
            {
                if (CheckDijkstraConnectivityInner(notVisitedVertexIndex, verticesIndicesSet))
                {
                    return true;
                }
            }

            return false;
        }
        
        var verticesIndicesSet = new HashSet<int> { 0 };
        return CheckDijkstraConnectivityInner(0, verticesIndicesSet);
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
    /// <param name="verticesCount"></param>
    /// <param name="simplicesDimension"></param>
    /// <returns></returns>
    public static BigInteger GetSimplicesMaxCount(
        int verticesCount,
        int simplicesDimension)
    {
        return BigIntegerExtensions.CombinationsCount(verticesCount, simplicesDimension);
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
    /// 
    /// </summary>
    /// <param name="simplexBitIndex"></param>
    /// <param name="simplicesDimension"></param>
    /// <param name="verticesCount"></param>
    /// <returns></returns>
    public static HyperEdge BitIndexToSimplex(
        int simplexBitIndex,
        int simplicesDimension,
        int verticesCount)
    {
        return BitIndexToSimplex(simplexBitIndex, simplicesDimension, verticesCount,
            GetSimplicesMaxCount(verticesCount, simplicesDimension));
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
        
        var result = new int[simplicesDimension];
        var r = (BigInteger)simplexBitIndex;
        var j = 0;
        
        for (var i = 0; i < simplicesDimension; i++)
        {
            var cs = j + 1;
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
    
    #region System.Collections.IEnumerable implementation
    
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