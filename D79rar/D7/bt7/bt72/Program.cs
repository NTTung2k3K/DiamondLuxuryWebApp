using System.Text;

namespace bt72
{

    public class Program
    {
        public static void Main()
        {
            Console.InputEncoding = Encoding.UTF8;
            Console.OutputEncoding = Encoding.UTF8;

            List<int> danhSachNgauNhien = TaoDanhSachNgauNhien(1000000, 1, 1000000);

            Console.Write("Vui lòng nhập giá trị N: ");
            if (!int.TryParse(Console.ReadLine(), out int N))
            {
                Console.WriteLine("Giá trị nhập không hợp lệ. Vui lòng nhập một số nguyên.");
                return;
            }

            List<int> danhSachSauKhiLoaiBo = danhSachNgauNhien.Where(x => x >= N).ToList();

            double giaTriTrungBinh = danhSachSauKhiLoaiBo.Average();

            Random random = new Random();
            int phanTuNgauNhien = danhSachNgauNhien[random.Next(danhSachNgauNhien.Count)];

            Console.WriteLine($"Giá trị trung bình của danh sách sau khi loại bỏ các số nhỏ hơn {N}: {giaTriTrungBinh}");
            Console.WriteLine($"Phần tử ngẫu nhiên được chọn từ danh sách ban đầu: {phanTuNgauNhien}");
        }

        private static List<int> TaoDanhSachNgauNhien(int kichThuoc, int min, int max)
        {
            Random random = new Random();
            List<int> danhSach = new List<int>();
            for (int i = 0; i < kichThuoc; i++)
            {
                danhSach.Add(random.Next(min, max + 1));
            }
            return danhSach;
        }
    }

}

