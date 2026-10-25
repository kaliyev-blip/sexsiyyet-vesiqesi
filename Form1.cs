using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace WinFormsApp2;

public partial class Form1 : Form
{
    private readonly Dictionary<string, Label> _deyer = new Dictionary<string, Label>();
    private readonly CultureInfo _az = CultureInfo.GetCultureInfo("az-AZ");
    private bool _proqram;
    private bool _etibarAvto = true;

    public Form1()
    {
        InitializeComponent();
        KartiQur();
        Qur();
    }

    private void Qur()
    {
        txtSeriya.PlaceholderText = "AA1234567";
        txtFin.PlaceholderText = "1A2B3C4";
        txtSoyad.PlaceholderText = "Məmmədov";
        txtAd.PlaceholderText = "Əli";
        txtAta.PlaceholderText = "Vəli";
        txtDogumYeri.PlaceholderText = "Bakı";
        txtVeren.PlaceholderText = "din";

        cmbCins.DropDownStyle = ComboBoxStyle.DropDownList;
        cmbAile.DropDownStyle = ComboBoxStyle.DropDownList;
        cmbCins.Items.AddRange(new object[] { "Kişi", "Qadın" });
        cmbAile.Items.AddRange(new object[] { "Subay", "Evli", "Boşanmış", "Dul" });

        foreach (var d in new[] { dtpDogum, dtpVerilme, dtpEtibar })
        {
            d.Format = DateTimePickerFormat.Custom;
            d.CustomFormat = "dd.MM.yyyy";
            d.ShowCheckBox = true;
            d.Checked = false;
        }

        txtSeriya.TextChanged += (s, e) => LatinBoyuk(txtSeriya, 11);
        txtFin.TextChanged += (s, e) => LatinBoyuk(txtFin, 7);
        txtSoyad.Leave += AdiDuzelt;
        txtAd.Leave += AdiDuzelt;
        txtAta.Leave += AdiDuzelt;
        txtDogumYeri.Leave += AdiDuzelt;
        txtVeren.Leave += VereniDuzelt;

        txtSoyad.TextChanged += (s, e) => KartiYenile();
        txtAd.TextChanged += (s, e) => KartiYenile();
        txtAta.TextChanged += (s, e) => KartiYenile();
        txtDogumYeri.TextChanged += (s, e) => KartiYenile();
        cmbCins.SelectedIndexChanged += (s, e) => KartiYenile();
        cmbAile.SelectedIndexChanged += (s, e) => KartiYenile();
        dtpDogum.ValueChanged += (s, e) => KartiYenile();
        dtpVerilme.ValueChanged += VerilmeDeyisdi;
        dtpEtibar.ValueChanged += EtibarDeyisdi;

        btnSekil.Click += SekilYukle;
        btnTemizle.Click += Temizle;
        btnYadda.Click += YaddaSaxla;
        picFoto.Paint += FotoCiz;

        KartiYenile();
    }

    private void KartiQur()
    {
        string[] sol = { "Soyad", "Ad", "Ata adı", "Doğum tarixi", "Doğum yeri", "Cins" };
        string[] sag = { "Seriya", "FİN", "Ailə vəz.", "Verilmə", "Etibarlıdır" };
        for (int i = 0; i < sol.Length; i++) Etiket(sol[i], 152, 64 + i * 26, 150);
        for (int i = 0; i < sag.Length; i++) Etiket(sag[i], 308, 64 + i * 26, 150);
    }

    private void Etiket(string ad, int x, int y, int en)
    {
        var l = new Label
        {
            AutoSize = false,
            AutoEllipsis = true,
            Location = new Point(x, y),
            Size = new Size(en, 22),
            Text = ad + ":"
        };
        pnlKart.Controls.Add(l);
        _deyer[ad] = l;
    }

    private void LatinBoyuk(TextBox tb, int max)
    {
        int pos = tb.SelectionStart;
        string temiz = new string(tb.Text.ToUpperInvariant().Where(Latin).Take(max).ToArray());
        if (tb.Text == temiz)
        {
            KartiYenile();
            return;
        }
        tb.Text = temiz;
        tb.SelectionStart = Math.Min(pos, temiz.Length);
    }

    private static bool Latin(char c) => (c >= 'A' && c <= 'Z') || (c >= '0' && c <= '9');

    private void AdiDuzelt(object sender, EventArgs e)
    {
        var tb = (TextBox)sender;
        string yeni = AdBoyuk(tb.Text);
        if (tb.Text != yeni) tb.Text = yeni;
        KartiYenile();
    }

    private string AdBoyuk(string metn)
    {
        var sozler = metn.Trim().Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
        return string.Join(" ", sozler.Select(soz =>
            string.Join("-", soz.Split('-').Select(SozBoyuk))));
    }

