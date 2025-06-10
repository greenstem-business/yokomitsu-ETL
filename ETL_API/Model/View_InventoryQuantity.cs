using System.ComponentModel.DataAnnotations.Schema;

namespace ETL_API.Model
{
    public class View_InventoryQuantity
    {
        [Column("Stock Code")]
        public string StockCode { get; set; }

        [Column("Stock Name")]
        public string StockName { get; set; }

        [Column("Location Code")]
        public string LocationCode { get; set; }

        [Column("Inventory Quantity")]
        public double InventoryQuantity { get; set; }
    }
}
