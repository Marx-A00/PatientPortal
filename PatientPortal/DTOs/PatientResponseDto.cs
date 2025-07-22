namespace PatientPortal.DTOs;

public class PatientResponseDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public DateTime DOB { get; set; }
    public string Email { get; set; } = string.Empty;
    public int Age => CalculateAge(DOB);

    private static int CalculateAge(DateTime dateOfBirth)
    {
        var today = DateTime.Today;
        var age = today.Year - dateOfBirth.Year;
        if (dateOfBirth.Date > today.AddYears(-age)) age--;
        return age;
    }

    public static PatientResponseDto FromPatient(Models.Patient patient)
    {
        return new PatientResponseDto
        {
            Id = patient.Id,
            Name = patient.Name,
            DOB = patient.DOB,
            Email = patient.Email
        };
    }
}