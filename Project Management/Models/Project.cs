namespace Project_Management.Models
{
    public class Project
    {
        public Guid ProjectId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Guid AdminId { get; set; }
        public ICollection<Developer> Developers { get; set; }
        public ICollection<Item> Items { get; set; }
    }
}
