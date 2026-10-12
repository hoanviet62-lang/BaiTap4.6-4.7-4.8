#nullable disable
using System;
using System.Drawing;
using System.Windows.Forms;

namespace TechMartProductManager
{
    public class FormChildStudent : Form
    {
        private DataGridView dgv = new DataGridView { Dock = DockStyle.Fill, SelectionMode = DataGridViewSelectionMode.FullRowSelect, ReadOnly = true };
        public Action<string> OnStatusUpdate;

        public FormChildStudent()
        {
            Text = "Quản lý Sinh viên";
            Size = new Size(400, 300);

            dgv.Columns.Add("Id", "Mã SV");
            dgv.Columns.Add("Name", "Họ tên");
            dgv.Rows.Add("SV01", "Nguyễn Văn A");
            dgv.Rows.Add("SV02", "Trần Thị B");

            ContextMenuStrip contextMenu = new ContextMenuStrip();
            ToolStripMenuItem menuXoa = new ToolStripMenuItem("Xóa dòng đang chọn");
            menuXoa.Click += (s, e) => {
                if (dgv.SelectedRows.Count > 0)
                {
                    string name = dgv.SelectedRows[0].Cells["Name"].Value.ToString();
                    dgv.Rows.RemoveAt(dgv.SelectedRows[0].Index);
                    OnStatusUpdate?.Invoke($"Đã xóa sinh viên: {name}");
                }
            };
            contextMenu.Items.Add(menuXoa);
            dgv.ContextMenuStrip = contextMenu;

            Controls.Add(dgv);
        }
    }

    public class FormChildProduct : Form
    {
        private DataGridView dgv = new DataGridView { Dock = DockStyle.Fill, SelectionMode = DataGridViewSelectionMode.FullRowSelect, ReadOnly = true };
        public Action<string> OnStatusUpdate;

        public FormChildProduct()
        {
            Text = "Quản lý Sản phẩm";
            Size = new Size(400, 300);

            dgv.Columns.Add("PId", "Mã SP");
            dgv.Columns.Add("PName", "Tên sản phẩm");
            dgv.Rows.Add("SP01", "Laptop Gaming");
            dgv.Rows.Add("SP02", "Bàn phím cơ");

            ContextMenuStrip contextMenu = new ContextMenuStrip();
            ToolStripMenuItem menuLamMoi = new ToolStripMenuItem("Làm mới danh mục sản phẩm");
            menuLamMoi.Click += (s, e) => {
                OnStatusUpdate?.Invoke("Đã làm mới dữ liệu sản phẩm.");
            };
            contextMenu.Items.Add(menuLamMoi);
            dgv.ContextMenuStrip = contextMenu;

            Controls.Add(dgv);
        }
    }

    public class FormBai4_7 : Form
    {
        private StatusStrip statusStrip = new StatusStrip();
        private ToolStripStatusLabel lblStatusTime = new ToolStripStatusLabel();
        private ToolStripStatusLabel lblStatusMsg = new ToolStripStatusLabel();
        private System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();

        public FormBai4_7()
        {
            Text = "Bài 4.7: Khung ứng dụng MDI & Quản lý đa cửa sổ";
            Size = new Size(900, 650);
            StartPosition = FormStartPosition.CenterScreen;
            IsMdiContainer = true;

            MenuStrip menuStrip = new MenuStrip();
            
            ToolStripMenuItem menuHeThong = new ToolStripMenuItem("Hệ thống");
            ToolStripMenuItem itemDangXuat = new ToolStripMenuItem("Đăng xuất", null, (s, e) => {
                MessageBox.Show("Đã đăng xuất hệ thống!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                lblStatusMsg.Text = "Trạng thái: Đã đăng xuất";
            });
            ToolStripMenuItem itemThoat = new ToolStripMenuItem("Thoát", null, (s, e) => Close());
            menuHeThong.DropDownItems.AddRange(new ToolStripItem[] { itemDangXuat, new ToolStripSeparator(), itemThoat });

            ToolStripMenuItem menuDanhMuc = new ToolStripMenuItem("Danh mục");
            ToolStripMenuItem itemQuanLySV = new ToolStripMenuItem("Quản lý Sinh viên", null, (s, e) => MởFormCon<FormChildStudent>());
            ToolStripMenuItem itemQuanLySP = new ToolStripMenuItem("Quản lý Sản phẩm", null, (s, e) => MởFormCon<FormChildProduct>());
            menuDanhMuc.DropDownItems.AddRange(new ToolStripItem[] { itemQuanLySV, itemQuanLySP });

            ToolStripMenuItem menuCuaSo = new ToolStripMenuItem("Cửa sổ");
            ToolStripMenuItem itemCascade = new ToolStripMenuItem("Cascade", null, (s, e) => LayoutMdi(MdiLayout.Cascade));
            ToolStripMenuItem itemTileH = new ToolStripMenuItem("Tile Horizontal", null, (s, e) => LayoutMdi(MdiLayout.TileHorizontal));
            ToolStripMenuItem itemTileV = new ToolStripMenuItem("Tile Vertical", null, (s, e) => LayoutMdi(MdiLayout.TileVertical));
            menuCuaSo.DropDownItems.AddRange(new ToolStripItem[] { itemCascade, itemTileH, itemTileV });

            menuStrip.Items.AddRange(new ToolStripItem[] { menuHeThong, menuDanhMuc, menuCuaSo });
            MainMenuStrip = menuStrip;
            Controls.Add(menuStrip);

            ToolStrip toolStrip = new ToolStrip();
            ToolStripButton tsbStudent = new ToolStripButton("Quản lý SV", null, (s, e) => MởFormCon<FormChildStudent>());
            ToolStripButton tsbProduct = new ToolStripButton("Quản lý SP", null, (s, e) => MởFormCon<FormChildProduct>());
            ToolStripButton tsbExit = new ToolStripButton("Thoát", null, (s, e) => Close());
            toolStrip.Items.AddRange(new ToolStripItem[] { tsbStudent, tsbProduct, new ToolStripSeparator(), tsbExit });
            Controls.Add(toolStrip);

            lblStatusMsg.Text = "Trạng thái: Sẵn sàng";
            lblStatusMsg.Spring = true;
            lblStatusMsg.TextAlign = ContentAlignment.MiddleLeft;

            timer.Interval = 1000;
            timer.Tick += (s, e) => {
                lblStatusTime.Text = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            };
            timer.Start();

            statusStrip.Items.AddRange(new ToolStripItem[] { lblStatusMsg, lblStatusTime });
            Controls.Add(statusStrip);
        }

        private void MởFormCon<T>() where T : Form, new()
        {
            foreach (Form frm in MdiChildren)
            {
                if (frm is T)
                {
                    frm.Activate();
                    lblStatusMsg.Text = $"Đã kích hoạt lại cửa sổ: {frm.Text}";
                    return;
                }
            }

            T newForm = new T();
            newForm.MdiParent = this;
            
            if (newForm is FormChildStudent childSV)
            {
                childSV.OnStatusUpdate = (msg) => lblStatusMsg.Text = msg;
            }
            else if (newForm is FormChildProduct childSP)
            {
                childSP.OnStatusUpdate = (msg) => lblStatusMsg.Text = msg;
            }

            newForm.Show();
            lblStatusMsg.Text = $"Đã mở cửa sổ mới: {newForm.Text}";
        }
    }
}