using System;
using System.Linq;
using System.Windows.Input;
using PhDThesis.Math.Domain;
using Wpf.MVVM.Core.Commands;
using Wpf.MVVM.Core.ViewModels;

namespace PhDThesis.HypergraphConstructor.Launcher.App.ViewModels;

/// <summary>
/// 
/// </summary>
internal sealed class MainWindowViewModel
    : ViewModelBase
{

    #region Fields

    private Lazy<ICommand> _quitCommand;
    private Lazy<ICommand> _showHelpCommand;
    private Lazy<ICommand> _constructHypergraphCommand;
    private HomogenousHypergraph _constructedHypergraph;

    #endregion

    #region Constructors

    public MainWindowViewModel()
    {
        _quitCommand = new Lazy<ICommand>(new RelayCommand(_ => QuitCommandAction()));
        _showHelpCommand = new Lazy<ICommand>(new RelayCommand(_ => ShowHelpCommandAction()));
        _constructHypergraphCommand = new Lazy<ICommand>(new RelayCommand(_ => ConstructHypergraphCommandAction()));
    }

    #endregion

    #region Properties

    public ICommand QuitCommand =>
        _quitCommand.Value;

    public ICommand ShowHelpCommand =>
        _showHelpCommand.Value;

    public ICommand ConstructHypergraphCommand =>
        _constructHypergraphCommand.Value;

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

    private void QuitCommandAction()
    {

    }

    private void ShowHelpCommandAction()
    {

    }

    private void ConstructHypergraphCommandAction()
    {
        var hg = new HomogenousHypergraph(11, 10,
            new[] { 0, 1, 13 }.OrderBy(x => x),
            new[] { 0, 1, 4 }.OrderBy(x => x),
            new[] { 11, 2, 5 }.OrderBy(x => x),
            new[] { 1, 5, 11 }.OrderBy(x => x),
            new[] { 3, 15, 4 }.OrderBy(x => x));

        ConstructedHypergraph = hg;
    }

    #endregion
    
    #endregion

}