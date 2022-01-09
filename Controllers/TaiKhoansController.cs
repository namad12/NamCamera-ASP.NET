using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using NamCamera.Models;

namespace NamCamera.Controllers
{
    public class TaiKhoansController : Controller
    {
        private Model1 db = new Model1();

        // GET: TaiKhoans
        public ActionResult Index()
        {
            //return View(db.TaiKhoans.ToList());
            return View();
        }

        public ActionResult CheckLogin(string username, string password)
        {
            // lấy ra số tài khoản trong bảng TaiKhoan
            Int32 queryCount = db.Database.SqlQuery<Int32>("select count(TenID) from TaiKhoan").FirstOrDefault(); 

            for(int i=1; i<=queryCount; i++)
            {
                string queryUser = db.Database.SqlQuery<string>("select TenDangNhap from TaiKhoan where TenID = '" + i + "'").FirstOrDefault();
                string queryPass = db.Database.SqlQuery<string>("select MatKhau from TaiKhoan where TenID = '" + i + "'").FirstOrDefault();
                string queryType = db.Database.SqlQuery<string>("select Loai from TaiKhoan where TenID = '" + i + "'").FirstOrDefault();
                if (username == queryUser && password == queryPass)
                {
                   if (queryType == "user")
                    {
                        Session["user"] = username;
                        //Session["pass"] = password;
                        return RedirectToAction("Index", "SanPhams");
                    }
                   else
                    {
                        Session["user"] = username;
                        //Session["pass"] = password;
                        return RedirectToAction("Index", "QuanLySanPhams");
                    }
                }
            
            }
            ViewBag.error = "Tên đăng nhập hoặc mật khẩu không đúng";
            return View("Index");
        }

        public ActionResult Logout()
        {
            Session.Remove("user");
            return RedirectToAction("Index", "SanPhams");
        }

        // GET: TaiKhoans/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TaiKhoan taiKhoan = db.TaiKhoans.Find(id);
            if (taiKhoan == null)
            {
                return HttpNotFound();
            }
            return View(taiKhoan);
        }

        // GET: TaiKhoans/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: TaiKhoans/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "TenID,TenDangNhap,MatKhau,Loai")] TaiKhoan taiKhoan)
        {
            if (ModelState.IsValid)
            {
                db.TaiKhoans.Add(taiKhoan);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(taiKhoan);
        }

        // GET: TaiKhoans/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TaiKhoan taiKhoan = db.TaiKhoans.Find(id);
            if (taiKhoan == null)
            {
                return HttpNotFound();
            }
            return View(taiKhoan);
        }

        // POST: TaiKhoans/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "TenID,TenDangNhap,MatKhau,Loai")] TaiKhoan taiKhoan)
        {
            if (ModelState.IsValid)
            {
                db.Entry(taiKhoan).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(taiKhoan);
        }

        // GET: TaiKhoans/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TaiKhoan taiKhoan = db.TaiKhoans.Find(id);
            if (taiKhoan == null)
            {
                return HttpNotFound();
            }
            return View(taiKhoan);
        }

        // POST: TaiKhoans/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TaiKhoan taiKhoan = db.TaiKhoans.Find(id);
            db.TaiKhoans.Remove(taiKhoan);
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
