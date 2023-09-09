using UnityEngine;

namespace My.Football.Fields.Setup
{
   /// <summary>
   /// prefabs that are used to create parts of the field.
   /// </summary>
   [CreateAssetMenu(fileName = "Field Components", menuName = "Field/Field Components")]
   public class FieldComponents : ScriptableObject
   {
      [Header("Grass")]
      [SerializeField] Sprite grass;

      [Header("Goals")]
      [SerializeField] Sprite goalNetting;

      [Header("Ball Placement")]
      [SerializeField] Sprite centerSpot;
      [SerializeField] Sprite penaltySpot;
      [Tooltip("The 1yd arc where the ball must be placed.")]
      [SerializeField] Sprite cornerArc;

      [Header("Restraining Arcs")]
      [SerializeField] Sprite centerCircle;
      [Tooltip("The restraining arc for all players when a penalty is being taken.")]
      [SerializeField] Sprite penaltyArc;
      [Tooltip("The restraining arc for opposing players.")]
      [SerializeField] Sprite cornerRestrainingArc;

      [Header("Lines")]
      [SerializeField] Material lineMaterial;

      public Sprite Square => grass;
      public Sprite Circle => penaltySpot;
      public Sprite CenterCircle => centerCircle;
      public Sprite PenaltyArc => penaltyArc;
      public Sprite CornerArc => cornerArc;
      public Sprite CornerRestrainingArc => cornerRestrainingArc;
      public Material LineMaterial => lineMaterial;
   }
}