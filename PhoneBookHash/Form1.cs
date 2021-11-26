using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PhoneBookHash
{
    public partial class Form1 : Form
    {
        MyHashTable myHash = new MyHashTable();

        public Form1()
        {
            InitializeComponent();

            dataGridView1.Columns.Add("Номер", "Номер");
            dataGridView1.Columns["Номер"].Width = 50;
            dataGridView1.Columns.Add("Телефон", "Телефон");
            dataGridView1.Columns["Телефон"].Width = 80;
            dataGridView1.Columns.Add("Фамилия", "Фамилия");
            dataGridView1.Columns["Фамилия"].Width = 140;

            btnAdd.Enabled = btnFind.Enabled = btnDel.Enabled = false;

            tbPhone.TextChanged += TbPhone_TextChanged;
            btnAdd.Click += BtnAdd_Click;
            btnDel.Click += BtnDel_Click;
            btnSaveExit.Click += BtnSaveExit_Click;
            btnFind.Click += BtnFind_Click;

            FillHashTable();
            RefreshNotes();
        }

        private void BtnFind_Click(object sender, EventArgs e)
        {
            MessageBox.Show(myHash.Find(tbPhone.Text));
        }

        private void BtnSaveExit_Click(object sender, EventArgs e)
        {
            using (var strWriter = new StreamWriter(new FileStream("phoneBook.txt", FileMode.Create)))
            {
                foreach (var note in myHash)
                {
                    strWriter.WriteLine(note.Name);
                    strWriter.WriteLine(note.Phone);
                }

                MessageBox.Show("Файл сохранен");
            }

            Close();
        }

        private void BtnDel_Click(object sender, EventArgs e)
        {
            string phone = tbPhone.Text;

            if (!string.IsNullOrEmpty(phone))
            {
                if (myHash.Remove(phone))
                {
                    MessageBox.Show($"Абонент с номером {phone} удален");
                    RefreshNotes();
                }

            }
        }

        private void RefreshNotes()
        {
            dataGridView1.RowCount = myHash.Size;

            for (int i = 0; i < myHash.Size; i++)
            {
                dataGridView1[0, i].Value = Convert.ToString(i + 1);
                dataGridView1[1, i].Value = string.Empty;
                dataGridView1[2, i].Value = string.Empty;
            }

            for (int i = 0; i < myHash.Size; i++)
            {
                dataGridView1["Телефон", i].Value = myHash[i].Note.Phone;
                dataGridView1["Фамилия", i].Value = myHash[i].Note.Name;
            }
        }

        void FillHashTable()
        {
            using (var strReader = new StreamReader(new FileStream("phoneBook.txt", FileMode.OpenOrCreate)))
            {
                string fio, phone;

                while ((fio = strReader.ReadLine()) != null && (phone = strReader.ReadLine()) != null)
                    myHash.Add(fio, phone);
            }
        }

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            if (myHash.Add(tbName.Text, tbPhone.Text) != -1)
            {
                RefreshNotes();
                MessageBox.Show("Абонент добавлен");
            }
            else
                MessageBox.Show("В записной книжке нет места");

        }

        private void TbPhone_TextChanged(object sender, EventArgs e)
        {
            var tb = sender as TextBox;

            if (tb == null) return;

            double res;

            btnAdd.Enabled = btnFind.Enabled = btnDel.Enabled = double.TryParse(tb.Text, out res);
        }
    }
}