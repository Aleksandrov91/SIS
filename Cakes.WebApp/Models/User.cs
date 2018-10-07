namespace Cakes.WebApp.Models
{
    using System;
    using System.Collections.Generic;

    public class User : BaseModel<int>
    {
        public User()
        {
            this.DateOfRegistration = DateTime.UtcNow;
            this.Orders = new HashSet<Order>();
        }

        public string Name { get; set; }

        public string Username { get; set; }

        public DateTime DateOfRegistration { get; set; }

        public virtual ICollection<Order> Orders { get; set; }
    }
}
