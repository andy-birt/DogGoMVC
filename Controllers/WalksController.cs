using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using DogGoMVC.Repositories;
using DogGoMVC.Models;
using DogGoMVC.Models.ViewModels;
using System.Collections.Generic;
using System;

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
                // Get the values from multiselect in create form before creating new walks
                // They will be in Key/Value pairs but I just want the value since it's iterable
                // The iterable variable dogIds will have each value that was selected
                var dogIds = Request.Form["Walk.DogId"];

                // Iterate over the ids that were selected in the form and create a new record
                // for each of the values that were selected in the multiselect
                foreach (var dogId in dogIds)
                {
                    // They will be string so convert to int then
                    // let the repo take care of the work
                    walk.DogId = Int32.Parse(dogId);
                    _walkRepo.AddWalk(walk);
                }
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                // It's important to replicate the Get functionality into the catch of the Post
                // I was experiencing errors here as well as the OwnersController and couldn't
                // figure out why... turns out this did the trick.. maybe not best solution
                // but it doesn't break now at least.
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
