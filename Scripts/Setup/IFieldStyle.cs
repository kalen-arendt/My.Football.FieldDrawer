using UnityEngine;

namespace My.Football.Fields.Setup
{
   public interface IFieldStyle
   {
      Color BackgroundGrassColor { get; }
      Color[] DZoneColors { get; }
      Color GrassColor { get; }
      Color LineColor { get; }
      Color NetColor { get; }
      Color[] WZoneColors { get; }
   }
}