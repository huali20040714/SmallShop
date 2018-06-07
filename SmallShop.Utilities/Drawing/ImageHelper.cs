using System;
using System.IO;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Web;

namespace SmallShop.Utilities
{
    public class ImageHelper
    {
        #region 洗码卡模版图片填充卡号

        public static string CreateUserXiMaCard(string sourcePicture, string xiMaCardNo, int xiMaCardId, string savePath)
        {
            // 判断参数是否有效
            if (string.IsNullOrEmpty(sourcePicture) || string.IsNullOrEmpty(xiMaCardNo) || xiMaCardId <= 0 || string.IsNullOrEmpty(savePath))
                return string.Empty;

            // 源图片，水印图片全路径
            string sourcePictureExt = Path.GetExtension(sourcePicture).ToLower();
            // 判断文件是否存在,以及类型是否正确
            if (!File.Exists(sourcePicture) || (sourcePictureExt != ".jpg" && sourcePictureExt != ".png"))
                return string.Empty;

            //创建目录
            if (!Directory.Exists(savePath))
                Directory.CreateDirectory(savePath);

            // 目标名片名称及全路径
            string targetImage = $"{xiMaCardId}_A.jpg";

            // 将需要加上水印的图片装载到Image对象中
            Image imgSourcePicture = Image.FromFile(sourcePicture);
            // 确定其长宽
            int phWidth = imgSourcePicture.Width;
            int phHeight = imgSourcePicture.Height;
            //// 封装 GDI+ 位图，此位图由图形图像及其属性的像素数据组成。            
            //Bitmap bmSourcePicture = new Bitmap(phWidth, phHeight, PixelFormat.Format24bppRgb);
            //// 设定分辨率
            //bmSourcePicture.SetResolution(imgSourcePicture.HorizontalResolution, imgSourcePicture.VerticalResolution);
            // 定义一个绘图画面用来装载，底图为洗码卡模版图片
            Graphics grSourcePicture = Graphics.FromImage(imgSourcePicture);

            //SmoothingMode：指定是否将平滑处理（消除锯齿）应用于直线、曲线和已填充区域的边缘。
            // 成员名称   说明 
            // AntiAlias      指定消除锯齿的呈现
            // Default        指定不消除锯齿
            // HighQuality    指定高质量、低速度呈现
            // HighSpeed      指定高速度、低质量呈现
            // Invalid        指定一个无效模式
            // None           指定不消除锯齿
            grSourcePicture.SmoothingMode = SmoothingMode.AntiAlias;
            //// 第一次描绘，将我们的底图描绘在绘图画面上
            //grSourcePicture.DrawImage(imgSourcePicture, new Rectangle(0, 0, phWidth, phHeight), 0, 0, phWidth, phHeight, GraphicsUnit.Pixel);

            // 绘制文字(洗码卡卡号)
            var fontSize = 120;
            var x = 5;
            var y = 240;
            if (xiMaCardNo.Length == 4)
            {
                fontSize = 245;
                y = 110;
            }
            else if (xiMaCardNo.Length == 5)
            {
                fontSize = 205;
                y = 145;
            }
            else if (xiMaCardNo.Length == 6)
            {
                fontSize = 175;
                y = 160;
            }
            else if (xiMaCardNo.Length == 7)
            {
                fontSize = 155;
                y = 180;
            }
            else if (xiMaCardNo.Length == 8)
            {
                fontSize = 130;
                y = 220;
            }
            var font = new Font("微软雅黑", fontSize, FontStyle.Bold);
            var brush = new SolidBrush(Color.FromArgb(255, 255, 255, 255));
            grSourcePicture.DrawString(xiMaCardNo, font, brush, new PointF(x, y));
            brush.Dispose();

            //绘制编号(洗码卡的自增Id,从1001开始自增)
            x = 795;
            y = 150;
            font = new Font("宋体", 18, FontStyle.Regular);
            brush = new SolidBrush(Color.FromArgb(255, 255, 255, 204));
            grSourcePicture.DrawString($"编号:{xiMaCardId}", font, brush, new PointF(x, y));
            brush.Dispose();

            imgSourcePicture.Save(Path.Combine(savePath, targetImage), ImageFormat.Jpeg);
            grSourcePicture.Dispose();
            imgSourcePicture.Dispose();

            return targetImage;
        }

