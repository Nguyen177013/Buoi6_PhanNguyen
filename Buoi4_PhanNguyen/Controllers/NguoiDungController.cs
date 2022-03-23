using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Buoi4_PhanNguyen.Models;
namespace Buoi4_PhanNguyen.Controllers
{
    public class NguoiDungController : Controller
    {
        MyDataDataContext context = new MyDataDataContext();
        [HttpGet]

        //Chức năng đăng ký người dùng
        public ActionResult DangKy()
        {
            return View();
        }
        [HttpPost]
        public ActionResult DangKy(FormCollection collection, KhachHang kh)
        {
            var hoten = collection["hoten"];
            var tendangnhap = collection["tendangnhap"];
            var matkhau = collection["matkhau"];
            var MatKhauXacNhan = collection["MatKhauXacNhan"];
            var email = collection["email"];
            var diachi = collection["diachi"];
            var dienthoai = collection["dienthoai"];
            var ngaysinh = String.Format("{0:MM/dd/yyyy}",collection["ngaysinh"]);

            if (String.IsNullOrEmpty(MatKhauXacNhan))
                ViewData["NhapMKXN"] = "Phải nhập mật khẩu xác nhận!";
            else
            {
                if (!matkhau.Equals(MatKhauXacNhan))
                    ViewData["MatKhauGiongNhau"] = "Mật khẩu và mật khẩu xác nhận không giống nhau";
                else
                {
                    kh.hoten = hoten;
                    kh.tendangnhap = tendangnhap;
                    kh.matkhau = matkhau;  
                    kh.email = email;
                    kh.diachi = diachi;
                    kh.dienthoai = dienthoai;
                    kh.ngaysinh = DateTime.Parse(ngaysinh);

                    context.KhachHangs.InsertOnSubmit(kh);
                    context.SubmitChanges();
                    return RedirectToAction("DangNhap");
                }
            }
            return this.DangKy();
        }
        [HttpGet]
        public ActionResult DangNhap()
        {
            return View();
        }
        [HttpPost]
        public ActionResult DangNhap(FormCollection collection)
        {
            var tendangnhap = collection["tendangnhap"];
            var matkhau = collection["matkhau"];

            KhachHang kh = context.KhachHangs.SingleOrDefault(p => p.tendangnhap == tendangnhap && p.matkhau == matkhau);
            if (kh != null)
            {
                Session["TaiKhoan"] = kh;
            }    
            else
                ViewData["Login"] = "Tài Khoản hoặc Mật Khẩu Không Hợp Lệ";
            return RedirectToAction("Index", "Home");
        }
        // GET: NguoiDung
        public ActionResult Index()
        {
            return View();
        }
    }
}