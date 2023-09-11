using UnityEngine;

namespace My.Football.Fields.Setup
{
   public interface IFieldComponents
   {
      Sprite BackgroundGrass { get; }
      Sprite CenterCircle { get; }
      Sprite CenterSpot { get; }
      Sprite CornerArc { get; }
      Sprite CornerRestrainingArc { get; }
      Sprite GrassSquare { get; }
      Material LineMaterial { get; }
      Sprite PenaltyArc { get; }
      Sprite PenaltySpot { get; }
   }
}