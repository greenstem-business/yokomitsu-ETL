using System.ComponentModel.DataAnnotations.Schema;

namespace ETL_API.Model
{
    public class View_SalesTransaction
    {
        [Column("Document No")]
        public string DocumentNo { get; set; }

        [Column("Transaction Type")]
        public string TransactionType { get; set; }

        [Column("Transaction Description")]
        public string TransactionDescription { get; set; }

        [Column("Stock Code")]
        public string StockCode { get; set; }

        [Column("Quantity")]
        public decimal Quantity { get; set; }

        [Column("Total Amount")]
        public decimal TotalAmount { get; set; }

        [Column("Unit Cost")]
        public decimal UnitCost { get; set; }

        [Column("Location")]
        public string Location { get; set; }

        [Column("Issue Date")]
        public DateTime IssueDate { get; set; }

        [Column("Customer Code")]
        public string CustomerCode { get; set; }

        [Column("Customer Name")]
        public string CustomerName { get; set; }

        [Column("Salesman Code")]
        public string SalesmanCode { get; set; }

        [Column("Salesman Name")]
        public string SalesmanName { get; set; }

        [Column("Nett Qty")]
        public decimal NettQty { get; set; }
    }
}
