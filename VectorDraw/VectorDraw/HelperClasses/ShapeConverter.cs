using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using VectorDraw.Models;
using System.Windows;



namespace VectorDraw.HelperClasses
{
   public class ShapeConverter : JsonConverter
   {
      public override bool CanConvert(Type objectType)
      {
         return objectType == typeof(Shape);
      }

      public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
      {
         JObject obj = JObject.Load(reader);
         Shape shape = null;

         // Determine the vector type based on properties
         bool hasFilled = obj.Properties().Any(p => string.Equals(p.Name, "filled", StringComparison.OrdinalIgnoreCase));
         bool hasRadius = obj.Properties().Any(p => string.Equals(p.Name, "radius", StringComparison.OrdinalIgnoreCase));

         List<string> pointKeys = obj.Properties()
                .Where(p => IsPointKey(p.Name))
                .Select(p => p.Name)
                .ToList();

         if (hasRadius)
         {
            // Deserialize as an EllipseVector
            shape = obj.ToObject<Ellipse>(serializer);
         }
         else if (hasFilled)
         {
            // Deserialize as a CloseVector
            shape = obj.ToObject<CloseVector>(serializer);
            
         }
         else if (pointKeys.Count > 1)
         {
            // Deserialize as an OpenVector
            shape = obj.ToObject<OpenVector>(serializer);
         }
         else
         {
            throw new JsonSerializationException("Unable to determine shape type from JSON.");
         }

         // Populate the Points property for OpenVector (List<string>)
         if (shape is OpenVector openVector)
         {
            openVector.Points = pointKeys
                .Select(key =>
                {
               // Convert the Point to a string in "x;y" format
               var pointString = obj[key]?.ToString();

               // Manually parse the "x;y" string into a Point structure
               if (!string.IsNullOrEmpty(pointString))
                   {
                      var coordinates = pointString.Split(';');
                      if (coordinates.Length == 2)
                      {
                     // Replace commas with periods for proper decimal parsing
                     string xStr = coordinates[0].Replace(',', '.').Trim();
                         string yStr = coordinates[1].Replace(',', '.').Trim();

                         if (double.TryParse(xStr, out double x) && double.TryParse(yStr, out double y))
                         {
                            var point = new Point(x, y);
                            return $"{point.X};{point.Y}";
                         }
                      }
                   }
                   return null;
                })
                .Where(point => point != null) // Exclude null points
                .ToList();
         }
         else if (shape is CloseVector closeVector)
         {
            // Same process for CloseVector (List<string>)
            closeVector.Points = pointKeys
                .Select(key =>
                {
                   var pointString = obj[key]?.ToString();

               // Manually parse the "x;y" string into a Point structure
               if (!string.IsNullOrEmpty(pointString))
                   {
                      var coordinates = pointString.Split(';');
                      if (coordinates.Length == 2)
                      {
                     // Replace commas with periods for proper decimal parsing
                     string xStr = coordinates[0].Replace(',', '.').Trim();
                         string yStr = coordinates[1].Replace(',', '.').Trim();

                         if (double.TryParse(xStr, out double x) && double.TryParse(yStr, out double y))
                         {
                            var point = new Point(x, y);
                            return $"{point.X};{point.Y}";
                         }
                      }
                   }
                   return null;
                })
                .Where(point => point != null) // Exclude null points
                .ToList();
         }

         return shape;
      }

      public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
      {
         // Serialize the shape back to JSON if needed
         serializer.Serialize(writer, value);
      }
      // Helper method to identify point keys
      private static bool IsPointKey(string propertyName)
      {
         // Check if the property name is a single letter from "a" to "z" or "A" to "Z"
         return propertyName.Length == 1 && char.IsLetter(propertyName[0]);
      }
   }
}
