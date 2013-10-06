using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Text;
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
            graphics.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
            graphics.DrawString(character.ToString(), font, new SolidBrush(Color.White), -1, 2);

            for (int nowHeight = 0; nowHeight < bmp.Height; nowHeight++)
            {
                for (int nowWidth = 0; nowWidth < bmp.Width; nowWidth += 2)
                {
                    Int32 huidu1 = Convert.ToInt32((0.3 * bmp.GetPixel(nowWidth, nowHeight).R + 0.59 * bmp.GetPixel(nowWidth, nowHeight).G + 0.11 * bmp.GetPixel(nowWidth, nowHeight).B) / 0x20 - 1);
                    Int32 huidu2 = Convert.ToInt32((0.3 * bmp.GetPixel(nowWidth + 1, nowHeight).R + 0.59 * bmp.GetPixel(nowWidth + 1, nowHeight).G + 0.11 * bmp.GetPixel(nowWidth + 1, nowHeight).B) / 0x20 - 1);
                    if (huidu1 <= 0) huidu1 = 0;
                    if (huidu2 <= 0) huidu2 = 0;
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

        public static Int64 Bytes2Int64(Byte[] theData)
        {
            Int64 result = 0;
            for (Int32 index = theData.Length - 1; index >= 0; index--)
                result = result * 0x100 + Convert.ToInt32(theData[index]);
            return result;
        }

        public static Byte[] Int642Bytes(Int64 theInt64, Int32 c)
        {
            Byte[] result = new Byte[c];
            Int32 index = 0;
            while (theInt64 != 0)
            {
                result[index++] = (Byte)(theInt64 % 0x100);
                theInt64 /= 0x100;
            }
            return result;
        }

        /// <summary>
        /// 生成游戏文件lt.bin
        /// </summary>
        /// <param name="chsCodeTable">译文对应码表</param>
        /// <param name="outputFilePath">输出文件路径</param>
        /// <returns>是否成功</returns>
        public static Boolean buildTheGameFile_lt_bin(CodeTable chsCodeTable, String outputFilePath, bool type)
        {
            FileStream oriFileStream_ltbin = File.Open(@"lt.bin", FileMode.Open, FileAccess.Read);
            FileStream outputFileStream_ltbin = new FileStream(outputFilePath, FileMode.Create, FileAccess.ReadWrite);
            oriFileStream_ltbin.CopyTo(outputFileStream_ltbin);
            outputFileStream_ltbin.Seek(0, SeekOrigin.Begin);

            BinaryWriter outputFileBinaryWriter_ltbin = new BinaryWriter(outputFileStream_ltbin);
                        
            List<Char> chsCharList = chsCodeTable.getAllCharAsList();

            if (type) chsCharList = new List<Char>(Toolkit.ChsChangeToCht(new String(chsCharList.ToArray())).ToCharArray());

            Font thisFont = new Font("微软雅黑", 10);
            FontDialog fontDialog = new FontDialog();
            fontDialog.Font = thisFont;
            if (fontDialog.ShowDialog() == DialogResult.OK)
            {
                thisFont = fontDialog.Font;
            }

            for (Int32 i = 0; i < chsCharList.Count; i++)
            {
                Char thisChar = chsCharList[i];
                List<Byte> thisBytes = Toolkit.Character2Bytes(thisChar, thisFont);
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

            FileStream outputFileStream_scbin = new FileStream(outputFilePath, FileMode.Create, FileAccess.ReadWrite);
            oriFileStream_scbin.CopyTo(outputFileStream_scbin);

            Dictionary<Int64, Int64> normalPtrDicV2K = new Dictionary<Int64, Int64>();
            Dictionary<Int64, Int64> normalPtrDicK2V = new Dictionary<Int64, Int64>();
            Dictionary<Int64, Int64> choosePtrDicV2K = new Dictionary<Int64, Int64>();
            Dictionary<Int64, Int64> choosePtrDicK2V = new Dictionary<Int64, Int64>();
            Dictionary<Int64, List<Int64>> specialPtrDicV2K = new Dictionary<Int64, List<Int64>>();
            Dictionary<Int64, Int64> specialPtrDicK2V = new Dictionary<Int64, Int64>();

            //读取所有普通指针
            oriFileStream_scbin.Seek(0x90, SeekOrigin.Begin);
            while (oriFileStream_scbin.Position < 0x2c568)
            {
                Int64 position = oriFileStream_scbin.Position;
                Byte[] theData = new Byte[4];
                oriFileStream_scbin.Read(theData, 0, 4);
                normalPtrDicV2K.Add(Bytes2Int64(theData) + 0x30000, position);
            }

            //读取所有选项指针
            oriFileStream_scbin.Seek(0x2c570, SeekOrigin.Begin);
            while (oriFileStream_scbin.Position < 0x2c7b0)
            {
                Int64 position = oriFileStream_scbin.Position;
                Byte[] theData = new Byte[4];
                oriFileStream_scbin.Read(theData, 0, 4);
                while (Bytes2Int64(theData) != 0)
                {
                    oriFileStream_scbin.Read(theData, 0, 4);
                    if (Bytes2Int64(theData) == 0) break;
                    choosePtrDicV2K.Add(Bytes2Int64(theData) + 0x30000, oriFileStream_scbin.Position - 4);
                }
            }

            //读取所有特殊指针
            oriFileStream_scbin.Seek(0x30000, SeekOrigin.Begin);
            while (oriFileStream_scbin.Position < 0x324e4)
            {
                Int64 position = oriFileStream_scbin.Position;
                Byte[] theData = new Byte[4];
                oriFileStream_scbin.Read(theData, 0, 4);
                List<Int64> thisList;
                if (specialPtrDicV2K.ContainsKey(Bytes2Int64(theData) + 0x30000)) thisList = specialPtrDicV2K[Bytes2Int64(theData) + 0x30000]; else thisList = new List<Int64>();
                thisList.Add(position);
                specialPtrDicV2K[Bytes2Int64(theData) + 0x30000] = thisList;
            }

            //解析脚本
            oriFileStream_scbin.Seek(0x324e4, SeekOrigin.Begin);
            outputFileStream_scbin.Seek(0x324e4, SeekOrigin.Begin);
            while (oriFileStream_scbin.Position < 0x35e050)
            {
                Byte[] thisDataBytes = new Byte[2];

                //检查普通指针
                if (normalPtrDicV2K.ContainsKey(oriFileStream_scbin.Position))
                {
                    normalPtrDicK2V.Add(normalPtrDicV2K[oriFileStream_scbin.Position], outputFileStream_scbin.Position);
                }
                //检查选项指针
                if (choosePtrDicV2K.ContainsKey(oriFileStream_scbin.Position))
                {
                    choosePtrDicK2V.Add(choosePtrDicV2K[oriFileStream_scbin.Position], outputFileStream_scbin.Position);
                }
                //检查特殊指针
                if (specialPtrDicV2K.ContainsKey(oriFileStream_scbin.Position))
                {
                    foreach (Int64 thisKey in specialPtrDicV2K[oriFileStream_scbin.Position])
                        specialPtrDicK2V.Add(thisKey, outputFileStream_scbin.Position);
                }

                oriFileStream_scbin.Read(thisDataBytes, 0, 2);

                Int32 thisData = Convert.ToInt32(Bytes2Int64(thisDataBytes));
                switch (thisData)
                {
                    case 0xfff0:
                        outputFileStream_scbin.Write(thisDataBytes, 0, 2);
                        oriFileStream_scbin.Read(thisDataBytes, 0, 2);

                        //ID
                        Int32 ID = Convert.ToInt32(Bytes2Int64(thisDataBytes));
                        outputFileStream_scbin.Write(thisDataBytes, 0, 2);

                        String chsName = gameText.getChsNamebyID(ID);
                        String chsSentences = gameText.getChsSentencesbyID(ID);

                        //姓名处理
                        foreach (Char thisChar in chsName.ToCharArray())
                        {
                            Byte[] thisCodeByte = chsCodeTable.getCode(thisChar);
                            thisCodeByte = (thisCodeByte == null) ? new Byte[] { 0x01, 0x00 } : thisCodeByte;       //0001,'■'
                            outputFileStream_scbin.Write(thisCodeByte, 0, 2);
                        }

                        outputFileStream_scbin.Write(new Byte[] { 0xff, 0xff }, 0, 2);                              //分割

                        //文本处理
                        foreach (Char thisChar in chsSentences.ToCharArray())
                        {
                            Byte[] thisCodeByte = chsCodeTable.getCode(thisChar);
                            thisCodeByte = (thisCodeByte == null) ? new Byte[] { 0x01, 0x00 } : thisCodeByte;       //0001,'■'
                            outputFileStream_scbin.Write(thisCodeByte, 0, 2);
                        }
                        do { oriFileStream_scbin.Read(thisDataBytes, 0, 2); } while (Bytes2Int64(thisDataBytes) != 0xfffe);
                        outputFileStream_scbin.Write(thisDataBytes, 0, 2);
                        break;

                    default:
                        if (choosePtrDicV2K.ContainsKey(oriFileStream_scbin.Position - 2))
                        {
                            //处理选项文本
                            while (true)
                            {
                                oriFileStream_scbin.Read(thisDataBytes, 0, 2);

                                if (thisDataBytes[0] == 0xFF && thisDataBytes[1] == 0xFF)     //结束标记
                                {
                                    oriFileStream_scbin.Read(thisDataBytes, 0, 2);
                                    oriFileStream_scbin.Read(thisDataBytes, 0, 2);

                                    //自定义ID，读取的ID号+9W
                                    Int32 _ID = 90000 + Convert.ToInt32(thisDataBytes[1]) * 0x100 + Convert.ToInt32(thisDataBytes[0]);
                                    String _chsSentences = gameText.getChsSentencesbyID(_ID);

                                    foreach (Char thisChar in _chsSentences.ToCharArray())
                                    {
                                        Byte[] thisCodeByte = chsCodeTable.getCode(thisChar);
                                        thisCodeByte = (thisCodeByte == null) ? new Byte[] { 0x01, 0x00 } : thisCodeByte;       //0001,'■'
                                        outputFileStream_scbin.Write(thisCodeByte, 0, 2);
                                    }

                                    outputFileStream_scbin.Write(new Byte[] { 0xff, 0xff }, 0, 2);
                                    outputFileStream_scbin.Write(new Byte[] { 0x00, 0x00 }, 0, 2);
                                    outputFileStream_scbin.Write(thisDataBytes, 0, 2);
                                    break;
                                }
                            }
                        }
                        else
                            outputFileStream_scbin.Write(thisDataBytes, 0, 2);
                        break;
                }
            }

            //补零
            while (outputFileStream_scbin.Position < 0x35efe0)
            {
                outputFileStream_scbin.Write(new Byte[2] { 0, 0 }, 0, 2);
            }

            //更新所有普通指针
            outputFileStream_scbin.Seek(0x90, SeekOrigin.Begin);
            while (outputFileStream_scbin.Position < 0x2c568)
            {
                outputFileStream_scbin.Write(Int642Bytes(normalPtrDicK2V[outputFileStream_scbin.Position] - 0x30000, 4), 0, 4);
            }

            //更新所有选项指针
            outputFileStream_scbin.Seek(0x2c570, SeekOrigin.Begin);
            while (outputFileStream_scbin.Position < 0x2c7b0)
            {
                if (choosePtrDicK2V.ContainsKey(outputFileStream_scbin.Position)) outputFileStream_scbin.Write(Int642Bytes(choosePtrDicK2V[outputFileStream_scbin.Position] - 0x30000, 4), 0, 4);
                else outputFileStream_scbin.Seek(4, SeekOrigin.Current);
            }

            //更新所有特殊指针
            outputFileStream_scbin.Seek(0x30000, SeekOrigin.Begin);
            while (outputFileStream_scbin.Position < 0x324e4)
            {
                if (specialPtrDicK2V.ContainsKey(outputFileStream_scbin.Position)) outputFileStream_scbin.Write(Int642Bytes(specialPtrDicK2V[outputFileStream_scbin.Position] - 0x30000, 4), 0, 4);
                else outputFileStream_scbin.Seek(4, SeekOrigin.Current);
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
        public static Boolean buildTheGameFile_sc_bin_old(GameText gameText, CodeTable chsCodeTable, String outputFilePath)
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

        public static void outputSC_txt(CodeTable oriCodeTable)
        {
            FileStream oriFileStream_scbin = File.Open(@"sc.bin", FileMode.Open, FileAccess.Read);
            StreamWriter sctxt = new StreamWriter(new FileStream(@"sc.txt", FileMode.Create, FileAccess.ReadWrite));
            Dictionary<Int64, Int64> normalPtr = new Dictionary<Int64, Int64>();
            Dictionary<Int64, List<Int64>> specialPtr = new Dictionary<Int64, List<Int64>>();

            //读取所有普通指针
            oriFileStream_scbin.Seek(0x90, SeekOrigin.Begin);
            while (oriFileStream_scbin.Position < 0x2c568)
            {
                Int64 position = oriFileStream_scbin.Position;
                Byte[] theData = new Byte[4];
                oriFileStream_scbin.Read(theData, 0, 4);
                normalPtr.Add(Bytes2Int64(theData), position);
            }

            //读取所有特殊指针
            oriFileStream_scbin.Seek(0x30000, SeekOrigin.Begin);
            while (oriFileStream_scbin.Position < 0x324e4)
            {
                Int64 position = oriFileStream_scbin.Position;
                Byte[] theData = new Byte[4];
                oriFileStream_scbin.Read(theData, 0, 4);

                List<Int64> thisList;
                if (specialPtr.ContainsKey(Bytes2Int64(theData))) thisList = specialPtr[Bytes2Int64(theData)]; else thisList = new List<Int64>();
                thisList.Add(position);
                specialPtr.Add(Bytes2Int64(theData), thisList);
            }


            Boolean flag = false;

            //解析脚本
            oriFileStream_scbin.Seek(0x324e4, SeekOrigin.Begin);
            while (oriFileStream_scbin.Position < 0x35e050)
            {
                Byte[] theData = new Byte[2];
                if (normalPtr.ContainsKey(oriFileStream_scbin.Position - 0x30000)) { sctxt.WriteLine(); sctxt.Write("[普通指针][" + normalPtr[oriFileStream_scbin.Position - 0x30000] + "]"); }
                if (specialPtr.ContainsKey(oriFileStream_scbin.Position - 0x30000)) { sctxt.WriteLine(); sctxt.Write("[特殊指针][" + specialPtr[oriFileStream_scbin.Position - 0x30000] + "]"); }
                oriFileStream_scbin.Read(theData, 0, 2);
                Int32 theInt32 = Convert.ToInt32(Bytes2Int64(theData));
                switch (theInt32)
                {
                    case 0xff68:
                        sctxt.WriteLine();
                        sctxt.Write("[语音]");
                        oriFileStream_scbin.Read(theData, 0, 2);
                        sctxt.Write("[ID:" + Bytes2Int64(theData) + "]");
                        oriFileStream_scbin.Read(theData, 0, 2);
                        sctxt.Write("[人物:" + Bytes2Int64(theData) + "]");
                        break;

                    case 0xfff0:
                        sctxt.WriteLine();
                        sctxt.Write("[文本]");
                        oriFileStream_scbin.Read(theData, 0, 2);
                        sctxt.Write("[ID:" + Bytes2Int64(theData) + "]");
                        flag = true;
                        break;

                    case 0xfffb:
                        sctxt.WriteLine();
                        sctxt.Write("[清空文本]");
                        flag = false;
                        break;

                    case 0xfffd:
                        sctxt.WriteLine();
                        sctxt.Write("[保留文本]");
                        flag = false;
                        break;

                    case 0xfffe:
                        sctxt.WriteLine();
                        sctxt.Write("[等待输入]");
                        flag = false;
                        break;

                    case 0xffff:
                        if (flag) sctxt.Write("[分割]");
                        else
                        {
                            if (theInt32 > 0xff00) sctxt.WriteLine();
                            sctxt.Write("[" + theInt32.ToString("X4") + "]");
                        }
                        break;


                    default:
                        if (flag) sctxt.Write(oriCodeTable.getCharacter(theInt32));
                        else
                        {
                            if (theInt32 > 0xff00) sctxt.WriteLine();
                            sctxt.Write("[" + theInt32.ToString("X4") + "]");
                        }
                        break;
                }
            }
            oriFileStream_scbin.Close();
            sctxt.Close();
        }

        public static string ChsChangeToCht(string chsText)
        {
            return Microsoft.VisualBasic.Strings.StrConv(chsText, Microsoft.VisualBasic.VbStrConv.TraditionalChinese, 0);
        }
    }
}
