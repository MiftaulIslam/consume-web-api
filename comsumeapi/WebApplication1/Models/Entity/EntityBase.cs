using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Models.Entity
{
    public class EntityBase 
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
    }
}
