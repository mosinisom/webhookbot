public class Messages
{
    public int Id { get; set; }
    public long From_chat_id { get; set; }
    public long To_chat_id { get; set; }
    public string Text { get; set; } = string.Empty;
    public DateTime Date { get; set; }
}