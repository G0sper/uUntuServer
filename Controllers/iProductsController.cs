using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using uUntu;
using Microsoft.AspNet.Identity;

namespace uUntu.Controllers
{
    public class iProductsController : Controller
    {
        private EntityDtsEntities db = new EntityDtsEntities();

        // GET: iProducts
        public async Task<ActionResult> Index()
        {
            return View(await db.iProducts.ToListAsync());
        }

        [HttpPost]
        public async Task<ActionResult> OrderSearch(string searchString)
        {
            return View(await db.iProducts.Where(p => p.Name.Contains(searchString)).ToListAsync());
        }

        public ActionResult AddBasket(int productId)
        {
            var eprod = db.iProducts.Find(productId);

            if (eprod != null)
            {
                iCart basket = new iCart();


                basket.product_id = productId;
                basket.account_id = User.Identity.GetUserId();
                basket.product_name = eprod.Name;
                basket.product_descript = eprod.Description;
                basket.product_price = eprod.Price;
                basket.product_qua = 1;
                basket.product_img = eprod.Img;

                db.iCarts.Add(basket);
                db.SaveChanges();
            }

            return Redirect("/");
        }

        // GET: iProducts/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            iProduct iProduct = await db.iProducts.FindAsync(id);
            if (iProduct == null)
            {
                return HttpNotFound();
            }
            return View(iProduct);
        }

        // GET: iProducts/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: iProducts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Name,Description,Price,Img")] iProduct iProduct)
        {
            if (ModelState.IsValid)
            {
                db.iProducts.Add(iProduct);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(iProduct);
        }

        // GET: iProducts/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            iProduct iProduct = await db.iProducts.FindAsync(id);
            if (iProduct == null)
            {
                return HttpNotFound();
            }
            return View(iProduct);
        }

        // POST: iProducts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Name,Description,Price,Img")] iProduct iProduct)
        {
            if (ModelState.IsValid)
            {
                db.Entry(iProduct).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(iProduct);
        }

        // GET: iProducts/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            iProduct iProduct = await db.iProducts.FindAsync(id);
            if (iProduct == null)
            {
                return HttpNotFound();
            }
            return View(iProduct);
        }

        // POST: iProducts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            iProduct iProduct = await db.iProducts.FindAsync(id);
            db.iProducts.Remove(iProduct);
            await db.SaveChangesAsync();
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
