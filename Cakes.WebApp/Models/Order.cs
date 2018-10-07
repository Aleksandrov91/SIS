namespace Cakes.WebApp.Models
{
    using System;
    using System.Collections.Generic;

    public class Order : BaseModel<int>
    {
        public Order()
        {
            this.Products = new HashSet<ProductOrder>();
        }

        public int UserId { get; set; }

        public virtual User User { get; set; }

        public DateTime DateOfCreation { get; set; }

        public virtual ICollection<ProductOrder> Products { get; set; }
    }
}
