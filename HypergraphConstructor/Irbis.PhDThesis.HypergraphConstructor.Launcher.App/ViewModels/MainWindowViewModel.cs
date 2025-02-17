using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

using Irbis.WPF.MVVM.Core.Commands;
using Irbis.WPF.MVVM.Core.ViewModels;

using Irbis.PhDThesis.Domain.Extensions;
using Irbis.PhDThesis.Math.Domain;
using Irbis.PhDThesis.Math.Domain.Reconstruction;
using Irbis.PhDThesis.Math.Reconstruction.VerticesDegreesVector.Greedy;
using Irbis.PhDThesis.Math.Reconstruction.VerticesDegreesVector.Reduction;

namespace Irbis.PhDThesis.HypergraphConstructor.Launcher.App.ViewModels;

/// <summary>
/// 
/// </summary>
internal sealed class MainWindowViewModel:
    ViewModelBase,
    IDisposable
{

    #region Fields
    
    #region Command fields
    
    /// <summary>
    /// 
    /// </summary>
    private readonly Lazy<ICommand> _quitCommand;
    
    /// <summary>
    /// 
    /// </summary>
    private readonly Lazy<ICommand> _showHelpCommand;
    
    /// <summary>
    /// 
    /// </summary>
    private readonly Lazy<ICommand> _setupSimplicesDimensionCommand;
    
    /// <summary>
    /// 
    /// </summary>
    private readonly Lazy<ICommand> _setupRestorationAlgorithmCommand;
    
    /// <summary>
    /// 
    /// </summary>
    private readonly Lazy<ICommand> _constructHypergraphCommand;
    
    #endregion
    
    /// <summary>
    /// 
    /// </summary>
    private bool _restorationOperationInProgress;
    
    /// <summary>
    /// 
    /// </summary>
    private string _verticesDegreesVectorString;
    
    /// <summary>
    /// 
    /// </summary>
    private RestorationAlgorithmFromVerticesDegreesVector _restorationAlgorithm;

    /// <summary>
    /// 
    /// </summary>
    private int _simplicesDimension = 2;

    /// <summary>
    /// 
    /// </summary>
    private HomogenousHypergraph _constructedHypergraph;

    /// <summary>
    /// 
    /// </summary>
    private readonly IReadOnlyDictionary<RestorationAlgorithmFromVerticesDegreesVector, HomogenousHypergraphFromVerticesDegreesVectorRestorerBase> _reconstructors;
    
    /// <summary>
    /// 
    /// </summary>
    private CancellationTokenSource _restorationOperationCancellationSource;

    #endregion

    #region Constructors
    
    /// <summary>
    /// 
    /// </summary>
    public MainWindowViewModel()
    {
        _reconstructors = new ReadOnlyDictionary<RestorationAlgorithmFromVerticesDegreesVector, HomogenousHypergraphFromVerticesDegreesVectorRestorerBase>(
            new Dictionary<RestorationAlgorithmFromVerticesDegreesVector, HomogenousHypergraphFromVerticesDegreesVectorRestorerBase>
            {
                { RestorationAlgorithmFromVerticesDegreesVector.Greedy, new HomogenousHypergraphFromVerticesDegreesVectorGreedyRestorer() },
                { RestorationAlgorithmFromVerticesDegreesVector.Reduction, new HomogenousHypergraphFromVerticesDegreesVectorReductionRestorer() }
            });
        _quitCommand = new Lazy<ICommand>(new RelayCommand(_ => QuitCommandAction()));
        _showHelpCommand = new Lazy<ICommand>(new RelayCommand(_ => ShowHelpCommandAction()));
        _setupSimplicesDimensionCommand = new Lazy<ICommand>(new RelayCommand(param => SetupSimplicesDimensionCommandAction((int)param)));
        _setupRestorationAlgorithmCommand = new Lazy<ICommand>(new RelayCommand(param => SetupRestorationAlgorithmCommandAction((RestorationAlgorithmFromVerticesDegreesVector)param)));
        _constructHypergraphCommand = new Lazy<ICommand>(new AsyncRelayCommand(_ => ConstructHypergraphAsyncCommandAction(_restorationOperationCancellationSource.Token)));
        _restorationOperationCancellationSource = new CancellationTokenSource();
    }

    #endregion

    #region Properties
    
    #region Command properties
    
    /// <summary>
    /// 
    /// </summary>
    public ICommand QuitCommand =>
        _quitCommand.Value;
    
    /// <summary>
    /// 
    /// </summary>
    public ICommand ShowHelpCommand =>
        _showHelpCommand.Value;
    
    /// <summary>
    /// 
    /// </summary>
    public ICommand SetupSimplicesDimensionCommand =>
        _setupSimplicesDimensionCommand.Value;

    /// <summary>
    /// 
    /// </summary>
    public ICommand SetupRestorationAlgorithmCommand =>
        _setupRestorationAlgorithmCommand.Value;
    
    /// <summary>
    /// 
    /// </summary>
    public ICommand ConstructHypergraphCommand =>
        _constructHypergraphCommand.Value;
    
    #endregion

    /// <summary>
    /// 
    /// </summary>
    public bool RestorationOperationInProgress
    {
        get =>
            _restorationOperationInProgress;

        private set =>
            SetProperty(ref _restorationOperationInProgress, value);
    }
    
    /// <summary>
    /// 
    /// </summary>
    public string VerticesDegreesVectorString
    {
        private get =>
            _verticesDegreesVectorString;

        set =>
            SetProperty(ref _verticesDegreesVectorString, value);
    }
    
    /// <summary>
    /// 
    /// </summary>
    public RestorationAlgorithmFromVerticesDegreesVector RestorationAlgorithm
    {
        private get =>
            _restorationAlgorithm;

        set =>
            SetProperty(ref _restorationAlgorithm, value);
    }
    
    /// <summary>
    /// 
    /// </summary>
    public int SimplicesDimension
    {
        get =>
            _simplicesDimension;

        set =>
            SetProperty(ref _simplicesDimension, value);
    }

    /// <summary>
    /// 
    /// </summary>
    public HomogenousHypergraph ConstructedHypergraph
    {
        get =>
            _constructedHypergraph;

        private set =>
            SetProperty(ref _constructedHypergraph, value);
    }

    #endregion

    #region Methods

    #region Command methods
    
    /// <summary>
    /// 
    /// </summary>
    private void QuitCommandAction()
    {

    }
    
    /// <summary>
    /// 
    /// </summary>
    private void ShowHelpCommandAction()
    {

    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="increment"></param>
    private void SetupSimplicesDimensionCommandAction(
        int increment)
    {
        var newSimplicesDimensionValue = SimplicesDimension + increment;
        if (newSimplicesDimensionValue < 2)
        {
            return;
        }

        SimplicesDimension = newSimplicesDimensionValue;
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="restorationAlgorithm"></param>
    private void SetupRestorationAlgorithmCommandAction(
        RestorationAlgorithmFromVerticesDegreesVector restorationAlgorithm)
    {
        RestorationAlgorithm = restorationAlgorithm;
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="cancellationToken"></param>
    private async Task ConstructHypergraphAsyncCommandAction(
        CancellationToken cancellationToken = default)
    {
        if (RestorationOperationInProgress)
        {
            _restorationOperationCancellationSource.Cancel();
            
            return;
        }
        
        var verticesDegreesStrings = VerticesDegreesVectorString.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

        if (verticesDegreesStrings.Length == 0)
        {
            MessageBox.Show("Invalid vertices degrees vector!");

            return;
        }

        var verticesDegreesVector = new int[verticesDegreesStrings.Length];

        var parsed = true;
        verticesDegreesStrings.ForEach((vertexDegreeString, index) =>
        {
            if (!parsed)
            {
                return;
            }
            
            if (!int.TryParse(vertexDegreeString, out verticesDegreesVector[index]) || verticesDegreesVector[index] < 0)
            {
                parsed = false;
            }
        });

        if (!parsed)
        {
            MessageBox.Show("Invalid vertices degrees vector!");

            return;
        }
        
        try
        {
            RestorationOperationInProgress = true;

            var hgs = _reconstructors[RestorationAlgorithm].RestoreAll(new VerticesDegreesVector(verticesDegreesVector), SimplicesDimension);

            foreach (var hg in hgs)
            {
                ConstructedHypergraph = hg;
                await Task.Delay(40, cancellationToken);
            }
        }
        catch (OperationCanceledException)
        {
            _restorationOperationCancellationSource.Dispose();
            _restorationOperationCancellationSource = new CancellationTokenSource();
        }
        catch (Exception)
        {
            MessageBox.Show("Vertices degrees vector couldn't be restored into hypergraph.");
        }
        finally
        {
            RestorationOperationInProgress = false;
        }
    }

    #endregion
    
    #endregion
    
    #region System.IDisposable implementation
    
    /// <inheritdoc cref="IDisposable.Dispose" />
    public void Dispose()
    {
        _restorationOperationCancellationSource?.Cancel();
        _restorationOperationCancellationSource?.Dispose();
        
        GC.SuppressFinalize(this);
    }
    
    /// <summary>
    /// 
    /// </summary>
    ~MainWindowViewModel()
    {
        _restorationOperationCancellationSource?.Cancel();
        _restorationOperationCancellationSource?.Dispose();
    }
    
    #endregion

}