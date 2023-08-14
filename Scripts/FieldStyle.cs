using UnityEngine;

namespace My.Football.Fields
{
   // the style of the field, including color scheme and grass sprite
   [CreateAssetMenu( fileName = "Field Style - ", menuName = "Field/Field Style" )]
	public class FieldStyle: ScriptableObject
	{
      [Header( "Grass and Background" )]
      [SerializeField] Color bkgGrassColor = new Color(0, 0.46875f, 0.24f, 1);
      [SerializeField] Color grassColor = new Color( 0, 0.703125f, 0, 1 );

      [Header( "Lines" )]
      [SerializeField] Color lineColor = Color.black;
		[SerializeField] float lineWidth = 0.3f;

      [Header( "Netting" )]
      [SerializeField] Color netColor = Color.white;

		[Header( "Zone Coloring" )]
      [SerializeField]
      private Color[]
			wZoneColors = new Color[2] { new Color(.5f, .5f, .5f, .15f), new Color(0, 0, 0, .2f) },
         dZoneColors = new Color[2] { new Color ( 0, 0, 0, .2f ), new Color ( 1, 1, 1, .1f ) };



      public Color BackgroundGrassColor => bkgGrassColor;
		public Color GrassColor => grassColor;

		public Color LineColor => lineColor;
      public float LineWidth => lineWidth;

      public Color NetColor => netColor;

		public Color[] WZoneColors => wZoneColors;
		public Color[] DZoneColors => dZoneColors;
	}
}