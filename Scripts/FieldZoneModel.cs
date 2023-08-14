using UnityEngine;
using System.Linq;

namespace My.Football.Fields
{
   /// <summary>
   /// The dimentions of the field.
   /// </summary>
   [CreateAssetMenu(fileName = "Field []", menuName = "Field/Field Config")]
   public class FieldZoneModel : ScriptableObject, IFieldZoneModel
   {
      [SerializeField] int[] verticalZoneDepths;
      [SerializeField] int[] horizontalZoneWidths;

      public int[] VerticalZoneDepths => verticalZoneDepths;
      public int[] HorizontalZoneWidths => horizontalZoneWidths;

      public int Length => VerticalZoneDepths.Sum();
      public int Width => HorizontalZoneWidths.Sum();

      public Vector2 Min => new Vector2(-Width / 2, -Length / 2);
      public Vector2 Max => new Vector2(Width / 2, Length / 2);
   }
}