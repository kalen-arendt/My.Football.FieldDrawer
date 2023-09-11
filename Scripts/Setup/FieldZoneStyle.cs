using UnityEngine;

namespace My.Football.Fields.Setup
{
   [CreateAssetMenu(fileName = "Zone Style - ", menuName = "Field/Zone Style")]
   public class FieldZoneStyle : AbstractZoneBoarderStyle
   {
      [SerializeField] Material zoneLinesMaterial;
      [SerializeField] [Range( 0.1f, 0.5f )] float zoneBoarderWidth = .3f;
      [SerializeField] [Range( 0.0f, 0.5f )] float zoneDividerWidth = .2f;
      [SerializeField] Color zoneBoarderColor = new Color( 0, 0, 0, .6f );
      [SerializeField] Color zoneDividerColor = new Color( 0, 0, 0, .4f );

      public override Material LinesMaterial => zoneLinesMaterial;
      public override float BoarderWidth => zoneBoarderWidth;
      public override float HalfBoarderWidth => BoarderWidth / 2f;

      public override float DividerWidth => zoneDividerWidth;
      public override float HalfDividerWidth => zoneDividerWidth / 2f;

      public override Color BoarderColor => zoneBoarderColor;
      public override Color DividerColor => zoneDividerColor;
   }
}