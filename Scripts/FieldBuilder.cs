using My.Football.Fields.Setup;

using UnityEngine;

namespace My.Football.Fields
{
   [SelectionBase]
   [DisallowMultipleComponent]
   public class FieldBuilder : MonoBehaviour
   {
      [SerializeField] private FieldZoneModel config;
      [SerializeField] private FieldComponents components;
      [SerializeField] private FieldStyle fieldStyle;
      [SerializeField] private FieldZoneStyle zoneStyle;

      //[HideInInspector]
      [SerializeField] private FieldDrawer drawer;

      public FieldZoneModel Config => config;

      private void OnValidate ()
      {
         drawer = new FieldDrawer(config, components, fieldStyle, zoneStyle, transform);
      }

      virtual protected void Awake ()
      {
         //
         // SET PROPERTIES AND DRAW FIELD
         //
         if (config && components && fieldStyle && zoneStyle)
         {
            drawer = new FieldDrawer(config, components, fieldStyle, zoneStyle, transform);
            drawer.Draw();
         }
         else
         {
            Debug.LogWarning("Field has not been assigned all necesary components!");
         }
      }

      [ContextMenu("Draw")]
      public void DrawField ()
      {
         drawer.Draw();
      }

      [ContextMenu("Erase")]
      public void EraseField ()
      {
         drawer.Erase();
      }
   }
}