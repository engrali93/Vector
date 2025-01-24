
using CommunityToolkit.Mvvm.Input;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using VectorDraw.Models;
using VectorDraw.ViewModels;

namespace VectorDraw.Views
{

   public partial class MainWindowView : Window
   {
      
      public MainWindowView()
      {
         InitializeComponent();

         DataContext = new MainWindowViewModel();


         // Set DataContext to MainWindowViewModel
         MainWindowViewModel viewModel = new MainWindowViewModel();
         this.DataContext = viewModel;

      }


      private bool isDragging = false;
      private Point initialMousePosition;

      private void Canvas_SizeChanged(object sender, SizeChangedEventArgs e)
      {
         if (DataContext is MainWindowViewModel viewModel)
         {
            viewModel.CanvasWidth = e.NewSize.Width;
            viewModel.CanvasHeight = e.NewSize.Height;
         }
      }

      private void Canvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
      {
         if (sender is UIElement canvas)
         {
            isDragging = true;
            Mouse.OverrideCursor = Cursors.Hand;
            canvas.CaptureMouse();

            // Store the mouse position when the drag starts
            initialMousePosition = e.GetPosition(canvas);
         }
      }

      private void Canvas_MouseMove(object sender, MouseEventArgs e)
      {
         if (isDragging && sender is UIElement canvas)
         {
            // Get the current mouse position
            Point currentMousePosition = e.GetPosition(canvas);

            // Calculate the drag distance
            double deltaX = currentMousePosition.X - initialMousePosition.X;
            double deltaY = currentMousePosition.Y - initialMousePosition.Y;

            // Update the offsets in the ViewModel
            var viewModel = DataContext as MainWindowViewModel;
            if (viewModel != null)
            {
               viewModel.OffsetX += deltaX;
               viewModel.OffsetY += deltaY;

               // Update the initial mouse position for the next movement
               initialMousePosition = currentMousePosition;
            }
         }
      }

      private void Canvas_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
      {
         isDragging = false;
         Mouse.OverrideCursor = null;
         (sender as UIElement)?.ReleaseMouseCapture();
      }




   }
}