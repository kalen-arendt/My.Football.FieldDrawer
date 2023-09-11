using UnityEngine;

namespace My.Football.Fields.Setup
{
   /// <summary>
   /// prefabs that are used to create parts of the field.
   /// </summary>
   [CreateAssetMenu(fileName = "Field Components", menuName = "Field/Field Components")]
   public class FieldComponents : ScriptableObject, IFieldComponents
   {
      [Header("Grass and Background")]
      [SerializeField] Sprite grass;
      [SerializeField] Sprite backgroundGrass;


      [Header("Goals")]
      [SerializeField] Sprite goalNetting;


      [Header("Center Circle")]
      [SerializeField] Sprite centerSpot;
      [SerializeField] Sprite centerCircle;


      [Header("Penalty Spots")]
      [SerializeField] Sprite penaltySpot;
      [Tooltip("The restraining arc for all players when a penalty is being taken.")]
      [SerializeField] Sprite penaltyArc;


      [Header("Corners")]
      [Tooltip("The 1yd arc where the ball must be placed.")]
      [SerializeField] Sprite cornerArc;
      [Tooltip("The restraining arc for opposing players.")]
      [SerializeField] Sprite cornerRestrainingArc;


      [Header("Lines")]
      [SerializeField] Material lineMaterial;


      public Sprite GrassSquare => grass;
      public Sprite BackgroundGrass => backgroundGrass;

      public Sprite CenterSpot => centerSpot;
      public Sprite CenterCircle => centerCircle;

      public Sprite PenaltySpot => penaltySpot;
      public Sprite PenaltyArc => penaltyArc;


      public Sprite CornerArc => cornerArc;
      public Sprite CornerRestrainingArc => cornerRestrainingArc;

      public Material LineMaterial => lineMaterial;
   }
}