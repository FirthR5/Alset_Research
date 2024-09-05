using System.ComponentModel.DataAnnotations;

namespace Alset_Research.DTO
{
    public class JournalDTO
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime PublicationDate { get; set; }
        public string PDFFile { get; set; }
        public string ResearcherFirstName { get; set; }
        public string ResearcherLastName { get; set; }
    }
}
