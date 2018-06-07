using System.Collections.Generic;
using System.Drawing;
using System.IO;
using ZXing;
using ZXing.Common;
using ZXing.QrCode.Internal;

namespace SmallShop.Utilities.Qrcode
{
    public static class QrcodeHelper
    {
        /// <summary>
        /// 创建二维码
        /// </summary>
        /// <param name="msg">二维码中保存的信息</param>
        /// <returns></returns>
        public static Bitmap Create(string msg)
        {
            var writer = new MultiFormatWriter();
            var hint = new Dictionary<EncodeHintType, object>();
            //设置二维码为utf-8编码
            hint.Add(EncodeHintType.CHARACTER_SET, "utf-8");
            //设置纠错等级， 高
            hint.Add(EncodeHintType.ERROR_CORRECTION, ErrorCorrectionLevel.H);
            var bm = writer.encode(msg, BarcodeFormat.QR_CODE, 200, 200, hint);
            var barcodeWriter = new BarcodeWriter();

            return barcodeWriter.Write(bm);
        }

        /// <summary>
        /// 根据图片地址识别二维码
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public static string Read(string filename)
        {
            if (string.IsNullOrEmpty(filename) || !File.Exists(filename))
                return "";

            var reader = new BarcodeReader();
            reader.Options.CharacterSet = "UTF-8";
            var map = new Bitmap(filename);
            var result = reader.Decode(map);

            return result == null ? "" : result.Text;
        }
    }
}
