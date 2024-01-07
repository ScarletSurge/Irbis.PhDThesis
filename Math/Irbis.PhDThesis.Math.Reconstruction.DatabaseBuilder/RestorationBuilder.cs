using Irbis.PhDThesis.Domain.Helpers.Guarding;

namespace Irbis.PhDThesis.Math.Reconstruction.DatabaseBuilder;

/// <summary>
/// 
/// </summary>
public class RestorationBuilder:
    IDisposable
{
    
    #region Fields

    private readonly int _homogenousHypergraphVerticesDegree;
    private readonly int _homogenousHypergraphVerticesCount;
    private readonly StreamWriter _outputFileWriter;
    
    #endregion
    
    #region Constructors
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="homogenousHypergraphVerticesDegree"></param>
    /// <param name="homogenousHypergraphVerticesCount"></param>
    /// <param name="outputFilePath"></param>
    public RestorationBuilder(
        int homogenousHypergraphVerticesDegree,
        int homogenousHypergraphVerticesCount,
        string outputFilePath)
    {
        Guardant.Instance
            .ThrowIfLowerThanOrEqualTo(_homogenousHypergraphVerticesDegree = homogenousHypergraphVerticesDegree, 1)
            .ThrowIfLowerThan(_homogenousHypergraphVerticesCount = homogenousHypergraphVerticesCount, _homogenousHypergraphVerticesDegree)
            .ThrowIfNull(outputFilePath);

        _outputFileWriter = new StreamWriter(new FileStream(outputFilePath, FileMode.Create, FileAccess.Write));
    }
    
    #endregion
    
    #region Methods
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="cancellationToken"></param>
    public async Task ConstructRestorationsAsync(
        CancellationToken cancellationToken = default)
    {
        
    }
    
    #endregion
    
    #region System.IDisposable implementation
    
    /// <inheritdoc cref="IDisposable.Dispose" />
    public void Dispose()
    {
        _outputFileWriter?.Dispose();
        GC.SuppressFinalize(this);
    }
    
    /// <summary>
    /// 
    /// </summary>
    ~RestorationBuilder()
    {
        _outputFileWriter?.Dispose();
    }
    
    #endregion

}