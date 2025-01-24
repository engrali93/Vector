using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using VectorDraw.Models;
using VectorDraw.HelperClasses;
using Microsoft.Win32;

namespace VectorDraw.ViewModels
{
   public class MainWindowViewModel : ObservableObject
   {
      public ObservableCollection<System.Windows.UIElement> CanvasElements { get; set; }
     
      private List<Shape> shapes; // The list of shapes loaded from JSON
      //private double zoomLevel1 = 1.0;
      private Point lastMousePosition; // To track the last mouse position
      private bool isDragging = false; // Flag to indicate dragging state

      private double _zoomLevel = 1.0;
      public double zoomLevel
      {
         get => _zoomLevel;
         set
         {
            if (_zoomLevel != value)
            {
               _zoomLevel = value;
               OnPropertyChanged();              
            }
         }
      }
      private double _offsetX = 0;
      public double OffsetX
      {
         get => _offsetX;
         set
         {
            if (_offsetX != value)
            {
               _offsetX = value;
               OnPropertyChanged();
               if(shapes != null)
               UpdateCanvas(); // Re-render elements when offset changes
            }
         }
      }

      private double _offsetY = 0;
      public double OffsetY
      {
         get => _offsetY;
         set
         {
            if (_offsetY != value)
            {
               _offsetY = value;
               OnPropertyChanged();
               if (shapes != null)
                  UpdateCanvas(); // Re-render elements when offset changes
            }
         }
      }

      private double _canvasWidth = 100;
      public double CanvasWidth
      {
         get => _canvasWidth;
         set
         {
            if (_canvasWidth != value)
            {
               _canvasWidth = value;
               OnPropertyChanged();
               UpdateOffsets();
               UpdateCanvas();
            }
         }
      }

      private double _canvasHeight = 100;
      public double CanvasHeight
      {
         get => _canvasHeight;
         set
         {
            if (_canvasHeight != value)
            {
               _canvasHeight = value;
               OnPropertyChanged();
               UpdateOffsets();
               UpdateCanvas();
            }
         }
      }

      public ICommand ZoomInCommand { get; set; }
      public ICommand ZoomOutCommand { get; set; }
      public ICommand LoadJsonCommand { get; set; }  // Command to load the JSON file
      public ObservableCollection<Shape> ShapeList { get; set; }
      public MainWindowViewModel()
      {
         CanvasElements = new ObservableCollection<System.Windows.UIElement>();
         ShapeList = new ObservableCollection<Shape>(); // Initialize the shape list
         LoadJsonCommand = new RelayCommand(LoadJsonFile);  // Bind the command to the method
         //LoadShapes();
         UpdateOffsets();
         ZoomInCommand = new RelayCommand(ZoomIn);
         ZoomOutCommand = new RelayCommand(ZoomOut);
      }
      public void OnMouseDown(Point position)
      {
         isDragging = true;
         lastMousePosition = position;
      }

      public void OnMouseMove(Point position)
      {
         if (isDragging)
         {
            OffsetX += position.X - lastMousePosition.X;
            OffsetY += position.Y - lastMousePosition.Y;
            lastMousePosition = position;
         }
      }

      public void OnMouseUp()
      {
         isDragging = false;
      }
      private void LoadJsonFile()
      {
         // Open file dialog to select the JSON file
        
         OpenFileDialog openFileDialog = new OpenFileDialog();
         openFileDialog.Filter = "JSON Files (*.json)|*.json|All Files (*.*)|*.*";  // Filter for JSON files

         // Show the file dialog and check if the user selected a file
         if (openFileDialog.ShowDialog() == true)
         {
            string filePath = openFileDialog.FileName;

            // Read and deserialize the selected JSON file
            string jsonData = File.ReadAllText(filePath);

            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.Converters.Add(new ShapeConverter());
            shapes = JsonConvert.DeserializeObject<List<Shape>>(jsonData, settings);

            // Update ShapeList
            ShapeList.Clear();
            foreach (Shape shape in shapes)
            {
               ShapeList.Add(shape); // Add the actual shape object to the list
            }

            UpdateOffsets();
            UpdateCanvas();
         }
      }


      private Shape _selectedShape;
      public Shape SelectedShape
      {
         get => _selectedShape;
         set
         {
            if (_selectedShape != value)
            {
               _selectedShape = value;
               OnPropertyChanged();
               HighlightSelectedShape(); // Trigger highlighting when the selection changes
               SelectedShapeInfo = _selectedShape != null ?
                  GetShapeContent(_selectedShape)
                  : string.Empty; ;
            }
         }
      }
      private string _selectedShapeInfo;
      public string SelectedShapeInfo
      {
         get => _selectedShapeInfo;
         set
         {
            _selectedShapeInfo = value;
            OnPropertyChanged(nameof(SelectedShapeInfo));
         }
      }
      private void HighlightSelectedShape()
      {

         foreach (var element in CanvasElements)
         {
            if (element is System.Windows.Shapes.Path path)
            {
               var shape = shapes.FirstOrDefault(s => ShapeMatches(path, s)); // Check if this path corresponds to the shape
               if (shape != null)
               {
                  if (shape == SelectedShape)
                  {
                     // Highlight using the shape's fill color or stroke color
                     path.Stroke = shape.GetFillBrush() != Brushes.Transparent
                         ? shape.GetFillBrush()
                         : Brushes.Black; // Default to black if no fill
                     path.StrokeThickness = 3; // Thicker border for highlight

                   
                  }
                  else
                  {
                     // Reset to normal state
                     path.Stroke = shape.GetFillBrush() == Brushes.Transparent
                         ? Brushes.Black
                         : shape.GetFillBrush();
                     path.StrokeThickness = 1; // Normal thickness
                   
                  }
               }
            }
         }

      }

