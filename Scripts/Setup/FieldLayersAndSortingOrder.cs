using UnityEngine;

namespace My.Football.Fields.Setup
{
   /// <summary>
   /// prefabs that are used to create parts of the field.
   /// </summary>
   [CreateAssetMenu(fileName = "Field Layers and Sorting Order", menuName = "Field/Layers and Sorting Order")]
   public class FieldLayersAndSortingOrder : ScriptableObject, IFieldLayersAndSortingOrder
   {
      [Header("GameObject Layer")]
      [SerializeField] private int objectLayer = 8;

      [Header("Sorting Layer and Ordering")]
      [SerializeField] private string fieldSortingLayer = "Field";
      [SerializeField] private int grassOrderInLayer = 0;
      [SerializeField] private int linesOrderInLayer = 10;

      public int ObjectLayer => objectLayer;
      public string SortingLayer => fieldSortingLayer;
      public int GrassOrderInLayer => grassOrderInLayer;
      public int LinesOrderInLayer => linesOrderInLayer;
   }
}