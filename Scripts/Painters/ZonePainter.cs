using My.Football.Fields.Setup;
using My.Unity.Creational;

using UnityEngine;

namespace My.Football.Fields.Painters
{
   internal class ZonePainter
   {
      private readonly FieldSetup fieldSetup;

      public ZonePainter (FieldSetup fieldSetup)
      {
         this.fieldSetup = fieldSetup;
      }

      public void Paint (Transform parent)
      {
         parent = TransformFactory.CreateChild("Zones", parent);

         DrawXZones(fieldSetup, TransformFactory.CreateChild("X-Zones", parent));
         DrawYZones(fieldSetup, TransformFactory.CreateChild("Y-Zones", parent));
      }

      private void DrawXZones (FieldSetup fieldSetup, Transform parent)
      {
         var zoneWidths = fieldSetup.ZoneModel.HorizontalZoneWidths;
         float x = -fieldSetup.HalfFieldWidth;

         for (var i = 0; i < zoneWidths.Length; i++)
         {
            SpriteRendererFactory.CreateLocal(
               parent: parent,
               name: "XZone " + (i + 1),
               position: new Vector2(x + (zoneWidths[i] / 2f), 0),
               sprite: fieldSetup.Components.Square,
               color: fieldSetup.FieldStyle.WZoneColors[i % fieldSetup.FieldStyle.WZoneColors.Length],
               size: new Vector2(zoneWidths[i], fieldSetup.FieldLength),
               layerName: fieldSetup.LayersAndSortingOrder.SortingLayer,
               sortOrder: fieldSetup.LayersAndSortingOrder.GrassOrderInLayer + 2,
               drawMode: SpriteDrawMode.Sliced
            );

            x += zoneWidths[i];
         }
      }

      private void DrawYZones (FieldSetup fieldSetup, Transform parent)
      {
         var zoneDepth = fieldSetup.ZoneModel.VerticalZoneDepths;
         float y = -fieldSetup.HalfFieldLength;

         for (var i = 0; i < zoneDepth.Length; i++)
         {
            SpriteRendererFactory.CreateLocal(
               parent: parent,
               name: "YZone " + (i + 1),
               position: new Vector2(0, y + (zoneDepth[i] / 2f)),
               sprite: fieldSetup.Components.Square,
               color: fieldSetup.FieldStyle.DZoneColors[i % fieldSetup.FieldStyle.DZoneColors.Length],
               size: new Vector2(fieldSetup.FieldWidth, zoneDepth[i]),
               layerName: fieldSetup.LayersAndSortingOrder.SortingLayer,
               sortOrder: fieldSetup.LayersAndSortingOrder.GrassOrderInLayer + 1,
               drawMode: SpriteDrawMode.Sliced
            );

            y += zoneDepth[i];
         }
      }
   }
}