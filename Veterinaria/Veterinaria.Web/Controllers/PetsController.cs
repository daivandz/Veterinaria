using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Veterinaria.Web.Models;

namespace Veterinaria.Web.Controllers
{
    public class PetsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult AllPets()
        {
            var pets = db.Pets.Include(o => o.Owner).Include(u => u.Owner.ApplicationUser).ToList();
            return View(pets);
        }

        // GET: Pets
        public ActionResult Index()
        {
            var user = User.Identity.GetUserId();
            var ow = db.Owners.Where(o => o.UserId == user).FirstOrDefault();
            var pets = db.Pets.Include(u => u.Owner).Where(p => p.OwnerId == ow.id).ToList();

            return View(pets);
        }

        // GET: Pets/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Pet pet = db.Pets.Find(id);
            if (pet == null)
            {
                return HttpNotFound();
            }
            return View(pet);
        }

        // GET: Pets/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Pets/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Pet pet, HttpPostedFileBase hpb)
        {
            if (ModelState.IsValid)
            {
                if (hpb != null)
                {
                    string nombreArchivo = System.IO.Path.GetFileName(hpb.FileName);
                    string filePath = "~/Content/img/perfil/" + pet.Name + "_" + nombreArchivo;
                    hpb.SaveAs(Server.MapPath(filePath));
                    pet.ImgUrl = pet.Name + "_" + nombreArchivo;
                }

                //Solo funciona si esta autenticado un Cliente
                var userId = User.Identity.GetUserId();
                //Funciona para traer al usuario de la base de datos
                var own = db.Owners.Where(o => o.UserId == userId).FirstOrDefault();
                //Agregamos el Id del Owner que buscamos
                pet.OwnerId = own.id;
                db.Pets.Add(pet);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(pet);
        }

        // GET: Pets/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Pet pet = db.Pets.Find(id);
            if (pet == null)
            {
                return HttpNotFound();
            }
            return View(pet);
        }

        // POST: Pets/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,PetType,Age,BirthDate,Color,Race,Weight,Height,Owner")] Pet pet)
        {
            if (ModelState.IsValid)
            {
                db.Entry(pet).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(pet);
        }

        // GET: Pets/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Pet pet = db.Pets.Find(id);
            if (pet == null)
            {
                return HttpNotFound();
            }
            return View(pet);
        }

        // POST: Pets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Pet pet = db.Pets.Find(id);
            db.Pets.Remove(pet);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