    private string SozBoyuk(string soz)
    {
        string kicik = soz.ToLower(_az);
        if (kicik == "oğlu" || kicik == "qızı" || kicik == "oglu" || kicik == "qizi") return kicik;
        if (kicik.Length == 0) return kicik;
        return char.ToUpper(kicik[0], _az) + kicik.Substring(1);
    }

    private void VereniDuzelt(object sender, EventArgs e)
    {
        txtVeren.Text = VerenMetni(txtVeren.Text);
    }

    private string VerenMetni(string metn)
    {
        string a = metn.Trim().ToLower(_az);
        if (a == "din" || a == "daxili işlər nazirliyi" || a == "daxili isler nazirliyi")
            return "Daxili İşlər Nazirliyi";
        if (a == "asan" || a == "asan xidmət" || a == "asan xidmet")
            return "ASAN Xidmət";
        if (a == "dmx" || a == "miqrasiya")
            return "Dövlət Miqrasiya Xidməti";
        return metn.Trim();
    }

    private void VerilmeDeyisdi(object sender, EventArgs e)
    {
        if (!_proqram && dtpVerilme.Checked && _etibarAvto)
        {
            _proqram = true;
            dtpEtibar.Value = dtpVerilme.Value.AddYears(10);
            dtpEtibar.Checked = true;
            _proqram = false;
        }
        KartiYenile();
    }

    private void EtibarDeyisdi(object sender, EventArgs e)
    {
        if (!_proqram) _etibarAvto = false;
        KartiYenile();
    }

    private void KartiYenile()
    {
        Deyer("Soyad", txtSoyad.Text);
        Deyer("Ad", txtAd.Text);
        Deyer("Ata adı", txtAta.Text);
        Deyer("Doğum tarixi", Tarix(dtpDogum));
        Deyer("Doğum yeri", txtDogumYeri.Text);
        Deyer("Cins", cmbCins.Text);
        Deyer("Seriya", txtSeriya.Text);
        Deyer("FİN", txtFin.Text);
        Deyer("Ailə vəz.", cmbAile.Text);
        Deyer("Verilmə", Tarix(dtpVerilme));
        Deyer("Etibarlıdır", Tarix(dtpEtibar));
    }

    private static string Tarix(DateTimePicker d) => d.Checked ? d.Value.ToString("dd.MM.yyyy") : "";

    private void Deyer(string ad, string qiymet)
    {
        _deyer[ad].Text = string.IsNullOrWhiteSpace(qiymet) ? ad + ":" : ad + ":  " + qiymet.Trim();
    }

