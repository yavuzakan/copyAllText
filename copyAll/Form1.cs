using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace copyAll
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            this.Load += Form1_Load;
            database.Create_db();
            textBox1.ScrollBars = ScrollBars.Vertical;
            ara();


            //Create right click menu..
            ContextMenuStrip s = new ContextMenuStrip();

            // add one right click menu item named as hello           
            ToolStripMenuItem sil = new ToolStripMenuItem();
            sil.Text = "Delete All Notes";

            // add the clickevent of hello item
            sil.Click += sil_Click;

            // add the item in right click menu
            s.Items.Add(sil);

            this.ContextMenuStrip = s;
        }
        void sil_Click(object sender, EventArgs e)
        {
            /*
             * delete from your_table;    
                delete from sqlite_sequence where name='your_table';
             * */


            DialogResult dialogResult = MessageBox.Show("Sure", "Delete All Notes", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                dataGridView1.Rows.Clear();
                string path = "deneme.db";
                string cs = @"URI=file:"+Application.StartupPath+"\\deneme.db";
                var con = new SQLiteConnection(cs);
                SQLiteDataReader dr;
                con.Open();

                //string stm = "select * FROM data ORDER BY id ASC  ";
                //SELECT * FROM (SELECT * FROM graphs WHERE sid=2 ORDER BY id DESC LIMIT 10) g ORDER BY g.id
                string stm = "delete from data";
                var cmd = new SQLiteCommand(stm, con);
                dr = cmd.ExecuteReader();

                stm = "delete from sqlite_sequence where name='data'";
                cmd = new SQLiteCommand(stm, con);
                dr = cmd.ExecuteReader();

                con.Close();

                this.dataGridView1.AllowUserToAddRows = false;


                dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                //dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                dataGridView1.ReadOnly = true;
                textBox1.Text="";

                // dataGridView1.Columns[0].Visible = false;
            }
            else if (dialogResult == DialogResult.No)
            {
                //do something else
            }


        }



        [DllImport("User32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr SetClipboardViewer(IntPtr hWndNewViewer);

        public const int WM_DRAWCLIPBOARD = 0x0308;

        private void button1_Click(object sender, EventArgs e)
        {
            ara2();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            SetClipboardViewer(this.Handle);
        }

        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);
            if (m.Msg != WM_DRAWCLIPBOARD)
                return;

            string veri = Clipboard.GetText();
            //Code To handle Clipboard change event
            database.add(veri);
            textBox1.Text = veri;
            ara();
        }

        public void ara()
        {

            dataGridView1.Rows.Clear();
            string path = "deneme.db";
            string cs = @"URI=file:"+Application.StartupPath+"\\deneme.db";
            var con = new SQLiteConnection(cs);
            SQLiteDataReader dr;
            con.Open();

            //string stm = "select * FROM data ORDER BY id ASC  ";
            //SELECT * FROM (SELECT * FROM graphs WHERE sid=2 ORDER BY id DESC LIMIT 10) g ORDER BY g.id
            string stm = "select * FROM data ORDER BY id asc LIMIT 10  ";
            var cmd = new SQLiteCommand(stm, con);
            dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                dataGridView1.Rows.Insert(0, dr.GetValue(0).ToString(), dr.GetValue(1).ToString(), dr.GetValue(2).ToString());

            }

            con.Close();

            this.dataGridView1.AllowUserToAddRows = false;


            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            //dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView1.ReadOnly = true;

            // dataGridView1.Columns[0].Visible = false;






        }
        public void ara2()
        {
            dataGridView1.Rows.Clear();
            string path = "deneme.db";
            string cs = @"URI=file:"+Application.StartupPath+"\\deneme.db";
            var con = new SQLiteConnection(cs);
            SQLiteDataReader dr;
            con.Open();


            string arama = '%' + textBox2.Text + '%';

            //string stm = "select * FROM data ORDER BY id ASC  ";
            //SELECT * FROM (SELECT * FROM graphs WHERE sid=2 ORDER BY id DESC LIMIT 10) g ORDER BY g.id
            string stm = "select * FROM data where veri LIKE '"+arama+"' ORDER BY id desc  ";

            var cmd = new SQLiteCommand(stm, con);
            dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                dataGridView1.Rows.Insert(0, dr.GetValue(0).ToString(), dr.GetValue(1).ToString(), dr.GetValue(2).ToString());

            }

            con.Close();

            this.dataGridView1.AllowUserToAddRows = false;


            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            //dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView1.ReadOnly = true;

            dataGridView1.Columns[0].Visible = false;






        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow dataGridViewRow = dataGridView1.Rows[e.RowIndex];


                textBox1.Text = dataGridViewRow.Cells["veri"].Value.ToString();




            }
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.Show();
            notifyIcon1.Visible = false;
            WindowState = FormWindowState.Normal;
        }

        private void notifyIcon1_MouseClick(object sender, MouseEventArgs e)
        {
            this.Show();
            notifyIcon1.Visible = false;
            WindowState = FormWindowState.Normal;
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Minimized)
            {
                this.Hide();
                notifyIcon1.Visible = true;
                // notifyIcon1.ShowBalloonTip(1000);
            }
            else if (FormWindowState.Normal == this.WindowState)
            {
                notifyIcon1.Visible = false;
            }
        }
    }
}
