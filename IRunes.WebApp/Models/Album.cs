namespace IRunes.WebApp.Models
{
    using System.Collections.Generic;
    using System.Linq;

    public class Album : BaseModel<string>
    {
        public Album()
        {
            this.Tracks = new HashSet<Track>();
        }

        public string Name { get; set; }

        public string CoverUrl { get; set; }

        public decimal Price => this.Tracks.Sum(t => t.Price) - this.Tracks.Sum(t => t.Price) * 0.13m;

        public virtual ICollection<Track> Tracks { get; set; }
    }
}
