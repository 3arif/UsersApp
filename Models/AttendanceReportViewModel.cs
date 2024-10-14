namespace UsersApp.Models
{
    public class AttendanceReportViewModel
    {
        public Guid ClientId { get; set; }
        public string ClientName { get; set; }
        public int Workdays { get; set; }
        public double TotalHours { get; set; }
        public bool QualifiesForSalary { get; set; }
    }
}
