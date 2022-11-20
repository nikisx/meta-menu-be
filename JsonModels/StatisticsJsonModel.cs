namespace meta_menu_be.JsonModels
{
    public class StatisticsJsonModel
    {
        public double Morning { get; set; }
        public double Lunch { get; set; }
        public double Afternoon { get; set; }
        public double Evening { get; set; }
        public int LastMonthOrdersCount { get; set; }
        public int CurrentMonthOrdersCount { get; set; }
    }
}
