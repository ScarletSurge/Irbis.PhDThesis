using System;
using System.Collections;

using PhDThesis.Domain.Helpers.Guarding;

namespace PhDThesis.Math.Domain;

/// <summary>
/// 
/// </summary>
public sealed class VerticesDegreesVector:
    IEquatable<VerticesDegreesVector>,
    IEnumerable<uint>,
    ICloneable
{
    
    #region Fields
    
    /// <summary>
    /// 
    /// </summary>
    private readonly uint[] _verticesDegrees;
    
    #endregion
    
    #region Constructors
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="verticesDegrees"></param>
    public VerticesDegreesVector(
        params uint[]? verticesDegrees)
    {
        if (verticesDegrees is null)
        {
            throw new ArgumentNullException(nameof(verticesDegrees));
        }
        
        if (verticesDegrees.Length == 0)
        {
            throw new ArgumentException("Vertices degrees vector can't be empty", nameof(verticesDegrees));
        }
        
        _verticesDegrees = verticesDegrees.ToArray();
    }
    
    #endregion
    
    #region Properties

    /// <summary>
    /// 
    /// </summary>
    /// <param name="vertexId"></param>
    public uint this[uint vertexId]
    {
        get =>
            _verticesDegrees[vertexId];

        private set =>
            _verticesDegrees[vertexId] = value;
    }
    
    /// <summary>
    /// 
    /// </summary>
    public int VerticesCount =>
        _verticesDegrees.Length;

    #endregion
    
    #region Methods
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="hyperEdge"></param>
    public void AddSimplex(
        HyperEdge hyperEdge)
    {
        Guardant.Instance
            .ThrowIfNullOrEmpty(hyperEdge);

        foreach (var vertex in hyperEdge)
        {
            Guardant.Instance
                .ThrowIfGreaterThanOrEqualTo(vertex, (uint)_verticesDegrees.Length);
        }

        foreach (var vertex in hyperEdge)
        {
            ++this[vertex];
        }
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="hyperEdge"></param>
    /// <returns></returns>
    public bool TryRemoveSimplex(
        HyperEdge hyperEdge)
    {
        try
        {
            RemoveSimplex(hyperEdge);
            return true;
        }
        catch (GuardException)
        {
            return false;
        }
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="hyperEdge"></param>
    public void RemoveSimplex(
        HyperEdge hyperEdge)
    {
        Guardant.Instance
            .ThrowIfNullOrEmpty(hyperEdge)
            .ThrowIfGreaterThan(hyperEdge.VerticesCount, _verticesDegrees.Length);

        foreach (var vertex in hyperEdge)
        {
            Guardant.Instance
                .ThrowIfGreaterThanOrEqualTo(vertex, (uint)_verticesDegrees.Length);
        }

        foreach (var vertex in hyperEdge)
        {
            --this[vertex];
        }
    }
    
    #endregion

    #region System.Object overrides
    
    /// <inheritdoc cref="object.Equals(object?)" />
    public override bool Equals(
        object? obj)
    {
        if (obj is null)
        {
            return false;
        }

        if (obj is VerticesDegreesVector verticesDegreesVector)
        {
            return Equals(verticesDegreesVector);
        }

        return false;
    }
    
    /// <inheritdoc cref="object.GetHashCode" />
    public override int GetHashCode()
    {
        var combinedHashCode = default(HashCode);
        
        foreach (var vertexDegree in _verticesDegrees)
        {
            combinedHashCode.Add(vertexDegree.GetHashCode());
        }

        return combinedHashCode.ToHashCode();
    }
    
    /// <inheritdoc cref="object.ToString" />
    public override string ToString()
    {
        return $"[ {string.Join(", ", _verticesDegrees)} ]";
    }

    #endregion
    
    #region System.IEquatable<VerticesDegreesVector> implementation
    
    /// <inheritdoc cref="IEquatable{T}.Equals(T?)" />
    public bool Equals(
        VerticesDegreesVector? other)
    {
        if (other is null)
        {
            return false;
        }

        return _verticesDegrees.Equals(other);
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
    public IEnumerator<uint> GetEnumerator()
    {
        return ((IEnumerable<uint>)_verticesDegrees).GetEnumerator();
    }
    
    #endregion
    
    #region System.ICloneable implementation
    
    /// <inheritdoc cref="ICloneable.Clone" />
    public object Clone()
    {
        return new VerticesDegreesVector(_verticesDegrees);
    }
    
    #endregion

}