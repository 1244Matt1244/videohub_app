namespace VideoApp.Domain.Entities;

public class Payment
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public Guid? VideoId { get; set; }
    public string StripeSessionId { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public string Status { get; set; } = "pending";
    public DateTime CreatedAt { get; set; }
}
