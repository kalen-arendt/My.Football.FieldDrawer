using My.Football.Fields.Setup;

using UnityEngine;

namespace My.Football.Fields.Painters
{
   internal class FieldOutlinePainter : AbstractFieldMarkingsPainter
   {
      public FieldOutlinePainter (FieldSetup fieldSetup) : base(fieldSetup)
      {
      }


      public void Paint (Transform parent)
      {
         DrawOutline(parent);
         DrawHalfLine(parent);
         DrawCenter(parent);
      }

      private void DrawOutline (Transform parent)
      {
         var halfLineWidth = fieldSetup.Category.LineWidth / 2;
         var x = fieldSetup.HalfFieldWidth - halfLineWidth;
         var y = fieldSetup.HalfFieldLength - halfLineWidth;

         CreateLine(
            parent,
            "Outline",
            new Vector3[4] {
               new Vector2(-x, -y),
               new Vector2(+x, -y),
               new Vector2(+x, +y),
               new Vector2(-x, +y)
            },
            true
         );
      }

      private void DrawHalfLine (Transform parent)
      {
         CreateLine(
            parent,
            "Half Line",
            new Vector3[2] {
               new Vector2(-fieldSetup.HalfFieldWidth, 0),
               new Vector2(+fieldSetup.HalfFieldWidth, 0)
            },
            false
         );
      }

      private void DrawCenter (Transform parent)
      {
         CreateLocalSprite(
            parent,
            "Center Spot",
            Vector2.zero,
            fieldSetup.Components.Circle,
            fieldSetup.Category.CenterSpotDiameter,
            SpriteDrawMode.Simple
         );

         CreateLocalSprite(
            parent,
            "Center Circle",
            Vector2.zero,
            fieldSetup.Components.CenterCircle,
            fieldSetup.Category.CenterCircleDiameter
         );
      }
   }
}