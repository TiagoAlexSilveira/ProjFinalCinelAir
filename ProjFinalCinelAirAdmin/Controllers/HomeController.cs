using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ProjFinalCinelAir.CommonCore.Data;
using ProjFinalCinelAir.CommonCore.Data.Entities;
using ProjFinalCinelAirAdmin.Helpers;
using ProjFinalCinelAirAdmin.Models;

namespace ProjFinalCinelAirAdmin.Controllers
{
    [Authorize(Roles = "Admin, SuperUser, RegularUser")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IClientHelper _clientHelper;
        private readonly DataContext _context;

        public HomeController(ILogger<HomeController> logger, IClientHelper clientHelper, DataContext context)
        {
            _logger = logger;
            _clientHelper = clientHelper;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            GenericViewModel model = new GenericViewModel();

            if (this.User.Identity.IsAuthenticated && this.User.IsInRole("Admin"))
            {
                // Seleccionar todos os clientes q  ue se registaram na aplicação E e precisam de confirmação do Admin
                model.Clients = _clientHelper.GetClientsToValidate();

                return View(model);
                

            }

            else if (this.User.Identity.IsAuthenticated && this.User.IsInRole("SuperUser"))
            {
                model.Partners = _context.Partner.Where(x => x.isValidated == false).ToList(); // Lista de Parceiros que não se encontram validados        
                model.AwardTickets = _context.AwardTicket.Where(x => x.isValidated == false && x.Seats > 0).ToList();


                List<Client> clientsList = new List<Client>();
                // LIsta de Notificações que necessitam de resposta do SuperUser
                List<Notification> NotificationList = _context.Notification.Where(x => x.Subject.Contains("Complaint") && x.isRepliedByEmployee == false).ToList();


                foreach (var item in NotificationList)
                {
                    Client client = await _clientHelper.GetClientByIdAsync(item.ClientId);
                    NotificationsViewModel notificationsViewModel = new NotificationsViewModel()
                    {
                        Client = client,
                        ClientId = client.Id,
                        Id = item.Id,
                        Message = item.Message,
                        Date = item.Date,
                        Subject = item.Subject
                    };

                    model.Notifications.Add(notificationsViewModel);

                }
               
             
                return View(model);


            }

            return View();
        }

        public IActionResult Privacy(string id)
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
