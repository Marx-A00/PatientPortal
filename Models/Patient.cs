namespace PatientPortal.Models;

public class Patient
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public DateTime DOB { get; set; }
    public required string Email { get; set; }
    
}