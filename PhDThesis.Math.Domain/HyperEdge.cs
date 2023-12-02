using System.Collections;

using PhDThesis.Domain.Helpers.Guarding;

namespace PhDThesis.Math.Domain;

/// <summary>
/// 
/// </summary>
public sealed class HyperEdge:
    IEquatable<HyperEdge>,
    IEnumerable<uint>
{
    
    #region Fields
    
    /// <summary>
    /// 
    /// </summary>
    private readonly SortedSet<uint> _vertices;
    
    #endregion
    
    #region Constructors

    /// <summary>
    /// 
    /// </summary>
    /// <param name="values"></param>
    public HyperEdge(
        params uint[] values)
    {
        Guardant.Instance
            .ThrowIfNullOrEmpty(values);

        _vertices = new SortedSet<uint>(values);
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="values"></param>
    public HyperEdge(
        IEnumerable<uint> values)
    {
        Guardant.Instance
            .ThrowIfNullOrEmpty(values);
        
        _vertices = new SortedSet<uint>(values);
    }
    
    #endregion
    
    #region Properties
    
    /// <summary>
    /// 
    /// </summary>
    public int VerticesCount =>
        _vertices.Count;
    
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

        if (obj is HyperEdge hyperEdge)
        {
            return Equals(hyperEdge);
        }

        return false;
    }
    
    /// <inheritdoc cref="object.GetHashCode" />
    public override int GetHashCode()
    {
        var combinedHashCode = default(HashCode);
        
        foreach (var vertexIndex in _vertices)
        {
            combinedHashCode.Add(vertexIndex.GetHashCode());
        }

        return combinedHashCode.ToHashCode();
    }

    /// <inheritdoc cref="object.ToString" />
    public override string ToString()
    {
        return string.Join(", ", _vertices);
    }

    #endregion
    
    #region System.IEquatable<HyperEdge> implementation

    public bool Equals(
        HyperEdge? hyperEdge)
    {
        if (hyperEdge is null)
        {
            return false;
        }

        return _vertices.Equals(hyperEdge._vertices);
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
    public IEnumerator<uint> GetEnumerator()
    {
        return _vertices.GetEnumerator();
    }
    
    #endregion

}