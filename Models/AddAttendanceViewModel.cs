using System.ComponentModel.DataAnnotations;

namespace UsersApp.Models
{
    public class AddAttendanceViewModel : IValidatableObject
    {
        public Guid ClientId { get; set; }
        public string name {  get; set; }
        public DateOnly date { get; set; }
        public TimeOnly timeStart { get; set; }
        public TimeOnly timeEnd { get; set; }
        public AbsenceType Absence { get; set; } = AbsenceType.Present;

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Absence != AbsenceType.Present)
            {
                if (timeStart != TimeOnly.MinValue || timeEnd != TimeOnly.MinValue)
                {
                    yield return new ValidationResult(
                        "For absences other than 'Present', timeStart and timeEnd must be set to 00:00.",
                        new[] { nameof(timeStart), nameof(timeEnd) });
                }
            }
        }
    }
}
