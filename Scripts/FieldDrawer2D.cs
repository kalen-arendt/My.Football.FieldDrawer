using My.Football.Fields.Painters;
using My.Football.Fields.Setup;

using UnityEngine;

namespace My.Football.Fields
{
   [SelectionBase]
   public class FieldDrawer2D : MonoBehaviour
   {
      [SerializeField] private FieldSetup fieldSetup;

      [ContextMenu("Draw")]
      public void Draw ()
      {
         if (fieldSetup.ValidateSetup())
         {
            Erase();

            new GrassPainter(fieldSetup).Paint(transform);
            new FieldMarkingsDrawer(fieldSetup).Paint(transform);
            new ZonePainter(fieldSetup).Paint(transform);
            new ZoneLinesPainter(fieldSetup).Paint(transform);

            foreach (Transform t in transform.GetComponentsInChildren<Transform>())
            {
               t.gameObject.layer = fieldSetup.LayersAndSortingOrder.ObjectLayer;
            }
         }
      }

      [ContextMenu("Erase")]
      public void Erase ()
      {
         while (transform.childCount > 0)
         {
            DestroyImmediate(transform.GetChild(0).gameObject);
         }
      }
   }
}