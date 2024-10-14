using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace De_01
{
    public partial class frmQuanlysinhvien : Form
    {
        QuanlySVEntities you = new QuanlySVEntities(); // Kết nối tới cơ sở dữ liệu
        public frmQuanlysinhvien()
        {
            InitializeComponent();
            InitializeDataGridView(); // Cấu hình DataGridView khi form khởi tạo
        }

        // Sự kiện khi form load, sẽ gọi hàm loadData để tải dữ liệu lên DataGridView
        private void frmQuanlysinhvien_Load(object sender, EventArgs e)
        {
            loadData();
        }

        // Hàm cấu hình DataGridView
        void InitializeDataGridView()
        {
            dgvSinhvien.AutoGenerateColumns = false; // Không tự động sinh cột

            // Cấu hình các cột
            DataGridViewTextBoxColumn colMaSV = new DataGridViewTextBoxColumn
            {
                DataPropertyName = "MaSV",
                HeaderText = "Mã Sinh Viên",
                Width = 100
            };
            dgvSinhvien.Columns.Add(colMaSV);

            DataGridViewTextBoxColumn colHotenSV = new DataGridViewTextBoxColumn
            {
                DataPropertyName = "HotenSV",
                HeaderText = "Họ Tên",
                Width = 200
            };
            dgvSinhvien.Columns.Add(colHotenSV);

            DataGridViewTextBoxColumn colMaLop = new DataGridViewTextBoxColumn
            {
                DataPropertyName = "MaLop",
                HeaderText = "Mã Lớp",
                Width = 100
            };
            dgvSinhvien.Columns.Add(colMaLop);

            DataGridViewTextBoxColumn colNgaysinh = new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Ngaysinh",
                HeaderText = "Ngày Sinh",
                Width = 100
            };
            dgvSinhvien.Columns.Add(colNgaysinh);
        }

        // Hàm load dữ liệu sinh viên từ cơ sở dữ liệu và hiển thị trên DataGridView
        void loadData()
        {
            var t = you.Sinhviens.ToList();
            dgvSinhvien.DataSource = t;
        }

        // Sự kiện khi người dùng nhập tìm kiếm tên sinh viên
        private void btnfind_Click(object sender, EventArgs e)
        {
            string searchTerm = txtfind.Text.ToLower(); // Lấy từ khóa tìm kiếm và chuyển thành chữ thường
            var filteredSinhviens = you.Sinhviens.Where(sv => sv.HotenSV.ToLower().Contains(searchTerm)).ToList(); // Tìm sinh viên có tên chứa từ khóa
            dgvSinhvien.DataSource = filteredSinhviens; // Hiển thị kết quả tìm kiếm trên DataGridView
        }

        // Sự kiện khi người dùng nhấn nút thêm sinh viên
        private void btnadd_Click(object sender, EventArgs e)
        {
            Sinhvien sv = new Sinhvien
            {
                MaSV = txtmssv.Text,
                HotenSV = txthoten.Text,
                MaLop = cmblop.SelectedItem.ToString(),
                Ngaysinh = dtpngaysinh.Value
            };
            you.Sinhviens.Add(sv); // Thêm sinh viên vào cơ sở dữ liệu
            you.SaveChanges(); // Lưu thay đổi vào cơ sở dữ liệu
            loadData(); // Tải lại dữ liệu để hiển thị sinh viên mới được thêm
        }

        // Sự kiện khi người dùng nhấn nút xóa sinh viên
        private void btndelete_Click(object sender, EventArgs e)
        {
            if (dgvSinhvien.SelectedRows.Count > 0)
            {
                string maSV = dgvSinhvien.SelectedRows[0].Cells["MaSV"].Value.ToString();
                Sinhvien sv = you.Sinhviens.FirstOrDefault(s => s.MaSV == maSV);
                if (sv != null)
                {
                    you.Sinhviens.Remove(sv); // Xóa sinh viên khỏi cơ sở dữ liệu
                    you.SaveChanges(); // Lưu thay đổi vào cơ sở dữ liệu
                    loadData(); // Tải lại dữ liệu sau khi xóa
                }
            }
        }

        // Sự kiện khi người dùng nhấn nút sửa thông tin sinh viên
        private void btnupd_Click(object sender, EventArgs e)
        {
            if (dgvSinhvien.SelectedRows.Count > 0)
            {
                string maSV = dgvSinhvien.SelectedRows[0].Cells["MaSV"].Value.ToString();
                Sinhvien sv = you.Sinhviens.FirstOrDefault(s => s.MaSV == maSV);
                if (sv != null)
                {
                    sv.HotenSV = txthoten.Text;
                    sv.MaLop = cmblop.SelectedItem.ToString();
                    sv.Ngaysinh = dtpngaysinh.Value;

                    you.SaveChanges(); // Lưu thay đổi sau khi sửa
                    loadData(); // Tải lại dữ liệu sau khi sửa
                }
            }
        }

        // Sự kiện khi người dùng nhấn nút thoát
        private void btnout_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Bạn có chắc chắn muốn thoát không?", "Xác nhận thoát", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                this.Close(); // Đóng form nếu người dùng chọn Yes
            }
        }

        // Sự kiện khi người dùng nhấn nút lưu
        private void btnsave_Click(object sender, EventArgs e)
        {
            try
            {
                you.SaveChanges(); // Lưu thay đổi xuống cơ sở dữ liệu
                MessageBox.Show("Dữ liệu đã được lưu!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                loadData(); // Tải lại dữ liệu sau khi lưu
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi lưu dữ liệu: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Sự kiện khi người dùng nhấn nút hủy lưu
        private void btnunsave_Click(object sender, EventArgs e)
        {
            you = new QuanlySVEntities(); // Khởi tạo lại đối tượng để hủy bỏ các thay đổi chưa được lưu
            loadData(); // Tải lại dữ liệu gốc từ cơ sở dữ liệu
            MessageBox.Show("Thay đổi đã được hủy!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        // Các sự kiện khác của form
        private void txtmssv_TextChanged(object sender, EventArgs e) { }
        private void txthoten_TextChanged(object sender, EventArgs e) { }
        private void cmblop_SelectedIndexChanged(object sender, EventArgs e) { }
        private void dtpngaysinh_ValueChanged(object sender, EventArgs e) { }
        private void txtfind_TextChanged(object sender, EventArgs e) { }
        private void dgvSinhvien_CellContentClick(object sender, DataGridViewCellEventArgs e) { }
    }
}
