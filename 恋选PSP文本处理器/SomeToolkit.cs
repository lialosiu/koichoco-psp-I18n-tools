using System;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Text.RegularExpressions;

namespace 恋选PSP文本处理器
{
    class SomeToolkit
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

        public static Int32 readFromTXT(String fileName, ref NameList namelist, ref TextList textlist)
        {
            Int32 successCount = 0;
            StreamReader thisFile = new StreamReader(fileName);
            while (!thisFile.EndOfStream)
            {
                String thisLine = thisFile.ReadLine();
                Match thisLine_Match = Regex.Match(thisLine, @"O(\d{5})\[(.*)](.*)");
                if (thisLine_Match.Success)
                {
                    Int32 lineNum = Int32.Parse(thisLine_Match.Groups[1].ToString());
                    String oriName = thisLine_Match.Groups[2].ToString();
                    String oriText = thisLine_Match.Groups[3].ToString();

                    thisLine = thisFile.ReadLine();
                    thisLine_Match = Regex.Match(thisLine, "C" + lineNum.ToString("D5") + @"\[(.*)](.*)");
                    if (thisLine_Match.Success)
                    {
                        String chsName = thisLine_Match.Groups[1].ToString();
                        String chsText = thisLine_Match.Groups[2].ToString();
                        if (!namelist.setChsName(namelist.getIDByOriName(oriName), chsName))
                            oriName.ToCharArray();
                        textlist.change(lineNum, chsText);
                        successCount++;
                    }
                }
            }
            thisFile.Close();
            return successCount;
        }

        public static Boolean writeToTXT(String fileName, NameList namelist, TextList textlist)
        {
            try
            {
                StreamWriter thisFile = new StreamWriter(fileName);
                foreach (TextList.Text i in textlist.getAll_NOTNULL())
                {
                    if (i == null) continue;
                    thisFile.WriteLine("O" + i.LineID + "[" + namelist.getOriName(i.NameID) + "]" + i.OriText);
                    thisFile.WriteLine("C" + i.LineID + "[" + namelist.getChsName(i.NameID) + "]" + i.ChsText);
                    thisFile.WriteLine("");
                }
                thisFile.Close();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static Boolean serializeNameListToFile(String FileName,NameList namelist)
        {
            Stream stream = new FileStream(FileName, FileMode.Create, FileAccess.Write, FileShare.None);
            new BinaryFormatter().Serialize(stream, namelist);
            stream.Close();
            return true;
        }

        public static Boolean deserializeNameListFromFile(String FileName,ref NameList namelist)
        {
            Stream stream;
            try
            {
                stream = new FileStream(FileName, FileMode.Open, FileAccess.Read, FileShare.Read);
                namelist = (NameList)new BinaryFormatter().Deserialize(stream);
                stream.Close();
            }
            catch (FileNotFoundException)
            {
                return false;
            }
            return true;
        }

        public static Boolean serializeTextListToFile(String FileName, TextList textlist)
        {
            Stream stream = new FileStream(FileName, FileMode.Create, FileAccess.Write, FileShare.None);
            new BinaryFormatter().Serialize(stream, textlist);
            stream.Close();
            return true;
        }

        public static Boolean deserializeTextListFromFile(String FileName, ref TextList textlist)
        {
            try
            {
            Stream stream = new FileStream(FileName, FileMode.Open, FileAccess.Read, FileShare.Read);
            textlist = (TextList)new BinaryFormatter().Deserialize(stream);
            stream.Close();
            }
            catch (FileNotFoundException)
            {
                return false;
            }
            return true;
        }

        public static Boolean initialize(ref NameList namelist, ref TextList textlist)
        {
            namelist = new NameList();
            textlist = new TextList();

            CodeTable oricodetable = new CodeTable();
            oricodetable.loadCodeTable();

            Stream pointReader = Assembly.GetExecutingAssembly().GetManifestResourceStream("恋选PSP文本处理器.Files.sc.bin");
            Stream textReader = Assembly.GetExecutingAssembly().GetManifestResourceStream("恋选PSP文本处理器.Files.sc.bin");

            pointReader.Seek(0x90, SeekOrigin.Begin);                     //跳至文本指针表
            while (true)
            {
                Byte[] textPoint_Byte = new Byte[4];
                pointReader.Read(textPoint_Byte, 0, 4);

                //换算文本指针所指地址
                Int32 textPoint = 0x30000 + Convert.ToInt32(textPoint_Byte[3]) * 0x100 * 0x100 * 0x100 + Convert.ToInt32(textPoint_Byte[2]) * 0x100 * 0x100 + Convert.ToInt32(textPoint_Byte[1]) * 0x100 + Convert.ToInt32(textPoint_Byte[0]);

                if (textPoint <= 0x30000) break;                            //文本指针结束标记

                textReader.Seek(textPoint, SeekOrigin.Begin);               //根据当前文本指针跳至文本位置

                Int32 lineNum = 0, code = 0;
                Char word = '\0';
                StringBuilder text = new StringBuilder();
                StringBuilder name = new StringBuilder();
                Byte[] word_Byte = new Byte[2];
                while (true)
                {
                    textReader.Read(word_Byte, 0, 2);
                    if (word_Byte[0] == 0xf0 && word_Byte[1] == 0xff)       //入口标记
                    {
                        textReader.Read(word_Byte, 0, 2);                   //读取行号
                        lineNum = Convert.ToInt32(word_Byte[1]) * 0x100 + Convert.ToInt32(word_Byte[0]);
                        while (true)                                        //读取姓名
                        {
                            textReader.Read(word_Byte, 0, 2);
                            if (word_Byte[0] == 0xff && word_Byte[1] == 0xff) break;    //正文入口标记，结束姓名处理
                            code = Convert.ToInt32(word_Byte[1]) * 0x100 + Convert.ToInt32(word_Byte[0]);
                            word = oricodetable.getWord(code);
                            if (word == '\0') return false;
                            name.Append(word);
                        }
                        continue;
                    }

                    if (word_Byte[0] == 0xfe && word_Byte[1] == 0xff) break;	//出口标记

                    //正文处理
                    code = Convert.ToInt32(word_Byte[1]) * 0x100 + Convert.ToInt32(word_Byte[0]);
                    word = oricodetable.getWord(code);
                    if (word == '\0') return false;
                    text.Append(word);
                }
                //添加数据
                namelist.add(name.ToString(), name.ToString());
                textlist.add(lineNum, namelist.getIDByOriName(name.ToString()), text.ToString(), text.ToString());
            }
            //完成
            textReader.Close();
            pointReader.Close();

            return true;
        }

    }
}
