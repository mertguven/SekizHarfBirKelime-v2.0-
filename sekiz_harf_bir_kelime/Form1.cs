using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Drawing;

namespace sekiz_harf_bir_kelime
{
    public partial class Form1 : Form
    {
        [DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        private static extern IntPtr CreateRoundRectRgn
        (
            int nLeftRect,
            int nTopRect,
            int nRightRect,
            int nBottomRect,
            int nWidthEllipse,
            int nHeightEllipse
        );
        public Form1()
        {
            InitializeComponent();
            this.Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, Width, Height, 15, 15));
        }
        List<string> sekizli = new List<string>();
        List<string> yedili = new List<string>();
        List<string> altili = new List<string>();
        List<string> besli = new List<string>();
        List<string> dortlu = new List<string>();
        List<string> uclu = new List<string>();

        tdk_kelimeEntities ent = new tdk_kelimeEntities();//entity framework nesnesi oluşturuluyor
        Random rastgele = new Random();

        char[] dizi = { 'a', 'b', 'c', 'ç', 'd', 'e', 'f', 'g', 'ğ', 'h', 'ı', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'ö', 'p', 'r', 's', 'ş', 't', 'u', 'ü', 'v', 'y', 'z' };
        string metin = "";
        List<string> harflerq = new List<string>();
        char[] karakterler = new char[8];
        int harfyeri;

