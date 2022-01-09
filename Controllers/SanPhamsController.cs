using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using NamCamera.Models;
using PagedList;

namespace NamCamera.Controllers
{
    public class SanPhamsController : Controller
    {
        private Model1 db = new Model1();

        // GET: SanPhams
        public ActionResult Index(string searchStr, int? page)
        {
            //Session["TenSanPham"] = searchStr;
            //string temp = (string)Session["SearchSanPham"];
            ViewBag.TimKiem = searchStr; // hiện chữ mã người dùng tìm kiếm
            var sanPhams = db.SanPhams.Include(s => s.BinhLuans);
            int pagesize;
            if ( !String.IsNullOrEmpty(searchStr))
            {
                sanPhams = sanPhams.Where(e => e.TenSanPham.Contains(searchStr));
                sanPhams = sanPhams.OrderBy(e => e.SanPhamID);
                pagesize = 100;
            } else
            {
                sanPhams = sanPhams.OrderBy(e => e.SanPhamID);
                pagesize = 8;
            }
           
            // sắp xếp phân trang
           

            int pageNumber = (page ?? 1);
            //return View(db.SanPhams.ToList());
            return View(sanPhams.ToPagedList(pageNumber, pagesize));
        }

        public ActionResult HangSX(string searchHang, int? pageHangSX)
        {
            var sanPhams = db.SanPhams.Include(s => s.BinhLuans);
            if (!String.IsNullOrEmpty(searchHang))
            {
                sanPhams = sanPhams.Where(e => e.Hang == searchHang );
            }
            Session["hangSX"] = searchHang;
            // sắp xếp phân trang
            sanPhams = sanPhams.OrderBy(e => e.SanPhamID);
            int pagesize = 100;

            int pageNumber = (pageHangSX ?? 1);
            //return View(db.SanPhams.ToList());
            return View(sanPhams.ToPagedList(pageNumber, pagesize));
        }

        public ActionResult MucGia(int searchGia, int? pageGia)
        {
            var sanPhams = db.SanPhams.Include(s => s.BinhLuans);
            if (searchGia == 30)
            {
                sanPhams = sanPhams.Where(e => e.GiaBan > 0 && e.GiaBan < 30);
                Session["mucGia"] = "Dưới $" + searchGia;

            }
            if (searchGia == 50)
            {
                sanPhams = sanPhams.Where(e => e.GiaBan > 30 && e.GiaBan < 70);
                Session["mucGia"] = "Từ $30 -> $70";

            }
            if (searchGia == 70)
            {
                sanPhams = sanPhams.Where(e => e.GiaBan > 70);
                Session["mucGia"] = "Trên $" + searchGia;

            }

            // sắp xếp phân trang
            sanPhams = sanPhams.OrderBy(e => e.SanPhamID);
            int pagesize = 100;

            int pageNumber = (pageGia ?? 1);
            //return View(db.SanPhams.ToList());
            return View(sanPhams.ToPagedList(pageNumber, pagesize));
        }

        // GET: SanPhams/Details/5
        public ActionResult Details(int? id, string comment)
        {
            if ((string)Session["user"] == null)
            {
                ViewBag.Error = "Bạn phải đăng nhập mới có thể bình luận";
            } else
            {
                if (comment != null)
                {
                    Session["IDTenSanPham"] = id;
                    int queryTenID = db.Database.SqlQuery<int>("Select TenID from TaiKhoan where TenDangNhap = '" + (string)Session["user"] + "'").FirstOrDefault();
                    Session["TenUserBinhLuan"] = queryTenID;
                    Session["NoiDungBinhLuan"] = comment;

                    int queryCount = db.Database.SqlQuery<int>("select count(BinhLuanID) from BinhLuan").FirstOrDefault();
                    var t = new BinhLuan(queryCount + 1, (int)Session["TenUserBinhLuan"], (int)Session["IDTenSanPham"], (string)Session["NoiDungBinhLuan"]);
                    int idSP = (int)Session["IDTenSanPham"];
                    db.BinhLuans.Add(t);
                    db.SaveChanges();
                    //return RedirectToAction("TaoBinhLuan", "BinhLuans");
                    return RedirectToAction("Details", "SanPhams", new { id = idSP });
                }
                
            }

            List<string> lstCmt = new List<string>();
            List<string> lstTen = new List<string>();
            int countCmt = db.Database.SqlQuery<int>("select count(NoiDung) from BinhLuan inner join SanPham on SanPham.SanPhamID = BinhLuan.SanPhamID inner join TaiKhoan on TaiKhoan.TenID = BinhLuan.TenID where SanPham.SanPhamID = '"+ id +"' ").FirstOrDefault();
            string queryNoiDung;
            string queryTen;

            for (int i=1; i<100; i++)
            {
                queryNoiDung = db.Database.SqlQuery<string>("select NoiDung from BinhLuan inner join SanPham on SanPham.SanPhamID = BinhLuan.SanPhamID inner join TaiKhoan on TaiKhoan.TenID = BinhLuan.TenID where SanPham.SanPhamID = '" + id + "' and  BinhLuanID='"+ i +"' ").FirstOrDefault();
                queryTen = db.Database.SqlQuery<string>("select TaiKhoan.TenDangNhap from BinhLuan inner join SanPham on SanPham.SanPhamID = BinhLuan.SanPhamID inner join TaiKhoan on TaiKhoan.TenID = BinhLuan.TenID where SanPham.SanPhamID = '" + id + "' and  BinhLuanID='" + i + "' ").FirstOrDefault();
                if (queryTen != null && queryNoiDung != null)
                {
                    lstCmt.Add(queryNoiDung);
                    lstTen.Add(queryTen);
                }
            }

            Session["countCmt"] = countCmt;
            Session["NoiDung"] = lstCmt;
            Session["Ten"] = lstTen;

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SanPham sanPham = db.SanPhams.Find(id);
            if (sanPham == null)
            {
                return HttpNotFound();
            }
            return View(sanPham);
        }


        // GET: SanPhams/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: SanPhams/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "SanPhamID,TenSanPham,Hang,GiaNhap,GiaBan,SoLuong,Anh01,Anh02,Anh03,ThongTin")] SanPham sanPham)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.SanPhams.Add(sanPham);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                ViewBag.Error = "ID Unique " + ex;
                return View("Create");
            }

            return View(sanPham);
        }

        // GET: SanPhams/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SanPham sanPham = db.SanPhams.Find(id);
            if (sanPham == null)
            {
                return HttpNotFound();
            }
            return View(sanPham);
        }

        // POST: SanPhams/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "SanPhamID,TenSanPham,Hang,GiaNhap,GiaBan,SoLuong,Anh01,Anh02,Anh03,ThongTin")] SanPham sanPham)
        {
            if (ModelState.IsValid)
            {
                db.Entry(sanPham).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(sanPham);
        }

        // GET: SanPhams/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SanPham sanPham = db.SanPhams.Find(id);
            if (sanPham == null)
            {
                return HttpNotFound();
            }
            return View(sanPham);
        }

        // POST: SanPhams/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            SanPham sanPham = db.SanPhams.Find(id);
            db.SanPhams.Remove(sanPham);
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
