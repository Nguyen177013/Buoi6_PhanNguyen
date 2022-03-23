using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Buoi4_PhanNguyen.Models;
namespace Buoi4_PhanNguyen.Controllers
{
    public class GioHangController : Controller
    {
        MyDataDataContext context = new MyDataDataContext();

        // chức năng lấy giỏ hàng
        public List<GioHang> layGioHang()
        {
            List<GioHang> lstGioHang = Session["Giohang"] as List<GioHang>;
            if(lstGioHang == null)
            {
                lstGioHang = new List<GioHang>();
                Session["Giohang"] = lstGioHang;
            }
            return lstGioHang;
        }
        //chức năng thêm giỏ hàng
        public ActionResult ThemGioHang(int id,string strURL)
        {
            List<GioHang> lstGioHang = layGioHang();
            GioHang sanpham = lstGioHang.Find(n => n.masach == id);
            if(sanpham == null)
            {
                sanpham = new GioHang(id);
                lstGioHang.Add(sanpham);
                return Redirect(strURL);
            }
            else
            {
                sanpham.soluong++;
                return Redirect(strURL);
            }
        }
        // chức năng tổng số lượng tồn
        private int TongSoLuong()
        {
            int tsl = 0;
            List<GioHang> lstGioHang = Session["Giohang"] as List<GioHang>;
            if (lstGioHang != null)
            {
                tsl = lstGioHang.Sum(n => n.soluong);
            }
            return tsl;
        }
        // chức năng tổng số lượng sản phẩm
        private int TongSoLuongSanPham()
        {
            int tsl = 0;
            List<GioHang> lstGioHang = Session["Giohang"] as List<GioHang>;
            if(lstGioHang != null)
            {
                tsl = lstGioHang.Count;
            }
            return tsl;
        }
        // chức năng tổng tiền
        private double TongTien()
        {
            double tt = 0;
            List<GioHang> lstGioHang = Session["Giohang"] as List<GioHang>;
            if (lstGioHang != null)
            {
                tt = lstGioHang.Sum(n => n.thanhtien);
            }
            return tt;
        }
        public ActionResult GioHang()
        {
            List<GioHang> gioHang = layGioHang();
            ViewBag.Tongsoluong = TongSoLuong();
            ViewBag.Tongtien = TongTien();
            ViewBag.Tongsoluongsanpham = TongSoLuongSanPham();
            return View(gioHang);
        }
        public ActionResult GioHangPartial()
        {
            ViewBag.Tongsoluong = TongSoLuong();
            ViewBag.Tongtien = TongTien();
            ViewBag.Tongsoluongsanpham = TongSoLuongSanPham();
            return PartialView();
        }

        public ActionResult XoaGioHang(int id)
        {
            List<GioHang> lstgioHang = layGioHang();
            GioHang sanpham = lstgioHang.SingleOrDefault(n => n.masach == id);
            if (sanpham != null)
            {
                lstgioHang.RemoveAll(n => n.masach == id);
                return RedirectToAction("GioHang");
            }
            return RedirectToAction("GioHang");
        }
        public ActionResult CapnhatGiohang(int id, FormCollection collection)
        {
            List<GioHang> lstGiohang = layGioHang();
            GioHang sanpham = lstGiohang.SingleOrDefault(p => p.masach == id);
            if (sanpham != null)
            {
                sanpham.soluong = int.Parse(collection["txtSoLg"].ToString());
            }
            return RedirectToAction("GioHang");
        }
        public ActionResult XoaTatCaGioHang()
        {
            List<GioHang> lstGioHang = layGioHang();
            lstGioHang.Clear();
            return RedirectToAction("GioHang");
        }

        //chức năng đặt hàng
        [HttpGet]
        public ActionResult DatHang()
        {
            if (Session["TaiKhoan"] == null || Session["TaiKhoan"].ToString() == "")
                return RedirectToAction("DangNhap", "NguoiDung");
            if (Session["Giohang"] == null)
                return RedirectToAction("Index", "Sach");
            List<GioHang> lstGioHang = layGioHang();
            ViewBag.Tongsoluong = TongSoLuong();
            ViewBag.Tongtien = TongTien();
            ViewBag.Tongsoluongsanpham = TongSoLuongSanPham();
            return View(lstGioHang);
        }
        public ActionResult DatHang(FormCollection collection)
        {
            DonHang dh = new DonHang();
            KhachHang kh = (KhachHang)Session["TaiKhoan"];
            Sach s = new Sach();

            List<GioHang> gh = layGioHang();
            var ngaygiao = String.Format("{0:MM/dd/yyyy}", collection["NgayGiao"]);

            dh.makh = kh.makh;
            dh.ngaygiao = DateTime.Now;
            dh.ngaygiao = DateTime.Parse(ngaygiao);
            dh.giaohang = false;
            dh.thanhtoan = false;

            context.DonHangs.InsertOnSubmit(dh);
            context.SubmitChanges();
            foreach(var ele in gh)
            {
                ChiTietDonHang ctdh = new ChiTietDonHang();
                ctdh.madon = dh.madon;
                ctdh.masach = ele.masach;
                ctdh.soluong = ele.soluong;
                ctdh.gia = (decimal)ele.giaban;
                s =  context.Saches.Single(p=>p.masach == ele.masach);
                s.soluongton -= ctdh.soluong;
                context.SubmitChanges();
                context.ChiTietDonHangs.InsertOnSubmit(ctdh);
            }
            context.SubmitChanges();
            Session["Giohang"] = null;
            return RedirectToAction("XacnhanDonhang", "GioHang");
        }
        public ActionResult XacnhanDonhang()
        {
            return View();
        }

        // GET: GioHang
        public ActionResult Index()
        {
            return View();
        }
    }
}