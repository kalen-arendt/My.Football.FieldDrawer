namespace My.Football.Fields.Setup
{
   public interface IFieldCategoryConfig
   {
      float CenterCircleDiameter { get; }
      float CenterSpotDiameter { get; }
      float GoalAreaSize { get; }
      float GoalDepth { get; }
      float GoalWidth { get; }
      float HalfGoalWidth { get; }
      float HalfLineWidth { get; }
      float LineWidth { get; }
      float OuterGrassWidth { get; }
      float PenaltyArcHeight { get; }
      float PenaltyBoxSize { get; }
      float PenaltySpotDiameter { get; }
      float PenaltySpotDistance { get; }
      float RestrainingDistance { get; }
   }
}