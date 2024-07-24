namespace DesktopPet
{
    partial class CharacterForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            updateTimer = new System.Windows.Forms.Timer(components);
            SuspendLayout();
            // 
            // updateTimer
            // 
            updateTimer.Enabled = true;
            updateTimer.Interval = 1;
            updateTimer.Tick += updateTimer_Tick;
            // 
            // CharacterForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            DoubleBuffered = true;
            FormBorderStyle = FormBorderStyle.None;
            Name = "CharacterForm";
            Text = "Form1";
            TopMost = true;
            Load += CharacterForm_Load;
            MouseDown += CharacterForm_MouseDown;
            MouseUp += CharacterForm_MouseUp;
            ResumeLayout(false);
        }


        #endregion

        private System.Windows.Forms.Timer updateTimer;
    }
}
