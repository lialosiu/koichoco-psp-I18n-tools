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

        /// <summary>
        /// 序列化GameText，并储存为文件
        /// </summary>
        /// <param name="fileName">文件名</param>
        /// <param name="gameText">GameText</param>
        /// <returns>是否成功</returns>
        public static Boolean serializeGameTextToFile(String fileName, GameText gameText)
        {
            try
            {
                Stream stream = new FileStream(fileName, FileMode.Create, FileAccess.Write, FileShare.None);
                new BinaryFormatter().Serialize(stream, gameText);
                stream.Close();
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 从文件反序列化出GameText
        /// </summary>
        /// <param name="fileName">文件名</param>
        /// <param name="gameText">GameText</param>
        /// <returns>是否成功</returns>
        public static Boolean deserializeTextListFromFile(String FileName, ref GameText gameText)
        {
            try
            {
                Stream stream = new FileStream(FileName, FileMode.Open, FileAccess.Read, FileShare.Read);
                gameText = (GameText)new BinaryFormatter().Deserialize(stream);
                stream.Close();
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 生成游戏文件lt.bin
        /// </summary>
        /// <param name="chsCodeTable">译文对应码表</param>
        /// <param name="outputFilePath">输出文件路径</param>
        /// <returns>是否成功</returns>
        public static Boolean buildTheGameFile_lt_bin(CodeTable chsCodeTable, String outputFilePath)
        {
            FileStream oriFileStream_ltbin = File.Open(@"lt.bin", FileMode.Open, FileAccess.Read);
            FileStream outputFileStream_ltbin = new FileStream(outputFilePath, FileMode.Create, FileAccess.ReadWrite);
            oriFileStream_ltbin.CopyTo(outputFileStream_ltbin);
            outputFileStream_ltbin.Seek(0, SeekOrigin.Begin);

            BinaryWriter outputFileBinaryWriter_ltbin = new BinaryWriter(outputFileStream_ltbin);

            List<Char> chsCharList = chsCodeTable.getAllCharAsList();

            for (Int32 i = 0; i < chsCharList.Count; i++)
            {
                Char thisChar = chsCharList[i];
                List<Byte> thisBytes = Toolkit.Character2Bytes(thisChar, new Font("微软雅黑", 10));
                outputFileBinaryWriter_ltbin.Write(thisBytes.ToArray());
            }

            oriFileStream_ltbin.Close();

            Toolkit.updateTheVerifyCode(outputFileStream_ltbin);

            outputFileBinaryWriter_ltbin.Close();
            outputFileStream_ltbin.Close();
            return true;
        }

        /// <summary>
        /// 生成txt码表
        /// </summary>
        /// <param name="chsCodeTable">译文对应码表</param>
        /// <param name="outputFilePath">输出文件路径</param>
        /// <returns>是否成功</returns>
        public static Boolean buildTheTxtCodeTableFile(CodeTable chsCodeTable)//, String outputFilePath)
        {
            FileStream outputFileStream_txt = new FileStream("chsCodeTable.txt", FileMode.Create, FileAccess.ReadWrite);
            StreamWriter outputStreamWriter_txt = new StreamWriter(outputFileStream_txt, Encoding.Unicode);

            List<Char> chsCharList = chsCodeTable.getAllCharAsList();

            for (Int32 i = 0; i < chsCharList.Count; i++)
            {
                Char thisChar = chsCharList[i];
                outputStreamWriter_txt.WriteLine((i % 0x100).ToString("X2") + (i / 0x100).ToString("X2") + '=' + thisChar);
            }
            outputStreamWriter_txt.Close();
            outputFileStream_txt.Close();
            return true;
        }

        /// <summary>
        /// 生成游戏文件sc.bin
        /// </summary>
        /// <param name="gameText">GameText</param>
        /// <param name="chsCodeTable">译文对应码表</param>
        /// <param name="outputFilePath">输出文件路径</param>
        /// <returns>是否成功</returns>
        public static Boolean buildTheGameFile_sc_bin(GameText gameText, CodeTable chsCodeTable, String outputFilePath)
        {
            FileStream oriFileStream_scbin = File.Open(@"sc.bin", FileMode.Open, FileAccess.Read);
            Int64 offsetTextPoint = 0x90;
            Int64 offsetText = 0;

            Byte[] _Bytes;

            FileStream outputFileStream_scbin = new FileStream(outputFilePath, FileMode.Create, FileAccess.ReadWrite);
            oriFileStream_scbin.CopyTo(outputFileStream_scbin);

            Boolean normalTextEnd = false;
            Boolean startAChoose = true;

            Int32 _ID = 0;
            String _ChsName;
            String _ChsSentences;
            Int32 SentencesCharIndex = 0;

            while (true)
            {
                _Bytes = new Byte[4];
                oriFileStream_scbin.Seek(offsetTextPoint, SeekOrigin.Begin);
                oriFileStream_scbin.Read(_Bytes, 0, 4);
                offsetTextPoint = oriFileStream_scbin.Position;

                //普通文本指针结束
                if (normalTextEnd == false && offsetTextPoint > 0x2C568)
                {
                    offsetTextPoint = 0x2C570;
                    normalTextEnd = true;
                    continue;
                }

                //所有结束
                if (offsetTextPoint > 0x2C7B0)
                {
                    break;
                }

                if (normalTextEnd == false)
                {
                    //处理普通文本

                    //换算文本指针所指地址
                    offsetText = 0x30000 + Convert.ToInt32(_Bytes[3]) * 0x100 * 0x100 * 0x100 + Convert.ToInt32(_Bytes[2]) * 0x100 * 0x100 + Convert.ToInt32(_Bytes[1]) * 0x100 + Convert.ToInt32(_Bytes[0]);
                    //根据当前文本指针跳至文本位置
                    oriFileStream_scbin.Seek(offsetText, SeekOrigin.Begin);

                    _ID = 0;
                    _ChsName = null;
                    _ChsSentences = null;

                    SentencesCharIndex = 0;

                    _Bytes = new Byte[2];

                    while (true)
                    {
                        oriFileStream_scbin.Read(_Bytes, 0, 2);

                        //入口标记
                        if (_Bytes[0] == 0xF0 && _Bytes[1] == 0xFF)
                        {
                            //读取ID
                            oriFileStream_scbin.Read(_Bytes, 0, 2);
                            _ID = Convert.ToInt32(_Bytes[1]) * 0x100 + Convert.ToInt32(_Bytes[0]);

                            //获取当前行翻译后姓名与文本
                            _ChsName = gameText.getChsNamebyID(_ID);
                            _ChsSentences = gameText.getChsSentencesbyID(_ID);

                            Int32 NameCharIndex = 0;

                            //姓名处理
                            while (true)
                            {
                                oriFileStream_scbin.Read(_Bytes, 0, 2);
                                if (_Bytes[0] == 0xff && _Bytes[1] == 0xff) break;      //正文入口标记，结束姓名处理
                                outputFileStream_scbin.Seek(oriFileStream_scbin.Position - 2, SeekOrigin.Begin);      //输出文件指针同步

                                _Bytes = new Byte[] { 0x00, 0x00 };

                                if (NameCharIndex < _ChsName.Length)
                                {
                                    Byte[] thisCodeByte = chsCodeTable.getCode(_ChsName[NameCharIndex++]);
                                    _Bytes = thisCodeByte == null ? new Byte[] { 0x01, 0x00 } : thisCodeByte;      //0001,'■'
                                }
                                outputFileStream_scbin.Write(_Bytes, 0, 2);
                            }
                            continue;
                        }

                        //强制换行标记？貌似在那个小游戏上面是换行的……
                        if (_Bytes[0] == 0xFE && _Bytes[1] == 0xFF)
                        {
                            continue;
                        }

                        //结束标记
                        if (_Bytes[0] == 0xFB && _Bytes[1] == 0xFF)
                        {
                            break;
                        }

                        //结束标记(不刷新)
                        if (_Bytes[0] == 0xFD && _Bytes[1] == 0xFF)
                        {
                            break;
                        }

                        //输出文件文本指针同步
                        outputFileStream_scbin.Seek(oriFileStream_scbin.Position - 2, SeekOrigin.Begin);

                        _Bytes = new Byte[] { 0x00, 0x00 };

                        if (SentencesCharIndex < _ChsSentences.Length)
                        {
                            Byte[] thisCodeByte = chsCodeTable.getCode(_ChsSentences[SentencesCharIndex++]);
                            _Bytes = thisCodeByte == null ? new Byte[] { 0x01, 0x00 } : thisCodeByte;      //0001,'■'
                        }
                        outputFileStream_scbin.Write(_Bytes, 0, 2);
                    }
                }
                else
                {
                    //处理选项文本

                    //换算文本指针所指地址
                    offsetText = 0x30000 + Convert.ToInt32(_Bytes[3]) * 0x100 * 0x100 * 0x100 + Convert.ToInt32(_Bytes[2]) * 0x100 * 0x100 + Convert.ToInt32(_Bytes[1]) * 0x100 + Convert.ToInt32(_Bytes[0]);

                    if (offsetText == 0x30000)
                    {
                        startAChoose = true;
                        continue;
                    }

                    if (startAChoose)
                    {
                        oriFileStream_scbin.Read(_Bytes, 0, 4);
                        offsetTextPoint = oriFileStream_scbin.Position;
                        offsetText = 0x30000 + Convert.ToInt32(_Bytes[3]) * 0x100 * 0x100 * 0x100 + Convert.ToInt32(_Bytes[2]) * 0x100 * 0x100 + Convert.ToInt32(_Bytes[1]) * 0x100 + Convert.ToInt32(_Bytes[0]);
                        startAChoose = false;
                    }

                    //根据当前文本指针跳至文本位置
                    oriFileStream_scbin.Seek(offsetText, SeekOrigin.Begin);

                    //读取ID
                    while (true)
                    {
                        oriFileStream_scbin.Read(_Bytes, 0, 2);

                        if (_Bytes[0] == 0xFF && _Bytes[1] == 0xFF)     //结束标记
                        {
                            oriFileStream_scbin.Read(_Bytes, 0, 4);

                            //自定义ID，读取的ID号+9W
                            _ID = 90000 + Convert.ToInt32(_Bytes[3]) * 0x100 + Convert.ToInt32(_Bytes[2]);
                            break;
                        }
                    }

                    //根据当前文本指针跳至文本位置
                    oriFileStream_scbin.Seek(offsetText, SeekOrigin.Begin);

                    //获取选项译文
                    _ChsSentences = gameText.getChsSentencesbyID(_ID);

                    SentencesCharIndex = 0;

                    //输出文本
                    while (true)
                    {
                        oriFileStream_scbin.Read(_Bytes, 0, 2);

                        //输出文件文本指针同步
                        outputFileStream_scbin.Seek(oriFileStream_scbin.Position - 2, SeekOrigin.Begin);

                        if (_Bytes[0] == 0xFF && _Bytes[1] == 0xFF)     //结束标记
                        {
                            break;
                        }

                        _Bytes = new Byte[] { 0x00, 0x00 };
                        if (SentencesCharIndex < _ChsSentences.Length)
                        {
                            Byte[] thisCodeByte = chsCodeTable.getCode(_ChsSentences[SentencesCharIndex++]);
                            _Bytes = thisCodeByte == null ? new Byte[] { 0x01, 0x00 } : thisCodeByte;      //0001,'■'
                        }
                        outputFileStream_scbin.Write(_Bytes, 0, 2);
                    }
                }
            }

            oriFileStream_scbin.Close();

            Toolkit.updateTheVerifyCode(outputFileStream_scbin);

            outputFileStream_scbin.Close();

            return true;
        }

        /// <summary>
        /// 生成游戏文件sc.bin
        /// </summary>
        /// <param name="gameText">GameText</param>
        /// <param name="chsCodeTable">译文对应码表</param>
        /// <param name="outputFilePath">输出文件路径</param>
        /// <returns>是否成功</returns>
        public static Boolean buildTheGameFile_sc_bin__test(GameText gameText, CodeTable chsCodeTable, String outputFilePath)
        {
            FileStream oriFileStream_scbin = File.Open(@"sc.bin", FileMode.Open, FileAccess.Read);
            Int64 offsetTextPtr = 0x90;

            Byte[] _Bytes;

            FileStream outputFileStream_scbin = new FileStream(outputFilePath, FileMode.Create, FileAccess.ReadWrite);
            oriFileStream_scbin.CopyTo(outputFileStream_scbin);

            Int32 ID = 0;
            Byte[] ID_Bytes = new Byte[2] { 0, 0 };

            _Bytes = new Byte[4];
            Int64 ori_offsetTextNow = 0;
            Int64 ori_offsetTextNext = 0;
            Int64 chs_offsetTextNow = 0;

            while (true)
            {
                //读取指针
                _Bytes = new Byte[4];
                oriFileStream_scbin.Seek(offsetTextPtr, SeekOrigin.Begin);
                oriFileStream_scbin.Read(_Bytes, 0, 4);
                //换算文本指针所指地址
                ori_offsetTextNow = ori_offsetTextNext;
                ori_offsetTextNext = 0x30000 + Convert.ToInt32(_Bytes[3]) * 0x100 * 0x100 * 0x100 + Convert.ToInt32(_Bytes[2]) * 0x100 * 0x100 + Convert.ToInt32(_Bytes[1]) * 0x100 + Convert.ToInt32(_Bytes[0]);
                if (chs_offsetTextNow == 0) chs_offsetTextNow = ori_offsetTextNext;

                //保存读取【文本指针】的指针当前位置
                offsetTextPtr = oriFileStream_scbin.Position;

                if (ori_offsetTextNow == 0) continue;


                //普通文本指针结束
                if (offsetTextPtr > 0x2C568)
                {
                    offsetTextPtr = 0x2C570;
                    break;
                }

                //跳到当前【文本指针】所指位置
                oriFileStream_scbin.Seek(ori_offsetTextNow, SeekOrigin.Begin);

                List<Byte[]> arow = new List<Byte[]>();
                //一直循环直到读取位置超过下一个【文本指针】所指地址
                while (oriFileStream_scbin.Position < ori_offsetTextNext)
                {
                    _Bytes = new Byte[2];
                    oriFileStream_scbin.Read(_Bytes, 0, 2);
                    arow.Add(_Bytes);
                }

                //获取原特殊代码
                Boolean startflag = false;
                List<Byte[]> result = new List<Byte[]>();
                foreach (Byte[] thisBytes in arow)
                {
                    if (arow.IndexOf(thisBytes) == 1) { ID = Convert.ToInt32(thisBytes[1]) * 0x100 + Convert.ToInt32(thisBytes[0]); ID_Bytes = new Byte[2] { thisBytes[0], thisBytes[1] }; }
                    if (thisBytes[0] == 0xFE && thisBytes[1] == 0xFF) { startflag = true; continue; }
                    if (startflag) result.Add(thisBytes);
                }

                //输出文件定向到指针对应位置
                outputFileStream_scbin.Seek(chs_offsetTextNow, SeekOrigin.Begin);
                outputFileStream_scbin.Write(new Byte[] { 0xf0, 0xff }, 0, 2);      //入口
                outputFileStream_scbin.Write(ID_Bytes, 0, 2);                       //ID

                //获取当前行翻译后姓名与文本
                String ChsName = gameText.getChsNamebyID(ID);
                String ChsSentences = gameText.getChsSentencesbyID(ID);

                //姓名处理
                foreach (Char thisChar in ChsName.ToCharArray())
                {
                    Byte[] thisCodeByte = chsCodeTable.getCode(thisChar);
                    thisCodeByte = (thisCodeByte == null) ? new Byte[] { 0x01, 0x00 } : thisCodeByte;      //0001,'■'
                    outputFileStream_scbin.Write(thisCodeByte, 0, 2);
                }

                outputFileStream_scbin.Write(new Byte[] { 0xff, 0xff }, 0, 2);      //分割

                //文本处理
                foreach (Char thisChar in ChsSentences.ToCharArray())
                {
                    Byte[] thisCodeByte = chsCodeTable.getCode(thisChar);
                    thisCodeByte = (thisCodeByte == null) ? new Byte[] { 0x01, 0x00 } : thisCodeByte;      //0001,'■'
                    outputFileStream_scbin.Write(thisCodeByte, 0, 2);
                }

                foreach (Byte[] thisBytes in result)
                {
                    outputFileStream_scbin.Write(thisBytes, 0, 2);
                }

                chs_offsetTextNow = outputFileStream_scbin.Position;
                outputFileStream_scbin.Seek(offsetTextPtr, SeekOrigin.Begin);
                Int64 chs_offsetTextNow_forptr = chs_offsetTextNow - 0x30000;
                outputFileStream_scbin.Write(new Byte[] { Convert.ToByte(chs_offsetTextNow_forptr % 0x100), Convert.ToByte((chs_offsetTextNow_forptr / 0x100) % 0x100), Convert.ToByte((chs_offsetTextNow_forptr / 0x100 / 0x100) % 0x100), Convert.ToByte((chs_offsetTextNow_forptr / 0x100 / 0x100 / 0x100) % 0x100) }, 0, 4);
            }

            oriFileStream_scbin.Close();

            Toolkit.updateTheVerifyCode(outputFileStream_scbin);

            outputFileStream_scbin.Close();

            return true;
        }
    }
}
