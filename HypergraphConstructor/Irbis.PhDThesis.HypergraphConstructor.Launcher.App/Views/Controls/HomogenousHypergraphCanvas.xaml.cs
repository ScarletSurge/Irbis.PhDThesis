using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

using Irbis.PhDThesis.Math.Domain;

namespace Irbis.PhDThesis.HypergraphConstructor.Launcher.App.Views.Controls;

/// <summary>
/// 
/// </summary>
public partial class HomogenousHypergraphCanvas:
    UserControl
{
    
    #region Constants
    
    // TODO: move to dependency properties
    // TODO: add colors dependency properties
    
    /// <summary>
    /// 
    /// </summary>
    private const float VertexRadius = 3;
    
    /// <summary>
    /// 
    /// </summary>
    private const float SimplexPartThickness = 0.5f;
    
    /// <summary>
    /// 
    /// </summary>
    private const float SimplexCenterRadius = 1.5f;
    
    /// <summary>
    /// 
    /// </summary>
    private const int VertexZIndex = 2;
    
    /// <summary>
    /// 
    /// </summary>
    private const int SimplexPartZIndex = 0;
    
    /// <summary>
    /// 
    /// </summary>
    private const int SimplexCenterZIndex = 1;
    
    #endregion
    
    #region Fields
    
    /// <summary>
    /// 
    /// </summary>
    private HomogenousHypergraph _currentHypergraph;
    
    #endregion
    
    #region Constructors

    /// <summary>
    /// 
    /// </summary>
    public HomogenousHypergraphCanvas()
    {
        InitializeComponent();
        
        SizeChanged += (_, _) =>
        {
            RedrawHypergraph();
        };
    }
    
    #endregion
    
    #region Dependency properties
    
    /// <summary>
    /// 
    /// </summary>
    public HomogenousHypergraph HomogenousHypergraphToShow
    {
        get =>
            (HomogenousHypergraph)GetValue(HomogenousHypergraphToShowProperty);

        set =>
            SetValue(HomogenousHypergraphToShowProperty, value);
    }
    
    /// <summary>
    /// 
    /// </summary>
    public static readonly DependencyProperty HomogenousHypergraphToShowProperty = DependencyProperty.Register(nameof(HomogenousHypergraphToShow), typeof(HomogenousHypergraph), typeof(HomogenousHypergraphCanvas), new FrameworkPropertyMetadata((d, e) =>
        {
            if (d is not HomogenousHypergraphCanvas homogenousHypergraphCanvas)
            {
                throw new ArgumentException($"Parameter \"{d}\" should be of type \"{typeof(HomogenousHypergraphCanvas).FullName}\"",
                    nameof(d));
            }
            
            homogenousHypergraphCanvas.ReconstructHypergraphWith(homogenousHypergraphCanvas._currentHypergraph = (HomogenousHypergraph)e.NewValue);
        }));
    
    #endregion
    
    #region Methods
    
    /// <summary>
    /// 
    /// </summary>
    private void RedrawHypergraph()
    {
        if (_currentHypergraph is null)
        {
            return;
        }
        
        // TODO: separate this logic from reconstruction
        
        ReconstructHypergraphWith(_currentHypergraph);
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="homogenousHypergraph"></param>
    private void ReconstructHypergraphWith(
        HomogenousHypergraph homogenousHypergraph)
    {
        var targetCanvas = _componentsCanvas;
        targetCanvas.Children.Clear();
        
        if (homogenousHypergraph is null)
        {
            return;
        }
        
        var targetCanvasHeight = targetCanvas.ActualHeight;
        var targetCanvasWidth = targetCanvas.ActualWidth;
        var targetCanvasCenterPoint = new Point(targetCanvasWidth / 2, targetCanvasHeight / 2);

        var vertices = new (Ellipse, Point)[homogenousHypergraph.VerticesCount];

        var rectanglePerimeter = (targetCanvas.RenderSize.Height + targetCanvas.RenderSize.Width) * 2;
        for (var i = 0; i < homogenousHypergraph.VerticesCount; i++)
        {
            var targetVertexPoint = CalculateRectanglePoint(targetCanvasCenterPoint, targetCanvasHeight, targetCanvasWidth, rectanglePerimeter * i / homogenousHypergraph.VerticesCount);
            
            Ellipse vertexView;
            targetCanvas.Children.Add(vertexView = new Ellipse
            {
                Fill = Brushes.Red,
                Height = VertexRadius * 2,
                Width = VertexRadius * 2,
            });
            Canvas.SetLeft(vertexView, targetVertexPoint.X - VertexRadius);
            Canvas.SetTop(vertexView, targetVertexPoint.Y - VertexRadius);
            Panel.SetZIndex(vertexView, VertexZIndex);
            vertices[i] = (vertexView, targetVertexPoint);
        }
        
        for (var i = 0; i < homogenousHypergraph.SimplicesMaxCount; i++)
        {
            if (!homogenousHypergraph.ContainsSimplex(i))
            {
                continue;
            }
            
            var simplex = homogenousHypergraph.BitIndexToSimplex(i);
            
            var verticesIds = simplex.ToArray();
            var simplexCenterPoint = new Point(verticesIds.Sum(id => vertices[id].Item2.X) / homogenousHypergraph.SimplicesDimension, verticesIds.Sum(id => vertices[id].Item2.Y) / homogenousHypergraph.SimplicesDimension);
            for (var j = 0; j < homogenousHypergraph.SimplicesDimension; j++)
            {
                var simplexPartView = new Line
                {
                    X1 = simplexCenterPoint.X,
                    Y1 = simplexCenterPoint.Y,
                    X2 = vertices[verticesIds[j]].Item2.X,
                    Y2 = vertices[verticesIds[j]].Item2.Y,
                    Stroke = Brushes.Green,
                    StrokeThickness = SimplexPartThickness
                };
                targetCanvas.Children.Add(simplexPartView);
                Panel.SetZIndex(simplexPartView, SimplexPartZIndex);
            }
            
            var simplexCenterView = new Ellipse
            {
                Fill = Brushes.Blue,
                Height = SimplexCenterRadius * 2,
                Width = SimplexCenterRadius * 2
            };
            targetCanvas.Children.Add(simplexCenterView);
            Canvas.SetLeft(simplexCenterView, simplexCenterPoint.X - SimplexCenterRadius);
            Canvas.SetTop(simplexCenterView, simplexCenterPoint.Y - SimplexCenterRadius);
            Panel.SetZIndex(simplexCenterView, SimplexCenterZIndex);
        }
        
        Point CalculateRectanglePoint(
            Point circleCenterPoint,
            double rectangleHeight,
            double rectangleWidth,
            double rectanglePerimeterPart)
        {
            double x, y;
        
            if (rectanglePerimeterPart <= rectangleWidth)
            {
                x = circleCenterPoint.X - rectangleWidth / 2 + rectanglePerimeterPart;
                y = circleCenterPoint.Y - rectangleHeight / 2;
            }
            else if (rectanglePerimeterPart <= rectangleWidth + rectangleHeight)
            {
                x = circleCenterPoint.X + rectangleWidth / 2;
                y = circleCenterPoint.Y - rectangleHeight / 2 + (rectanglePerimeterPart - rectangleWidth);
            }
            else if (rectanglePerimeterPart <= 2 * rectangleWidth + rectangleHeight)
            {
                x = circleCenterPoint.X + rectangleWidth / 2 - (rectanglePerimeterPart - (rectangleWidth + rectangleHeight));
                y = circleCenterPoint.Y + rectangleHeight / 2;
            }
            else
            {
                x = circleCenterPoint.X - rectangleWidth / 2;
                y = circleCenterPoint.Y + rectangleHeight / 2 - (rectanglePerimeterPart - (2 * rectangleWidth + rectangleHeight));
            }

            return new Point(x, y);
        }
    }
    
    #endregion
    
}