using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Buoi4_PhanNguyen.Models;
namespace Buoi4_PhanNguyen.Controllers
{
    public class TheLoaiController : Controller
    {
        MyDataDataContext contex = new MyDataDataContext();

        // GET: TheLoai
        public ActionResult Index()
        {
            var all_type = from tt in contex.TheLoais select tt;
            return View(all_type);
        }

        public ActionResult Detail(int id)
        {
            var D_theloai = contex.TheLoais.Where(m => m.maloai == id).First();
            return View(D_theloai);
        }

        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(FormCollection collection, TheLoai tl)
        {
            var ten = collection["tenloai"];
            if (string.IsNullOrEmpty(ten))
            {
                ViewData["Error"] = "Không Được Để Trống";
            }
            else
            {
                tl.tenloai = ten;
                contex.TheLoais.InsertOnSubmit(tl);
                contex.SubmitChanges();
                return RedirectToAction("Index");
            }
            return this.Create();
        }

        public ActionResult Edit(int id)
        {
            var category = contex.TheLoais.First(m => m.maloai == id);
            return View(category);
        }
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collect)
        {
            var theloai = contex.TheLoais.First(p => p.maloai == id);
            var tenloai = collect["tenloai"];
            theloai.maloai = id;
            if (string.IsNullOrEmpty(tenloai))
            {
                ViewData["Error"] = "Vui Lòng Nhập Tên";
            }
            else
            {
                theloai.tenloai = tenloai;
                UpdateModel(theloai);
                contex.SubmitChanges();
                return RedirectToAction("Index");
            }
            return this.Edit(id);
        }

        public ActionResult Delete(int id)
        {
            var theloai = contex.TheLoais.First(p => p.maloai == id);
            return View(theloai);
        }
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collect)
        {
            var theloai = contex.TheLoais.Where(p => p.maloai == id).First();
            contex.TheLoais.DeleteOnSubmit(theloai);
            contex.SubmitChanges();
            return RedirectToAction("Index");
        }
    }
}