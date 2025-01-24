using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VectorDraw.Models
{
   public class CloseVector : Shape
   {
      public List<string> Points { get; set; } // List of points in "x;y" format

      public CloseVector()
      {
         Points = new List<string>();
      }
   }
}
