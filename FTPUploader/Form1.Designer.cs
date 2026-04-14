namespace FTPUploader
{
    partial class Form1
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
            btnSelect = new Button();
            btnUpload = new Button();
            txtFilePath = new TextBox();
            progressBar1 = new ProgressBar();
            lblStatus = new Label();
            SuspendLayout();
            // 
            // btnSelect
            // 
            btnSelect.Location = new Point(218, 39);
            btnSelect.Name = "btnSelect";
            btnSelect.Size = new Size(94, 29);
            btnSelect.TabIndex = 0;
            btnSelect.Text = "Select File";
            btnSelect.UseVisualStyleBackColor = true;
            btnSelect.Click += btnSelect_Click;
            // 
            // btnUpload
            // 
            btnUpload.Location = new Point(375, 39);
            btnUpload.Name = "btnUpload";
            btnUpload.Size = new Size(94, 29);
            btnUpload.TabIndex = 1;
            btnUpload.Text = "Upload";
            btnUpload.UseVisualStyleBackColor = true;
            btnUpload.Click += btnUpload_Click;
            // 
            // txtFilePath
            // 
            txtFilePath.Location = new Point(218, 93);
            txtFilePath.Name = "txtFilePath";
            txtFilePath.Size = new Size(258, 27);
            txtFilePath.TabIndex = 2;
            // 
            // progressBar1
            // 
            progressBar1.Location = new Point(218, 156);
            progressBar1.Name = "progressBar1";
            progressBar1.Size = new Size(258, 29);
            progressBar1.TabIndex = 3;
            // 
            // lblStatus
            // 
            lblStatus.AutoSize = true;
            lblStatus.Location = new Point(218, 230);
            lblStatus.Name = "lblStatus";
            lblStatus.Size = new Size(116, 20);
            lblStatus.TabIndex = 4;
            lblStatus.Text = "Status: Waiting...";
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(lblStatus);
            Controls.Add(progressBar1);
            Controls.Add(txtFilePath);
            Controls.Add(btnUpload);
            Controls.Add(btnSelect);
            Name = "Form1";
            Text = "Form1";
            Load += Form1_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button btnSelect;
        private Button btnUpload;
        private TextBox txtFilePath;
        private ProgressBar progressBar1;
        private Label lblStatus;
    }
}
