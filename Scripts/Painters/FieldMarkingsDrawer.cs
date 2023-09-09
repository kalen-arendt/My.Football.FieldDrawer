using My.Football.Fields.Setup;
using My.Unity.Creational;

using UnityEngine;

namespace My.Football.Fields.Painters
{
   internal class FieldMarkingsDrawer
   {
      private readonly FieldSetup fieldSetup;

      public FieldMarkingsDrawer (FieldSetup fieldSetup)
      {
         this.fieldSetup = fieldSetup;
      }

      public void Paint (Transform parent)
      {
         parent = TransformFactory.CreateChild("Field Markings", parent);

         new FieldOutlinePainter(fieldSetup).Paint(parent);

         DrawHalfFieldMarkings(parent, -1);
         DrawHalfFieldMarkings(parent, +1);

         new CornerArcPainter(fieldSetup).Paint(parent);
      }

      private void DrawHalfFieldMarkings (Transform parent, int scale)
      {
         GameObjectBuilder goBuilder = new GameObjectBuilder("Half Field Markings").SetParent(parent);

         parent = goBuilder.Build().transform;

         new PenaltyAreaPainter(fieldSetup).Paint(parent);
         new GoalAreaPainter(fieldSetup).Paint(parent);

         var goalLineY = -fieldSetup.HalfFieldLength * scale;

         goBuilder.SetLocalPosition(new Vector2(0, goalLineY))
                  .SetLocalScale(new Vector2(1, scale));
      }
   }
}