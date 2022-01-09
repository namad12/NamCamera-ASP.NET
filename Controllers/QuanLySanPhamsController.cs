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
    public class QuanLySanPhamsController : Controller
    {
        private Model1 db = new Model1();

        // GET: QuanLySanPhams
        public ActionResult Index(int? page)
        {
            var sanPhams = db.SanPhams.Include(s => s.BinhLuans);

            // sắp xếp phân trang
            sanPhams = sanPhams.OrderBy(e => e.SanPhamID);
            int pagesize = 5;

            int pageNumber = (page ?? 1);
            //return View(db.SanPhams.ToList());
            return View(sanPhams.ToPagedList(pageNumber, pagesize));
        }

        public ActionResult CapNhat(int? page)
        {
            var sanPhams = db.SanPhams.Include(s => s.BinhLuans);

            // sắp xếp phân trang
            sanPhams = sanPhams.OrderBy(e => e.SanPhamID);
            int pagesize = 10;

            int pageNumber = (page ?? 1);
            //return View(db.SanPhams.ToList());
            return View(sanPhams.ToPagedList(pageNumber, pagesize));
        }

        public ActionResult ThongKeHang()
        {
            int querySony = db.Database.SqlQuery<int>("select sum(SoLuong) from SanPham where Hang = 'Sony'").FirstOrDefault();
            int queryCanon = db.Database.SqlQuery<int>("select sum(SoLuong) from SanPham where Hang = 'Canon'").FirstOrDefault();
            int queryNikon = db.Database.SqlQuery<int>("select sum(SoLuong) from SanPham where Hang = 'Nikon'").FirstOrDefault();

            Session["Sony"] = querySony;
            Session["Canon"] = queryCanon;
            Session["Nikon"] = queryNikon;

            //int gia30 = db.Database.SqlQuery<int>("select count(TenSanPham) from SanPham where GiaNhap<30").FirstOrDefault();
            //int gia3070 = db.Database.SqlQuery<int>("select count(TenSanPham) from SanPham where GiaNhap>=30 and GiaNhap<=70").FirstOrDefault();
            //int gia70 = db.Database.SqlQuery<int>("select count(TenSanPham) from SanPham where GiaNhap>70").FirstOrDefault();

            //Session["Gia30"] = gia30;
            //Session["Gia3070"] = gia3070;
            //Session["Gia70"] = gia70;

            return View();
        }

        public ActionResult ThongKeMucGia()
        {
            //int querySony = db.Database.SqlQuery<int>("select sum(SoLuong) from SanPham where Hang = 'Sony'").FirstOrDefault();
            //int queryCanon = db.Database.SqlQuery<int>("select sum(SoLuong) from SanPham where Hang = 'Canon'").FirstOrDefault();
            //int queryNikon = db.Database.SqlQuery<int>("select sum(SoLuong) from SanPham where Hang = 'Nikon'").FirstOrDefault();

            //Session["Sony"] = querySony;
            //Session["Canon"] = queryCanon;
            //Session["Nikon"] = queryNikon;

            int gia30 = db.Database.SqlQuery<int>("select count(TenSanPham) from SanPham where GiaNhap<30").FirstOrDefault();
            int gia3070 = db.Database.SqlQuery<int>("select count(TenSanPham) from SanPham where GiaNhap>=30 and GiaNhap<=70").FirstOrDefault();
            int gia70 = db.Database.SqlQuery<int>("select count(TenSanPham) from SanPham where GiaNhap>70").FirstOrDefault();

            Session["Gia30"] = gia30;
            Session["Gia3070"] = gia3070;
            Session["Gia70"] = gia70;

            return View();
        }
        public ActionResult Xoa(int? page)
        {
            var sanPhams = db.SanPhams.Include(s => s.BinhLuans);

            // sắp xếp phân trang
            sanPhams = sanPhams.OrderBy(e => e.SanPhamID);
            int pagesize = 10;

            int pageNumber = (page ?? 1);
            //return View(db.SanPhams.ToList());
            return View(sanPhams.ToPagedList(pageNumber, pagesize));
        }
        // GET: QuanLySanPhams/Details/5
        public ActionResult Details(int? id)
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

        // GET: QuanLySanPhams/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: QuanLySanPhams/Create
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
                    sanPham.Anh01 = "";
                    sanPham.Anh02 = "";
                    sanPham.Anh03 = "";

                    var f1 = Request.Files["ImageFile1"];
                    if (f1 != null && f1.ContentLength> 0)
                    {
                        string FileName = System.IO.Path.GetFileName(f1.FileName);
                        string UploadPath = Server.MapPath("~/Content/CameraImages/" + FileName);
                        f1.SaveAs(UploadPath);
                        sanPham.Anh01 = FileName;
                    }

                    var f2 = Request.Files["ImageFile2"];
                    if (f2 != null && f2.ContentLength > 0)
                    {
                        string FileName = System.IO.Path.GetFileName(f2.FileName);
                        string UploadPath = Server.MapPath("~/Content/CameraImages/" + FileName);
                        f2.SaveAs(UploadPath);
                        sanPham.Anh02 = FileName;
                    }

                    var f3 = Request.Files["ImageFile3"];
                    if (f3 != null && f3.ContentLength > 0)
                    {
                        string FileName = System.IO.Path.GetFileName(f3.FileName);
                        string UploadPath = Server.MapPath("~/Content/CameraImages/" + FileName);
                        f3.SaveAs(UploadPath);
                        sanPham.Anh03 = FileName;
                    }

                    db.SanPhams.Add(sanPham);
                    db.SaveChanges();
                    return RedirectToAction("Index");

                } 
                else
                {
                    ViewBag.Error = "Lỗi nhập dữ liệu";
                    return View(sanPham);
                }


            }
            catch (Exception ex)
            {
                ViewBag.Error = "ID của máy ảnh phải là duy nhất";
                return View(sanPham);

            }


        }

        // GET: QuanLySanPhams/Edit/5
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

        // POST: QuanLySanPhams/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "SanPhamID,TenSanPham,Hang,GiaNhap,GiaBan,SoLuong,Anh01,Anh02,Anh03,ThongTin")] SanPham sanPham)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var f1 = Request.Files["ImageFile1"];
                    if (f1 != null && f1.ContentLength > 0)
                    {
                        string FileName = System.IO.Path.GetFileName(f1.FileName);
                        string UploadPath = Server.MapPath("~/Content/CameraImages/" + FileName);
                        f1.SaveAs(UploadPath);
                        sanPham.Anh01 = FileName;
                    }

                    var f2 = Request.Files["ImageFile2"];
                    if (f2 != null && f2.ContentLength > 0)
                    {
                        string FileName = System.IO.Path.GetFileName(f2.FileName);
                        string UploadPath = Server.MapPath("~/Content/CameraImages/" + FileName);
                        f2.SaveAs(UploadPath);
                        sanPham.Anh02 = FileName;
                    }

                    var f3 = Request.Files["ImageFile3"];
                    if (f3 != null && f3.ContentLength > 0)
                    {
                        string FileName = System.IO.Path.GetFileName(f3.FileName);
                        string UploadPath = Server.MapPath("~/Content/CameraImages/" + FileName);
                        f3.SaveAs(UploadPath);
                        sanPham.Anh03 = FileName;
                    }

                    db.Entry(sanPham).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("CapNhat");

                }
                else
                {
                    ViewBag.Error = "Lỗi nhập dữ liệu";
                    return View(sanPham);
                }
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Xuất hiện lỗi " + ex.InnerException.Message + "<br/> Bạn phải nhập lại dữ liệu vào tất cả các trường";
                return View(sanPham);
            }

        }

        // GET: QuanLySanPhams/Delete/5
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

        // POST: QuanLySanPhams/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            SanPham sanPham = db.SanPhams.Find(id);
            try
            {
                    db.SanPhams.Remove(sanPham);
                    db.SaveChanges();
                    return RedirectToAction("Xoa");
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Lỗi: " + ex.Message +"\n (Sản phẩm này đang là khóa ngoại của bảng Bình Luận)";
                return View("Delete", sanPham);
            }
            
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
