using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Model.Base
{
    public class BaseEntity
    {
        [Column("id")]
        public long Id { get; set; }
    }
}
