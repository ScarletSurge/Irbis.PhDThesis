using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Irbis.WPF.MVVM.Core.Commands;
using Irbis.WPF.MVVM.Core.ViewModels;

using PhDThesis.Domain.Extensions;
using PhDThesis.Math.Domain;
using PhDThesis.Math.Domain.Reconstruction;
using PhDThesis.Math.Reconstruction.Greedy;
using PhDThesis.Math.Reconstruction.Reduction;

namespace PhDThesis.HypergraphConstructor.Launcher.App.ViewModels;

/// <summary>
/// 
/// </summary>
internal sealed class MainWindowViewModel
    : ViewModelBase
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
    private readonly IReadOnlyDictionary<RestorationAlgorithmFromVerticesDegreesVector, HomogenousHypergraphFromVerticesDegreesVectorReconstructorBase> _reconstructors;

    #endregion

    #region Constructors
    
    /// <summary>
    /// 
    /// </summary>
    public MainWindowViewModel()
    {
        _reconstructors = new ReadOnlyDictionary<RestorationAlgorithmFromVerticesDegreesVector, HomogenousHypergraphFromVerticesDegreesVectorReconstructorBase>(
            new Dictionary<RestorationAlgorithmFromVerticesDegreesVector, HomogenousHypergraphFromVerticesDegreesVectorReconstructorBase>
            {
                { RestorationAlgorithmFromVerticesDegreesVector.Greedy, new HomogenousHypergraphFromVerticesDegreesVectorGreedyReconstructor() },
                { RestorationAlgorithmFromVerticesDegreesVector.Reduction, new HomogenousHypergraphFromVerticesDegreesVectorReductionReconstructor() }
            });
        _quitCommand = new Lazy<ICommand>(new RelayCommand(_ => QuitCommandAction()));
        _showHelpCommand = new Lazy<ICommand>(new RelayCommand(_ => ShowHelpCommandAction()));
        _setupSimplicesDimensionCommand = new Lazy<ICommand>(new RelayCommand(param => SetupSimplicesDimensionCommandAction((int)param)));
        _setupRestorationAlgorithmCommand = new Lazy<ICommand>(new RelayCommand(param => SetupRestorationAlgorithmCommandAction((RestorationAlgorithmFromVerticesDegreesVector)param)));
        _constructHypergraphCommand = new Lazy<ICommand>(new AsyncRelayCommand(_ => ConstructHypergraphAsyncCommandAction()));
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

        //private set =>
        //    SetProperty(ref _restorationOperationInProgress, value);

        private set
        {
            _restorationOperationInProgress = value;
            OnPropertyChanged(nameof(RestorationOperationInProgress));
        }
    }
    
    /// <summary>
    /// 
    /// </summary>
    public string VerticesDegreesVectorString
    {
        private get =>
            _verticesDegreesVectorString;

        set
        {
            _verticesDegreesVectorString = value;
            OnPropertyChanged(nameof(VerticesDegreesVectorString));
        }
    }
    
    /// <summary>
    /// 
    /// </summary>
    public RestorationAlgorithmFromVerticesDegreesVector RestorationAlgorithm
    {
        private get =>
            _restorationAlgorithm;
        
        set
        {
            _restorationAlgorithm = value;
            OnPropertyChanged(nameof(RestorationAlgorithm));
        }
    }
    
    /// <summary>
    /// 
    /// </summary>
    public int SimplicesDimension
    {
        get =>
            _simplicesDimension;

        set
        {
            _simplicesDimension = value;
            OnPropertyChanged(nameof(SimplicesDimension));
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public HomogenousHypergraph ConstructedHypergraph
    {
        get =>
            _constructedHypergraph;

        private set
        {
            _constructedHypergraph = value;
            OnPropertyChanged(nameof(ConstructedHypergraph));
        }
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
        var verticesDegreesStrings =
            VerticesDegreesVectorString.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

        if (verticesDegreesStrings.Length == 0)
        {
            MessageBox.Show("Invalid vertices degrees vector!");

            return;
        }

        var verticesDegreesVector = new uint[verticesDegreesStrings.Length];

        var parsed = true;
        verticesDegreesStrings.ForEach((vertexDegreeString, index) =>
        {
            if (!parsed)
            {
                return;
            }
            
            if (!uint.TryParse(vertexDegreeString, out verticesDegreesVector[index]))
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
            ConstructedHypergraph = await _reconstructors[RestorationAlgorithm]
                .RestoreAsync(new VerticesDegreesVector(verticesDegreesVector), SimplicesDimension, cancellationToken);
        }
        catch (Exception)
        {
            RestorationOperationInProgress = false;
            MessageBox.Show("Vertices degrees vector couldn't be restored into hypergraph.");
        }
        finally
        {
            RestorationOperationInProgress = false;
        }
    }

    #endregion
    
    #endregion

}