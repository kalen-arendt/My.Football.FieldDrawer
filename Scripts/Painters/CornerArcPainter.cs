using My.Football.Fields.Setup;
using My.Unity.Creational;

using UnityEngine;

namespace My.Football.Fields.Painters
{
   internal class CornerArcPainter : AbstractFieldMarkingsPainter
   {
      public CornerArcPainter (FieldSetup fieldSetup) : base(fieldSetup)
      {
      }

      public void Paint (Transform parent)
      {
         int[] scales = {-1, +1};

         parent = TransformFactory.CreateChild("Corner Arcs", parent);

         foreach (var scaleX in scales)
         {
            foreach (var scaleY in scales)
            {
               GameObjectBuilder builder = new GameObjectBuilder("Corner").SetParent(parent);

               DrawCornerArc(builder.Build().transform);

               builder.SetLocalPosition(new Vector2(fieldSetup.HalfFieldWidth * -scaleX, fieldSetup.HalfFieldLength * -scaleY))
                      .SetLocalScale(new Vector2(scaleX, scaleY));
            }
         }
      }

      private void DrawCornerArc (Transform parent)
      {
         SpriteRenderer cornerArc = CreateLocalSprite(
            parent: parent,
            name: "Corner Arc",
            position: Vector2.zero,
            sprite: fieldSetup.Components.CornerArc,
            size: 1
         );

         cornerArc.flipY = true;

         SpriteRenderer restrainingArc = CreateLocalSprite(
            parent: parent,
            name: "Corner Arc",
            position: Vector2.zero,
            sprite: fieldSetup.Components.CornerRestrainingArc,
            size: 0.5f,
            drawMode: SpriteDrawMode.Tiled
         );

         restrainingArc.transform.localScale = new Vector2(fieldSetup.Category.CenterCircleDiameter, fieldSetup.Category.CenterCircleDiameter);

         restrainingArc.flipX = true;
         restrainingArc.flipY = true;
      }
   }
}