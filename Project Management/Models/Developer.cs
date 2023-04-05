namespace Project_Management.Models
{
    public class Developer : User
    {
        public Guid? ProjectId { get; set; }
        public Project Project { get; set; }

    }
}
