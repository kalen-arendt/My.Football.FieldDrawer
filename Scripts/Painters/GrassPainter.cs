using My.Football.Fields.Setup;
using My.Unity.Creational;

using UnityEngine;

namespace My.Football.Fields.Painters
{
   internal class GrassPainter
   {
      private readonly FieldSetup fieldSetup;

      public GrassPainter (FieldSetup fieldSetup)
      {
         this.fieldSetup = fieldSetup;
      }

      public void Paint (Transform parent)
      {
         SpriteRendererFactory.CreateLocal(
            parent,
            "Background Grass",
            Vector2.zero,
            fieldSetup.Components.BackgroundGrass,
            fieldSetup.FieldStyle.BackgroundGrassColor,
            new Vector2(
               fieldSetup.FieldWidth + (fieldSetup.Category.OuterGrassWidth * 2),
               fieldSetup.FieldLength + (fieldSetup.Category.OuterGrassWidth * 2)),
            fieldSetup.LayersAndSortingOrder.SortingLayer,
            fieldSetup.LayersAndSortingOrder.GrassOrderInLayer - 1
         );

         SpriteRendererFactory.CreateLocal(
            parent,
            "Field Grass",
            Vector2.zero,
            fieldSetup.Components.GrassSquare,
            fieldSetup.FieldStyle.GrassColor,
            new Vector2(fieldSetup.FieldWidth, fieldSetup.FieldLength),
            fieldSetup.LayersAndSortingOrder.SortingLayer,
            fieldSetup.LayersAndSortingOrder.GrassOrderInLayer
         );
      }
   }
}