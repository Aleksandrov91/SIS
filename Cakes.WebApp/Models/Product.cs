namespace Cakes.WebApp.Models
{
    using System.Collections.Generic;

    public class Product : BaseModel<int>
    {
        public Product()
        {
            this.Orders = new HashSet<ProductOrder>();
        }

        public string Name { get; set; }

        public decimal Price { get; set; }

        public string ImageUrl { get; set; }

        public ICollection<ProductOrder> Orders { get; set; }
    }
}
