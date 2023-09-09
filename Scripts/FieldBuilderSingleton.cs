using My.Football.I;

namespace My.Football.Fields
{
   public class FieldBuilderSingleton : FieldBuilder
   {
      public static FieldBuilder Instance { get; private set; }

      bool isInstance = false;

      public static IFieldZoneModel FieldConfig => Instance != null ? Instance.Config : null;

      override protected void Awake ()
      {
         if (FieldSettings.Config != null)
         {
            DestroyImmediate(Instance);
            return;
         }

         Instance = this;
         isInstance = true;
         FieldSettings.Config = Config;
         base.Awake();
      }

      private void OnDestroy ()
      {
         if (isInstance)
         {
            FieldSettings.Config = null;
         }
      }
   }
}