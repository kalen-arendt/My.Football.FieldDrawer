using My.Football.Fields.Setup;
using My.Unity.Creational;

using UnityEngine;

namespace My.Football.Fields.Painters
{
   internal abstract class AbstractFieldMarkingsPainter
   {
      protected readonly FieldSetup fieldSetup;

      public AbstractFieldMarkingsPainter (FieldSetup fieldSetup)
      {
         this.fieldSetup = fieldSetup;
      }

      protected void CreateLine (Transform parent, string name, Vector3[] points, bool loop)
      {
         LineRendererFactory.CreateLocal(
            parent,
            name,
            fieldSetup.Components.LineMaterial,
            fieldSetup.Category.LineWidth,
            fieldSetup.FieldStyle.LineColor,
            points,
            loop,
            fieldSetup.LayersAndSortingOrder.SortingLayer,
            fieldSetup.LayersAndSortingOrder.LinesOrderInLayer
         );
      }

      protected void DrawBox (
         Transform parent,
         string name,
         float baseY,
         float halfGoalWidth,
         float boxSize,
         float halfLineWidth)
      {
         var halfWidth = halfGoalWidth + boxSize - halfLineWidth;
         var topY = baseY + boxSize - halfLineWidth;

         CreateLine(
            parent: parent,
            name: name,
            points: new Vector3[4] {
               new Vector2(-halfWidth, baseY),
               new Vector2(-halfWidth, topY),
               new Vector2(+halfWidth, topY),
               new Vector2(+halfWidth, baseY)
            },
            loop: false
         );
      }

      protected SpriteRenderer CreateLocalSprite (
         Transform parent,
         string name,
         Vector2 position,
         Sprite sprite,
         float size,
         SpriteDrawMode drawMode = SpriteDrawMode.Sliced)
      {
         return SpriteRendererFactory.CreateLocal(
            parent,
            name,
            position,
            sprite,
            fieldSetup.FieldStyle.LineColor,
            new Vector2(size, size),
            fieldSetup.LayersAndSortingOrder.SortingLayer,
            fieldSetup.LayersAndSortingOrder.LinesOrderInLayer,
            drawMode
         );
      }

      protected SpriteRenderer CreateLocalSprite (
         Transform parent,
         string name,
         Vector2 position,
         Sprite sprite,
         Vector2 size,
         SpriteDrawMode drawMode = SpriteDrawMode.Sliced)
      {
         return SpriteRendererFactory.CreateLocal(
            parent,
            name,
            position,
            sprite,
            fieldSetup.FieldStyle.LineColor,
            size,
            fieldSetup.LayersAndSortingOrder.SortingLayer,
            fieldSetup.LayersAndSortingOrder.LinesOrderInLayer,
            drawMode
         );
      }
   }
}