using OptimizationUI.Models;

namespace OptimizationUI.ViewModels
{
    public class WarehouseViewModel
    {
        #region Properties

        public Warehouse Warehouse { get; set; }

        #endregion
        
        #region ChartProperties

        public bool ShowCustom { get; set; } = true;
        public bool ShowBest { get; set; } = true;
        public bool ShowAvg { get; set; } = true;
        public bool ShowWorst { get; set; } = true;

        #endregion

    }
}