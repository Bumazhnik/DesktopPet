using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace DesktopPet
{
    public partial class CharacterForm : Form
    {
        public CharacterForm()
        {
            InitializeComponent();
        }
        bool hasHandle = false;

        private void CharacterForm_Load(object sender, EventArgs e)
        {
            var bmp = new Bitmap("char.png");
            Size = bmp.Size;
            SetBits(bmp);
            Debug.WriteLine(Size);
        }
        protected override void OnClosing(CancelEventArgs e)
        {
            //e.Cancel = true;
            base.OnClosing(e);
            hasHandle = false;
        }

        protected override void OnHandleCreated(EventArgs e)
        {
            InitializeStyles();
            base.OnHandleCreated(e);
            hasHandle = true;
        }
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cParms = base.CreateParams;
                cParms.ExStyle |= 0x00080000; // WS_EX_LAYERED
                return cParms;
            }
        }

        private void InitializeStyles()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.UserPaint, true);
            UpdateStyles();
        }
        public void SetBits(Bitmap bitmap)
        {
            if (!hasHandle) return;
            if (!Image.IsCanonicalPixelFormat(bitmap.PixelFormat) ||
                !Image.IsAlphaPixelFormat(bitmap.PixelFormat))
                throw new ApplicationException("The picture must be " +
                          "32bit picture with alpha channel");
            IntPtr oldBits = IntPtr.Zero;
            IntPtr screenDC = Win32.GetDC(IntPtr.Zero);
            IntPtr hBitmap = IntPtr.Zero;
            IntPtr memDc = Win32.CreateCompatibleDC(screenDC);
            try
            {
                Win32.Point topLoc = new Win32.Point(Left, Top);
                Win32.Size bitMapSize = new Win32.Size(bitmap.Width, bitmap.Height);
                Win32.BLENDFUNCTION blendFunc = new Win32.BLENDFUNCTION();
                Win32.Point srcLoc = new Win32.Point(0, 0);
                hBitmap = bitmap.GetHbitmap(Color.FromArgb(0));
                oldBits = Win32.SelectObject(memDc, hBitmap);
                blendFunc.BlendOp = Win32.AC_SRC_OVER;
                blendFunc.SourceConstantAlpha = 255;
                blendFunc.AlphaFormat = Win32.AC_SRC_ALPHA;
                blendFunc.BlendFlags = 0;
                Win32.UpdateLayeredWindow(Handle, screenDC, ref topLoc, ref bitMapSize,
                                 memDc, ref srcLoc, 0, ref blendFunc, Win32.ULW_ALPHA);
            }
            finally
            {
                if (hBitmap != IntPtr.Zero)
                {
                    Win32.SelectObject(memDc, oldBits);
                    Win32.DeleteObject(hBitmap);
                }
                Win32.ReleaseDC(IntPtr.Zero, screenDC);
                Win32.DeleteDC(memDc);
            }
        }
        Point offset;
        bool dragging = false;
        private void CharacterForm_MouseDown(object sender, MouseEventArgs e)
        {
            dragging = true;
            offset = new Point(Left - Cursor.Position.X, Top - Cursor.Position.Y);
        }

        private void updateTimer_Tick(object sender, EventArgs e)
        {
            if (dragging)
            {
                Left = offset.X + Cursor.Position.X;
                Top = offset.Y + Cursor.Position.Y;
            }

        }

        private void CharacterForm_MouseUp(object sender, MouseEventArgs e)
        {
            dragging = false;
        }
    }
}
