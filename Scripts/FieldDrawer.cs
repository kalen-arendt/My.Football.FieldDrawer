using My.Football.Fields.Setup;
using My.Unity.Creational;
using My.Unity.Debugging;

using UnityEngine;

namespace My.Football.Fields
{
   /// <summary>
   /// Draw a Field based on a FieldConfig and FieldStyle
   /// </summary>
   [System.Serializable]
   public class FieldDrawer
   {
      [SerializeField] private FieldCategoryConfig fieldConfig;
      [SerializeField] private FieldZoneModel zoneModel;
      [SerializeField] private FieldComponents components;
      [SerializeField] private FieldStyle fieldStyle;
      [SerializeField] private FieldZoneStyle zoneStyle;

      [SerializeField] private Transform transform;

      private const string FIELD_SORTING_LAYER = "Field";
      private const int FIELD_LAYER = 8;

      private const float GRASS_OUTLINE_WIDTH = 10;

      private const int
         GRASS_ORDER = 0,
         LINES_ORDER = 10;

      private int
         width,
         length;

      public FieldDrawer (
         FieldZoneModel zoneModel,
         FieldComponents components,
         FieldStyle fieldStyle,
         FieldZoneStyle zoneStyle,
         Transform transform)
      {
         this.zoneModel = zoneModel.WarnIfNull();
         this.components = components.WarnIfNull();
         this.fieldStyle = fieldStyle.WarnIfNull();
         this.zoneStyle = zoneStyle.WarnIfNull();
         this.transform = transform.WarnIfNull();

         width = zoneModel.Width;
         length = zoneModel.Length;
      }

      public void Draw ()
      {
         if (zoneModel && fieldStyle && components && transform)
         {
            Erase();

            width = zoneModel.Width;
            length = zoneModel.Length;

            DrawGrass();
            DrawZones();
            DrawFieldMarkings();
            DrawZoneBoundryLines();

            transform.gameObject.layer = FIELD_LAYER;

            foreach (Transform item in transform.GetComponentsInChildren<Transform>())
            {
               item.gameObject.layer = FIELD_LAYER;
            }
         }
         else
         {
            Debug.LogWarning("Field Drawer is missing components to draw field.");
         }
      }

      public void Erase ()
      {
         while (transform.childCount > 0)
         {
            Object.DestroyImmediate(transform.GetChild(0).gameObject);
         }
      }


      #region Draw Grass

      private void DrawGrass ()
      {
         SpriteRendererFactory.CreateLocal(
            transform,
            "Background Grass",
            Vector2.zero,
            components.Square,
            fieldStyle.BackgroundGrassColor,
            new Vector2(width + (GRASS_OUTLINE_WIDTH * 2), length + (GRASS_OUTLINE_WIDTH * 2)),
            FIELD_SORTING_LAYER,
            GRASS_ORDER - 1
         );

         SpriteRendererFactory.CreateLocal(
            transform,
            "Field Grass",
            Vector2.zero,
            components.Square,
            fieldStyle.GrassColor,
            new Vector2(width, length),
            FIELD_SORTING_LAYER,
            GRASS_ORDER
         );
      }

      private void DrawZones ()
      {
         Transform parent = TransformFactory.CreateChild("Zones", transform);

         DrawXZones(TransformFactory.CreateChild("X-Zones", parent));
         DrawYZones(TransformFactory.CreateChild("Y-Zones", parent));
      }

      /// <summary>
      /// draw the zones determined by the x position
      /// </summary>
      /// <param name="parent">the parent Transform of the instantiated zones</param>
      private void DrawXZones (Transform parent)
      {
         var zoneWidths = zoneModel.HorizontalZoneWidths;
         float x = -(width / 2);

         for (var i = 0; i < zoneWidths.Length; i++)
         {
            SpriteRendererFactory.CreateLocal(
               parent: parent,
               name: "XZone " + (i + 1),
               position: new Vector2(x + (zoneWidths[i] / 2f), 0),
               sprite: components.Square,
               color: fieldStyle.WZoneColors[i % fieldStyle.WZoneColors.Length],
               size: new Vector2(zoneWidths[i], length),
               layerName: FIELD_SORTING_LAYER,
               sortOrder: GRASS_ORDER + 2,
               drawMode: SpriteDrawMode.Sliced
            );

            x += zoneWidths[i];
         }
      }

