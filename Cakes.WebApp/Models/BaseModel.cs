namespace Cakes.WebApp.Models
{
    public abstract class BaseModel<TKey>
    {
        public TKey Id { get; set; }
    }
}
