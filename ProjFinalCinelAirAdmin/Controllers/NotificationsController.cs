using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjFinalCinelAir.CommonCore.Data;
using ProjFinalCinelAir.CommonCore.Data.Entities;
using ProjFinalCinelAirAdmin.Data;
using ProjFinalCinelAirAdmin.Helpers;
using ProjFinalCinelAirAdmin.Models;


namespace ProjFinalCinelAirAdmin.Controllers
{
    public class RichTextEditorModel
    {
        [Required(ErrorMessage = "Value is required")]
        // Specify AllowHtml attribute on MVC application alone
        [AllowHtml]
        public string Value { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public int Id { get; set; }

        public int ClientId { get; set; }

        public string Message { get; set; }

        public string Subject { get; set; }
    }

    [Authorize(Roles = "Admin, SuperUser, RegularUser")]
    public class NotificationsController : Controller
    {
        private readonly DataContext _context;
        private readonly IClientHelper _clientHelper;
        private readonly IMailHelper _mailHelper;
        private readonly IUserHelper _userHelper;
        RichTextEditorModel rteModel = new RichTextEditorModel();

        public NotificationsController(DataContext context, IClientHelper clientHelper, IMailHelper mailHelper, IUserHelper userHelper)
        {
            _context = context;
            _clientHelper = clientHelper;
            _mailHelper = mailHelper;
            _userHelper = userHelper;
        }

        public IActionResult Index()
        {

            // Obter todas as notificações
            List<Notification> notifications = _context.Notification.Include(x => x.Client).Where(x => x.Subject.Contains("Complaint")).ToList();

            List<NotificationsViewModel> ListviewModel = new List<NotificationsViewModel>();

            foreach (var item in notifications)
            {

                NotificationsViewModel model = new NotificationsViewModel()
                {
                    Client = item.Client,
                    ClientId = item.ClientId,
                    Message = item.Message,
                    Subject = "Reply",
                    Date = item.Date,
                    Id = item.Id

                };

                ListviewModel.Add(model);

            }
            return View(ListviewModel);
        }


        // GET: Notification/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {

                return NotFound();
            }

            Notification notification = _context.Notification.Where(x => x.Id == id).FirstOrDefault();


            if (notification == null)
            {

                return NotFound();
            }

            Client client = await _clientHelper.GetClientByIdAsync(notification.ClientId);

            rteModel.Value = "<p>Dear client,</p>";
            rteModel.Id = notification.Id;
            rteModel.Message = notification.Message;
            rteModel.Subject = notification.Subject;
            rteModel.FirstName = client.FirstName;
            rteModel.LastName = client.LastName;
            rteModel.ClientId = client.Id;

           // model.richTextEditor = rteModel;
            return View(rteModel);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        // Post: Notification/Edit/5
        public async Task<IActionResult> Edit(RichTextEditorModel model)
        {
            Notification notification = _context.Notification.Where(x => x.Id == model.Id).FirstOrDefault();

            if (notification == null)
            {
                return NotFound();
            }

            notification.isRepliedByEmployee = true; // Reclamação foi respondida

            //Enviar um email ao cliente
            Client client = await _clientHelper.GetClientByIdAsync(notification.ClientId);
            User user = await _userHelper.GetUserByIdAsync(client.UserId);

            _mailHelper.SendMail(user.UserName, "CinelAir Replay Complaint", $"<h1>CinelAir Reply</h1>" +
                  $"{model.Value }");

            ViewBag.Message = "Reply sent to the client";

            _context.Notification.Update(notification);
            _context.SaveChanges();

            return View();


        }

    }
}
