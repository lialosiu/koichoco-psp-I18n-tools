using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace 恋选PSP文本处理器
{
    public class Toolkit
    {
        // 半角转全角
        public static String ToSBC(String input)
        {
            char[] c = input.ToCharArray();
            for (int i = 0; i < c.Length; i++)
            {
                if (c[i] == 32)
                {
                    c[i] = (char)12288;
                    continue;
                }
                if (c[i] < 127)
                    c[i] = (char)(c[i] + 65248);
            }
            return new String(c);
        }

        /// <summary>
        /// 字符转换为恋选PSP专用字库的格式
        /// </summary>
        /// <param name="character">要转换的字符</param>
        /// <param name="font">要调用的字体</param>
        /// <returns>封在List<Btye>中的专用字库的格式</returns>
        public static List<Byte> Character2Bytes(Char character, Font font)
        {
            List<Byte> thisBytes = new List<Byte>();
            Bitmap bmp = new Bitmap(16, 16);
            Graphics graphics = Graphics.FromImage(bmp);
            graphics.DrawString(character.ToString(), font, new SolidBrush(Color.White), 0, -1);

            for (int nowHeight = 0; nowHeight < bmp.Height; nowHeight++)
            {
                for (int nowWidth = 0; nowWidth < bmp.Width; nowWidth += 2)
                {
                    Int32 huidu1 = Convert.ToInt32((0.3 * bmp.GetPixel(nowWidth, nowHeight).R + 0.6 * bmp.GetPixel(nowWidth, nowHeight).G + 0.1 * bmp.GetPixel(nowWidth, nowHeight).B) / 36);
                    Int32 huidu2 = Convert.ToInt32((0.3 * bmp.GetPixel(nowWidth + 1, nowHeight).R + 0.6 * bmp.GetPixel(nowWidth + 1, nowHeight).G + 0.1 * bmp.GetPixel(nowWidth + 1, nowHeight).B) / 36);
                    Int32 huidu = huidu2 * 0x10 + huidu1;
                    thisBytes.Add((Byte)huidu);
                }
            }
            return thisBytes;
        }

        /// <summary>
        /// 更新校验码
        /// </summary>
        /// <param name="thisFileStream">文件流</param>
        public static void updateTheVerifyCode(FileStream thisFileStream)
        {
            Int32[] count = new Int32[16] { 0x11, 0x11, 0x11, 0x11, 0x11, 0x11, 0x11, 0x11, 0x11, 0x11, 0x11, 0x11, 0x11, 0x11, 0x11, 0x11 };
            Byte[] thisLineByte = new Byte[16];
            thisFileStream.Seek(0, SeekOrigin.Begin);

            while (true)
            {
                if (thisFileStream.Position >= thisFileStream.Length - 0x10) break;
                thisFileStream.Read(thisLineByte, 0, 16);
                for (Int32 i = 0; i <= 15; i++)
                {
                    count[i] += thisLineByte[i];
                    if (count[i] >= 0x100)
                    {
                        count[i] %= 0x100;
                        if (!(i == 7 || i == 15)) count[i + 1]++;
                    }
                }
            }
            for (Int32 i = 0; i <= 15; i++)
            {
                thisLineByte[i] = (Byte)count[i];
            }
            thisFileStream.Write(thisLineByte, 0, 16);
        }

        public static Boolean serializeGameTextToFile(String FileName, GameText gameText)
        {
            Stream stream = new FileStream(FileName, FileMode.Create, FileAccess.Write, FileShare.None);
            new BinaryFormatter().Serialize(stream, gameText);
            stream.Close();
            return true;
        }

        public static Boolean deserializeTextListFromFile(String FileName, ref GameText gameText)
        {
            try
            {
                Stream stream = new FileStream(FileName, FileMode.Open, FileAccess.Read, FileShare.Read);
                gameText = (GameText)new BinaryFormatter().Deserialize(stream);
                stream.Close();
            }
            catch (FileNotFoundException)
            {
                return false;
            }
            return true;
        }

    }
}
