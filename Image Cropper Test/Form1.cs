using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Image_Cropper_Test
{
    public partial class Form1 : Form
    {
        // IMAGE CROPPER Variables
        private bool _isSelecting;
        private Point _startPoint;
        private Rectangle _selection;
        private ImageCropper _imageCropper;
        private Bitmap _originalImage;
        private Bitmap _workingImage;
        public Form1()
        {
            InitializeComponent();
            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox1.MouseDown += pictureBox1_MouseDown;
            pictureBox1.MouseMove += pictureBox1_MouseMove;
            pictureBox1.MouseUp += pictureBox1_MouseUp;
            pictureBox1.Paint += pictureBox1_Paint;

            _imageCropper = new ImageCropper(pictureBox1);


        }

        private void checkBoxCrop_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void buttonCrop_Click(object sender, EventArgs e)
        {
            if (checkBoxCrop.Checked)
            {
                // checkbos estiver actirvo
                _imageCropper.Crop();
            }
            else { return; }


            // ✅ CLEAR SELECTION
            _selection = Rectangle.Empty;
            

            // force repaint (removes rectangle)
            pictureBox1.Invalidate();

        }

        private void openFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Dispose previous images
            pictureBox1.Image?.Dispose();
            _originalImage?.Dispose();
            _workingImage?.Dispose();

            using (OpenFileDialog open = new OpenFileDialog())
            {
                open.Filter = "Image Files|*.png;*.jpg;*.jpeg;*.bmp;*.gif";

                if (open.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        _originalImage = new Bitmap(open.FileName);
                        _workingImage = new Bitmap(open.FileName);

                        pictureBox1.Image = _workingImage;   // No need to create a copy
                        pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Failed to load image: " + ex.Message);
                    }
                }
            }
        }


        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {

            if (pictureBox1.Image == null) return;

            _isSelecting = true;
            _startPoint = e.Location;
            _selection = Rectangle.Empty;
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (!_isSelecting) return;

            int x = Math.Min(e.X, _startPoint.X);
            int y = Math.Min(e.Y, _startPoint.Y);
            int w = Math.Abs(e.X - _startPoint.X);
            int h = Math.Abs(e.Y - _startPoint.Y);

            _selection = new Rectangle(x, y, w, h);
            _imageCropper.Selection = _selection;
            pictureBox1.Invalidate();
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            if (!_isSelecting) return;
            _isSelecting = false;
                       
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            if (checkBoxCrop.Checked)
            {
                if (_selection.Width > 0 && _selection.Height > 0)
                {
                    var pen = new Pen(Color.Red, 2);
                    e.Graphics.DrawRectangle(pen, _selection);
                }
            }
            else { return; }

        }
    }
}
