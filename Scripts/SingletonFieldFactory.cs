using UnityEngine;

namespace My.Football.Fields
{
   public class SingletonFieldFactory : MonoBehaviour
   {
      [SerializeField] private FieldBuilder fieldBuilderPrefab;

      private static SingletonFieldFactory instance;

      public static FieldBuilder FieldBuilderInstance;


      private void Awake ()
      {
         if (instance)
         {
            DestroyImmediate(this);
            return;
         }

         instance = this;
         FieldBuilderInstance = Instantiate(fieldBuilderPrefab);
      }
   }
}