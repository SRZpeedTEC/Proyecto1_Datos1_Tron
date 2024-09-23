namespace Proyecto1_Datos1_Tron
{
    partial class Victoria
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
            this.button1 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.Ganadorlbl = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Image = global::Proyecto1_Datos1_Tron.Properties.Resources.MenuF;
            this.button1.Location = new System.Drawing.Point(147, 404);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(383, 73);
            this.button1.TabIndex = 1;
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Image = global::Proyecto1_Datos1_Tron.Properties.Resources.FinJuego;
            this.label1.Location = new System.Drawing.Point(79, 93);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(539, 98);
            this.label1.TabIndex = 0;
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // Ganadorlbl
            // 
            this.Ganadorlbl.AutoSize = true;
            this.Ganadorlbl.BackColor = System.Drawing.Color.Transparent;
            this.Ganadorlbl.Font = new System.Drawing.Font("Playbill", 36F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Ganadorlbl.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(33)))), ((int)(((byte)(73)))));
            this.Ganadorlbl.Location = new System.Drawing.Point(234, 231);
            this.Ganadorlbl.Name = "Ganadorlbl";
            this.Ganadorlbl.Size = new System.Drawing.Size(157, 49);
            this.Ganadorlbl.TabIndex = 2;
            this.Ganadorlbl.Text = "GANADOR :";
            this.Ganadorlbl.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.Ganadorlbl.Click += new System.EventHandler(this.label2_Click);
            // 
            // Victoria
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(704, 618);
            this.Controls.Add(this.Ganadorlbl);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label1);
            this.Name = "Victoria";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Victoria";
            this.Load += new System.EventHandler(this.Victoria_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label Ganadorlbl;
    }
}