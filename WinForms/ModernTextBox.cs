using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

    class ModernTextBox : TextBox
    {
        private Label Watermark;
        private Timer tmr = new Timer();

        public ModernTextBox()
        {
            tmr.Tick += Tmr_Tick;
            Watermark = new Label();
            this.TextAlignChanged += ModernTextBox_TextAlignChanged;
            this.WatermarkText = "Watermark Text";
            this.Controls.Add(Watermark);
            Watermark.MouseUp += Watermark_MouseUp;
            Watermark.Font = this.Font;
            Watermark.AutoSize = false;
            Watermark.Size = this.Size;
            Watermark.Location = new System.Drawing.Point(0, -2);
            Watermark.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right;
            Watermark.ForeColor = System.Drawing.Color.Gray;
            Watermark.Text = this.WatermarkText;
            Watermark.Show();
            tmr.Interval = 100;
            tmr.Start();
            switch (this.TextAlign) {
                case HorizontalAlignment.Left:
                    Watermark.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
                    break;
                case HorizontalAlignment.Center:
                    Watermark.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
                    break;
                case HorizontalAlignment.Right:
                    Watermark.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
                    break;
            }
            if (this.Text != "" && this.Focused)
            {
                Watermark.Hide();
            }
        }

        private void Tmr_Tick(object sender, EventArgs e)
        {
            if (this.Text == "" && !this.Focused) Watermark.Show();
            else Watermark.Hide();
        }

        private void Watermark_MouseUp(object sender, MouseEventArgs e)
        {
            Watermark.Hide();
            this.Focus();
        }

        private void ModernTextBox_TextAlignChanged(object sender, EventArgs e)
        {
            switch (this.TextAlign)
            {
                case HorizontalAlignment.Left:
                    Watermark.TextAlign = System.Drawing.ContentAlignment.TopLeft;
                    break;
                case HorizontalAlignment.Center:
                    Watermark.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
                    break;
                case HorizontalAlignment.Right:
                    Watermark.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
                    break;
            }
        }

        public string WatermarkText
        {
            get { return Watermark.Text; }
            set { Watermark.Text = value; }
        }
    }
