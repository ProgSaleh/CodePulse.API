namespace CodePulse.API.Models.DTO
{
    public class BlogImageDto
    {
        public Guid Id { get; set; }
        public string FileName { get; set; }
        public string FileExtension { get; set; }
        public string Title { get; set; }
        public string url { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
