using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
namespace Ant.Component
{
    public class Progress
    {
        public Progress(int width, int height)
        {
            mImage = new Bitmap(width, height);
           
            mWidth = width;
            mHeight = height;
            ForeColor = Color.Black;
       
            LeftLinearGradientColor = Color.FromArgb(208, 231, 253);
            RightLinearGradientColor = Color.FromArgb(10, 94, 177);
            TextFormat = StringFormat.GenericDefault;
            TextFormat.LineAlignment = StringAlignment.Center;
            TextFormat.Alignment = StringAlignment.Center;
            TextFormat.FormatFlags = StringFormatFlags.LineLimit | StringFormatFlags.NoWrap;
            TextFormat.Trimming = StringTrimming.EllipsisCharacter;
            mBounds = new Rectangle(0, 0, Width, Height);
            BorderWidth = 2;
           
            BackColor = Color.White;
           
        }

        public int BorderWidth
        {
            get;
            set;
        }
      
        private Rectangle mBounds;
        public Rectangle Bounds
        {
            get
            {
                return mBounds;
            }
        }
        public static StringFormat TextFormat
        {
            get;
            set;
        }
        public Color BackColor
        {
            get;
            set;
        }
        public Color LeftLinearGradientColor
        {
            get;
            set;
        }
        public Color RightLinearGradientColor
        {
            get;
            set;
        }
        public Color ForeColor
        {
            get;
            set;
        }
        private Bitmap mImage;
    
        private int mWidth;
        private int mHeight;
        public int Width
        {
            get
            {
                return mWidth;
            }
        }
        public Image Image
        {
            get
            {
                return mImage;
            }
        }
        public void Draw(string label, double value, double maxvalue)
        {
            lock (this)
            {

                using (Graphics e = Graphics.FromImage(mImage))
                {
                    using (SolidBrush soli = new SolidBrush(BackColor))
                    {
                        e.FillRectangle(soli, Bounds);
                    }
                    int width;
                    if (value == maxvalue)
                    {
                        width = Width - 2 - (BorderWidth * 2);
                    }
                    else
                    {
                        double p = value / maxvalue;
                        width = Convert.ToInt32((Width - 2 - (BorderWidth * 2)) * p);
                    }

                    using (LinearGradientBrush full = new LinearGradientBrush(Bounds, LeftLinearGradientColor, RightLinearGradientColor, LinearGradientMode.Horizontal))
                    {
                        e.FillRectangle(full, BorderWidth + 1, BorderWidth + 1, width, Bounds.Height - 2 - (BorderWidth * 2));

                    }
                    e.DrawRectangle(new Pen(RightLinearGradientColor, BorderWidth), mBounds);
                    e.DrawString(label, new Font("Ariel", 9), new SolidBrush(ForeColor), Bounds, TextFormat);
                }
            }
        }
        public int Height
        {
            get
            {
                return mHeight;
            }
        }
    }
}
