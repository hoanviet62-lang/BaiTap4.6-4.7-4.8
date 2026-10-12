using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace TechMartProductManager
{
    public class Student
    {
        public string StudentId { get; set; }
        public string FullName { get; set; }
        public int Age { get; set; }
        public string Gender { get; set; }
        public string AvatarPath { get; set; }
    }

    public class FormBai4_6 : Form
    {
        private TextBox txtId = new TextBox { Location = new Point(90, 20), Width = 150 };
        private TextBox txtName = new TextBox { Location = new Point(90, 60), Width = 150 };
        private TextBox txtAge = new TextBox { Location = new Point(90, 100), Width = 150 };
        private ComboBox cboGender = new ComboBox { Location = new Point(90, 140), Width = 150, DropDownStyle = ComboBoxStyle.DropDownList };
        
        private PictureBox picAvatar = new PictureBox { Location = new Point(260, 20), Size = new Size(120, 150), BorderStyle = BorderStyle.FixedSingle, SizeMode = PictureBoxSizeMode.StretchImage };
        private Button btnChonAnh = new Button { Text = "Chọn ảnh", Location = new Point(260, 180), Size = new Size(120, 30), BackColor = Color.WhiteSmoke };

        private Button btnThem = new Button { Text = "Thêm", Location = new Point(15, 230), Size = new Size(85, 35), BackColor = Color.LightSkyBlue, Font = new Font("Segoe UI", 9, FontStyle.Bold) };
        private Button btnCapNhat = new Button { Text = "Cập nhật", Location = new Point(110, 230), Size = new Size(85, 35) };
        private Button btnXoa = new Button { Text = "Xóa", Location = new Point(205, 230), Size = new Size(85, 35), BackColor = Color.MistyRose };
        private Button btnXuatCSV = new Button { Text = "Xuất CSV", Location = new Point(300, 230), Size = new Size(95, 35), BackColor = Color.LightGreen, Font = new Font("Segoe UI", 9, FontStyle.Bold) };

        private DataGridView dgvSinhVien = new DataGridView { Location = new Point(15, 280), Size = new Size(650, 250), AutoGenerateColumns = true, SelectionMode = DataGridViewSelectionMode.FullRowSelect, ReadOnly = true };

        private BindingList<Student> listStudents = new BindingList<Student>();
        private BindingSource bindingSource = new BindingSource();
        private string tempAvatarPath = "";

        public FormBai4_6()
        {
            Text = "Bài 4.6: Quản lý sinh viên với BindingSource & File CSV";
            Size = new Size(700, 580);
            StartPosition = FormStartPosition.CenterScreen;
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;

            cboGender.Items.AddRange(new object[] { "Nam", "Nữ" });
            cboGender.SelectedIndex = 0;

            bindingSource.DataSource = listStudents;
            dgvSinhVien.DataSource = bindingSource;

            txtId.DataBindings.Add("Text", bindingSource, "StudentId", true, DataSourceUpdateMode.OnPropertyChanged);
            txtName.DataBindings.Add("Text", bindingSource, "FullName", true, DataSourceUpdateMode.OnPropertyChanged);
            txtAge.DataBindings.Add("Text", bindingSource, "Age", true, DataSourceUpdateMode.OnPropertyChanged);
            cboGender.DataBindings.Add("SelectedItem", bindingSource, "Gender", true, DataSourceUpdateMode.OnPropertyChanged);

            GroupBox grpInput = new GroupBox { Text = "Thông tin sinh viên", Location = new Point(15, 10), Size = new Size(650, 260) };
            grpInput.Controls.AddRange(new Control[] {
                new Label { Text = "Mã SV:", Location = new Point(15, 23), AutoSize = true }, txtId,
                new Label { Text = "Họ tên:", Location = new Point(15, 63), AutoSize = true }, txtName,
                new Label { Text = "Tuổi:", Location = new Point(15, 103), AutoSize = true }, txtAge,
                new Label { Text = "Giới tính:", Location = new Point(15, 143), AutoSize = true }, cboGender,
                picAvatar, btnChonAnh, btnThem, btnCapNhat, btnXoa, btnXuatCSV
            });

            Controls.AddRange(new Control[] { grpInput, dgvSinhVien });

            bindingSource.CurrentChanged += (s, e) => {
                if (bindingSource.Current is Student currentStudent)
                {
                    tempAvatarPath = currentStudent.AvatarPath ?? "";
                    if (!string.IsNullOrEmpty(tempAvatarPath) && File.Exists(tempAvatarPath))
                    {
                        picAvatar.Image = Image.FromFile(tempAvatarPath);
                    }
                    else
                    {
                        picAvatar.Image = null;
                    }
                }
            };

            btnChonAnh.Click += (s, e) => {
                using (OpenFileDialog ofd = new OpenFileDialog())
                {
                    ofd.Filter = "Image Files (*.jpg;*.png;*.bmp)|*.jpg;*.png;*.bmp";
                    if (ofd.ShowDialog() == DialogResult.OK)
                    {
                        tempAvatarPath = ofd.FileName;
                        picAvatar.Image = Image.FromFile(tempAvatarPath);

                        if (bindingSource.Current is Student currentStudent)
                        {
                            currentStudent.AvatarPath = tempAvatarPath;
                            bindingSource.ResetCurrentItem();
                        }
                    }
                }
            };

            btnThem.Click += (s, e) => {
                if (string.IsNullOrWhiteSpace(txtId.Text))
                {
                    MessageBox.Show("Vui lòng nhập Mã sinh viên!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if (!int.TryParse(txtAge.Text, out int age))
                {
                    MessageBox.Show("Tuổi phải là số nguyên hợp lệ!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                Student newStudent = new Student
                {
                    StudentId = txtId.Text.Trim(),
                    FullName = txtName.Text.Trim(),
                    Age = age,
                    Gender = cboGender.SelectedItem?.ToString() ?? "Nam",
                    AvatarPath = tempAvatarPath
                };

                listStudents.Add(newStudent);
                bindingSource.Position = listStudents.Count - 1;
                tempAvatarPath = "";
            };

            btnCapNhat.Click += (s, e) => {
                if (bindingSource.Current is Student currentStudent)
                {
                    currentStudent.AvatarPath = tempAvatarPath;
                }
                bindingSource.ResetCurrentItem();
                MessageBox.Show("Cập nhật thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            };

            btnXoa.Click += (s, e) => {
                if (bindingSource.Current is Student currentStudent)
                {
                    listStudents.Remove(currentStudent);
                    picAvatar.Image = null;
                    tempAvatarPath = "";
                }
            };

            btnXuatCSV.Click += (s, e) => {
                using (SaveFileDialog sfd = new SaveFileDialog())
                {
                    sfd.Filter = "CSV files (*.csv)|*.csv|All files (*.*)|*.*";
                    sfd.FileName = "DanhSachSinhVien.csv";
                    if (sfd.ShowDialog() == DialogResult.OK)
                    {
                        StringBuilder sb = new StringBuilder();
                        sb.AppendLine("StudentId,FullName,Age,Gender,AvatarPath");
                        foreach (var st in listStudents)
                        {
                            sb.AppendLine($"{st.StudentId},{st.FullName},{st.Age},{st.Gender},{st.AvatarPath}");
                        }
                        File.WriteAllText(sfd.FileName, sb.ToString(), Encoding.UTF8);
                        MessageBox.Show("Xuất file CSV thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            };
        }
    }
}