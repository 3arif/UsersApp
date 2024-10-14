namespace UsersApp.Models
{
    public class Client
    {
        public Guid id { get; set; }
        public string name { get; set; }
        public string surname { get; set; }
        public string phone {  get; set; }

        public ICollection<Attendance> Attendances { get;  }=new List<Attendance>();

    }
}
