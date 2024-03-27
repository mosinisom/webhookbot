public class User
{
    public int Id { get; set; }
    public long Chat_id { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Institute { get; set; } = string.Empty;
    public int Year  { get; set; }
    public string Description { get; set; } = string.Empty;
    public int LikesCount { get; set; }
}