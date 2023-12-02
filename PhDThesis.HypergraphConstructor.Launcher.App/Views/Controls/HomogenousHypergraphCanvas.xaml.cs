using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

using PhDThesis.Math.Domain;

namespace PhDThesis.HypergraphConstructor.Launcher.App.Views.Controls;

/// <summary>
/// 
/// </summary>
public partial class HomogenousHypergraphCanvas:
    UserControl
{
    
    // TODO: move to dependency properties
    // TODO: add colors dependency properties
    private const float VertexRadius = 3;
    private const float SimplexPartThickness = 0.5f;
    private const float SimplexCenterRadius = 1.5f;
    private const int VertexZIndex = 2;
    private const int SimplexPartZIndex = 0;
    private const int SimplexCenterZIndex = 1;
    
    private HomogenousHypergraph _currentHypergraphModel;
    //private Ellipse[] _currentVertices;
    //private Line[] _currentSimplicesParts;
    //private Ellipse[] _currentSimplicesCenters;

    /// <summary>
    /// 
    /// </summary>
    public HomogenousHypergraphCanvas()
    {
        InitializeComponent();
        SizeChanged += (sender, args) =>
        {
            ReconstructHypergraph();
        };
    }
    
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
    public static readonly DependencyProperty HomogenousHypergraphToShowProperty = DependencyProperty.Register(
        nameof(HomogenousHypergraphToShow),
        typeof(HomogenousHypergraph),
        typeof(HomogenousHypergraphCanvas),
        new FrameworkPropertyMetadata((d, e) =>
        {
            if (!(d is HomogenousHypergraphCanvas homogenousHypergraphCanvas))
            {
                throw new ArgumentException($"Parameter \"{d}\" should be of type \"{typeof(HomogenousHypergraphCanvas).FullName}\"",
                    nameof(d));
            }

            homogenousHypergraphCanvas._currentHypergraphModel = (HomogenousHypergraph)e.NewValue;
            homogenousHypergraphCanvas.ReconstructHypergraph();
        }));
    
    /// <summary>
    /// 
    /// </summary>
    private void ReconstructHypergraph()
    {
        var targetCanvas = _componentsCanvas;
        targetCanvas.Children.Clear();
        
        if (_currentHypergraphModel is null)
        {
            return;
        }
        
        var targetCanvasHeight = targetCanvas.ActualHeight;
        var targetCanvasWidth = targetCanvas.ActualWidth;
        var targetCanvasCenterPoint = new Point(targetCanvasWidth / 2, targetCanvasHeight / 2);

        var vertices = new (Ellipse, Point)[_currentHypergraphModel.VerticesCount];
        var halfSide = (int)(0.375 * System.Math.Min(targetCanvasWidth, targetCanvasHeight));
        var startPoint = targetCanvasCenterPoint with { X = targetCanvasCenterPoint.X + halfSide };
        var fullWay = halfSide * 8;
        var fullWayPartLength = fullWay / _currentHypergraphModel.VerticesCount;
        
        var radius = (int)(0.375 * System.Math.Min(targetCanvasWidth, targetCanvasHeight));
        for (var i = 0; i < _currentHypergraphModel.VerticesCount; i++)
        {
            var vertexPoint = targetCanvasCenterPoint with { X = targetCanvasCenterPoint.X + radius };
            var angle = 360f / _currentHypergraphModel.VerticesCount * i;
            var rotateMatrix = new Matrix();
            rotateMatrix.RotateAt(angle, targetCanvasCenterPoint.X, targetCanvasCenterPoint.Y);
            var targetVertexPoint = rotateMatrix.Transform(vertexPoint);
            
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
        
        for (var i = 0; i < _currentHypergraphModel.SimplicesMaxCount; i++)
        {
            if (!_currentHypergraphModel.ContainsSimplex(i))
            {
                continue;
            }
            
            var simplex = _currentHypergraphModel.BitIndexToSimplex(i);
            
            var verticesIds = simplex.ToArray();
            var simplexCenterPoint = new Point(
                verticesIds.Sum(id => vertices[id].Item2.X) / _currentHypergraphModel.SimplicesDimension,
                verticesIds.Sum(id => vertices[id].Item2.Y) / _currentHypergraphModel.SimplicesDimension);
            for (var j = 0; j < _currentHypergraphModel.SimplicesDimension; j++)
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
    }
    
}