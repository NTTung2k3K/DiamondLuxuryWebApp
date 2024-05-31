namespace bt73
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    class Program
    {
        static void Main()
        {
            Console.InputEncoding = Encoding.UTF8;
            Console.OutputEncoding = Encoding.UTF8;

            List<string> hoList = new List<string>
        {
            "Nguyễn", "Lê", "Trần", "Lâm", "Phạm", "Hoàng", "Phan", "Vũ", "Đặng", "Bùi",
            "Đỗ", "Hồ", "Ngô", "Dương", "Lý", "Võ", "Mai", "Trịnh", "Chu", "Đinh"
        };

            List<string> chuLotList = new List<string>
        {
            "Thị", "Văn", "Hữu", "Minh", "Quốc", "Ngọc", "Gia", "Đức", "Tấn", "Nhật",
            "Thanh", "Bảo", "Anh", "Khánh", "Thảo", "Duy", "Quỳnh", "Phương", "Hà", "Kiều",
            "Tuyết", "Hoài", "Thiện", "Thành", "Thu", "Chí", "Đình", "Xuân", "Tường", "Trọng",
            "Đăng", "Trúc", "Việt", "Hải", "Hùng", "Lan", "Hồng", "Yến", "Kim", "Nguyệt",
            "Châu", "Linh", "Nam", "Thắng", "Thùy", "Giang", "Quang", "Lộc", "Hiền", "Vinh"
        };

            List<string> tenList = new List<string>
        {
            "Anh", "Bình", "Chi", "Dũng", "Em", "Phúc", "Gấm", "Hiếu", "Khanh", "Lan",
            "Mai", "Nguyên", "Oanh", "Phong", "Quân", "Rạng", "Sơn", "Thủy", "Uyên", "Vân",
            "Xương", "Yến", "Bảo", "Cường", "Đạt", "Dung", "Hòa", "Khoa", "Liên", "Minh",
            "Nhân", "Phượng", "Quý", "Sáng", "Tiến", "Vỹ", "Xuân", "Yên", "Ánh", "Bằng",
            "Châu", "Diệp", "Hạnh", "Kim", "Long", "Nam", "Quỳnh", "Trang", "Thắng", "Hoàng",
            "Hương", "Nhung", "Phước", "Toàn", "Tuấn", "Vỹ", "Bích", "Hà", "Hiền", "Lâm",
            "Loan", "Mai", "Nga", "Thảo", "Thủy", "Trinh", "Vi", "Vy", "Anh", "Bắc",
            "Ca", "Danh", "Huy", "Khôi", "Mẫn", "Nghĩa", "Quang", "Sơn", "Tài", "Tâm",
            "Uy", "Vĩ", "Xuân", "Duyên", "Giao", "Khánh", "Lam", "Ngọc", "Phương", "Tiên"
        };

            Random rand = new Random();
            List<string> listName = new List<string>();

            for (int i = 0; i < 1000; i++)
            {
                string ho = hoList[rand.Next(hoList.Count)];
                string chuLot = chuLotList[rand.Next(chuLotList.Count)];
                string ten = tenList[rand.Next(tenList.Count)];
                listName.Add($"{ho} {chuLot} {ten}");
            }

            // Sort danh sách theo tên
            listName = listName.OrderBy(name => name.Split(' ').Last()).ToList();

            Console.Write("Nhập Page Index: ");
            int pageIndex = int.Parse(Console.ReadLine());
            Console.Write("Nhập Page Size: ");
            int pageSize = int.Parse(Console.ReadLine());

            int startIndex = (pageIndex - 1) * pageSize;
            int endIndex = Math.Min(startIndex + pageSize, listName.Count);

            for (int i = startIndex; i < endIndex; i++)
            {
                Console.WriteLine(listName[i]);
            }
        }
    }

}
