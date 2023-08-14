using My.Unity.Creational;
using My.Unity.Debugging;

using UnityEngine;

namespace My.Football.Fields
{
   [System.Serializable]
   public class FieldConfig : IFieldConfig
   {
      [SerializeField] private FieldZoneModel zoneModel;
      [SerializeField] private FieldComponents components;
      [SerializeField] private FieldStyle fieldStyle;
      [SerializeField] private FieldZoneStyle zoneStyle;

      public FieldZoneModel ZoneModel => zoneModel;
      public FieldComponents Components => components;
      public FieldStyle FieldStyle => fieldStyle;
      public FieldZoneStyle ZoneStyle => zoneStyle;
   }
   
   public interface IFieldConfig
   {
      FieldZoneModel ZoneModel { get; }
      FieldComponents Components { get; }
      FieldStyle FieldStyle { get; }
      FieldZoneStyle ZoneStyle { get; }
   }

   /// <summary>
   /// Draw a Field based on a FieldConfig and FieldStyle
   /// </summary>
   [System.Serializable]
   public class FieldDrawer
   {
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

            // UNDONE
            //transform.localPosition = Vector2.zero;


            DrawGrass();
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
            components.Grass,
            fieldStyle.BackgroundGrassColor,
            new Vector2(width + (GRASS_OUTLINE_WIDTH * 2), length + (GRASS_OUTLINE_WIDTH * 2)),
            FIELD_SORTING_LAYER,
            GRASS_ORDER - 1
         );

         SpriteRendererFactory.CreateLocal(
            transform,
            "Grass",
            Vector2.zero,
            components.Grass,
            fieldStyle.GrassColor,
            new Vector2(width, length),
            FIELD_SORTING_LAYER,
            GRASS_ORDER
         );

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
               sprite: components.Grass,
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
               sprite: components.Grass,
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
         Transform linesParent = TransformFactory.CreateChild("Lines", transform);

         DrawOutline(linesParent);
         DrawHalfLine(linesParent);
         DrawCenter(linesParent);
         DrawCornerArcs(linesParent);
         DrawPenaltyAreas(linesParent);

         foreach (SpriteRenderer sprite in linesParent.GetComponentsInChildren<SpriteRenderer>())
         {
            sprite.sortingLayerName = FIELD_SORTING_LAYER;
            sprite.sortingOrder = LINES_ORDER;

            sprite.color = fieldStyle.LineColor;
         }

         foreach (LineRenderer line in linesParent.GetComponentsInChildren<LineRenderer>())
         {
            line.sortingLayerName = FIELD_SORTING_LAYER;
            line.sortingOrder = LINES_ORDER;

            line.startColor = line.endColor = fieldStyle.LineColor;
            line.startWidth = line.endWidth = fieldStyle.LineWidth;
         }

