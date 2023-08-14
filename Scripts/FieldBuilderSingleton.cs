namespace My.Football.Fields
{
   public class FieldBuilderSingleton : FieldBuilder
   {
      public static FieldBuilder Instance { get; private set; }

      public static IFieldZoneModel FieldConfig => Instance != null ? Instance.Config : null;

      override protected void Awake ()
      {
         if (Instance != null)
         {
            DestroyImmediate(Instance);
            return;
         }

         Instance = this;
         base.Awake();
      }
   }
}