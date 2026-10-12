#nullable disable
using System;
using System.Drawing;
using System.Windows.Forms;

namespace TechMartProductManager
{
    public class FormDashboard : Form
    {
        private Button btnBai46 = new Button { Text = "Mở Bài 4.6: BindingSource & CSV Export", Location = new Point(40, 30), Size = new Size(320, 45), BackColor = Color.AliceBlue, Font = new Font("Segoe UI", 10, FontStyle.Bold) };
        private Button btnBai47 = new Button { Text = "Mở Bài 4.7: Khung MDI & Đa cửa sổ", Location = new Point(40, 90), Size = new Size(320, 45), BackColor = Color.Honeydew, Font = new Font("Segoe UI", 10, FontStyle.Bold) };
        private Button btnBai48 = new Button { Text = "Mở Bài 4.8: Async/Await & ProgressBar", Location = new Point(40, 150), Size = new Size(320, 45), BackColor = Color.LightYellow, Font = new Font("Segoe UI", 10, FontStyle.Bold) };
        private Button btnExit = new Button { Text = "Thoát ứng dụng", Location = new Point(40, 220), Size = new Size(320, 40), BackColor = Color.MistyRose, Font = new Font("Segoe UI", 10, FontStyle.Bold) };

        public FormDashboard()
        {
            Text = "Dashboard Tổng hợp - Đồ án Windows Forms (Bài 4.6 - 4.8)";
            Size = new Size(415, 330);
            StartPosition = FormStartPosition.CenterScreen;
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;

            btnBai46.Click += (s, e) => {
                FormBai4_6 frm = new FormBai4_6(); 
                frm.ShowDialog();
            };

            btnBai47.Click += (s, e) => {
                FormBai4_7 frm = new FormBai4_7();
                frm.ShowDialog();
            };

            btnBai48.Click += (s, e) => {
                FormBai4_8 frm = new FormBai4_8();
                frm.ShowDialog();
            };

            btnExit.Click += (s, e) => Close();

            Controls.AddRange(new Control[] { btnBai46, btnBai47, btnBai48, btnExit });
        }
    }
}