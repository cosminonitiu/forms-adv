namespace FormAdvanced.Domain.Entities
{
    public enum NotificationType
    {
        Approve
    }

    public record Notification(
        string id,
        string Owner,
        string Type,
        string FormRequestName,
        string SubmittedRequestId,
        string RequesterUID,
        string RequesterName
    );   
}