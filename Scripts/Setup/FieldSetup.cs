using My.Football.Fields.Setup;
using My.Football.I;
using My.Unity.Debugging;

using UnityEngine;

namespace My.Football.Fields.Setup
{
   [System.Serializable]
   public class FieldSetup
   {
      [Header("Field Config")]
      [SerializeField] private FieldCategoryConfig categoryConfig;
      [SerializeField] private FieldComponents components;
      [SerializeField] private FieldLayersAndSortingOrder layersAndSortingOrder;

      [Header("Field Style")]
      [SerializeField] private FieldZoneModel zoneModel;
      [SerializeField] private FieldStyle fieldStyle;
      [SerializeField] private AbstractZoneBoarderStyle zoneStyle;


      public IFieldCategoryConfig Category => categoryConfig;
      public IFieldComponents Components => components;
      public IFieldLayersAndSortingOrder LayersAndSortingOrder => layersAndSortingOrder;
    

      public IFieldZoneModel ZoneModel => zoneModel;
      public IFieldStyle FieldStyle => fieldStyle;
      public IFieldZoneStyle ZoneStyle => zoneStyle;

      public int FieldWidth { get; private set; }
      public int FieldLength { get; private set; }
      public float HalfFieldWidth => FieldWidth / 2f;
      public float HalfFieldLength => FieldLength / 2f;

      public bool ValidateSetup()
      {
         bool isValid = categoryConfig.WarnIfNull()
                     && components.WarnIfNull()
                     && layersAndSortingOrder.WarnIfNull()
                     && zoneModel.WarnIfNull()
                     && fieldStyle.WarnIfNull();

         zoneStyle.WarnIfNull();

         if (zoneModel)
         {
            FieldWidth = zoneModel.Width;
            FieldLength = zoneModel.Length;
         }

         return isValid;
      }
   }
}