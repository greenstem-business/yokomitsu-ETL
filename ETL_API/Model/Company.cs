using System.ComponentModel.DataAnnotations.Schema;

namespace ETL_API.Model
{
    public class Company
    {
        [Column("Code")]
        public string Code { get; set; }

        [Column("Company_Name")]
        public string CompanyName { get; set; }
    }
}
