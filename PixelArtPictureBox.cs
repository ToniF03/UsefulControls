using System.Drawing.Drawing2D;
using System.Windows.Forms;

    public class PixelArtPictureBox : PictureBox
    {
        private InterpolationMode _interpolationMode = InterpolationMode.Default;
        public InterpolationMode InterpolationMode
        {
            get
            {
                return _interpolationMode;
            }
            set
            {
                if (value != _interpolationMode)
                {
                    _interpolationMode = value;
                    this.Refresh();
                }
            }
        }
        protected override void OnPaint(System.Windows.Forms.PaintEventArgs pe)
        {
            pe.Graphics.InterpolationMode = _interpolationMode;
            pe.Graphics.PixelOffsetMode = PixelOffsetMode.Half;
            pe.Graphics.SmoothingMode = SmoothingMode.None;
            base.OnPaint(pe);
        }
    }
