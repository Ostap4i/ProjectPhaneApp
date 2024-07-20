namespace ProjectPhoneApp
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.Button buttonDeleteLastChar;
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
            textBoxInput = new TextBox();
            listBoxSuggestions = new ListBox();
            buttonAddWord = new Button();
            buttonDeleteLastChar = new Button();
            SuspendLayout();
            // 
            // textBoxInput
            // 
            textBoxInput.Location = new Point(12, 12);
            textBoxInput.Name = "textBoxInput";
            textBoxInput.Size = new Size(197, 23);
            textBoxInput.TabIndex = 0;
            textBoxInput.TextChanged += textBoxInput_TextChanged;
            // 
            // listBoxSuggestions
            // 
            listBoxSuggestions.FormattingEnabled = true;
            listBoxSuggestions.ItemHeight = 15;
            listBoxSuggestions.Location = new Point(12, 32);
            listBoxSuggestions.Name = "listBoxSuggestions";
            listBoxSuggestions.Size = new Size(197, 94);
            listBoxSuggestions.TabIndex = 1;
            listBoxSuggestions.SelectedIndexChanged += listBoxSuggestions_SelectedIndexChanged;
            // 
            // buttonAddWord
            // 
            buttonAddWord.Location = new Point(149, 132);
            buttonAddWord.Name = "buttonAddWord";
            buttonAddWord.Size = new Size(43, 23);
            buttonAddWord.TabIndex = 2;
            buttonAddWord.Text = "Add Word";
            buttonAddWord.UseVisualStyleBackColor = true;
            buttonAddWord.Click += buttonAddWord_Click;
            // 
            // buttonDeleteLastChar
            // 
            buttonDeleteLastChar.Location = new Point(37, 132);
            buttonDeleteLastChar.Name = "buttonDeleteLastChar";
            buttonDeleteLastChar.Size = new Size(43, 23);
            buttonDeleteLastChar.TabIndex = 3;
            buttonDeleteLastChar.Text = "Delete Last";
            buttonDeleteLastChar.UseVisualStyleBackColor = true;
            buttonDeleteLastChar.Click += buttonDeleteLastChar_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.ControlDarkDark;
            ClientSize = new Size(223, 508);
            Controls.Add(buttonAddWord);
            Controls.Add(listBoxSuggestions);
            Controls.Add(textBoxInput);
            Controls.Add(buttonDeleteLastChar);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Name = "Form1";
            Load += Form1_Load;
            MouseDown += Form1_MouseDown;
            MouseMove += Form1_MouseMove;
            MouseUp += Form1_MouseUp;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.TextBox textBoxInput;
        private System.Windows.Forms.ListBox listBoxSuggestions;
        private System.Windows.Forms.Button buttonAddWord;
    }
}