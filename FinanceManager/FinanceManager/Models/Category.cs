using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FinanceManager.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string Description { get; set; }

        public virtual ICollection<Transaction> Transaction { get; set; }

        public Category()
        {
            Transaction = new HashSet<Transaction>();
        }
    }
}
