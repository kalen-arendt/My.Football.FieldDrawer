using Football.Core.FieldDrawing;

using UnityEngine;

namespace Football.Core
{
   public class FieldBuilder : MonoBehaviour
   {
      [SerializeField] private FieldConfig config;
      [SerializeField] private FieldComponents components;
      [SerializeField] private FieldStyle fieldStyle;
      [SerializeField] private FieldZoneStyle zoneStyle;

      [HideInInspector]
      [SerializeField] private FieldDrawer drawer;


      public static FieldBuilder Instance { get; private set; }

      public static FieldConfig FieldConfig => Instance != null ? Instance.config : null;

      public FieldConfig Config => config;

      public static int Width { get; private set; }
      public static int Length { get; private set; }

      public static Vector2 Min { get; private set; }
      public static Vector2 Max { get; private set; }

      public static int LoopX (int x)
      {
         return (x + Width) % Width;
      }

      public static int LoopY (int y)
      {
         return (y + Length) % Length;
      }

      public static Vector2 Clamp (Vector2 v)
      {
         return new Vector2(
                  Mathf.Clamp(v.x, Min.x, Max.x),
                  Mathf.Clamp(v.y, Min.y, Max.y)
               );
      }

      private void OnValidate ()
      {
         drawer = new FieldDrawer(config, components, fieldStyle, zoneStyle, transform);
      }

      private void Awake ()
      {
         //
         // SINGLETON
         //
         if (Instance)
         {
            DestroyImmediate(this);
         }
         else
         {
            Instance = this;
         }

         //
         // SET PROPERTIES AND DRAW FIELD
         //
         if (config && components && fieldStyle && zoneStyle)
         {
            drawer = new FieldDrawer(config, components, fieldStyle, zoneStyle, transform);

            Width = config.Width;
            Length = config.Length;

            Min = new Vector2(-Width / 2, -Length / 2);
            Max = new Vector2(Width / 2, Length / 2);

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