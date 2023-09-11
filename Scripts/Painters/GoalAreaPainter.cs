using My.Football.Fields.Setup;
using My.Unity.Creational;

using UnityEngine;

namespace My.Football.Fields.Painters
{
   internal class GoalAreaPainter : AbstractFieldMarkingsPainter
   {
      public GoalAreaPainter (FieldSetup fieldSetup) : base(fieldSetup)
      {
      }

      public void Paint (Transform parent)
      {
         DrawGoal(parent);
         DrawGoalArea(parent);
      }

      private void DrawGoal (Transform parent)
      {
         parent = TransformFactory.CreateChild("Goal", parent);

         var halfLineWidth = fieldSetup.Category.LineWidth / 2;
         var halfGoalWidth = (fieldSetup.Category.GoalWidth / 2) + halfLineWidth;

         var baseY = fieldSetup.Category.LineWidth;
         var depthY = -fieldSetup.Category.GoalDepth - halfLineWidth;

         DrawGoalOutline(parent, halfGoalWidth, baseY, depthY);
         DrawGoalNetting(parent);
      }

      private void DrawGoalOutline (Transform parent, float halfGoalWidth, float goallineY, float goalDepthY)
      {
         CreateLine(
            parent: parent,
            name: "Goal Outline",
            points: new Vector3[4] {
               new Vector2(-halfGoalWidth, goallineY),
               new Vector2(-halfGoalWidth, goalDepthY),
               new Vector2(+halfGoalWidth, goalDepthY),
               new Vector2(+halfGoalWidth, goallineY)
            },
            loop: false
         );
      }

      private void DrawGoalNetting (Transform parent)
      {
         SpriteRendererFactory.CreateLocal(
            parent,
            "Goal Netting",
            new Vector2(0, -fieldSetup.Category.GoalDepth / 2),
            fieldSetup.Components.GrassSquare,
            fieldSetup.FieldStyle.NetColor,
            new Vector2(fieldSetup.Category.GoalWidth, fieldSetup.Category.GoalDepth),
            fieldSetup.LayersAndSortingOrder.SortingLayer,
            fieldSetup.LayersAndSortingOrder.LinesOrderInLayer - 1
         );
      }

      private void DrawGoalArea (Transform parent)
      {
         DrawBox(
            parent,
            "Goal Area",
            0,
            fieldSetup.Category.HalfGoalWidth,
            fieldSetup.Category.GoalAreaSize,
            fieldSetup.Category.HalfLineWidth);
      }
   }
}