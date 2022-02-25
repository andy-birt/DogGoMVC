using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using DogGoMVC.Repositories;
using DogGoMVC.Models;
using DogGoMVC.Models.ViewModels;
using System.Collections.Generic;
using System.Linq;
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
            // Initialize the list of all walks before initializing items that can be deleted
            List<Walk> walks = _walkRepo.GetAllWalks();
            
            // This list will contain objects with bools that we'll need for the checkboxes
            List<ItemToDelete> items = new List<ItemToDelete>();

            for (int i = 0; i < walks.Count; i++)
            {
                // Create a new item that can be deleted
                ItemToDelete item = new ItemToDelete()
                {
                    // The id will be the id of the item we want to delete
                    Id = walks[i].Id,

                    // The bool will determine whether or not it gets deleted
                    IsToBeDeleted = false
                };

                // Add the item to the list
                items.Add(item);
            };

            WalksDeleteFormModel vm = new WalksDeleteFormModel()
            {
                // A new walk just so we can reference the model
                Walk = new Walk(),
                // Our original list of walks like any other index wants
                Walks = walks,
                Items = items
            };
            return View(vm);
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
                // It's also worth noting that the request is implicit of the controller object
                // That's why, you will notice, it's not declared anywhere but I can still access it
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
                var ids = Request.Form;
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // POST: WalksController/Delete
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult MultiDelete(List<ItemToDelete> items)
        {
            try
            {

                // Get all of the ids from the selected walks in the index to delete
                // This will return all of the checkboxes but will filter out the ones not selected
                // Returning only the checkboxes attached to walk ids
                var ids = Request.Form.Select(field => {
                    if (Int32.TryParse(field.Value[0], out int id))
                    {
                        return id;
                    }
                    return 0;
                }).Where(id => id > 0).ToList();

                _walkRepo.DeleteWalks(ids);       

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return RedirectToAction(nameof(Index));
            }
        }
    }
}
