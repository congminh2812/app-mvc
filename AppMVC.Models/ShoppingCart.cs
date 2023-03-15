using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AppMVC.Models
{
    public class ShoppingCart
    {
        public int Id { get; set; }

        [Range(1, 1000, ErrorMessage = "Please enter value between 1 and 1000")]
        public int Count { get; set; }

        public int ProductId { get; set; }

        [ForeignKey("ProductId")]
        public Product Product { get; set; }

        public string ApplicationUserId { get; set; }

        [ForeignKey("ApplicationUserId")]
        public ApplicationUser User { get; set; }

        [NotMapped]
        public double Price { get; set; }
    }
}
