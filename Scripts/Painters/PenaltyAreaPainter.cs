using My.Football.Fields.Setup;

using UnityEngine;

namespace My.Football.Fields.Painters
{
   internal class PenaltyAreaPainter : AbstractFieldMarkingsPainter
   {
      public PenaltyAreaPainter (FieldSetup fieldSetup) : base(fieldSetup)
      {
      }

      public void Paint (Transform parent)
      {
         DrawPenaltyArea(parent);
         DrawPenaltySpot(parent);
         DrawPenaltyArc(parent);
      }

      private void DrawPenaltyArea (Transform parent)
      {
         DrawBox(
            parent,
            "Penalty Area",
            0,
            fieldSetup.Category.HalfGoalWidth,
            fieldSetup.Category.PenaltyBoxSize,
            fieldSetup.Category.HalfLineWidth);
      }

      private void DrawPenaltySpot (Transform parent)
      {
         CreateLocalSprite(
            parent,
            "Penalty Spot",
            new Vector2(0, fieldSetup.Category.PenaltySpotDistance),
            fieldSetup.Components.PenaltySpot,
            fieldSetup.Category.PenaltySpotDiameter
         );
      }

      private void DrawPenaltyArc (Transform parent)
      {
         CreateLocalSprite(
            parent,
            "Penalty Arc",
            new Vector2(0, fieldSetup.Category.PenaltyBoxSize),
            fieldSetup.Components.PenaltyArc,
            new Vector2(1, fieldSetup.Category.PenaltyArcHeight / fieldSetup.Category.CenterCircleDiameter),
            SpriteDrawMode.Tiled
         ).transform.localScale = new Vector2(fieldSetup.Category.CenterCircleDiameter, -fieldSetup.Category.CenterCircleDiameter);
      }
   }
}