using DesktopPet.Characters;
using DesktopPet.KeyboardHook;
using DesktopPet.Structs;
using System.ComponentModel;
using System.Diagnostics;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace DesktopPet
{
    public partial class CharacterForm : Form
    {
        bool hasHandle = false;
        GameTime gameTime = new();
        ICharacter character;
        IDrawState drawState;
        GlobalKeyboardHook hook = new();
        CharacterConfig config;
        public CharacterForm(string configPath)
        {
            InitializeComponent();

            var db = new DrawBits(SetBits);

            if (File.Exists(configPath))
            {
                var json = File.ReadAllText(configPath);
                JsonSerializerOptions options = new()
                {
                    Converters =
                    {
                        new JsonStringEnumConverter()
                    }
                };
                config = JsonSerializer.Deserialize<CharacterConfig>(json, options) ?? new CharacterConfig();
            }
            else
                config = new CharacterConfig();
            config.SimplifyWords();

            foreach (var item in config.States)
            {
                var bmp = new Bitmap(item.Value);
                Size = bmp.Size;
                db.AddState(item.Key, bmp);
            }


            drawState = db;



            var bounds = new Bounds2();
            bounds.size = new(SystemInformation.VirtualScreen.Width, SystemInformation.VirtualScreen.Height - config.GroundHeight);
            character = new Character(new(0, 0, Size.Width, Size.Height), bounds);
        }

        private void CharacterForm_Load(object sender, EventArgs e)
        {
            Debug.WriteLine(Size);
            hook.OnKeyPressed += Hook_OnKeyPressed;
            hook.HookKeyboard();
        }
        StringBuilder keyboardInput = new StringBuilder();
        int maxLength = 50;
        private void AddKeyboardKey(Keys key)
        {
            if(key == Keys.Back && config.BackSpaceDeletes)
            {
                if(keyboardInput.Length > 0)
                    keyboardInput.Length--;
            }
            else if(key.IsAllowed())
            {
                keyboardInput.Append(key.ToString().ToLower());
                string text = keyboardInput.ToString();
                foreach (var item in config.FavoriteWords)
                {
                    if (text.EndsWith(item))
                    {
                        character.MakeHappy();
                        break;
                    }
                }
            }

            if(keyboardInput.Length > maxLength)
            {
                keyboardInput.Remove(0,keyboardInput.Length - maxLength);
            }
        }
        private void Hook_OnKeyPressed(object? sender, Keys e)
        {
            AddKeyboardKey(e);

            Debug.WriteLine(keyboardInput.ToString());
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            //e.Cancel = true;
            base.OnClosing(e);
            hasHandle = false;
            hook.UnHookKeyboard();
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
            Left = (int)character.CharacterBounds.position.x;
            Top = (int)character.CharacterBounds.position.y;

            gameTime.Tick();
        }

    }
}
