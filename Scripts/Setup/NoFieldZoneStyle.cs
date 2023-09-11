using UnityEngine;

namespace My.Football.Fields.Setup
{
   [CreateAssetMenu(fileName = "Zone Boarders - None", menuName = "Field/No Zone Style")]
   public class NoFieldZoneStyle : AbstractZoneBoarderStyle
   {
      public override Material LinesMaterial => null;
      public override float BoarderWidth => 0;
      public override float HalfBoarderWidth => 0;

      public override float DividerWidth => 0;
      public override float HalfDividerWidth => 0;

      public override Color BoarderColor => Color.clear;
      public override Color DividerColor => Color.clear;
   }
}