namespace My.Football.Fields.Setup
{
   public interface IFieldLayersAndSortingOrder
   {
      int GrassOrderInLayer { get; }
      int LinesOrderInLayer { get; }
      int ObjectLayer { get; }
      string SortingLayer { get; }
   }
}