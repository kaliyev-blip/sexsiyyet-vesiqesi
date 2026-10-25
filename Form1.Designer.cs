using System.Drawing;
using System.Windows.Forms;

namespace WinFormsApp2;

public partial class Form1
{
    private GroupBox grpKart;
    private Panel pnlKart;
    private PictureBox picFoto;
    private Label lblBasliq;
    private Label lblAlt;
    private Button btnSekil;

    private GroupBox grpMelumat;
    private Label lblSeriya;
    private TextBox txtSeriya;
    private Label lblFin;
    private TextBox txtFin;
    private Label lblSoyad;
    private TextBox txtSoyad;
    private Label lblAd;
    private TextBox txtAd;
    private Label lblAta;
    private TextBox txtAta;
    private Label lblCins;
    private ComboBox cmbCins;
    private Label lblDogum;
    private DateTimePicker dtpDogum;
    private Label lblYer;
    private TextBox txtDogumYeri;
    private Label lblAile;
    private ComboBox cmbAile;
    private Label lblVerilme;
    private DateTimePicker dtpVerilme;
    private Label lblEtibar;
    private DateTimePicker dtpEtibar;
    private Label lblVeren;
    private TextBox txtVeren;
    private Button btnTemizle;
    private Button btnYadda;

    private void InitializeComponent()
    {
        grpKart = new GroupBox();
        pnlKart = new Panel();
        picFoto = new PictureBox();
        lblBasliq = new Label();
        lblAlt = new Label();
        btnSekil = new Button();
        grpMelumat = new GroupBox();
        lblSeriya = new Label();
        txtSeriya = new TextBox();
        lblFin = new Label();
        txtFin = new TextBox();
        lblSoyad = new Label();
        txtSoyad = new TextBox();
        lblAd = new Label();
        txtAd = new TextBox();
        lblAta = new Label();
        txtAta = new TextBox();
        lblCins = new Label();
        cmbCins = new ComboBox();
        lblDogum = new Label();
        dtpDogum = new DateTimePicker();
        lblYer = new Label();
        txtDogumYeri = new TextBox();
        lblAile = new Label();
        cmbAile = new ComboBox();
        lblVerilme = new Label();
        dtpVerilme = new DateTimePicker();
        lblEtibar = new Label();
        dtpEtibar = new DateTimePicker();
        lblVeren = new Label();
        txtVeren = new TextBox();
        btnTemizle = new Button();
        btnYadda = new Button();

        grpKart.SuspendLayout();
        pnlKart.SuspendLayout();
        grpMelumat.SuspendLayout();
        SuspendLayout();

        Font = new Font("Segoe UI", 9F);

        grpKart.Text = "Şəxsiyyət vəsiqəsi";
        grpKart.Location = new Point(12, 8);
        grpKart.Size = new Size(500, 548);

        pnlKart.Location = new Point(14, 26);
        pnlKart.Size = new Size(472, 468);
        pnlKart.BackColor = Color.FromArgb(215, 231, 246);
        pnlKart.BorderStyle = BorderStyle.FixedSingle;

        lblBasliq.Text = "AZƏRBAYCAN RESPUBLİKASI";
        lblBasliq.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        lblBasliq.ForeColor = Color.FromArgb(26, 77, 124);
        lblBasliq.Location = new Point(12, 10);
        lblBasliq.AutoSize = true;

        lblAlt.Text = "Şəxsiyyət vəsiqəsi";
        lblAlt.Location = new Point(12, 34);
        lblAlt.AutoSize = true;

        picFoto.Location = new Point(12, 62);
        picFoto.Size = new Size(128, 164);
        picFoto.BackColor = Color.FromArgb(196, 214, 232);
        picFoto.BorderStyle = BorderStyle.FixedSingle;
        picFoto.SizeMode = PictureBoxSizeMode.Zoom;
        picFoto.TabStop = false;

        btnSekil.Text = "Şəkil yüklə...";
        btnSekil.Location = new Point(14, 504);
        btnSekil.Size = new Size(472, 32);

        pnlKart.Controls.Add(lblBasliq);
        pnlKart.Controls.Add(lblAlt);
        pnlKart.Controls.Add(picFoto);
        grpKart.Controls.Add(pnlKart);
        grpKart.Controls.Add(btnSekil);

        grpMelumat.Text = "Vəsiqə məlumatları";
        grpMelumat.Location = new Point(524, 8);
        grpMelumat.Size = new Size(420, 548);

        void Saha(Label ad, string metn, Control qutu, int sira)
        {
            int y = 28 + sira * 40;
            ad.Text = metn;
            ad.Location = new Point(16, y);
            ad.Size = new Size(150, 27);
            ad.TextAlign = ContentAlignment.MiddleLeft;
            qutu.Location = new Point(170, y);
            qutu.Size = new Size(232, 27);
            grpMelumat.Controls.Add(ad);
            grpMelumat.Controls.Add(qutu);
        }

        Saha(lblSeriya, "Seriya və nömrə:", txtSeriya, 0);
        Saha(lblFin, "FİN kod:", txtFin, 1);
        Saha(lblSoyad, "Soyad:", txtSoyad, 2);
        Saha(lblAd, "Ad:", txtAd, 3);
        Saha(lblAta, "Ata adı:", txtAta, 4);
        Saha(lblCins, "Cins:", cmbCins, 5);
        Saha(lblDogum, "Doğum tarixi:", dtpDogum, 6);
        Saha(lblYer, "Doğum yeri:", txtDogumYeri, 7);
        Saha(lblAile, "Ailə vəziyyəti:", cmbAile, 8);
        Saha(lblVerilme, "Verilmə tarixi:", dtpVerilme, 9);
        Saha(lblEtibar, "Etibarlıdır:", dtpEtibar, 10);
        Saha(lblVeren, "Verən orqan:", txtVeren, 11);

        btnTemizle.Text = "Təmizlə";
        btnTemizle.Location = new Point(700, 568);
        btnTemizle.Size = new Size(110, 34);

        btnYadda.Text = "Yadda saxla";
        btnYadda.Location = new Point(818, 568);
        btnYadda.Size = new Size(126, 34);

        AcceptButton = btnYadda;
        ClientSize = new Size(956, 614);
        Controls.Add(grpKart);
        Controls.Add(grpMelumat);
        Controls.Add(btnTemizle);
        Controls.Add(btnYadda);
        FormBorderStyle = FormBorderStyle.FixedSingle;
        MaximizeBox = false;
        StartPosition = FormStartPosition.CenterScreen;
        Text = "Azərbaycan Şəxsiyyət Vəsiqəsi";

        grpKart.ResumeLayout(false);
        pnlKart.ResumeLayout(false);
        grpMelumat.ResumeLayout(false);
        ResumeLayout(false);
    }
}