        private void rastgeleBtn_Click(object sender, EventArgs e)//Rastgele oluşturulan değerlerin kombinasyonunu alır ve veritabanı ile eşleşenleri listbox3'e atar
        {
            switch (MessageBox.Show("Oluşturulan;\nEn uzun kelime için: EVET \nBütün kelimeler için: HAYIR", "Hangisi?", MessageBoxButtons.YesNo, MessageBoxIcon.Question))
            {
                case DialogResult.Yes:
                    harflerTxt.Text = "";
                    for (int i = 0; i < 8; i++)
                    {
                        harfyeri = rastgele.Next(0, dizi.Length);
                        karakterler[i] = dizi[harfyeri];
                        harflerTxt.Text += karakterler[i].ToString();
                        harflerq.Add(karakterler[i].ToString());
                    }
                    OrtakIslem(1);
                    eslestirBtn.Enabled = false;
                    rastgeleBtn.Enabled = false;
                    veriCekBtn.Enabled = false;
                    break;
                case DialogResult.No:
                    MessageBox.Show("Bu işlem biraz uzun sürebilir!", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    harflerTxt.Text = "";
                    for (int i = 0; i < 8; i++)
                    {
                        harfyeri = rastgele.Next(0, dizi.Length);
                        karakterler[i] = dizi[harfyeri];
                        harflerTxt.Text += karakterler[i].ToString();
                        harflerq.Add(karakterler[i].ToString());
                    }
                    OrtakIslem(0);
                    eslestirBtn.Enabled = false;
                    rastgeleBtn.Enabled = false;
                    veriCekBtn.Enabled = false;
                    break;
            }
        }

        private void eslestirBtn_Click(object sender, EventArgs e)//Elle girilen değerlerin kombinasyonunu alır ve veritabanı ile eşleşenleri listbox3'e atar
        {
            if (harflerTxt.Text == "")
            {
                MessageBox.Show("Harfler kısmını doldurun!", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (harflerTxt.TextLength < 9)
            {
                MessageBox.Show("Minimum 9 harf girmeniz gerekiyor!", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                switch (MessageBox.Show("Oluşturulan;\nEn uzun kelime için: EVET \nBütün kelimeler için: HAYIR", "Hangisi?",MessageBoxButtons.YesNo,MessageBoxIcon.Question))
                {
                    case DialogResult.Yes:
                        metin = harflerTxt.Text;
                        karakterler = metin.ToCharArray();
                        for (int i = 0; i < 8; i++)
                        {
                            harflerq.Add(karakterler[i].ToString());
                        }
                        OrtakIslem(1);
                        rastgeleBtn.Enabled = false;
                        eslestirBtn.Enabled = false;
                        veriCekBtn.Enabled = false;
                        break;
                    case DialogResult.No:
                        MessageBox.Show("Bu işlem biraz uzun sürebilir!", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        metin = harflerTxt.Text;
                        karakterler = metin.ToCharArray();
                        for (int i = 0; i < 8; i++)
                        {
                            harflerq.Add(karakterler[i].ToString());
                        }
                        OrtakIslem(0);
                        rastgeleBtn.Enabled = false;
                        eslestirBtn.Enabled = false;
                        veriCekBtn.Enabled = false;
                        break;
                }
            }
        }

        private void veriCekBtn_Click(object sender, EventArgs e)//Entity framework kullanarak veritabanından kelimeleri çeker
        {
            dataGridView1.DataSource = ent.kelimelers.ToList();
            veriCekBtn.Enabled = false;
            eslestirBtn.Enabled = true;
            rastgeleBtn.Enabled = true;
            yeniOyunBtn.Enabled = true;
        }

        private void yeniOyunBtn_Click(object sender, EventArgs e)//Her şeyi sıfırlar
        {
            veriCekBtn.Enabled = true;
            eslestirBtn.Enabled = false;
            rastgeleBtn.Enabled = false;
            yeniOyunBtn.Enabled = false;
            listBox3.Items.Clear();
            sekizli.Clear();
            yedili.Clear();
            altili.Clear();
            besli.Clear();
            dortlu.Clear();
            uclu.Clear();
            dataGridView1.DataSource = null;
            harflerTxt.Text = "";
            harflerq.Clear();
        }

        public void OrtakIslem(int secim)//Rastgele ve elle eklenen işlemlerde ortak olanları bir yerde topladım
        {
            kombinasyon kmb = new kombinasyon();
            kmb.listele(harflerq,sekizli,yedili,altili,besli,dortlu,uclu);
            if (secim == 1) 
            {
                for (int j = 0; j < dataGridView1.RowCount; j++)//Eşleşenlerin listbox3'e yazıldığı yer
                {
                    if (sekizli.Contains(dataGridView1.Rows[j].Cells["words"].Value.ToString()))
                    {
                        listBox3.Items.Add(dataGridView1.Rows[j].Cells["words"].Value.ToString() + ": 11 Puan");
                        break;
                    }
                    if (yedili.Contains(dataGridView1.Rows[j].Cells["words"].Value.ToString()))
                    {
                        listBox3.Items.Add(dataGridView1.Rows[j].Cells["words"].Value.ToString() + ": 9 Puan");
                        break;
                    }
                    if (altili.Contains(dataGridView1.Rows[j].Cells["words"].Value.ToString()))
                    {
                        listBox3.Items.Add(dataGridView1.Rows[j].Cells["words"].Value.ToString() + ": 7 Puan");
                        break;
                    }
                    if (besli.Contains(dataGridView1.Rows[j].Cells["words"].Value.ToString()))
                    {
                        listBox3.Items.Add(dataGridView1.Rows[j].Cells["words"].Value.ToString() + ": 5 Puan");
                        break;
                    }
                    if (dortlu.Contains(dataGridView1.Rows[j].Cells["words"].Value.ToString()))
                    {
                        listBox3.Items.Add(dataGridView1.Rows[j].Cells["words"].Value.ToString() + ": 4 Puan");
                        break;
                    }
                    if (uclu.Contains(dataGridView1.Rows[j].Cells["words"].Value.ToString()))
                    {
                        listBox3.Items.Add(dataGridView1.Rows[j].Cells["words"].Value.ToString() + ": 3 Puan");
                        break;
                    }
                }
            }
            else
            {
                for (int j = 0; j < dataGridView1.RowCount; j++)//Eşleşenlerin listbox3'e yazıldığı yer
                {
                    if (sekizli.Contains(dataGridView1.Rows[j].Cells["words"].Value.ToString()))
                    {
                        listBox3.Items.Add(dataGridView1.Rows[j].Cells["words"].Value.ToString() + ": 11 Puan");
                    }
                    if (yedili.Contains(dataGridView1.Rows[j].Cells["words"].Value.ToString()))
                    {
                        listBox3.Items.Add(dataGridView1.Rows[j].Cells["words"].Value.ToString() + ": 9 Puan");
                    }
                    if (altili.Contains(dataGridView1.Rows[j].Cells["words"].Value.ToString()))
                    {
                        listBox3.Items.Add(dataGridView1.Rows[j].Cells["words"].Value.ToString() + ": 7 Puan");
                    }
                    if (besli.Contains(dataGridView1.Rows[j].Cells["words"].Value.ToString()))
                    {
                        listBox3.Items.Add(dataGridView1.Rows[j].Cells["words"].Value.ToString() + ": 5 Puan");
                    }
                    if (dortlu.Contains(dataGridView1.Rows[j].Cells["words"].Value.ToString()))
                    {
                        listBox3.Items.Add(dataGridView1.Rows[j].Cells["words"].Value.ToString() + ": 4 Puan");
                    }
                    if (uclu.Contains(dataGridView1.Rows[j].Cells["words"].Value.ToString()))
                    {
                        listBox3.Items.Add(dataGridView1.Rows[j].Cells["words"].Value.ToString() + ": 3 Puan");
                    }
                }
            }
            
            if (listBox3.Items.Count == 0)
            {
                MessageBox.Show("Veritabanıyla eşleşen kelime bulunamadı!", "BULUNAMADI", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                MessageBox.Show("Kelimeler bulundu!", "BULUNDU", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            eslestirBtn.Enabled = false;
            rastgeleBtn.Enabled = false;
            yeniOyunBtn.Enabled = false;
        }

        private void nasilBtn_Click(object sender, EventArgs e)
        {
            kelimeOyunuPnl.Visible = false;
            nasilCalisirPnl.Visible = true;
            nasilBtn.Normalcolor = Color.FromArgb(0, 196, 204);
            oynaBtn.Normalcolor = Color.Transparent;
        }

        private void oynaBtn_Click(object sender, EventArgs e)
        {
            kelimeOyunuPnl.Visible = true;
            nasilCalisirPnl.Visible = false;
            oynaBtn.Normalcolor = Color.FromArgb(0, 196, 204);
            nasilBtn.Normalcolor = Color.Transparent;
        }
    }
}
