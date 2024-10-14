using System.ComponentModel.DataAnnotations.Schema;

namespace UsersApp.Models
{
    public class Attendance
    {
        public Guid Id { get; set; }

        public Guid ClientId { get; set; }
        public Client Client { get; set; } = null!;

        public DateOnly date {  get; set; }
        public TimeOnly timeStart { get; set; }
        public TimeOnly timeEnd { get; set; }
        public AbsenceType Absence { get; set; }

    }

    public enum AbsenceType
    {
        Present,
        Announced,
        Unannounced,
        Vacation,
        Sickness,
        DayOff
    }
}
