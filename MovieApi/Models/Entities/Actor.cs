
namespace MovieApi.Models.Entities
{
    public class Actor
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public int BirthYear { get; set; }
        // Relatione: N:M med Movie via MovieActor
        public ICollection<Movie> Movies { get; set; } = new List<Movie>();

        internal IEnumerable<object> Include(Func<object, object> value)
        {
            throw new NotImplementedException();
        }
    }
}
