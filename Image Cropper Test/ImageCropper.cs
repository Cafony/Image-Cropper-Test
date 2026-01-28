using System.Drawing;
using System.Windows.Forms;

public class ImageCropper
{
    private PictureBox _pb;

    public Rectangle Selection { get; set; }

    public ImageCropper(PictureBox pb)
    {
        _pb = pb;
    }

    public void Crop()
    {
        if (_pb.Image == null) return;
        if (Selection.Width <= 0 || Selection.Height <= 0) return;

        Bitmap src = (Bitmap)_pb.Image;

        Rectangle imgRect = GetImageDisplayRect(_pb);

        float scaleX = (float)src.Width / imgRect.Width;
        float scaleY = (float)src.Height / imgRect.Height;

        Rectangle cropRect = new Rectangle(
            (int)((Selection.X - imgRect.X) * scaleX),
            (int)((Selection.Y - imgRect.Y) * scaleY),
            (int)(Selection.Width * scaleX),
            (int)(Selection.Height * scaleY)
        );

        cropRect.Intersect(new Rectangle(0, 0, src.Width, src.Height));

        Bitmap cropped = src.Clone(cropRect, src.PixelFormat);
        _pb.Image = cropped;
    }

    private Rectangle GetImageDisplayRect(PictureBox pb)
    {
        int imgW = pb.Image.Width;
        int imgH = pb.Image.Height;
        int boxW = pb.ClientSize.Width;
        int boxH = pb.ClientSize.Height;

        float imgRatio = (float)imgW / imgH;
        float boxRatio = (float)boxW / boxH;

        int drawW, drawH, offX = 0, offY = 0;

        if (imgRatio > boxRatio)
        {
            drawW = boxW;
            drawH = (int)(boxW / imgRatio);
            offY = (boxH - drawH) / 2;
        }
        else
        {
            drawH = boxH;
            drawW = (int)(boxH * imgRatio);
            offX = (boxW - drawW) / 2;
        }

        return new Rectangle(offX, offY, drawW, drawH);
    }
}
