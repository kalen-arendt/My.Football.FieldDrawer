using My.Football.Fields.Setup;

namespace My.Football.Fields
{
   public interface IFieldConfig
   {
      FieldZoneModel ZoneModel { get; }
      FieldComponents Components { get; }
      FieldStyle FieldStyle { get; }
      FieldZoneStyle ZoneStyle { get; }
   }
}