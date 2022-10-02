namespace waifu2x_ncnn_vulkan_gui
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            this.AllowDrop = true;
        }

        void Form1_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop)) e.Effect = DragDropEffects.Copy;
        }

        void Form1_DragDrop(object sender, DragEventArgs e)
        {
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
            LoadImage(files[0]);
        }

        private void LoadImage(string filepath)
        {
            try
            {
                pictureBox1.Image = Image.FromStream(new MemoryStream(File.ReadAllBytes(filepath)));
                txtInputPath.Text = filepath;
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            cbScale.SelectedIndex = 1;
            cbDenoise.SelectedIndex = 3;

        }

        private void buttonExec_Click(object sender, EventArgs e)
        {
            string format = "png";
            string inputpath = txtInputPath.Text;
            if (string.IsNullOrWhiteSpace(inputpath))
            {
                MessageBox.Show("请选择输入文件");
                return;
            }
            string filename = Path.GetFileNameWithoutExtension(inputpath);
            string filepath = Path.GetDirectoryName(inputpath);

            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "PNG Portable Network Graphics (*.png)|*.png|JPEG File Interchange Format(.jpg)|*.jpg|Webp Image|*.webp";
            sfd.Title = "导出图片";
            sfd.InitialDirectory = filepath;
            sfd.FileName = $"{filename}_{cbScale.Text}x{cbDenoise.Text}n";

            sfd.ShowDialog();

            if (sfd.FileName != "")
            {
                switch (sfd.FilterIndex)
                {
                    case 1:
                        format = "png";
                        break;
                    case 2:
                        format = "jpg";
                        break;
                    case 3:
                        format = "webp";
                        break;
                }
                if (File.Exists("waifu2x-ncnn-vulkan.exe"))
                {
                    System.Diagnostics.Process.Start("./waifu2x-ncnn-vulkan.exe", $" -v -i {inputpath} -o {sfd.FileName} -n {cbDenoise.Text} -s {cbScale.Text} -f {format}");
                }
                else{
                    MessageBox.Show("找不到waifu2x-ncnn-vulkan.exe");
                }
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.OpenFileDialog openFileDialog = new System.Windows.Forms.OpenFileDialog();
            // openFileDialog.InitialDirectory = "c:\\";
            openFileDialog.Filter = "All Images Files|*.png;*.jpeg;*.gif;*.jpg;*.bmp;*.tiff;*.tif;*.webp";
            openFileDialog.RestoreDirectory = true;
            openFileDialog.FilterIndex = 1;

            DialogResult result = openFileDialog.ShowDialog();
            if (result == System.Windows.Forms.DialogResult.OK)
            {
                LoadImage(openFileDialog.FileName);
            }
        }
    }
}