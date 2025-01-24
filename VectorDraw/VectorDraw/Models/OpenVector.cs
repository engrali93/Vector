using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VectorDraw.Models
{
   public class OpenVector : Shape
   {
      public List<string> Points { get; set; } // List of points in "x;y" format

      public OpenVector()
      {
         Points = new List<string>();
      }
   }
}
