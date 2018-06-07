using System;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Threading;

namespace SmallShop.Utilities
{
    /// <summary> 
    /// 用于产生随机的验证码 
    /// </summary> 
    public class ValidationCode
    {
        private Graphics g = null;
        private int bgWidth = 0;
        private int bgHeight = 0;

        public ValidationCode()
        {
            FontFace = "Comic Sans MS";
            FontSize = 15;
            ForeColor = Color.FromArgb(220, 220, 220);
            BackGroundColor = Color.FromArgb(190, 190, 190);
            MixedLineColor = Color.FromArgb(220, 220, 220);
            MixedLineWidth = 1;
            MixedLineCount = 5;
        }

        /// <summary>
        /// 用户存取验证码字符串 
        /// </summary>
        public string Code { get; set; }

        public string FontFace { get; set; }

        public int FontSize { get; set; }

        public Color ForeColor { get; set; }

        public Color BackGroundColor { get; set; }

        public Color MixedLineColor { get; set; }

        public int MixedLineWidth { get; set; }

        public int MixedLineCount { get; set; }

        /// <summary> 
        /// 根据指定长度，返回随机验证码 
        /// </summary> 
        /// <param name="length">制定长度</param> 
        /// <returns>随即验证码</returns> 
        public string Next(int length)
        {
            this.Code = GetRandomCode(length);

            return this.Code;
        }

        /// <summary> 
        /// 根据指定长度及背景图片样式，返回带有随机验证码的图片对象 
        /// </summary> 
        /// <param name="length">指定长度</param> 
        /// <param name="hatchStyle">背景图片样式</param> 
        /// <returns>Image对象</returns> 
        public Image NextImage(int length, HatchStyle hatchStyle, bool allowMixedLines)
        {
            if (string.IsNullOrEmpty(this.Code))
                this.Code = GetRandomCode(length);

            return DrawFixedImage(hatchStyle, allowMixedLines);
        }

        public Image DrawFixedImage(HatchStyle hatchStyle, bool allowMixedLines)
        {
            if (string.IsNullOrEmpty(this.Code))
                return null;

            //校验码字体 
            Font myFont = new Font(FontFace, FontSize);

            //根据校验码字体大小算出背景大小 
            bgWidth = (int)myFont.Size * this.Code.Length + 4;
            bgHeight = (int)myFont.Size * 2;
            //生成背景图片 
            Bitmap myBitmap = new Bitmap(bgWidth, bgHeight);
            g = Graphics.FromImage(myBitmap);
            this.DrawValidationCode(this.Code, myFont);
            if (allowMixedLines)
                this.DrawMixedLine();

            g.Dispose();

            return (Image)myBitmap;
        }

        #region 绘制验证码背景

        private void DrawBackground(HatchStyle hatchStyle)
        {
            //设置填充背景时用的笔刷 
            HatchBrush hBrush = new HatchBrush(hatchStyle, BackGroundColor);

            //填充背景图片 
            g.FillRectangle(hBrush, 0, 0, this.bgWidth, this.bgHeight);
        }

        #endregion

        #region 绘制验证码

        private void DrawValidationCode(string vCode, Font font)
        {
            g.DrawString(vCode, font, new SolidBrush(this.ForeColor), 2, 2);
        }

        #endregion

        #region 绘制干扰线条

        /// <summary> 
        /// 绘制干扰线条 
        /// </summary> 
        private void DrawMixedLine()
        {
            for (int i = 0; i < MixedLineCount; i++)
            {
                g.DrawBezier(new Pen(new SolidBrush(MixedLineColor), MixedLineWidth), RandomPoint(), RandomPoint(), RandomPoint(), RandomPoint());
            }
        }

        #endregion

        #region 返回指定长度的随机验证码字符串

        /// <summary> 
        /// 根据指定大小返回随机验证码 
        /// </summary> 
        /// <param name="length">字符串长度</param> 
        /// <returns>随机字符串</returns> 
        private string GetRandomCode(int length)
        {
            StringBuilder sb = new StringBuilder(6);

            for (int i = 0; i < length; i++)
            {
                sb.Append(Char.ConvertFromUtf32(RandomAZ09()));
            }

            return sb.ToString();
        }

        #endregion

        #region 产生随机数和随机点

        /// <summary> 
        /// 产生0-9A-Z的随机字符代码 
        /// </summary> 
        /// <returns>字符代码</returns> 
        private int RandomAZ09()
        {
            Thread.Sleep(15);
            int result = 48;
            Random ram = new Random();
            int i = ram.Next(1); //ram.Next(2);

            switch (i)
            {
                case 0:
                    result = ram.Next(48, 58);
                    break;
                case 1:
                    result = ram.Next(65, 91);
                    break;
            }

            return result;
        }

        /// <summary> 
        /// 返回一个随机点，该随机点范围在验证码背景大小范围内 
        /// </summary> 
        /// <returns>Point对象</returns> 
        private Point RandomPoint()
        {
            Thread.Sleep(15);
            Random ram = new Random();
            Point point = new Point(ram.Next(this.bgWidth), ram.Next(this.bgHeight));
            return point;
        }

        #endregion
    }
}
