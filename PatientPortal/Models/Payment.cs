namespace PatientPortal.Models;

public class Payment
{
    public int Id { get; set; }
    public required string CheckNumber { get; set; }
    public decimal Amount { get; set; }
    public required string Status { get; set; }

    public int PatientId { get; set; }

    public Patient Patient { get; set; } = null!;

    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedDate { get; set; }

}