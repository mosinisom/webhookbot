public class Banneduser
{
    int Banneduser_id { get; set; }
    int User_id { get; set; }
    DateTime Date { get; set; }
    string Reason { get; set; } = string.Empty;
}