      // Utility method to match a Path with a Shape (this may need adjustments based on your ShapeRenderer logic)
      private bool ShapeMatches(System.Windows.Shapes.Path path, Shape shape)
      {
         return path.Data.ToString() == ShapeRenderer.RenderShape(shape).ToString();
      }

      private void UpdateOffsets()
      {
         if (shapes == null || shapes.Count == 0)
         {
            OffsetX = CanvasWidth / 2;
            OffsetY = CanvasHeight / 2;
            return;
         }

         double minX = double.MaxValue, minY = double.MaxValue;
         double maxX = double.MinValue, maxY = double.MinValue;

         foreach (Shape shape in shapes)
         {
            PathGeometry geometry = ShapeRenderer.RenderShape(shape);
            Rect bounds = geometry.Bounds;
            minX = Math.Min(minX, bounds.Left);
            minY = Math.Min(minY, bounds.Top);
            maxX = Math.Max(maxX, bounds.Right);
            maxY = Math.Max(maxY, bounds.Bottom);
         }

         double shapesCenterX = (maxX + minX) / 2;
         double shapesCenterY = (maxY + minY) / 2;

         OffsetX =  ((CanvasWidth / 2) - (shapesCenterX * zoomLevel));
         OffsetY =  ((CanvasHeight / 2) - (shapesCenterY * zoomLevel));
      }



      // Method to get shape data content
      private string GetShapeContent(Shape shape)
      {
         string points = "";
         if (shape is OpenVector openVector)
         {
            //points = openVector.Points. ToString();
            points = string.Join(" _ ", openVector.Points.Select((value, index) => $"Point {index + 1} = {value}"));
         }       
         else if (shape is CloseVector closeVector)
         {
            //points = openVector.Points. ToString();
            points = string.Join(" _ ", closeVector.Points.Select((value, index) => $"Point {index + 1} = {value}"));
         }
         else if (shape is Ellipse ellipse)
         {
            points = "Radius: "+ ellipse.Radius.ToString();
         }
         
         string color = shape.GetFillBrush().ToString();
        
         return $"{shape.GetType().Name} \n Color: {shape.Color} \n Points: {points}";// GetFillBrush()}";
      }



      // Zoom methods (same as before)
      private void ZoomIn()
      {
         zoomLevel = Math.Min(zoomLevel + 0.1, 15.0); // Limit zoom level
         UpdateOffsets();
      }

      private void ZoomOut()
      {
         zoomLevel = Math.Max(zoomLevel - 0.1, 1); // Limit zoom level
         UpdateOffsets();
      }


      // Update canvas
      private void UpdateCanvas()
      {
         CanvasElements.Clear();

         if(shapes != null)
         {
            foreach (Shape shape in shapes)
            {
               var path = new System.Windows.Shapes.Path
               {
                  Stroke = shape.GetFillBrush() == Brushes.Transparent ? Brushes.Black : shape.GetFillBrush(),
                  Fill = shape.Filled.HasValue && shape.Filled.Value ? shape.GetFillBrush() : Brushes.Transparent,
                  Data = ShapeRenderer.RenderShape(shape)
               };



               // Apply transforms for scaling and translation
               TranslateTransform translation = new TranslateTransform(OffsetX, OffsetY);
               ScaleTransform scaling = new ScaleTransform(zoomLevel, zoomLevel);
               TransformGroup transformGroup = new TransformGroup();
               transformGroup.Children.Add(scaling);
               transformGroup.Children.Add(translation);
               path.RenderTransform = transformGroup;

               // Assign shape as DataContext for easy reference
               path.DataContext = shape;

               // Add mouse events to highlight shape
               path.MouseEnter += (sender, e) =>
               {
                  path.Stroke = Brushes.Yellow; // Highlight stroke on hover
                  path.StrokeThickness = 3; // Optional: Increase thickness
               };
               path.MouseLeave += (sender, e) =>
               {
                  path.Stroke = shape.GetFillBrush() == Brushes.Transparent ? Brushes.Black : shape.GetFillBrush(); // Reset stroke
                  path.StrokeThickness = 1; // Optional: Reset thickness
               };

               CanvasElements.Add(path);
            }
         }


      }
   



   }

}
