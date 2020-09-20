namespace SzachyMulti
{
    partial class Form1 : System.Windows.Forms.Form
    {
        /// <summary>
        /// Wymagana zmienna projektanta.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Wyczyść wszystkie używane zasoby.
        /// </summary>
        /// <param name="disposing">prawda, jeżeli zarządzane zasoby powinny zostać zlikwidowane; Fałsz w przeciwnym wypadku.</param>
        protected override void Dispose(bool disposing)
        {
            if(disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Kod generowany przez Projektanta formularzy systemu Windows

        /// <summary>
        /// Metoda wymagana do obsługi projektanta — nie należy modyfikować
        /// jej zawartości w edytorze kodu.
        /// </summary>
        private void InitializeComponent()
        {
            this.button1 = new System.Windows.Forms.Button();
            this.Names_Label = new System.Windows.Forms.Label();
            this.Chat_TextBox = new System.Windows.Forms.RichTextBox();
            this.Input_TextBox = new System.Windows.Forms.RichTextBox();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Enabled = false;
            this.button1.Location = new System.Drawing.Point(310, 400);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(47, 38);
            this.button1.TabIndex = 2;
            this.button1.Text = "Send";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // Names_Label
            // 
            this.Names_Label.AutoSize = true;
            this.Names_Label.Location = new System.Drawing.Point(13, 13);
            this.Names_Label.Name = "Names_Label";
            this.Names_Label.Size = new System.Drawing.Size(124, 26);
            this.Names_Label.TabIndex = 3;
            this.Names_Label.Text = "You: Nick_1 (Color_1)\r\nEnemy: Nick_2 (Color_2)";
            // 
            // Chat_TextBox
            // 
            this.Chat_TextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.Chat_TextBox.Location = new System.Drawing.Point(13, 45);
            this.Chat_TextBox.Name = "Chat_TextBox";
            this.Chat_TextBox.ReadOnly = true;
            this.Chat_TextBox.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
            this.Chat_TextBox.Size = new System.Drawing.Size(344, 338);
            this.Chat_TextBox.TabIndex = 0;
            this.Chat_TextBox.Text = "";
            // 
            // Input_TextBox
            // 
            this.Input_TextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.Input_TextBox.Location = new System.Drawing.Point(13, 400);
            this.Input_TextBox.Name = "Input_TextBox";
            this.Input_TextBox.Size = new System.Drawing.Size(290, 38);
            this.Input_TextBox.TabIndex = 1;
            this.Input_TextBox.Text = "";
            this.Input_TextBox.TextChanged += new System.EventHandler(this.Input_TextBox_TextChanged);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(369, 450);
            this.Controls.Add(this.Names_Label);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.Input_TextBox);
            this.Controls.Add(this.Chat_TextBox);
            this.Name = "Form1";
            this.Text = "Czat";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label Names_Label;
        private System.Windows.Forms.RichTextBox Chat_TextBox;
        private System.Windows.Forms.RichTextBox Input_TextBox;
    }
}

