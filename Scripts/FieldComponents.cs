using UnityEngine;

namespace Football.Core.FieldDrawing
{
	/// <summary>
	/// prefabs that are used to create parts of the field.
	/// </summary>
	[CreateAssetMenu( fileName = "Field Components", menuName = "Field/Field Components" )]
	public class FieldComponents: ScriptableObject {
		[SerializeField] GameObject defensivePenaltyArea;	// normal scale = defensive half orientation
		[SerializeField] GameObject bottomLeftCornerArc;	// normal scale = bottom left orientation
		[SerializeField] GameObject centerCircle;
		[SerializeField] GameObject goal;

		[SerializeField] Sprite grass;
		[SerializeField] LineRenderer lineRenderer;

		public GameObject Goal => goal;
		public GameObject PenaltyArea => defensivePenaltyArea;
		public GameObject CornerArc => bottomLeftCornerArc;
		public GameObject CenterCircle => centerCircle;
		public Sprite Grass => grass;
		public LineRenderer LineRenderer => lineRenderer;
	}
}