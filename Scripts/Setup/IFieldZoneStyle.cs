using UnityEngine;

namespace My.Football.Fields.Setup
{
   public interface IFieldZoneStyle
   {
      Color BoarderColor { get; }
      float BoarderWidth { get; }
      Color DividerColor { get; }
      float DividerWidth { get; }
      float HalfBoarderWidth { get; }
      float HalfDividerWidth { get; }
      Material LinesMaterial { get; }
   }
}