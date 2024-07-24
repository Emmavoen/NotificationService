namespace NotificationService.Domain.Entity;

public class NotificationActivity
{

    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }
    public string SentTo { get; set; }
    public DateTime SentAt { get; set; } = DateTime.UtcNow.AddHours(1);
    public bool Status { get; set; }
    public string Purpose { get; set; }
    public NotificationType NotificationType { get; set; }
    public bool HasAttachment { get; set; }

}