      /// <summary>
      /// draw the zones determined by the y position
      /// </summary>
      /// <param name="parent">the parent Transform of the instantiated zones</param>
      private void DrawYZones (Transform parent)
      {
         var zoneDepth = zoneModel.VerticalZoneDepths;
         float y = -(length / 2);

         for (var i = 0; i < zoneDepth.Length; i++)
         {
            SpriteRendererFactory.CreateLocal(
               parent: parent,
               name: "YZone " + (i + 1),
               position: new Vector2(0, y + (zoneDepth[i] / 2f)),
               sprite: components.Square,
               color: fieldStyle.DZoneColors[i % fieldStyle.DZoneColors.Length],
               size: new Vector2(width, zoneDepth[i]),
               layerName: FIELD_SORTING_LAYER,
               sortOrder: GRASS_ORDER + 1,
               drawMode: SpriteDrawMode.Sliced
            );

            y += zoneDepth[i];
         }
      }

      #endregion


      #region Draw Field Markings & Goals

      private void DrawFieldMarkings ()
      {
         Transform parent = TransformFactory.CreateChild("Field Markings", transform);

         DrawOutline(parent);
         DrawHalfLine(parent);
         DrawCenter(parent);
         
         DrawHalfFieldMarkings(parent, -1);
         DrawHalfFieldMarkings(parent, +1);

         DrawCornerArcs(parent);
      }

