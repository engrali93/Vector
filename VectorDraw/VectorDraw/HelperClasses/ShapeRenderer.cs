using System;
using System.Collections.Generic;
using System.Windows.Media;
using VectorDraw.Models;


namespace VectorDraw.HelperClasses
{
   public static class ShapeRenderer
   {
      // Dynamic renderer for shapes
      public static PathGeometry RenderShape(Shape shape)
      {
         switch (shape)
         {
            case OpenVector openVector:
               return OpenVectorGeometry(openVector);
            case CloseVector closeVector:
               return CloseVectorGeometry(closeVector);
            case Ellipse ellipse:
               return EllipseGeometry(ellipse);
            default:
               throw new ArgumentException($"Unsupported shape type: {shape.GetType().Name}");
         }
      }

      // Helper to parse points from "x;y" format
      private static System.Windows.Point ParsePoint(string pointStr)
      {
         var coordinates = pointStr.Split(';');
         if (coordinates.Length == 2)
         {
            return new System.Windows.Point(double.Parse(coordinates[0]), double.Parse(coordinates[1]));
         }
         throw new ArgumentException($"Invalid point format: {pointStr}");
      }

      // OpenVector rendering logic
      private static PathGeometry OpenVectorGeometry(OpenVector openVector)
      {
         var points = GetParsedPoints(openVector.Points);

         if (points.Count < 2)
            throw new ArgumentException("OpenVector requires at least two points.");

         var geometry = new PathGeometry();
         var figure = new PathFigure
         {
            StartPoint = points[0],
            IsClosed = false
         };

         for (int i = 1; i < points.Count; i++)
         {
            figure.Segments.Add(new LineSegment(points[i], true));
         }

         geometry.Figures.Add(figure);
         return geometry;
      }

      // CloseVector rendering logic
      private static PathGeometry CloseVectorGeometry(CloseVector closeVector)
      {
         var points = GetParsedPoints(closeVector.Points);

         if (points.Count < 3)
            throw new ArgumentException("CloseVector requires at least three points.");

         var geometry = new PathGeometry();
         var figure = new PathFigure
         {
            StartPoint = points[0],
            IsClosed = true
         };

         figure.Segments.Add(new PolyLineSegment(points, true));
         geometry.Figures.Add(figure);
         return geometry;
      }

      // Ellipse rendering logic
      private static PathGeometry EllipseGeometry(Ellipse ellipse)
      {
         var center = ParsePoint(ellipse.Center);
         double radius = ellipse.Radius;

         if (radius <= 0)
            throw new ArgumentException("Ellipse radius must be greater than zero.");

         var geometry = new PathGeometry();
         var figure = new PathFigure
         {
            StartPoint = new System.Windows.Point(center.X + radius, center.Y), // Start at the rightmost point
            IsClosed = true
         };

         // Define the elliptical arc
         figure.Segments.Add(new ArcSegment
         {
            Point = new System.Windows.Point(center.X - radius, center.Y), // Leftmost point
            Size = new System.Windows.Size(radius, radius),
            SweepDirection = SweepDirection.Clockwise,
            IsLargeArc = false
         });

         // Complete the ellipse
         figure.Segments.Add(new ArcSegment
         {
            Point = figure.StartPoint, // Back to starting point
            Size = new System.Windows.Size(radius, radius),
            SweepDirection = SweepDirection.Clockwise,
            IsLargeArc = false
         });

         geometry.Figures.Add(figure);
         return geometry;
      }

      // Parse points from string list
      private static List<System.Windows.Point> GetParsedPoints(IEnumerable<string> points)
      {
         var parsedPoints = new List<System.Windows.Point>();
         foreach (var point in points)
         {
            parsedPoints.Add(ParsePoint(point));
         }
         return parsedPoints;
      }
   }
}
