using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Buoi4_PhanNguyen.Models;
using System.ComponentModel.DataAnnotations;

namespace Buoi4_PhanNguyen.Models
{
    public class GioHang
    {
        MyDataDataContext context = new MyDataDataContext();
        public int masach { get; set; }
        [Display(Name = "Tên Sách")]
        public string tensach { get; set; }
        [Display(Name ="Ảnh Bìa")]
        public string hinh { get; set; }
        [Display(Name ="Giá Bán")]
        public double giaban { get; set; }
        [Display(Name = "Số Lượng")]
        public int soluong { get; set; }
        [Display(Name = "Thành Tiền")]
        public double thanhtien { get { return soluong * giaban; } }
        
        public GioHang(int id)
        {
            masach = id;
            Sach sach = context.Saches.Single(p => p.masach == masach);
            tensach = sach.tensach;
            hinh = sach.hinh;
            giaban = double.Parse(sach.giaban.ToString());
            soluong = 1;
        }
    }
}