         DrawGoals();
      }

      private void DrawOutline (Transform parent)
      {
         float
            halfLineWidth = fieldStyle.LineWidth / 2,
            x = (width / 2) - halfLineWidth,
            y = (length / 2) - halfLineWidth;

         LineRenderer outline = GameObject.Instantiate(components.LineRenderer, parent);
         outline.name = "Outline";

         outline.loop = true;
         outline.positionCount = 4; //5 ;
         outline.SetPositions(
            new Vector3[4] {
               new Vector2(-x, -y),
               new Vector2(+x, -y),
               new Vector2(+x, +y),
               new Vector2(-x, +y)//,
											 //new Vector2(-x, -y)
				}
         );
      } // end DrawOutline()

      private void DrawHalfLine (Transform parent)
      {
         LineRenderer halfLine = GameObject.Instantiate(components.LineRenderer, parent);
         halfLine.name = "Half Line";

         halfLine.positionCount = 2;
         halfLine.SetPositions(
            new Vector3[2] {
               new Vector2(-(width/2), 0),
               new Vector2(+(width/2), 0)
            }
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
         int
            initialX = 0 - (width / 2),
            initialY = 0 - (length / 2);

         Transform arcParent = TransformFactory.CreateChild("Corner Arcs", parent);

         for (var xi = 0; xi < 2; xi++)
         {
            for (var yj = 0; yj < 2; yj++)
            {
               // 1. Instantiate the GameObject
               Transform arc = GameObject.Instantiate(components.CornerArc, arcParent).transform;

               // 2. Position the GameObject
               arc.localPosition = new Vector2(
                  initialX + (width * xi),
                  initialY + (length * yj)
               );

               // 3. Scale (flip) the image
               arc.localScale = new Vector2(
                  -((xi * 2) - 1),
                  -((yj * 2) - 1)
               );
            }
         }
      } // end DrawCornerArcs()

      private void DrawCenter (Transform parent)
      {
         Transform t = GameObject.Instantiate(components.CenterCircle, parent).transform;
         t.localPosition = Vector2.zero;
      }

      private void DrawPenaltyAreas (Transform parent)
      {
         // DrawPenaltyAreas
         //				scale		position
         // Defense	1,+1		0,-length/2 = -length/2 + 0*length
         // Attack	1,-1		0,+length/2 = -length/2 + 1*length

         var bottomCenterY = -(length / 2);

         for (var i = 0; i < 2; i++)
         {
            Transform area = GameObject.Instantiate(components.PenaltyArea, parent).transform;

            // 2. Position the GameObject
            area.localPosition = new Vector2(
               0,
               bottomCenterY + (length * i)
            );

            // 3. Scale (flip) the image
            area.localScale = new Vector2(
               1,
               1 - (i * 2)
            );
         }
      } // end DrawPenaltyAreas()

      private void DrawGoals ()
      {
         for (var i = 0; i < 2; i++)
         {
            // Instantiate, Scale and Position the Prefab
            Transform area = GameObject.Instantiate(components.Goal, transform).transform;
            area.localPosition = new Vector2(0, -(length / 2) + (length * i));
            area.localScale = new Vector2(1, -((i * 2) - 1));

            // LineRenderer: goal POSTS & CROSSBAR
            LineRenderer lr = area.GetComponent<LineRenderer>();
            lr.sortingLayerName = FIELD_SORTING_LAYER;
            lr.sortingOrder = LINES_ORDER;

            lr.startColor = lr.endColor = fieldStyle.LineColor;

            // SpriteRenderer: goal NETTING
            SpriteRenderer sr = area.GetComponentInChildren<SpriteRenderer>();
            sr.sortingLayerName = FIELD_SORTING_LAYER;
            sr.sortingOrder = LINES_ORDER - 1;
            sr.color = fieldStyle.NetColor;
         }
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

         if (zoneStyle.ZoneBoarderWidth > 0)
         {
            DrawXZoneBoarders(xBoundries);
            DrawYZoneBoarders(yBoundries);
         }

         if (zoneStyle.ZoneDividerWidth > 0)
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
         float halfWidth = zoneStyle.ZoneBoarderWidth / 2;

         for (var i = 0; i < zoneWidths.Length - 1; i++)
         {
            // POSITION
            x += zoneWidths[i];
            var xPos = AlignPosition(x, halfWidth, true);

            LineRendererFactory.CreateLocal(
               parent,
               "Line - XBoarder - " + (i + 1),
               zoneStyle.ZoneLinesMaterial,
               zoneStyle.ZoneBoarderWidth,
               zoneStyle.ZoneBoarderColor,
               new Vector3[] {
                  new Vector2(xPos, -yBoarder),
                  new Vector2(xPos, +yBoarder)
               },
               FIELD_SORTING_LAYER,
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
            halfWidth = zoneStyle.ZoneBoarderWidth / 2f;

         for (var i = 0; i < zoneWidths.Length - 1; i++)
         {
            // POSITION
            y += zoneWidths[i];
            var yPos = AlignPosition(y, halfWidth, false);

            LineRendererFactory.CreateLocal(
               parent,
               "Line - XBoarder - " + (i + 1),
               zoneStyle.ZoneLinesMaterial,
               zoneStyle.ZoneBoarderWidth,
               zoneStyle.ZoneBoarderColor,
               new Vector3[] {
                  new Vector2(-xBoarder, yPos),
                  new Vector2(+xBoarder, yPos)
               },
               FIELD_SORTING_LAYER,
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
            halfWidth = zoneStyle.ZoneDividerWidth / 2f;

         for (var i = 0; i < zoneWidths.Length; i++)
         {
            // POSITION
            var halfZoneWidth = zoneWidths[i] / 2f;
            var xPos = AlignPosition(x + halfZoneWidth, halfWidth, true);

            LineRendererFactory.CreateLocal(
               parent,
               "Line - XDivider - " + (i + 1),
               zoneStyle.ZoneLinesMaterial,
               zoneStyle.ZoneDividerWidth,
               zoneStyle.ZoneDividerColor,
               new Vector3[] {
                  new Vector2(xPos, -yBoarder),
                  new Vector2(xPos, +yBoarder)
               },
               FIELD_SORTING_LAYER,
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
            halfWidth = zoneStyle.ZoneBoarderWidth / 2f;

         for (var i = 0; i < zoneDepths.Length; i++)
         {
            // POSITION
            var halfZoneDepth = zoneDepths[i] / 2f;
            var yPos = AlignPosition(y + halfZoneDepth, halfWidth, false);

            LineRendererFactory.CreateLocal(
               parent,
               "Line - YDivider - " + (i + 1),
               zoneStyle.ZoneLinesMaterial,
               zoneStyle.ZoneDividerWidth,
               zoneStyle.ZoneDividerColor,
               new Vector3[] {
                  new Vector2(-xBoarder, yPos),
                  new Vector2(+xBoarder, yPos)
               },
               FIELD_SORTING_LAYER,
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
      /// <param name="lineWidth">half the width of the line</param>
      /// <param name="alignTowardZero">inline towards zero? or away from zero?</param>
      /// <returns></returns>
      private static float AlignPosition (float position, float lineWidth, bool alignTowardZero = true)
      {
         float result = 0;

         if (position != 0)
         {
            result = ((position < 0) == alignTowardZero)
               ? position + lineWidth
               : position - lineWidth;
         }

         return result;
      }
   }
}