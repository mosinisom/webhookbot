public class Banneduser
{
    public int Id { get; set; }
    public int User_id { get; set; }
    public DateTime Date { get; set; }
    public string Reason { get; set; } = string.Empty;
}