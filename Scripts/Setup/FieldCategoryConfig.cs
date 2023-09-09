using UnityEngine;

namespace My.Football.Fields.Setup
{
   /// <summary>
   /// prefabs that are used to create parts of the field.
   /// </summary>
   [CreateAssetMenu(fileName = "Field Category Config", menuName = "Field/Category Config")]
   public class FieldCategoryConfig : ScriptableObject
   {
      [Header("Goal Dimensions")]

      [SerializeField] private float goalWidth = 8;
      public float GoalWidth => goalWidth;
      public float HalfGoalWidth => goalWidth / 2f;

      [Tooltip("How far back the goal extends from the field.")]
      [SerializeField] private float goalDepth = 2.4f;
      public float GoalDepth => goalDepth;


      [Header("Restraining Arcs")]
      [Tooltip("The minimum distance that opponents must be from dead-ball situations.")]
      [SerializeField] private float deadBallRestrainingDistance = 10;
      public float RestrainingDistance => deadBallRestrainingDistance;

      [Header("Goal Area")]
      [Tooltip("6 yard box")]
      [SerializeField] private float goalAreaSize = 6;
      public float GoalAreaSize => goalAreaSize;


      [Header("Penalty Box")]
      [Tooltip("18 yard box")]
      [SerializeField] private float penaltyBoxSize = 18;
      public float PenaltyBoxSize => penaltyBoxSize;


      [Header("Penalty Spot")]

      [SerializeField] private float penaltySpotDistance = 12;
      [SerializeField] private float penaltySpotDiameter = 0.75f;

      public float PenaltySpotDistance => penaltySpotDistance;
      public float PenaltySpotDiameter => penaltySpotDiameter;


      [Header("Center Spot")]
      [SerializeField] private float centerSpotDiameter = 0.9f;
      public float CenterSpotDiameter => centerSpotDiameter;

      [Header("Lines")]
      [SerializeField] float lineWidth = 0.3f;
      public float LineWidth => lineWidth;
      public float HalfLineWidth => lineWidth / 2f;


      [Header("Outer Grass")]
      [SerializeField] private float outerGrassWidth = 10;
      public float OuterGrassWidth => outerGrassWidth;


      public float CenterCircleDiameter => deadBallRestrainingDistance * 2;
      public float PenaltyArcHeight => PenaltySpotDistance + RestrainingDistance - PenaltyBoxSize;
   }
}