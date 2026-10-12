#nullable disable
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TechMartProductManager
{
    public class ProductItem
    {
        public string ProductId { get; set; }
        public string ProductName { get; set; }
        public decimal Price { get; set; }
    }

    public class FormBai4_8 : Form
    {
        private Button btnLoad = new Button { Text = "Nạp dữ liệu bất đồng bộ", Location = new Point(20, 20), Size = new Size(180, 35), BackColor = Color.LightSkyBlue, Font = new Font("Segoe UI", 9, FontStyle.Bold) };
        private ProgressBar progressBar = new ProgressBar { Location = new Point(210, 20), Size = new Size(320, 35), Minimum = 0, Maximum = 100 };
        private Label lblPercent = new Label { Text = "0%", Location = new Point(540, 25), Size = new Size(50, 25), Font = new Font("Segoe UI", 10, FontStyle.Bold), TextAlign = ContentAlignment.MiddleLeft };

        private DataGridView dgvData = new DataGridView { Location = new Point(20, 70), Size = new Size(595, 380), SelectionMode = DataGridViewSelectionMode.FullRowSelect, ReadOnly = true, AutoGenerateColumns = true };

        private StatusStrip statusStrip = new StatusStrip();
        private ToolStripStatusLabel lblStatusMsg = new ToolStripStatusLabel();

        public FormBai4_8()
        {
            Text = "Bài 4.8: Lập trình bất đồng bộ (Async/Await) & ProgressBar";
            Size = new Size(650, 520);
            StartPosition = FormStartPosition.CenterScreen;
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;

            lblStatusMsg.Text = "Trạng thái: Sẵn sàng";
            lblStatusMsg.Spring = true;
            lblStatusMsg.TextAlign = ContentAlignment.MiddleLeft;
            statusStrip.Items.Add(lblStatusMsg);

            Controls.AddRange(new Control[] { btnLoad, progressBar, lblPercent, dgvData, statusStrip });

            btnLoad.Click += async (s, e) => {
                btnLoad.Enabled = false;
                progressBar.Value = 0;
                lblPercent.Text = "0%";

                var progress = new Progress<int>(percent => {
                    progressBar.Value = percent;
                    lblPercent.Text = $"{percent}%";
                    lblStatusMsg.Text = $"Trạng thái: Đang nạp dữ liệu... {percent}%";
                });

                try
                {
                    List<ProductItem> data = await LoadDataFromServerAsync(progress);
                    dgvData.DataSource = data;
                    lblStatusMsg.Text = "Trạng thái: Nạp dữ liệu hoàn tất thành công!";
                    MessageBox.Show("Nạp dữ liệu bất đồng bộ thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Đã xảy ra lỗi: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    lblStatusMsg.Text = "Trạng thái: Nạp dữ liệu thất bại!";
                }
                finally
                {
                    btnLoad.Enabled = true;
                }
            };
        }

        private async Task<List<ProductItem>> LoadDataFromServerAsync(IProgress<int> progress)
        {
            return await Task.Run(() =>
            {
                List<ProductItem> list = new List<ProductItem>();

                for (int i = 1; i <= 100; i++)
                {
                    System.Threading.Thread.Sleep(30); 
                    progress?.Report(i);
                }

                list.Add(new ProductItem { ProductId = "SP01", ProductName = "Laptop Dell Inspiron", Price = 18500000 });
                list.Add(new ProductItem { ProductId = "SP02", ProductName = "MacBook Air M2", Price = 26500000 });
                list.Add(new ProductItem { ProductId = "SP03", ProductName = "Bàn phím cơ Logitech", Price = 1800000 });
                list.Add(new ProductItem { ProductId = "SP04", ProductName = "Chuột không dây", Price = 650000 });
                list.Add(new ProductItem { ProductId = "SP05", ProductName = "Màn hình LG UltraWide", Price = 5200000 });

                return list;
            });
        }
    }
}