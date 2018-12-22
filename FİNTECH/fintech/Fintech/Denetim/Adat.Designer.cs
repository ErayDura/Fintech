namespace Fintech
{
	partial class Adat
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.dataGridView1 = new System.Windows.Forms.DataGridView();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.button2 = new System.Windows.Forms.Button();
			this.label6 = new System.Windows.Forms.Label();
			this.textBoxAdatHarici = new System.Windows.Forms.TextBox();
			this.dateTimePicker2 = new System.Windows.Forms.DateTimePicker();
			this.dateTimePicker1 = new System.Windows.Forms.DateTimePicker();
			this.label5 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.textBoxFaizOrani = new System.Windows.Forms.TextBox();
			this.label3 = new System.Windows.Forms.Label();
			this.textBoxHesapKodu = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.button1 = new System.Windows.Forms.Button();
			this.panel1 = new System.Windows.Forms.Panel();
			this.buttonEkle = new System.Windows.Forms.Button();
			((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
			this.groupBox1.SuspendLayout();
			this.SuspendLayout();
			// 
			// dataGridView1
			// 
			this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.dataGridView1.Location = new System.Drawing.Point(128, 196);
			this.dataGridView1.Name = "dataGridView1";
			this.dataGridView1.Size = new System.Drawing.Size(1162, 456);
			this.dataGridView1.TabIndex = 2;
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.buttonEkle);
			this.groupBox1.Controls.Add(this.panel1);
			this.groupBox1.Controls.Add(this.button2);
			this.groupBox1.Controls.Add(this.label6);
			this.groupBox1.Controls.Add(this.textBoxAdatHarici);
			this.groupBox1.Controls.Add(this.dateTimePicker2);
			this.groupBox1.Controls.Add(this.dateTimePicker1);
			this.groupBox1.Controls.Add(this.label5);
			this.groupBox1.Controls.Add(this.label4);
			this.groupBox1.Controls.Add(this.textBoxFaizOrani);
			this.groupBox1.Controls.Add(this.label3);
			this.groupBox1.Controls.Add(this.textBoxHesapKodu);
			this.groupBox1.Controls.Add(this.label2);
			this.groupBox1.Location = new System.Drawing.Point(152, 12);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(1033, 178);
			this.groupBox1.TabIndex = 3;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Değerleri Doldurunuz";
			// 
			// button2
			// 
			this.button2.Location = new System.Drawing.Point(840, 29);
			this.button2.Name = "button2";
			this.button2.Size = new System.Drawing.Size(96, 27);
			this.button2.TabIndex = 12;
			this.button2.Text = "TCMB Faiz Oranı ";
			this.button2.UseVisualStyleBackColor = true;
			this.button2.Click += new System.EventHandler(this.button2_Click);
			// 
			// label6
			// 
			this.label6.AutoSize = true;
			this.label6.Location = new System.Drawing.Point(163, 20);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(87, 13);
			this.label6.TabIndex = 11;
			this.label6.Text = "Adat Harici Tutar";
			// 
			// textBoxAdatHarici
			// 
			this.textBoxAdatHarici.Location = new System.Drawing.Point(157, 36);
			this.textBoxAdatHarici.Name = "textBoxAdatHarici";
			this.textBoxAdatHarici.Size = new System.Drawing.Size(110, 20);
			this.textBoxAdatHarici.TabIndex = 10;
			this.textBoxAdatHarici.Text = "0";
			// 
			// dateTimePicker2
			// 
			this.dateTimePicker2.Location = new System.Drawing.Point(515, 33);
			this.dateTimePicker2.Name = "dateTimePicker2";
			this.dateTimePicker2.Size = new System.Drawing.Size(200, 20);
			this.dateTimePicker2.TabIndex = 9;
			// 
			// dateTimePicker1
			// 
			this.dateTimePicker1.Location = new System.Drawing.Point(298, 33);
			this.dateTimePicker1.Name = "dateTimePicker1";
			this.dateTimePicker1.Size = new System.Drawing.Size(200, 20);
			this.dateTimePicker1.TabIndex = 8;
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Location = new System.Drawing.Point(735, 17);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(71, 13);
			this.label5.TabIndex = 6;
			this.label5.Text = "Faiz Oranı (%)";
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(565, 17);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(58, 13);
			this.label4.TabIndex = 4;
			this.label4.Text = "Adat Tarihi";
			// 
			// textBoxFaizOrani
			// 
			this.textBoxFaizOrani.Location = new System.Drawing.Point(725, 33);
			this.textBoxFaizOrani.Name = "textBoxFaizOrani";
			this.textBoxFaizOrani.Size = new System.Drawing.Size(99, 20);
			this.textBoxFaizOrani.TabIndex = 3;
			this.textBoxFaizOrani.Text = "0";
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(341, 17);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(82, 13);
			this.label3.TabIndex = 2;
			this.label3.Text = "Başlangıç Tarihi";
			// 
			// textBoxHesapKodu
			// 
			this.textBoxHesapKodu.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
			this.textBoxHesapKodu.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
			this.textBoxHesapKodu.Location = new System.Drawing.Point(26, 36);
			this.textBoxHesapKodu.Name = "textBoxHesapKodu";
			this.textBoxHesapKodu.Size = new System.Drawing.Size(100, 20);
			this.textBoxHesapKodu.TabIndex = 1;
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(11, 19);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(144, 13);
			this.label2.TabIndex = 0;
			this.label2.Text = "Adatlandırılacak Hesap Kodu";
			// 
			// button1
			// 
			this.button1.Location = new System.Drawing.Point(1191, 80);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(84, 38);
			this.button1.TabIndex = 4;
			this.button1.Text = "Hesapla";
			this.button1.UseVisualStyleBackColor = true;
			this.button1.Click += new System.EventHandler(this.button1_Click);
			// 
			// panel1
			// 
			this.panel1.AutoScroll = true;
			this.panel1.Location = new System.Drawing.Point(298, 62);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(665, 110);
			this.panel1.TabIndex = 13;
			// 
			// buttonEkle
			// 
			this.buttonEkle.Location = new System.Drawing.Point(942, 24);
			this.buttonEkle.Name = "buttonEkle";
			this.buttonEkle.Size = new System.Drawing.Size(75, 37);
			this.buttonEkle.TabIndex = 14;
			this.buttonEkle.Text = "Ekle";
			this.buttonEkle.UseVisualStyleBackColor = true;
			this.buttonEkle.Click += new System.EventHandler(this.buttonEkle_Click);
			// 
			// Adat
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(1362, 741);
			this.Controls.Add(this.button1);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.dataGridView1);
			this.Name = "Adat";
			this.Text = "Adat";
			this.Load += new System.EventHandler(this.Adat_Load);
			((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion
		private System.Windows.Forms.DataGridView dataGridView1;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.DateTimePicker dateTimePicker1;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.TextBox textBoxFaizOrani;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.TextBox textBoxHesapKodu;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.TextBox textBoxAdatHarici;
		private System.Windows.Forms.DateTimePicker dateTimePicker2;
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.Button button2;
		private System.Windows.Forms.Button buttonEkle;
		private System.Windows.Forms.Panel panel1;
	}
}