using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjFinalCinelAir.CommonCore.Data.Entities;
using ProjFinalCinelAirAdmin.Data.Repositories;

namespace ProjFinalCinelAirAdmin.Controllers
{
    [Authorize(Roles = "Admin, SuperUser, RegularUser")]

    public class AwardTicketController : Controller
    {

        private readonly IAwardTicketRepository _awardTicketRepository;



        public AwardTicketController(IAwardTicketRepository awardTicketRepository)
        {
            _awardTicketRepository = awardTicketRepository;
        }


        public IActionResult Index()
        {
            var list = _awardTicketRepository.GetAll().ToList();
            return View(list);
            
        }

        // GET: AwardTicket/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {

                return NotFound();
            }

            var partner = await _awardTicketRepository.GetByIdAsync(id.Value);

            if (partner == null)
            {

                return NotFound();
            }


            return View(partner);
        }

        // GET: Partner/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {

                return NotFound();
            }

            AwardTicket awardTicket = await _awardTicketRepository.GetByIdAsync(id.Value);

            if (awardTicket == null)
            {

                return NotFound();
            }

            return View(awardTicket);
        }




        // POST: Partner/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(AwardTicket model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _awardTicketRepository.UpDateAsync(model); // Método Update já grava as alterações

                    ViewBag.Message = "Award Ticket Updated!";
                    return View();

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _awardTicketRepository.ExistsAsync(model.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

            }
            return View(model);
        }


    }
}
