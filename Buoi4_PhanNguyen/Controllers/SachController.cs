using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Buoi4_PhanNguyen.Models;
namespace Buoi4_PhanNguyen.Controllers
{
    public class SachController : Controller
    {
        MyDataDataContext context = new MyDataDataContext();

        // hiển thị danh sách sách
        public ActionResult listSach()
        {
            var all_sach = from ele in context.Saches select ele;
            return View(all_sach);
        }

        //method chi tiết
        public ActionResult Detail(int? id)
        {
            var detailBook = context.Saches.Where(p => p.masach == id).First();
            return View(detailBook);
        }

        //method thêm sách

        //hiển thị trang tạo cho user
        public ActionResult Create()
        {
            var listloai = context.TheLoais.ToList();
            ViewBag.theloai = new SelectList(listloai, "maloai", "tenloai");
            return View();
        }
        //lấy và lưu thông tin từ người dùng khi nhập
        [HttpPost]
        public ActionResult Create(FormCollection collection, Sach s)
        {
            var tensach = collection["tensach"];
            var loai = collection["loai"];
            var hinh = collection["hinh"];
            var giaban = Convert.ToDecimal(collection["giaban"]);
            var ngaycapnhat = Convert.ToDateTime(collection["ngaycapnhat"]);
            var soluong = Convert.ToInt32(collection["soluongton"]);
            if (string.IsNullOrEmpty(tensach))
                ViewData["Error"] = "Không Được Để Trống!!";
            else
            {
                s.tensach = tensach;
                s.hinh = hinh;
                s.maloai = int.Parse(loai);
                s.giaban = giaban;
                s.ngaycapnhat = ngaycapnhat;
                s.soluongton = soluong;
                context.Saches.InsertOnSubmit(s);
                context.SubmitChanges();
                return RedirectToAction("listSach");
            }
            return this.Create();
        }

        //method chỉ sửa

        //hiển thị trang cho user theo id sách
        public ActionResult Edit(int id)
        {
            var listloai = context.TheLoais.ToList();
            ViewBag.theloai = new SelectList(listloai, "maloai", "tenloai");
            var sach = context.Saches.First(m => m.masach == id);
            return View(sach);
        }
        //lấy và lưu thông tin từ người dùng khi nhập
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            var sach = context.Saches.First(m => m.masach == id);
            var tensach = collection["tensach"];
            var tenhinh = collection["hinh"];
            var loai = collection["loai"];
            var giaban = decimal.Parse(collection["giaban"]);
            var ngaycapnhat = Convert.ToDateTime(collection["ngaycapnhat"]);
            var soluong = Convert.ToInt32(collection["soluongton"]);
            sach.masach = id;
            if (string.IsNullOrEmpty(tensach))
                ViewData["Error"] = "Không Được Để Trống!!";
            else
            {
                
                sach.tensach = tensach;
                sach.hinh = tenhinh;
                sach.giaban = giaban;
                sach.maloai = int.Parse(loai);
                sach.ngaycapnhat = ngaycapnhat;
                sach.soluongton = soluong;
                UpdateModel(sach);
                context.SubmitChanges();
                return RedirectToAction("listSach");
            }
            return this.Edit(id);
        }

        //method xóa sách
        // ??
        public ActionResult Delete(int id)
        {
            var sach = context.Saches.First(p => p.masach == id);
            return View(sach);
        }
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            var sach = context.Saches.Where(p => p.masach == id).First();
            context.Saches.DeleteOnSubmit(sach);
            context.SubmitChanges();
            return RedirectToAction("listSach");
        }
        public ActionResult Index()
        {
            return View();
        }
        public string ProcessUpload(HttpPostedFileBase file)
        {
            if (file == null)
            {
                return "";
            }
            file.SaveAs(Server.MapPath("~/Content/images/" + file.FileName));
            return "/Content/images/" + file.FileName;
        }
    }
}