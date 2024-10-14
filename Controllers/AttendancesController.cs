using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using UsersApp.Data;
using UsersApp.Models;

namespace UsersApp.Controllers
{
    public class AttendancesController : Controller
    {
        private readonly AppDbContext dbContex;

        public AttendancesController(AppDbContext dbContex)
        {
            this.dbContex = dbContex;
        }
        [Authorize]

        [HttpGet]
        public async Task<IActionResult> Add(Guid id, string name)
        {
            var client = await dbContex.Clients.FindAsync(id);
            var attendance = new AddAttendanceViewModel();
            attendance.ClientId = id;
            attendance.name= client.name;
            return View(attendance);
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddAttendanceViewModel viewModel)
        {
            //var attendance = await dbContex.Attendances.FindAsync(viewModel.Id);
            if (viewModel.Absence != AbsenceType.Present)
            {
                viewModel.timeStart = TimeOnly.MinValue; // 00:00
                viewModel.timeEnd = TimeOnly.MinValue;   // 00:00
            }
            var attendance = new Attendance()
            {
                ClientId = viewModel.ClientId,
                date = viewModel.date,
                timeStart = viewModel.timeStart,
                timeEnd = viewModel.timeEnd,
                Absence = viewModel.Absence

            };
            await dbContex.Attendance.AddAsync(attendance);
            await dbContex.SaveChangesAsync();
            return View();
            // return RedirectToAction("ListToRecord", "Clients");
        }

        [HttpGet]
        public async Task<IActionResult> ListRecords(Guid id)
        {
            var client = await dbContex.Clients.FindAsync(id);
            var attendances = await dbContex.Attendance
                 .Where(x=>x.ClientId==id)
                .ToListAsync();
            var viewModel = new ListAttendanceViewModel
            {
                name = $"{client.name}",
                attendance = attendances
            };
            
            return View(viewModel);
        }

        public async Task<IActionResult> Delete(Guid id)
        {
            
            var attendance = await dbContex.Attendance.FindAsync(id);
            if (attendance == null)
            {
                return NotFound(); // if record doesn't exist, return 404 error
            }
            dbContex.Attendance.Remove(attendance);
            await dbContex.SaveChangesAsync();
           

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult AttendanceReport(Guid clientId)
        {
            var client = dbContex.Clients.FirstOrDefault(c => c.id == clientId);
            if (client == null)
            {
                return NotFound();
            }

            
            ViewBag.ClientName = client.name + " " + client.surname;
            ViewBag.ClientId = clientId;

            return View();
        }

        [HttpPost]
        public IActionResult CalculateAttendance(Guid clientId, DateOnly startDate, DateOnly endDate)
        {
            var client = dbContex.Clients.Include(c => c.Attendances)
                                         .FirstOrDefault(c => c.id == clientId);
            if (client == null)
            {
                return NotFound();
            }
            var attendancesInRange = client.Attendances
                .Where(a => a.date >= startDate && a.date <= endDate)
                .ToList();
            var presentAttendances = attendancesInRange
                .Where(a => a.Absence == AbsenceType.Present)
                .ToList();
            var workdays = presentAttendances.Count();
            var totalHours = presentAttendances.Sum(a => (a.timeEnd - a.timeStart).TotalHours);
            bool hasUnannouncedAbsence = attendancesInRange.Any(a => a.Absence == AbsenceType.Unannounced);
            var totalRecordedDays = attendancesInRange.Count();

            bool qualifiesForSalary = (totalRecordedDays >= 20) &&
                                      ((workdays >= 20 && !hasUnannouncedAbsence) ||
                                       (workdays < 20 && !hasUnannouncedAbsence));
            var model = new AttendanceReportViewModel
            {
                ClientId = clientId,
                ClientName = $"{client.name} {client.surname}",
                Workdays = workdays,
                TotalHours = totalHours,
                QualifiesForSalary = qualifiesForSalary
            };
            return View("AttendanceReportResult", model);
        }


    }

}

