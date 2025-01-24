using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace VectorDraw.Models
{
   public abstract class Shape
   {
      public string Type { get; set; }
      public string Color { get; set; }
      public bool? Filled { get; set; }
     

      // Converts ARGB string to a Brush
      public Brush GetFillBrush()
      {
         // Split the color string by ';'
         var colorParts = Color.Split(';');
         if (colorParts.Length == 4)
         {
            byte a = byte.Parse(colorParts[0].Trim());
            byte r = byte.Parse(colorParts[1].Trim());
            byte g = byte.Parse(colorParts[2].Trim());
            byte b = byte.Parse(colorParts[3].Trim());

            // Create a Color object using ARGB values (in .NET 4.7 use constructor)
            Color color = new Color
            {
               A = a,  // Alpha
               R = r,  // Red
               G = g,  // Green
               B = b   // Blue
            };

            // Return a SolidColorBrush with the parsed color
            return new SolidColorBrush(color);
         }

         return Brushes.Transparent;  // Fallback if color parsing fails
      }
   }

  
}
