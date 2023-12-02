using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

using PhDThesis.UI.WPF.Controls.ViewModels;

namespace PhDThesis.UI.WPF.Controls;

/// <summary>
/// 
/// </summary>
public partial class Spinner:
    UserControl
{
    
    #region Constructors
    
    /// <summary>
    /// 
    /// </summary>
    public Spinner()
    {
        InitializeComponent();
        DataContext = new SpinnerViewModel(6, SpinnerViewModel.SpinnerRotationDirection.Clockwise, 5.0, 15);
        Loaded += OnLoaded;
    }
    
    #endregion
    
    #region Properties
    
    /// <summary>
    /// 
    /// </summary>
    private SpinnerViewModel ViewModel =>
        (DataContext as SpinnerViewModel)!;

    #region Dependency properties
    
    #region Items count
    
    /// <summary>
    /// 
    /// </summary>
    public int SpinnerItemsCount
    {
        get =>
            (int)GetValue(SpinnerItemsCountProperty);
        
        set =>
            SetValue(SpinnerItemsCountProperty, value);
    }

    /// <summary>
    /// 
    /// </summary>
    public static readonly DependencyProperty SpinnerItemsCountProperty = DependencyProperty.Register(
        nameof(SpinnerItemsCount), typeof(int), typeof(Spinner), new PropertyMetadata((dependencyObject, eventArgs) =>
        {
            (dependencyObject as Spinner)!.ViewModel.ItemsCount = (int)eventArgs.NewValue;
        }));
    
    #endregion
    
    #region Items color
    
    /// <summary>
    /// 
    /// </summary>
    public Brush SpinnerItemsColor
    {
        get =>
            (Brush)GetValue(SpinnerItemsColorProperty);
        
        set =>
            SetValue(SpinnerItemsColorProperty, value);
    }
    
    /// <summary>
    /// 
    /// </summary>
    public static readonly DependencyProperty SpinnerItemsColorProperty = DependencyProperty.Register(
        nameof(SpinnerItemsColor), typeof(Brush), typeof(Spinner), new PropertyMetadata(Brushes.Black));
    
    #endregion
    
    #region Rotation direction
    
    /// <summary>
    /// 
    /// </summary>
    public SpinnerViewModel.SpinnerRotationDirection RotationDirection
    {
        get =>
            (SpinnerViewModel.SpinnerRotationDirection)GetValue(RotationDirectionProperty);
        
        set =>
            SetValue(RotationDirectionProperty, value);
    }

    /// <summary>
    /// 
    /// </summary>
    public static readonly DependencyProperty RotationDirectionProperty = DependencyProperty.Register(
        nameof(RotationDirection), typeof(SpinnerViewModel.SpinnerRotationDirection), typeof(Spinner), new PropertyMetadata(
            (dependencyObject, eventArgs) =>
            {
                (dependencyObject as Spinner)!.ViewModel.RotationDirection =
                    (SpinnerViewModel.SpinnerRotationDirection)eventArgs.NewValue;
            }));
    
    #endregion
    
    #region Rotation angle increment
    
    /// <summary>
    /// 
    /// </summary>
    public double RotationAngleIncrement
    {
        get =>
            (double)GetValue(RotationAngleIncrementProperty);

        set =>
            SetValue(RotationAngleIncrementProperty, value);
    }
    
    /// <summary>
    /// 
    /// </summary>
    public static readonly DependencyProperty RotationAngleIncrementProperty = DependencyProperty.Register(
        nameof(RotationAngleIncrement), typeof(double), typeof(Spinner), new FrameworkPropertyMetadata((dependencyObject, eventArgs) =>
        {
            (dependencyObject as Spinner)!.ViewModel.RotationAngleIncrement = (double)eventArgs.NewValue;
        }));
    
    #endregion
    
    #region Rotation invalidate timeout
    
    /// <summary>
    /// 
    /// </summary>
    public int RotationInvalidateTimeoutMs
    {
        get =>
            (int)GetValue(RotationInvalidateTimeoutMsProperty);

        set =>
            SetValue(RotationInvalidateTimeoutMsProperty, value);
    }

    /// <summary>
    /// 
    /// </summary>
    public static readonly DependencyProperty RotationInvalidateTimeoutMsProperty = DependencyProperty.Register(
        nameof(RotationInvalidateTimeoutMs), typeof(int), typeof(Spinner), new PropertyMetadata( (dependencyObject, eventArgs) =>
        {
            (dependencyObject as Spinner)!.ViewModel.TimerInterval = (int)eventArgs.NewValue;
        }));

    #endregion
    
    #endregion
    
    #endregion
    
    #region Events handlers
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="eventArgs"></param>
    private void OnLoaded(
        object? sender,
        EventArgs eventArgs)
    {
        var window = Window.GetWindow(this);
        window!.Closing += OnWindowClosing;
        SizeChanged += OnSizeChanged;
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="eventArgs"></param>
    private void OnSizeChanged(
        object? sender,
        SizeChangedEventArgs eventArgs)
    {
        ViewModel.SceneSize = eventArgs.NewSize;
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void OnWindowClosing(
        object? sender,
        CancelEventArgs e)
    {
        ViewModel.Stop();
        var window = Window.GetWindow(this);
        window!.Closing -= OnWindowClosing;
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="eventArgs"></param>
    private void OnWindowClosed(
        object? sender,
        EventArgs eventArgs)
    {
        Loaded -= OnLoaded;
        SizeChanged -= OnSizeChanged;
        var window = Window.GetWindow(this);
        window!.Closed -= OnWindowClosed;
    }
    
    #endregion

}