        public static string CreateStaffXiMaCard(string sourcePicture, int xiMaCardId, string staffNo, string realName, string savePath)
        {
            // 判断参数是否有效
            if (string.IsNullOrEmpty(sourcePicture) || string.IsNullOrEmpty(staffNo) || xiMaCardId <= 0 || string.IsNullOrEmpty(savePath))
                return string.Empty;

            // 源图片，水印图片全路径
            string sourcePictureExt = Path.GetExtension(sourcePicture).ToLower();
            // 判断文件是否存在,以及类型是否正确
            if (!File.Exists(sourcePicture) || (sourcePictureExt != ".jpg" && sourcePictureExt != ".png"))
                return string.Empty;

            //创建目录
            if (!Directory.Exists(savePath))
                Directory.CreateDirectory(savePath);

            // 目标名片名称及全路径
            string targetImage = $"{xiMaCardId}_A.jpg";

            // 将需要加上水印的图片装载到Image对象中
            Image imgSourcePicture = Image.FromFile(sourcePicture);
            // 确定其长宽
            int phWidth = imgSourcePicture.Width;
            int phHeight = imgSourcePicture.Height;
            //// 封装 GDI+ 位图，此位图由图形图像及其属性的像素数据组成。            
            //Bitmap bmSourcePicture = new Bitmap(phWidth, phHeight, PixelFormat.Format24bppRgb);
            //// 设定分辨率
            //bmSourcePicture.SetResolution(imgSourcePicture.HorizontalResolution, imgSourcePicture.VerticalResolution);
            // 定义一个绘图画面用来装载，底图为洗码卡模版图片
            Graphics grSourcePicture = Graphics.FromImage(imgSourcePicture);

            //SmoothingMode：指定是否将平滑处理（消除锯齿）应用于直线、曲线和已填充区域的边缘。
            // 成员名称   说明 
            // AntiAlias      指定消除锯齿的呈现
            // Default        指定不消除锯齿
            // HighQuality    指定高质量、低速度呈现
            // HighSpeed      指定高速度、低质量呈现
            // Invalid        指定一个无效模式
            // None           指定不消除锯齿
            grSourcePicture.SmoothingMode = SmoothingMode.AntiAlias;
            //// 第一次描绘，将我们的底图描绘在绘图画面上
            //grSourcePicture.DrawImage(imgSourcePicture, new Rectangle(0, 0, phWidth, phHeight), 0, 0, phWidth, phHeight, GraphicsUnit.Pixel);

            // 绘制文字(洗码卡卡号)
            var fontSize = 38;
            var x = 230;
            var y = 818;
            var font = new Font("宋体", fontSize, FontStyle.Regular);
            var brush = Brushes.Black;
            grSourcePicture.DrawString(staffNo, font, brush, new PointF(x, y));
            if (!string.IsNullOrEmpty(realName))
            {
                x = 230;
                y = 720;
                font = new Font("宋体", fontSize, FontStyle.Regular);
                grSourcePicture.DrawString(realName, font, brush, new PointF(x, y));
            }
            brush.Dispose();

            var myImageCodecInfo = GetEncoderInfo("image/jpeg");
            var myEncoderParameter = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, 100L);
            var myEncoderParameters = new EncoderParameters(1);
            myEncoderParameters.Param[0] = myEncoderParameter;
            imgSourcePicture.RotateFlip(RotateFlipType.Rotate90FlipNone);
            imgSourcePicture.Save(Path.Combine(savePath, targetImage), myImageCodecInfo, myEncoderParameters);

            grSourcePicture.Dispose();
            imgSourcePicture.Dispose();

            return targetImage;
        }

        private static ImageCodecInfo GetEncoderInfo(String mimeType)
        {
            int j;
            ImageCodecInfo[] encoders;
            encoders = ImageCodecInfo.GetImageEncoders();
            for (j = 0; j < encoders.Length; ++j)
            {
                if (encoders[j].MimeType == mimeType)
                    return encoders[j];
            }
            return null;
        }

        public static bool XiMaCardExists(int xiMaCardId)
        {
            string path = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
            string imgFile = Path.Combine(path, $@"Upload\XiMaCard\{xiMaCardId}_A.jpg");

            return File.Exists(imgFile);
        }

        public static string GetXiMaCardFrontUrl(int xiMaCardId)
        {
            var scheme = HttpContext.Current.Request.Url.Scheme;
            var host = HttpContext.Current.Request.Url.Host;
            var port = HttpContext.Current.Request.Url.Port;
            var path = $"Upload/XiMaCard/{xiMaCardId}_A.jpg";
            var url = $"{scheme}://{host}/{path}";
            if (port != 80)
                url = $"{scheme}://{host}:{port}/{path}";

            return url;
        }

        //type  [1为会员 ,2为员工]
        public static string GetXiMaCardBehindUrl(int type)
        {
            var scheme = HttpContext.Current.Request.Url.Scheme;
            var host = HttpContext.Current.Request.Url.Host;
            var port = HttpContext.Current.Request.Url.Port;
            var path = $"Assets/HttpServices/IcTemplate_{type}_B.jpg";
            var url = $"{scheme}://{host}/{path}";
            if (port != 80)
                url = $"{scheme}://{host}:{port}/{path}";

            return url;
        }

        #endregion
    }
}