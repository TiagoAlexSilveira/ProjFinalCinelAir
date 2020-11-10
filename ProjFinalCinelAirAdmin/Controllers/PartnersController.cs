using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjFinalCinelAir.CommonCore.Data.Entities;
using ProjFinalCinelAirAdmin.Data.Repositories;
using ProjFinalCinelAirAdmin.Helpers;
using ProjFinalCinelAirAdmin.Models;

namespace ProjFinalCinelAirAdmin.Controllers
{
    public class PartnersController : Controller
    {
        private readonly IPartnerRepository _partnerRepository;
        private readonly IUserHelper _userHelper;
        private readonly IImageHelper _imageHelper;

        public PartnersController(IPartnerRepository partnerRepository, IUserHelper userHelper, IImageHelper imageHelper)
        {
            _partnerRepository = partnerRepository;
            _userHelper = userHelper;
            _imageHelper = imageHelper;
        }

        public IActionResult Index()
        {
            return View(_partnerRepository.GetAll());

        }

        // GET: Partner/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {

                return NotFound();
            }

            var partner = await _partnerRepository.GetByIdAsync(id.Value);

            if (partner == null)
            {

                return NotFound();
            }


            return View(partner);
        }



        // GET: Airplanes/Create
        public IActionResult Create()
        {
            return View();
        }



        // POST: Partner/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PartnerViewModel model)
        {
            if (ModelState.IsValid)
            {

                var path = string.Empty;

                if (model.ImageFile != null && model.ImageFile.Length > 0)
                {
                    path = await _imageHelper.UpLoadImageAsync(model.ImageFile, "Partners");
                }

                var partner = convertModelInToClass(model, path);

                await _partnerRepository.CreateAsync(partner);

                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }

        private Partner convertModelInToClass(PartnerViewModel model, string path)
        {
            Partner partner = new Partner();

            partner.Name = model.Name;
            partner.Contact = model.Contact;
            partner.Description = model.Description;
            partner.ImageUrl = path;
            partner.isValidated = false;
            return partner;

        }

        private PartnerViewModel convertClassInToModel(Partner partner)
        {
            PartnerViewModel view = new PartnerViewModel();

            view.Name = partner.Name;
            view.Contact = partner.Contact;
            view.Description = partner.Description;
            view.ImageUrl = partner.ImageUrl;
            return view;

        }

        // GET: Partner/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {

                return NotFound();
            }

            Partner partner = await _partnerRepository.GetByIdAsync(id.Value);

            if (partner == null)
            {

                return NotFound();
            }

            var view = convertClassInToModel(partner);

            return View(view);
        }



        
        // POST: Partner/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(PartnerViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var path = string.Empty;

                    if (model.ImageFile != null && model.ImageFile.Length > 0)
                    {
                        path = await _imageHelper.UpLoadImageAsync(model.ImageFile, "Partners");

                    }

                    else
                    {
                        Partner oldPartner = await _partnerRepository.GetByIdAsync(model.Id);
                        path = oldPartner.ImageUrl;
                    }

                    Partner partner = new Partner() {

                        Name = model.Name,
                        Contact = model.Contact,
                        Description = model.Description,
                        ImageUrl = path,
                        Id = model.Id,
                    };                    
                 
                    await _partnerRepository.UpDateAsync(partner); // Método Update já grava as alterações

                    ViewBag.Message = "Partner Updated!";
                    return View();

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _partnerRepository.ExistsAsync(model.Id))
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



        
        // GET: Partner/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {

                return NotFound();
            }

            var partner = await _partnerRepository.GetByIdAsync(id.Value);

            if (partner == null)
            {

                return NotFound();
            }


            return View(partner);
        }



        // POST: Airplanes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (id == 0)
            {
                return NotFound();

            }

            var partner = await _partnerRepository.GetByIdAsync(id);

            if (partner == null)
            {
                return NotFound();

            }

            try
            {
                await _partnerRepository.DeleteAsync(partner); // Método já grava as alterações realizadas

                return RedirectToAction(nameof(Index));

            }
            catch (Exception) // Erro por algum motivo
            {
                ViewBag.Message = "Erro ao apagar!";

                return View();

            }

        }


        public IActionResult MyNotFound()
        {
            return View();

        }
    }

}
