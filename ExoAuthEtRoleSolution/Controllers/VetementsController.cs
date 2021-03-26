using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ExoAuthEtRoleSolution.Data;
using ExoAuthEtRoleSolution.Models.ExoAuthEtRoleSolution.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using ExoAuthEtRoleSolution.Authorization;

namespace ExoAuthEtRoleSolution.Controllers
{
    public class VetementsController : Controller
    {
        private ApplicationDbContext Contexte { get; }
        private IAuthorizationService AuthorizationService { get; }
        private UserManager<IdentityUser> UserManager { get; }

        public VetementsController(ApplicationDbContext context,
            IAuthorizationService authorizationService,
            UserManager<IdentityUser> userManager)
        {
            Contexte = context;
            AuthorizationService = authorizationService;
            UserManager = userManager;
        }

        // GET: Vetements
        public async Task<IActionResult> Index()
        {
            var vetements = from c in Contexte.Vetement select c;
            var isAuthorized = User.IsInRole(Constants.VetementAdministratorsRole);
            var currentUserId = UserManager.GetUserId(User);

            if (!isAuthorized)
                vetements = vetements.Where(v => v.ProprietaireId == currentUserId);
            return View(await vetements.ToListAsync());
        }

        // GET: Vetements/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();
            Vetement v = await Contexte.Vetement.FirstOrDefaultAsync(
                                             m => m.VetementId == id);
            if (v == null)
                return NotFound();
            
            var isAuthorized = await AuthorizationService.AuthorizeAsync(
                                      User, v, VetementOperations.Read);
            if (!isAuthorized.Succeeded)
                return Forbid();
 
            return View(v);
        }

        // GET: Vetements/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Vetements/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("VetementId,Nom")] Vetement vetement)
        {
            if (ModelState.IsValid)
            {

                vetement.ProprietaireId = UserManager.GetUserId(User);

                // requires using ContactManager.Authorization;
                var isAuthorized = await AuthorizationService.AuthorizeAsync(
                                                            User, vetement,
                                                            VetementOperations.Create);
                if (!isAuthorized.Succeeded)
                {
                    return Forbid();
                }

                Contexte.Add(vetement);
                await Contexte.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(vetement);
        }

        // GET: Vetements/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vetement = await Contexte
                .Vetement.AsNoTracking()
                .FirstOrDefaultAsync(m => m.VetementId == id);

            if (vetement == null)
            {
                return NotFound();
            }

            var isAuthorized = await AuthorizationService.AuthorizeAsync(
                                                     User, vetement,
                                                     VetementOperations.Update);
            if (!isAuthorized.Succeeded)
            {
                return Forbid();
            }

            return View(vetement);
        }

        // POST: Vetements/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("VetementId,Nom")] Vetement vetement)
        {
            if (id != vetement.VetementId)
            {
                return NotFound();
            }

            // Fetch Contact from DB to get OwnerID.
            var v = await Contexte
                .Vetement.AsNoTracking()
                .FirstOrDefaultAsync(m => m.VetementId == id);

            if (v == null)
            {
                return NotFound();
            }

            var isAuthorized = await AuthorizationService.AuthorizeAsync(
                                                     User, v,
                                                     VetementOperations.Update);
            if (!isAuthorized.Succeeded)
            {
                return Forbid();
            }
            
            vetement.ProprietaireId = v.ProprietaireId;
            
            try
            {
                Contexte.Vetement.Update(vetement);
                await Contexte.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!VetementExists(vetement.VetementId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            if (ModelState.IsValid)
            {
                return RedirectToAction(nameof(Index));
            }
            return View(vetement);
        }

        // GET: Vetements/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {

            if (id == null)
            {
                return NotFound();
            }

            var vetement = await Contexte.Vetement
                .FirstOrDefaultAsync(m => m.VetementId == id);

            if (vetement == null)
            {
                return NotFound();
            }

            var isAuthorized = await AuthorizationService.AuthorizeAsync(
                                                     User, vetement,
                                                     VetementOperations.Delete);
            if (!isAuthorized.Succeeded)
            {
                return Forbid();
            }

            return View(vetement);
        }

        // POST: Vetements/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var vetement = await Contexte.Vetement.FindAsync(id);

            if (vetement == null)
            {
                return NotFound();
            }

            var isAuthorized = await AuthorizationService.AuthorizeAsync(
                                                     User, vetement,
                                                     VetementOperations.Delete);
            if (!isAuthorized.Succeeded)
            {
                return Forbid();
            }

            Contexte.Vetement.Remove(vetement);
            await Contexte.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool VetementExists(int id)
        {
            return Contexte.Vetement.Any(e => e.VetementId == id);
        }
    }
}
