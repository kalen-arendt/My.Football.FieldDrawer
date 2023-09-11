using System.Linq;

using My.Football.Fields.Setup;
using My.Unity.Creational;

using UnityEngine;

namespace My.Football.Fields.Painters
{
   internal class ZoneLinesPainter
   {
      private readonly FieldSetup fieldSetup;

      internal ZoneLinesPainter (FieldSetup fieldSetup)
      {
         this.fieldSetup = fieldSetup;
      }

      public void Paint (Transform parent)
      {
         var zoneBoarders = fieldSetup.ZoneStyle;

         if (!zoneBoarders?.LinesMaterial)
         {
            return;
         }

         Transform header = TransformFactory.CreateChild("Zone Boundries Instance", parent);
         Transform xBoundries = TransformFactory.CreateChild("X Boundries", header);
         Transform yBoundries = TransformFactory.CreateChild("Y Boundries", header);

         if (fieldSetup.ZoneStyle.BoarderWidth > 0)
         {
            DrawXZoneBoarders(xBoundries);
            DrawYZoneBoarders(yBoundries);
         }

         if (fieldSetup.ZoneStyle.DividerWidth > 0)
         {
            DrawXZoneDividers(xBoundries);
            DrawYZoneDividers(yBoundries);
         }
      }

      private void DrawXZoneBoarders (Transform parent)
      {
         var x = -fieldSetup.HalfFieldWidth;

         var zoneWidths = fieldSetup.ZoneModel.HorizontalZoneWidths;
         foreach (var width in zoneWidths.Take(zoneWidths.Length - 1))
         {
            x += width;
            var xAligned = AlignPosition(x, fieldSetup.ZoneStyle.HalfBoarderWidth, true);

            DrawZoneBoarder(
               parent,
               "XBoarder",
               new Vector3[] {
                  new Vector2(xAligned, -fieldSetup.HalfFieldLength),
                  new Vector2(xAligned, +fieldSetup.HalfFieldLength)
               }
            );
         }
      }

      private void DrawYZoneBoarders (Transform parent)
      {
         var zoneDepths = fieldSetup.ZoneModel.VerticalZoneDepths;
         float y = -fieldSetup.HalfFieldLength;

         foreach (var depth in zoneDepths.Take(zoneDepths.Length - 1))
         {
            y += depth;
            var yAligned = AlignPosition(y, fieldSetup.ZoneStyle.HalfBoarderWidth, false);

            DrawZoneBoarder(
               parent,
               "YBoarder",
               new Vector3[] {
                  new Vector2(-fieldSetup.HalfFieldWidth, yAligned),
                  new Vector2(+fieldSetup.HalfFieldWidth, yAligned)
               }
            );
         }
      }

      private void DrawXZoneDividers (Transform parent)
      {
         var zoneWidths = fieldSetup.ZoneModel.HorizontalZoneWidths;
         var x = -fieldSetup.HalfFieldWidth;
         var yBoarder = fieldSetup.HalfFieldLength;

         for (var i = 0; i < zoneWidths.Length; i++)
         {
            // POSITION
            var halfZoneWidth = zoneWidths[i] / 2f;
            var xPos = AlignPosition(x + halfZoneWidth, fieldSetup.ZoneStyle.HalfDividerWidth, true);

            DrawZoneDivider(
               parent,
               "Line - XDivider - " + (i + 1),
               new Vector3[] {
                  new Vector2(xPos, -yBoarder),
                  new Vector2(xPos, +yBoarder)
               }
            );

            x += zoneWidths[i];
         }
      }

      private void DrawYZoneDividers (Transform parent)
      {
         var zoneDepths = fieldSetup.ZoneModel.VerticalZoneDepths;
         float
            y = -fieldSetup.HalfFieldLength,
            xBoarder = fieldSetup.HalfFieldWidth,
            halfWidth = fieldSetup.ZoneStyle.BoarderWidth / 2f;

         for (var i = 0; i < zoneDepths.Length; i++)
         {
            // POSITION
            var halfZoneDepth = zoneDepths[i] / 2f;
            var yPos = AlignPosition(y + halfZoneDepth, halfWidth, false);

            DrawZoneDivider(
               parent,
               "Line - YDivider - " + (i + 1),
               new Vector3[] {
                  new Vector2(-xBoarder, yPos),
                  new Vector2(+xBoarder, yPos)
               }
            );

            y += zoneDepths[i];
         }
      }

      private void DrawZoneBoarder (Transform parent, string name, Vector3[] points)
      {
         DrawZoneLine(
            parent,
            name,
            points,
            fieldSetup.ZoneStyle.BoarderWidth,
            fieldSetup.ZoneStyle.BoarderColor
         );
      }

      private void DrawZoneDivider (Transform parent, string name, Vector3[] points)
      {
         DrawZoneLine(
            parent,
            name,
            points,
            fieldSetup.ZoneStyle.DividerWidth,
            fieldSetup.ZoneStyle.DividerColor
         );
      }

      private void DrawZoneLine (Transform parent, string name, Vector3[] points, float lineWidth, Color color)
      {
         LineRendererFactory.CreateLocal(
            parent,
            name,
            fieldSetup.ZoneStyle.LinesMaterial,
            lineWidth,
            color,
            points,
            false,
            fieldSetup.LayersAndSortingOrder.SortingLayer,
            fieldSetup.LayersAndSortingOrder.LinesOrderInLayer - 1
         );
      }


      /// <summary>
      /// Align the position of a line renderer point so the edge of the line is flush with z
      /// </summary>
      /// <param name="position">the variable to align</param>
      /// <param name="halfLineWidth">half the width of the line</param>
      /// <param name="alignTowardZero">inline towards zero? or away from zero?</param>
      /// <returns></returns>
      private float AlignPosition (float position, float halfLineWidth, bool alignTowardZero = true)
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
   }
}