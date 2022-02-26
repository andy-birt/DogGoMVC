using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using DogGoMVC.Repositories;
using DogGoMVC.Models;
using System.Collections.Generic;
using System;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace DogGoMVC.Controllers
{
    public class DogsController : Controller
    {
        private readonly IDogRepository _dogRepo;

        public DogsController(IDogRepository dogRepo)
        {
            _dogRepo = dogRepo;
        }

        // GET: DogsController
        [Authorize]
        public ActionResult Index()
        {
            int ownerId = GetCurrentUserId();
            List<Dog> dogs = _dogRepo.GetDogsByOwnerId(ownerId);
            return View(dogs);
        }

        // GET: DogsController/Details/5
        public ActionResult Details(int id)
        {
            Dog dog = _dogRepo.GetDogById(id);
            return View(dog);
        }

        // GET: DogsController/Create
        [Authorize]
        public ActionResult Create()
        {
            return View();
        }

        // POST: DogsController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Create(Dog dog)
        {
            try
            {
                dog.OwnerId = GetCurrentUserId();
                _dogRepo.AddDog(dog);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: DogsController/Edit/5
        [Authorize]
        public ActionResult Edit(int id)
        {
            Dog dog = _dogRepo.GetDogById(id);

            int currentUserId = GetCurrentUserId();

            if (dog == null || dog.OwnerId != currentUserId)
            {
                return NotFound();
            }

            return View(dog);
        }

        // POST: DogsController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Edit(int id, Dog dog)
        {
            int currentUserId = GetCurrentUserId();

            try
            {
                if (dog == null || dog.OwnerId != currentUserId)
                {
                    throw new Exception();
                }
                _dogRepo.UpdateDog(dog);
                return RedirectToAction(nameof(Index));
            }
            catch(Exception ex)
            {
                return View(dog);
            }
        }

        // GET: DogsController/Delete/5
        [Authorize]
        public ActionResult Delete(int id)
        {
            int currentUserId = GetCurrentUserId();
            Dog dog = _dogRepo.GetDogById(id);
            if (currentUserId != dog.OwnerId)
            {
                return NotFound();
            }
            return View(dog);
        }

        // POST: DogsController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Delete(int id, Dog dog)
        {
            int currentUserId = GetCurrentUserId();

            try
            {
                if (dog == null || dog.OwnerId != currentUserId)
                {
                    throw new Exception();
                }
                _dogRepo.DeleteDog(id);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View(dog);
            }
        }

        private int GetCurrentUserId()
        {
            string id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return int.Parse(id);
        }
    }
}