      private void DrawOutline (Transform parent)
      {
         float halfLineWidth = fieldConfig.HalfLineWidth;
         float x = (width / 2) - halfLineWidth;
         float y = (length / 2) - halfLineWidth;

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
               new Vector2(-(width/2), 0),
               new Vector2(+(width/2), 0)
            },
            false
         );
      }

      private void DrawCornerArcs (Transform parent)
      {
         // Corner Arcs
         //	=> (location):	(scale) -> (position)
         //		(0,0):		(+,+)	->	(-,-)
         //		(0,1):		(+,-)	->	(-,+)
         //		(1,0):		(-,+)	->	(+,-)
         //		(1,1):		(-,-)	->	(+,+)
         //	
         //	0 -> +scale, -position
         //	1 -> -scale, +position

         //	Scale: i*2 - 1
         //	 >	(0 -> +1)	::	-((0 * 2) - 1) = +1
         //	 >	(1 -> -1)	::	-((1 * 2) - 1) = -1

         //	Position: 
         //	 >	(0 -> 0 + 0)
         //	 >	(1 -> 0 + 1)

         int halfWidth = width / 2;
         int halfLength = length / 2;

         parent = TransformFactory.CreateChild("Corner Arcs", parent);

         for (var xi = 0; xi < 2; xi++)
         {
            for (var yj = 0; yj < 2; yj++)
            {
               var goBuilder = new GameObjectBuilder("Corner").SetParent(parent);

               DrawCornerArc(goBuilder.Build().transform);

               // position and scale specific to each corner.
               var scaleX = xi == 0 ? +1 : -1;
               var scaleY = yj == 0 ? +1 : -1;

               goBuilder
                  .SetLocalPosition(new Vector2(halfWidth * -scaleX, halfLength * -scaleY))
                  .SetLocalScale(new Vector2(scaleX, scaleY));
            }
         }
      } // end DrawCornerArcs()

      private void DrawCornerArc(Transform parent)
      {
         var cornerArc = CreateLocalSprite(
            parent: parent,
            name: "Corner Arc",
            position: Vector2.zero,
            sprite: components.CornerArc,
            size: 1
         );

         cornerArc.flipY = true;

         var restrainingArc = CreateLocalSprite(
            parent: parent,
            name: "Corner Arc",
            position: Vector2.zero,
            sprite: components.CornerRestrainingArc,
            size: 0.5f,
            drawMode: SpriteDrawMode.Tiled
         );

         restrainingArc.transform.localScale = new Vector2(fieldConfig.CenterCircleDiameter, fieldConfig.CenterCircleDiameter);
       
         restrainingArc.flipX = true;
         restrainingArc.flipY = true;
      }

      private void DrawCenter (Transform parent)
      {
         CreateLocalSprite(
            parent,
            "Center Spot",
            Vector2.zero,
            components.Circle,
            fieldConfig.CenterSpotDiameter,
            SpriteDrawMode.Simple
         );

         CreateLocalSprite(
            parent,
            "Center Circle",
            Vector2.zero,
            components.CenterCircle,
            fieldConfig.CenterCircleDiameter
         );
      }

      private void DrawHalfFieldMarkings (Transform parent, int scale)
      {
         var goalLineY = -(length / 2f) * scale;
         var halfGoalWidth = fieldConfig.GoalWidth / 2f;
         float halfLineWidth = fieldConfig.LineWidth / 2f;

         var goBuilder = new GameObjectBuilder("Half Field Markings").SetParent(parent);

         parent = goBuilder.Build().transform;

         // Goal Area
         DrawBox(
            parent,
            "Goal Area",
            0,
            halfGoalWidth,
            fieldConfig.GoalAreaSize,
            halfLineWidth);

         // Penalty Area
         DrawBox(
            parent,
            "Penalty Area",
            0,
            halfGoalWidth,
            fieldConfig.PenaltyBoxSize,
            halfLineWidth);

         // Penalty Spot
         CreateLocalSprite(
            parent,
            "Penalty Spot",
            new Vector2(0, fieldConfig.PenaltySpotDistance),
            components.Circle,
            fieldConfig.PenaltySpotDiameter,
            SpriteDrawMode.Simple);

         // Penalty Arc
         CreateLocalSprite(
            parent,
            "Penalty Arc",
            new Vector2(0, fieldConfig.PenaltyBoxSize),
            components.PenaltyArc,
            new Vector2(1, fieldConfig.PenaltyArcHeight / fieldConfig.CenterCircleDiameter), // components.CenterCircleDiameter,
            SpriteDrawMode.Tiled
         ).transform.localScale = new Vector2(fieldConfig.CenterCircleDiameter, -fieldConfig.CenterCircleDiameter);

         DrawGoal(parent);

         goBuilder.SetLocalPosition(new Vector2(0, goalLineY))
                  .SetLocalScale(new Vector2(1, scale));
      }

      private void DrawGoal (Transform parent)
      {
         parent = TransformFactory.CreateChild("Goal", parent);
         //new GameObjectBuilder("Goal").SetParent(parent);

         //parent = goBuilder.Build().transform;

         // fieldStyle.LineWidth

         var halfLineWidth = fieldConfig.HalfLineWidth;
         var halfGoalWidth = (fieldConfig.GoalWidth / 2) + halfLineWidth;

         var baseY = fieldConfig.LineWidth;
         var depthY = -fieldConfig.GoalDepth - halfLineWidth;

         // Goal Outline
         CreateLine(
            parent: parent,
            name: "Goal Outline",
            points: new Vector3[4] {
               new Vector2(-halfGoalWidth, baseY),
               new Vector2(-halfGoalWidth, depthY),
               new Vector2(+halfGoalWidth, depthY),
               new Vector2(+halfGoalWidth, baseY)
            },
            loop: false
         );

         // Netting
         SpriteRendererFactory.CreateLocal(
            parent,
            "Goal Netting",
            new Vector2(0, -fieldConfig.GoalDepth / 2),
            components.Square,
            fieldStyle.NetColor,
            new Vector2(fieldConfig.GoalWidth, fieldConfig.GoalDepth),
            FIELD_SORTING_LAYER,
            LINES_ORDER - 1
         );
      }

      private void DrawBox (Transform parent, string name, float baseY, float halfGoalWidth, float boxSize, float halfLineWidth)
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

      #endregion


      #region Draw Zone Boundry Lines

      private void DrawZoneBoundryLines ()
      {
         if (!zoneStyle)
         {
            return;
         }

         Transform header = TransformFactory.CreateChild("Zone Boundries", transform);
         Transform xBoundries = TransformFactory.CreateChild("X Boundries", header);
         Transform yBoundries = TransformFactory.CreateChild("Y Boundries", header);

         if (zoneStyle.BoarderWidth > 0)
         {
            DrawXZoneBoarders(xBoundries);
            DrawYZoneBoarders(yBoundries);
         }

         if (zoneStyle.DividerWidth > 0)
         {
            DrawXZoneDividers(xBoundries);
            DrawYZoneDividers(yBoundries);
         }
      }

      private void DrawXZoneBoarders (Transform parent)
      {
         var zoneWidths = zoneModel.HorizontalZoneWidths;
         float x = -(width / 2);
         float yBoarder = length / 2;
         float halfWidth = zoneStyle.BoarderWidth / 2;

         for (var i = 0; i < zoneWidths.Length - 1; i++)
         {
            // POSITION
            x += zoneWidths[i];
            var xPos = AlignPosition(x, halfWidth, true);

            LineRendererFactory.CreateLocal(
               parent,
               "Line - XBoarder - " + (i + 1),
               zoneStyle.LinesMaterial,
               zoneStyle.BoarderWidth,
               zoneStyle.BoarderColor,
               new Vector3[] {
                  new Vector2(xPos, -yBoarder),
                  new Vector2(xPos, +yBoarder)
               },
               false, FIELD_SORTING_LAYER,
               LINES_ORDER - 1
            );
         }
      }

      private void DrawYZoneBoarders (Transform parent)
      {
         var zoneWidths = zoneModel.VerticalZoneDepths;
         float
            y = -(length / 2f),
            xBoarder = width / 2f,
            halfWidth = zoneStyle.BoarderWidth / 2f;

         for (var i = 0; i < zoneWidths.Length - 1; i++)
         {
            // POSITION
            y += zoneWidths[i];
            var yPos = AlignPosition(y, halfWidth, false);

            LineRendererFactory.CreateLocal(
               parent,
               "Line - XBoarder - " + (i + 1),
               zoneStyle.LinesMaterial,
               zoneStyle.BoarderWidth,
               zoneStyle.BoarderColor,
               new Vector3[] {
                  new Vector2(-xBoarder, yPos),
                  new Vector2(+xBoarder, yPos)
               },
               false, FIELD_SORTING_LAYER,
               LINES_ORDER - 1
            );
         }
      }


      private void DrawXZoneDividers (Transform parent)
      {
         var zoneWidths = zoneModel.HorizontalZoneWidths;
         float
            x = -(width / 2f),
            yBoarder = length / 2f,
            halfWidth = zoneStyle.DividerWidth / 2f;

         for (var i = 0; i < zoneWidths.Length; i++)
         {
            // POSITION
            var halfZoneWidth = zoneWidths[i] / 2f;
            var xPos = AlignPosition(x + halfZoneWidth, halfWidth, true);

            LineRendererFactory.CreateLocal(
               parent,
               "Line - XDivider - " + (i + 1),
               zoneStyle.LinesMaterial,
               zoneStyle.DividerWidth,
               zoneStyle.DividerColor,
               new Vector3[] {
                  new Vector2(xPos, -yBoarder),
                  new Vector2(xPos, +yBoarder)
               },
               false, FIELD_SORTING_LAYER,
               LINES_ORDER - 1
            );

            x += zoneWidths[i];
         }
      }

      private void DrawYZoneDividers (Transform parent)
      {
         var zoneDepths = zoneModel.VerticalZoneDepths;
         float
            y = -(length / 2f),
            xBoarder = width / 2f,
            halfWidth = zoneStyle.BoarderWidth / 2f;

         for (var i = 0; i < zoneDepths.Length; i++)
         {
            // POSITION
            var halfZoneDepth = zoneDepths[i] / 2f;
            var yPos = AlignPosition(y + halfZoneDepth, halfWidth, false);

            LineRendererFactory.CreateLocal(
               parent,
               "Line - YDivider - " + (i + 1),
               zoneStyle.LinesMaterial,
               zoneStyle.DividerWidth,
               zoneStyle.DividerColor,
               new Vector3[] {
                  new Vector2(-xBoarder, yPos),
                  new Vector2(+xBoarder, yPos)
               },
               false, FIELD_SORTING_LAYER,
               LINES_ORDER - 1
            );
            y += zoneDepths[i];
         }
      }

      #endregion

      // ----------------------------------------------
      // Helper Functions
      // ----------------------------------------------

      /// <summary>
      /// Align the position of a line renderer point so the edge of the line is flush with z
      /// </summary>
      /// <param name="position">the variable to align</param>
      /// <param name="halfLineWidth">half the width of the line</param>
      /// <param name="alignTowardZero">inline towards zero? or away from zero?</param>
      /// <returns></returns>
      private static float AlignPosition (float position, float halfLineWidth, bool alignTowardZero = true)
      {
         float result = 0;

         if (position != 0)
         {
            result = ((position < 0) == alignTowardZero)
               ? position + halfLineWidth
               : position - halfLineWidth;
         }

         return result;
      }

      private SpriteRenderer CreateLocalSprite (Transform parent, string name, Vector2 position, Sprite sprite, float size, SpriteDrawMode drawMode = SpriteDrawMode.Sliced)
      {
         return SpriteRendererFactory.CreateLocal(
            parent,
            name,
            position,
            sprite,
            fieldStyle.LineColor,
            new Vector2(size, size),
            FIELD_SORTING_LAYER,
            LINES_ORDER,
            drawMode
         );
      }

      private SpriteRenderer CreateLocalSprite (Transform parent, string name, Vector2 position, Sprite sprite, Vector2 size, SpriteDrawMode drawMode = SpriteDrawMode.Sliced)
      {
         return SpriteRendererFactory.CreateLocal(
            parent,
            name,
            position,
            sprite,
            fieldStyle.LineColor,
            size,
            FIELD_SORTING_LAYER,
            LINES_ORDER,
            drawMode
         );
      }

      private void CreateLine (Transform parent, string name, Vector3[] points, bool loop)
      {
         LineRendererFactory.CreateLocal(
            parent,
            name,
            components.LineMaterial,
            fieldConfig.LineWidth,
            fieldStyle.LineColor,
            points,
            loop,
            FIELD_SORTING_LAYER,
            LINES_ORDER
         );
      }
   }
}