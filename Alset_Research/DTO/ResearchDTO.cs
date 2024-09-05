using System.ComponentModel.DataAnnotations;

namespace Alset_Research.DTO
{
    public class ResearchDTO
    {
        [Key]
        public int ResearcherId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int FollowStatus { get; set; }
    }
}
