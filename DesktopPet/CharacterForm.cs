using DesktopPet.Characters;
using DesktopPet.Structs;
using System.ComponentModel;
using System.Diagnostics;

namespace DesktopPet
{
    public partial class CharacterForm : Form
    {
        bool hasHandle = false;
        GameTime gameTime = new();
        ICharacter character;
        IDrawState drawState;
        public CharacterForm()
        {
            InitializeComponent();
            var bmp = new Bitmap("char.png");
            Size = bmp.Size;
            var bounds = new Bounds2();
            bounds.size = new(SystemInformation.VirtualScreen.Width, SystemInformation.VirtualScreen.Height);
            character = new Character(new(bmp.Width, bmp.Height), bounds);
            var db = new DrawBits(SetBits);
            db.AddState(CharacterState.Idle, bmp);
            db.AddState(CharacterState.WalkingRight, bmp);
            var bmp1 = new Bitmap(bmp);
            bmp1.RotateFlip(RotateFlipType.RotateNoneFlipX);
            db.AddState(CharacterState.WalkingLeft, bmp1);
            var bmp2 = new Bitmap(bmp);
            bmp2.RotateFlip(RotateFlipType.RotateNoneFlipY);
            db.AddState(CharacterState.Happy, bmp2);
            drawState = db;
        }

        private void CharacterForm_Load(object sender, EventArgs e)
        {
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
                cParms.ExStyle |= 0x00000008; // WS_EX_TOPMOST
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
        private void CharacterForm_MouseUp(object sender, MouseEventArgs e)
        {
            character.OnMouseUp();
        }

        private void CharacterForm_MouseDown(object sender, MouseEventArgs e)
        {
            character.OnMouseDown(new(Cursor.Position.X, Cursor.Position.Y));
        }
        private void updateTimer_Tick(object sender, EventArgs e)
        {
            GameFrame frame;
            frame.cursor = new(Cursor.Position.X, Cursor.Position.Y);
            frame.delta = gameTime.Delta;
            character.Update(frame);
            drawState.DrawState(character.State);
            Left = (int)character.Position.x;
            Top = (int)character.Position.y;

            gameTime.Tick();
        }

        private void CharacterForm_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Space)
            {
                character.MakeHappy();
            }
        }
    }
}
