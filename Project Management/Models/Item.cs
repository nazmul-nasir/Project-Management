using System.ComponentModel.DataAnnotations;

namespace Project_Management.Models
{
    public class Item
    {
        [Key]
        public Guid TaskId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public bool? IsCompleted { get; set; }
        public Guid ProjectId { get; set; }
        public Project Project { get; set; }
    }
}
