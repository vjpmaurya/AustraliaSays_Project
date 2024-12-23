namespace AustraliyaDemo.Models
{
    public class CommentModel
    {
        public int Id { get; set; }
        public int ArticleId { get; set; }
        public string ArticleName { get; set; }
        public string Description { get; set; }
        public string Commenttext { get; set; }
       
    }
}
