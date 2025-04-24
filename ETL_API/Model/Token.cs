using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ETL_API.Model
{
    [Table("API_TOKENS")]
    public class Token
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("ID")]
        public int ID { get; set; }

        [Column("User Name")]
        public string UserName { get; set; }

        [Column("Token")]
        public string TokenString { get; set; }

        [Column("Created Date Time")]
        public DateTime CreatedDateTime { get; set; }

        [Column("Is Active")]
        public bool IsActive { get; set; }
    }
}
