using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace FTPUploader
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        // 1. Select Button: User se folder mangne ke liye
        private void btnSelect_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog fbd = new FolderBrowserDialog())
            {
                if (fbd.ShowDialog() == DialogResult.OK)
                {
                    txtFilePath.Text = fbd.SelectedPath; // Path textbox mein dikhayein
                }
            }
        }

        // 2. Upload Button: Background processing start karne ke liye
        private async void btnUpload_Click(object sender, EventArgs e)
        {
            string selectedPath = txtFilePath.Text;

            if (string.IsNullOrWhiteSpace(selectedPath))
            {
                MessageBox.Show("Please select a folder first.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                btnUpload.Enabled = false;
                progressBar1.Value = 0;
                lblStatus.Text = "Checking folder...";

                var progress = new Progress<int>(percent =>
                {
                    progressBar1.Value = percent;
                    lblStatus.Text = $"Uploading: {percent}%";
                });

                IConfiguration config = new ConfigurationBuilder()
                    .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                    .AddJsonFile("appsettings.json", optional: false)
                    .Build();

                var service = new FileService(config);

                // Result check karein
                bool hasFiles = await Task.Run(() => service.ProcessFiles(selectedPath, progress));

                if (hasFiles)
                {
                    lblStatus.Text = "Upload Successful!";
                    progressBar1.Value = 100;
                    MessageBox.Show("All files processed successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    lblStatus.Text = "No files found.";
                    progressBar1.Value = 0; // Bar zero par hi rahega
                    MessageBox.Show("No files found to process in the selected folder.", "Empty Folder", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                lblStatus.Text = "Error occurred.";
                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                btnUpload.Enabled = true;
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}