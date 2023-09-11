using UnityEngine;

namespace My.Football.Fields.Setup
{
   public abstract class AbstractZoneBoarderStyle : ScriptableObject, IFieldZoneStyle
   {
      public abstract Color BoarderColor { get; }
      public abstract float BoarderWidth { get; }
      public abstract Color DividerColor { get; }
      public abstract float DividerWidth { get; }
      public abstract float HalfBoarderWidth { get; }
      public abstract float HalfDividerWidth { get; }
      public abstract Material LinesMaterial { get; }
   }
}