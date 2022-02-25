using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using DogGoMVC.Repositories;
using DogGoMVC.Models;
using DogGoMVC.Models.ViewModels;
using System.Collections.Generic;

namespace DogGoMVC.Controllers
{
    public class WalksController : Controller
    {
        private readonly IWalkRepository _walkRepo;
        private readonly IWalkerRepository _walkerRepo;
        private readonly IDogRepository _dogRepo;

        public WalksController(IWalkRepository walkRepo, IWalkerRepository walkerRepo, IDogRepository dogRepo)
        {
            _walkRepo = walkRepo;
            _walkerRepo = walkerRepo;
            _dogRepo = dogRepo;
        }

        // GET: WalksController
        public ActionResult Index()
        {
            List<Walk> walks = _walkRepo.GetAllWalks();
            return View(walks);
        }

        // GET: WalksController/Details/5
        //public ActionResult Details(int id)
        //{
        //    return View();
        //}

        // GET: WalksController/Create
        public ActionResult Create()
        {
            WalkFormViewModel vm = new WalkFormViewModel()
            {
                Walk = new Walk(),
                Walkers = _walkerRepo.GetAllWalkers(),
                Dogs = _dogRepo.GetAllDogs()
            };
            return View(vm);
        }

        // POST: WalksController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Walk walk)
        {
            try
            {
                _walkRepo.AddWalk(walk);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                WalkFormViewModel vm = new WalkFormViewModel()
                {
                    Walk = new Walk(),
                    Walkers = _walkerRepo.GetAllWalkers(),
                    Dogs = _dogRepo.GetAllDogs()
                };
                return View(vm);
            }
        }

        // GET: WalksController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: WalksController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: WalksController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: WalksController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
