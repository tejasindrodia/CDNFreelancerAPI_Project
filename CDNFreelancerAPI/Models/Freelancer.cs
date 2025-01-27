namespace CDNFreelancerAPI.Models
{
    public class Freelancer
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public bool IsArchived { get; set; } = false;
        public ICollection<Skillset> Skillsets { get; set; }
        public ICollection<Hobby> Hobbies { get; set; }
    }
}