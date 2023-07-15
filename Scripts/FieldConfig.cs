using UnityEngine;
using System.Linq;

namespace Football.Core
{
	/// <summary>
	/// The dimentions of the field.
	/// </summary>
	[CreateAssetMenu( fileName = "Field []", menuName = "Field/Field Config" )]
	public class FieldConfig: ScriptableObject
	{
		[SerializeField] int[] verticalZoneDepths;
		[SerializeField] int[] horizontalZoneWidths;
		[SerializeField] float lineWidth = 0.3f;

		public int[] VerticalZoneDepths => verticalZoneDepths;
		public int[] HorizontalZoneWidths => horizontalZoneWidths;
		public int Length => VerticalZoneDepths.Sum();
      public int Width => HorizontalZoneWidths.Sum();
      public float LineWidth => lineWidth;
	}
}