    private void FotoCiz(object sender, PaintEventArgs e)
    {
        if (picFoto.Image != null) return;
        TextRenderer.DrawText(e.Graphics, "FOTO", Font, picFoto.ClientRectangle,
            Color.DimGray, TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
    }

    private void SekilYukle(object sender, EventArgs e)
    {
        using (var dlg = new OpenFileDialog())
        {
            dlg.Title = "Şəkil seçin";
            dlg.Filter = "Şəkillər|*.jpg;*.jpeg;*.png;*.bmp";
            if (dlg.ShowDialog() != DialogResult.OK) return;

            try
            {
                using (var oxu = new MemoryStream(File.ReadAllBytes(dlg.FileName)))
                using (var img = Image.FromStream(oxu))
                {
                    var kopiya = new Bitmap(img);
                    if (picFoto.Image != null) picFoto.Image.Dispose();
                    picFoto.Image = kopiya;
                }
                picFoto.Invalidate();
            }
            catch
            {
                MessageBox.Show("Şəkil açılmadı.", "Xəta", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }

    private void Temizle(object sender, EventArgs e)
    {
        txtSeriya.Clear();
        txtFin.Clear();
        txtSoyad.Clear();
        txtAd.Clear();
        txtAta.Clear();
        txtDogumYeri.Clear();
        txtVeren.Clear();
        cmbCins.SelectedIndex = -1;
        cmbAile.SelectedIndex = -1;

        _proqram = true;
        dtpDogum.Checked = false;
        dtpVerilme.Checked = false;
        dtpEtibar.Checked = false;
        _proqram = false;
        _etibarAvto = true;

        if (picFoto.Image != null) picFoto.Image.Dispose();
        picFoto.Image = null;
        picFoto.Invalidate();
        KartiYenile();
    }

    private void YaddaSaxla(object sender, EventArgs e)
    {
        txtSoyad.Text = AdBoyuk(txtSoyad.Text);
        txtAd.Text = AdBoyuk(txtAd.Text);
        txtAta.Text = AdBoyuk(txtAta.Text);
        txtDogumYeri.Text = AdBoyuk(txtDogumYeri.Text);
        txtVeren.Text = VerenMetni(txtVeren.Text);

        var sehv = Yoxla();
        if (sehv.Count > 0)
        {
            MessageBox.Show(string.Join(Environment.NewLine, sehv), "Yoxlama",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        if (dtpDogum.Checked && Yas(dtpDogum.Value) < 15)
        {
            if (Soruş("Yaş 15-dən kiçikdir. Yenə də saxlanılsın?") != DialogResult.Yes) return;
        }
        if (dtpEtibar.Checked && dtpEtibar.Value.Date < DateTime.Today)
        {
            if (Soruş("Etibar müddəti bitib. Yenə də saxlanılsın?") != DialogResult.Yes) return;
        }

        using (var dlg = new SaveFileDialog())
        {
            dlg.Filter = "Mətn|*.txt";
            dlg.FileName = FaylAdi(txtSoyad.Text, txtFin.Text);
            if (dlg.ShowDialog() != DialogResult.OK) return;

            var setir = new[]
            {
                "AZƏRBAYCAN RESPUBLİKASI — Şəxsiyyət vəsiqəsi",
                "Seriya və nömrə: " + txtSeriya.Text,
                "FİN: " + txtFin.Text,
                "Soyad: " + txtSoyad.Text,
                "Ad: " + txtAd.Text,
                "Ata adı: " + txtAta.Text,
                "Cins: " + cmbCins.Text,
                "Doğum tarixi: " + Tarix(dtpDogum),
                "Doğum yeri: " + txtDogumYeri.Text,
                "Ailə vəziyyəti: " + cmbAile.Text,
                "Verilmə tarixi: " + Tarix(dtpVerilme),
                "Etibarlıdır: " + Tarix(dtpEtibar),
                "Verən orqan: " + txtVeren.Text,
                "Şəkil: " + (picFoto.Image == null ? "yoxdur" : "yüklənib")
            };
            File.WriteAllLines(dlg.FileName, setir, Encoding.UTF8);
        }

        MessageBox.Show("Yadda saxlanıldı.", "Hazır", MessageBoxButtons.OK, MessageBoxIcon.Information);
    }

    private List<string> Yoxla()
    {
        var s = new List<string>();
        if (!Regex.IsMatch(txtSeriya.Text, "^[A-Z]{2,3}[0-9]{5,8}$"))
            s.Add("Seriya 2-3 hərf və 5-8 rəqəm olmalıdır. Məs: AA1234567.");
        if (!Regex.IsMatch(txtFin.Text, "^(?=.*[A-Z])(?=.*[0-9])[A-Z0-9]{7}$"))
            s.Add("FİN 7 simvol olmalıdır, həm hərf həm rəqəm. Məs: 1A2B3C4.");
        if (txtSoyad.Text.Trim().Length < 2) s.Add("Soyadı yazın.");
        if (txtAd.Text.Trim().Length < 2) s.Add("Adı yazın.");
        if (txtAta.Text.Trim().Length < 2) s.Add("Ata adını yazın.");
        if (cmbCins.SelectedIndex < 0) s.Add("Cinsi seçin.");
        if (!dtpDogum.Checked) s.Add("Doğum tarixini seçin.");
        else if (dtpDogum.Value.Date > DateTime.Today) s.Add("Doğum tarixi gələcək ola bilməz.");
        if (txtDogumYeri.Text.Trim().Length < 2) s.Add("Doğum yerini yazın.");
        if (cmbAile.SelectedIndex < 0) s.Add("Ailə vəziyyətini seçin.");
        if (!dtpVerilme.Checked) s.Add("Verilmə tarixini seçin.");
        if (!dtpEtibar.Checked) s.Add("Etibar tarixini seçin.");
        if (dtpDogum.Checked && dtpVerilme.Checked && dtpVerilme.Value.Date < dtpDogum.Value.Date)
            s.Add("Verilmə tarixi doğumdan əvvəl ola bilməz.");
        if (dtpVerilme.Checked && dtpEtibar.Checked && dtpEtibar.Value.Date <= dtpVerilme.Value.Date)
            s.Add("Etibar tarixi verilmədən sonra olmalıdır.");
        if (txtVeren.Text.Trim().Length < 2) s.Add("Verən orqanı yazın.");
        return s;
    }

    private static int Yas(DateTime dogum)
    {
        var bu = DateTime.Today;
        int yas = bu.Year - dogum.Year;
        if (dogum.Date > bu.AddYears(-yas)) yas--;
        return yas;
    }

    private static DialogResult Soruş(string metn)
    {
        return MessageBox.Show(metn, "Diqqət", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
    }

    private static string FaylAdi(string soyad, string fin)
    {
        string ad = (soyad + "_" + fin).Trim();
        foreach (char c in Path.GetInvalidFileNameChars()) ad = ad.Replace(c, '_');
        if (ad.Length == 0) ad = "vesiqe";
        return ad + ".txt";
    }

    protected override void OnFormClosed(FormClosedEventArgs e)
    {
        if (picFoto.Image != null) picFoto.Image.Dispose();
        base.OnFormClosed(e);
    }
}