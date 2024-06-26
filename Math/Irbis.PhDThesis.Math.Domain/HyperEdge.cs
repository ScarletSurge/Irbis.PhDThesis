﻿using System.Collections;

using Irbis.PhDThesis.Domain.Helpers.Guarding;

namespace Irbis.PhDThesis.Math.Domain;

/// <summary>
/// 
/// </summary>
public sealed class HyperEdge:
    IEquatable<HyperEdge>,
    IEnumerable<int>
{
    
    #region Fields
    
    /// <summary>
    /// 
    /// </summary>
    internal readonly SortedSet<int> _vertices;
    
    #endregion
    
    #region Constructors

    /// <summary>
    /// 
    /// </summary>
    /// <param name="values"></param>
    public HyperEdge(
        params int[] values)
    {
        Guardant.Instance
            .ThrowIfNullOrEmpty(values);

        _vertices = new SortedSet<int>(values);
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="values"></param>
    public HyperEdge(
        IEnumerable<int> values)
    {
        Guardant.Instance
            .ThrowIfNullOrEmpty(values);
        
        _vertices = new SortedSet<int>(values);
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
    public IEnumerator<int> GetEnumerator()
    {
        return _vertices.GetEnumerator();
    }
    
    #endregion

}