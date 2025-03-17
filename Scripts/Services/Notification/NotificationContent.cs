namespace HyperGames.UnityTemplate.UnityTemplate.Services
{
    public class NotificationContent
    {
        public string Title { get; set; }
        public string Body  { get; set; }

        public NotificationContent(string title, string body)
        {
            this.Title = title;
            this.Body  = body;
        }
    }
}