using My.Football.Fields.Setup;
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
      [SerializeField] private FieldZoneStyle zoneStyle;


      public FieldCategoryConfig Category => categoryConfig;
      public FieldComponents Components => components;
      public FieldLayersAndSortingOrder LayersAndSortingOrder => layersAndSortingOrder;
    

      public FieldZoneModel ZoneModel => zoneModel;
      public FieldStyle FieldStyle => fieldStyle;
      public FieldZoneStyle ZoneStyle => zoneStyle;

      public int FieldWidth { get; private set; }
      public int FieldLength { get; private set; }
      public int HalfFieldWidth => FieldWidth / 2;
      public int HalfFieldLength => FieldLength / 2;

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