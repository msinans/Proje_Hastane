using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Proje_Hastane
{
    public partial class FrmHastaDetay : Form
    {
        public FrmHastaDetay()
        {
            InitializeComponent();
        }
        public string tc;
        sqlbaglantisi bgl = new sqlbaglantisi();
        private void FrmHastaDetay_Load(object sender, EventArgs e)
        {
            LblTC.Text = tc; // Bu tc yi LblTC ye yazdıralım. // Bu tc değişkenini taşımak için FrmHastaGiris formuna fr.Show(); dan önce fr.tc = MskTC.Text; yazıyorum. 

            // Ad Soyad Çekme
            SqlCommand komut = new SqlCommand("Select HastaAd, HastaSoyad From Tbl_Hastalar Where HastaTC=@p1", bgl.baglanti()); // HastaTC si @p1 deki değeri eşit olan satırın ad soyadını getir.
            komut.Parameters.AddWithValue("@p1", LblTC.Text); //LblTC deki değeri @p1 e ata
            SqlDataReader dr = komut.ExecuteReader();
            while (dr.Read())
            {
                LblAdSoyad.Text = dr[0] + " " + dr[1]; // Araya boşluk kyduğum için string e çevirmeye gerek kalmadı. Bir komut satırı içerisinde sting ifade varsa diğer değerleri stringe çevirmeye gerek kalmaz
            }
            bgl.baglanti().Close();

            //Randevu Geçmişi

            DataTable dt = new DataTable(); // Bir veri tablosu oluşturdum.
            SqlDataAdapter da = new SqlDataAdapter("Select * From Tbl_Randevular Where HastaTC=" + tc, bgl.baglanti()); //SqlDataAdapter verileri DGV ye aktarmak için kullandığım command ım. Bunda parametre (@p1 gibi)kullanılmaz.HastaTC si tc isimli değişkenden gelene eşit olanı alıyor
            da.Fill(dt); // DataAdapter ın içini tablodan gelecek değer ile sanal olarak doldur
            dataGridView1.DataSource = dt; // DGV nin bilgi kaynağı eşittir dt den gelen tablo
                                           // DGV de bağlantı açıp kapamaya gerek yoktur

            // Branşları Çekme
            SqlCommand komut2 = new SqlCommand("Select BransAd From Tbl_Branslar", bgl.baglanti());
            SqlDataReader dr2 = komut2.ExecuteReader();
            while (dr2.Read())
            {
                CmbBrans.Items.Add(dr2[0]);
            }
            bgl.baglanti().Close();

        }

        private void CmbBrans_SelectedIndexChanged(object sender, EventArgs e)
        {
            CmbDoktor.Items.Clear(); // Önce comboBox ın içini temizlesin yoksa her branş seçtiğimde önceki branşın doktoru orada kalıyor.
            SqlCommand komut3 = new SqlCommand("Select DoktorAd, DoktorSoyad From Tbl_Doktorlar Where DoktorBrans=@p1", bgl.baglanti()); 
            komut3.Parameters.AddWithValue("@p1", CmbBrans.Text); // @p1 parametresinde alacağı değere göre doktorların ad soyadlarını getir.
            SqlDataReader dr3 = komut3.ExecuteReader();
            while (dr3.Read())
            {
                CmbDoktor.Items.Add(dr3[0] + " " + dr3[1]);
            }
            bgl.baglanti().Close();
        }

        private void CmbDoktor_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter("Select * From Tbl_Randevular Where RandevuBrans = ' " + CmbBrans.Text + " ' ", bgl.baglanti());
            da.Fill(dt);
            dataGridView2.DataSource = dt;
        }

        private void LnkBilgiDuzenle_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            FrmBilgiDuzenle fr = new FrmBilgiDuzenle();
            fr.TCno = LblTC.Text;
            fr.Show();
        }
    }
}
