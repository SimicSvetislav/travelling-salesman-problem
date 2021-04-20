namespace TSP
{
    partial class MyForm
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
            this.algName = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.distance = new System.Windows.Forms.Label();
            this.time = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // algName
            // 
            this.algName.AutoSize = true;
            this.algName.BackColor = System.Drawing.SystemColors.Control;
            this.algName.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.algName.ForeColor = System.Drawing.SystemColors.ControlText;
            this.algName.Location = new System.Drawing.Point(0, 0);
            this.algName.Name = "algName";
            this.algName.Size = new System.Drawing.Size(143, 24);
            this.algName.TabIndex = 0;
            this.algName.Text = "Algorithm name";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label2.Location = new System.Drawing.Point(0, 25);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(136, 24);
            this.label2.TabIndex = 1;
            this.label2.Text = "Total distance :";
            // 
            // distance
            // 
            this.distance.AutoSize = true;
            this.distance.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.distance.Location = new System.Drawing.Point(142, 24);
            this.distance.Name = "distance";
            this.distance.Size = new System.Drawing.Size(80, 24);
            this.distance.TabIndex = 2;
            this.distance.Text = "distance";
            // 
            // time
            // 
            this.time.AutoSize = true;
            this.time.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.time.Location = new System.Drawing.Point(258, 0);
            this.time.Name = "time";
            this.time.Size = new System.Drawing.Size(45, 24);
            this.time.TabIndex = 4;
            this.time.Text = "time";
            // 
            // MyForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(767, 523);
            this.Controls.Add(this.time);
            this.Controls.Add(this.distance);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.algName);
            this.Name = "MyForm";
            this.Text = "Form";
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.MyForm_Paint);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label algName;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label distance;
        private System.Windows.Forms.Label time;
    }
}