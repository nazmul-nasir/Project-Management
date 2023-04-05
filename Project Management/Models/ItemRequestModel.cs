namespace Project_Management.Models
{
    public class ItemRequestModel
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public bool IsCompleted { get; set; }
        public Guid ProjectId { get; set; }
    }
}
