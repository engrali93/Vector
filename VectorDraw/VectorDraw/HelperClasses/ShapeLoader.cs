using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using VectorDraw.Models;


namespace VectorDraw.HelperClasses
{
   public static class ShapeLoader
   {
      public static List<Shape> LoadShapesFromJson(string jsonData)
      {
         // Deserialize JSON data to a list of shapes
         return JsonConvert.DeserializeObject<List<Shape>>(jsonData, new ShapeConverter());
      }
   }
}
