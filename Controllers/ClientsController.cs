using Microsoft.AspNetCore.Mvc;
using UsersApp.Data;
using UsersApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;


namespace UsersApp.Controllers
{
    public class ClientsController : Controller
    {
        private readonly AppDbContext dbContex;

        public ClientsController(AppDbContext dbContex)
        {
            this.dbContex = dbContex;
        }
        [Authorize]
        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddClientViewModel viewModel)
        {
            var client = new Client
            {
                name = viewModel.name,
                surname = viewModel.surname,
                phone = viewModel.phone

            };
            await dbContex.Clients.AddAsync(client);

            await dbContex.SaveChangesAsync();
            
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> List()
        {
            var clients=await dbContex.Clients.ToListAsync();

            return View(clients);
        }

        [HttpGet]
        public async Task<IActionResult> ListForRecord()
        {
            var clients = await dbContex.Clients.ToListAsync();

            return View(clients);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
           var client = await dbContex.Clients.FindAsync(id);
            
            return View(client);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Client viewModel)
        {
            var client=await dbContex.Clients.FindAsync(viewModel.id);
            if(client is not null)
            {
                client.name=viewModel.name; 
                client.surname=viewModel.surname;
                client.phone=viewModel.phone;

                await dbContex.SaveChangesAsync();

            }
            return RedirectToAction("List","Clients");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(Client viewModel)
        {
            var client = await dbContex.Clients
                .AsNoTracking()
                .FirstOrDefaultAsync(x=>x.id==viewModel.id);
            if (client is not null) 
            { 
                dbContex.Clients.Remove(viewModel);
                await dbContex.SaveChangesAsync();
            }

            return RedirectToAction("List", "Clients");
        }


    }
}
