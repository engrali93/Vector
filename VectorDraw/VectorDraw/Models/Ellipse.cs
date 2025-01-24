using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VectorDraw.Models
{
   public class Ellipse : Shape
   {
      public string Center { get; set; } // Center of the ellipse in "x;y" format
      public double Radius { get; set; } // Radius of the ellipse
   }
}
