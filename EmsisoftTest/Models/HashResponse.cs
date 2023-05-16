namespace EmsisoftTest.Models
{
    public class HashResponse
    {
        public List<HashEntry> Hashes { get; set; }
    }

    public class HashEntry
    {
        public DateTime Date { get; set; }
        public int Count { get; set; }
    }

}
