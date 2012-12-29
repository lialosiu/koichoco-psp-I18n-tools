using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace 恋选PSP文本处理器
{
    class Text
    {
        public class oneLineText
        {
            public Int32 Status = -1;
            public Boolean Changed = false;
            public String LineNum;
            public String OriName;
            public String ChsName;
            public String OriText;
            public String ChsText;
            public oneLineText(String LineNum, String OriName, String ChsName, String OriText, String ChsText)
            {
                this.LineNum = LineNum;
                this.OriName = ToSBC(OriName);
                this.ChsName = ToSBC(ChsName);
                this.OriText = ToSBC(OriText);
                this.ChsText = ToSBC(ChsText);
                this.CheckStatus();
            }
            public Int32 CheckStatus()
            {
                this.Status = 0;
                if (this.ChsText.Length > this.OriText.Length) this.Status = 1;
                if (this.ChsText.Length == 0) this.Status = 2;
                return this.Status;
            }
        }

        Int32 lineNumCount = 0;
        oneLineText[] text = new oneLineText[50000];

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

        public Boolean add(Int32 LineNum,String OriName, String ChsName, String OriText, String ChsText)
        {
            if (text[LineNum] == null)
            {
                text[LineNum] = new oneLineText(LineNum.ToString("D5"), OriName, ChsName, OriText, ChsText);
                lineNumCount++;
                return true;
            }
            else return false;
        }

        public Boolean change(Int32 LineNum,String ChsName, String ChsText)
        {
            if (text[LineNum] == null) return false;
            else
            {
                text[LineNum].ChsName = ToSBC(ChsName);
                text[LineNum].ChsText = ToSBC(ChsText);
                text[LineNum].Changed = true;
                text[LineNum].CheckStatus();
                return true;
            }
        }
        public Boolean change(Int32 LineNum, String ChsText)
        {
            if (text[LineNum] == null) return false;
            else
            {
                text[LineNum].ChsText = ToSBC(ChsText);
                text[LineNum].Changed = true;
                text[LineNum].CheckStatus();
                return true;
            }
        }
        public Int32 getStatus(Int32 LineNum)
        {
            return text[LineNum].Status;
        }
        public String getOriName(Int32 LineNum)
        {
            return text[LineNum].OriName;
        }
        public String getChsName(Int32 LineNum)
        {
            return text[LineNum].ChsName;
        }
        public String getOriText(Int32 LineNum)
        {
            return text[LineNum].OriText;
        }
        public String getChsText(Int32 LineNum)
        {
            return text[LineNum].ChsText;
        }
        public Int32 getTotalLineNum()
        {
            return this.lineNumCount;
        }
        public oneLineText getOne(Int32 LineNum)
        {
            return text[LineNum];
        }
        public oneLineText[] getAll()
        {
            return text;
        }

        public Boolean resetText(CodeTable ori_codeTable)
        {
            if (lineNumCount != 0) return false;
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
                            word = ori_codeTable.getWord(code);
                            if (word == '\0') return false;
                            name.Append(word);
                        }
                        continue;
                    }

                    if (word_Byte[0] == 0xfe && word_Byte[1] == 0xff) break;	//出口标记

                    //正文处理
                    code = Convert.ToInt32(word_Byte[1]) * 0x100 + Convert.ToInt32(word_Byte[0]);
                    word = ori_codeTable.getWord(code);
                    if (word == '\0') return false;
                    text.Append(word);
                }

                //添加数据
                this.add(lineNum, name.ToString(), "", text.ToString(), "");
            }
            //完成
            textReader.Close();
            pointReader.Close();
            return true;
        }

        public Int32 readFromTXT(String fileName)
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
                        if (this.getOriName(lineNum) == oriName && this.getOriText(lineNum) == oriText)
                        {
                            this.change(lineNum, chsName, chsText);
                            successCount++;
                        }
                    }
                }
            }
            thisFile.Close();
            return successCount;
        }

        public Boolean writeToTXT(String fileName)
        {
            try
            {
                StreamWriter thisFile = new StreamWriter(fileName);
                foreach (oneLineText i in this.text)
                {
                    if (i == null) continue;
                    thisFile.WriteLine("O" + i.LineNum + "[" + i.OriName + "]" + i.OriText);
                    thisFile.WriteLine("C" + i.LineNum + "[" + i.ChsName + "]" + i.ChsText);
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

        public Boolean readFromXML(String fileName)
        {
            try
            {
                XDocument doc = XDocument.Load(fileName);
                var content =
                    from item in doc.Descendants("行")
                    select new
                    {
                        lineNum = item.Attribute("行号").Value,
                        oriName = item.Element("日文姓名").Value,
                        chsName = item.Element("简中姓名").Value,
                        oriText = item.Element("日文文本").Value,
                        chsText = item.Element("简中文本").Value
                    };
                foreach (var thisLine in content)
                    if (!this.add(Int32.Parse(thisLine.lineNum), thisLine.oriName, thisLine.chsName, thisLine.oriText, thisLine.chsText)) throw new Exception();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public Boolean writeToXML(String fileName)
        {
            try
            {
                var toXML = new XElement("文本",
                    from thisLine in this.text
                    where thisLine != null
                    select new XElement("行",
                        new XAttribute("行号", thisLine.LineNum),
                        new XElement("日文姓名", thisLine.OriName),
                        new XElement("简中姓名", thisLine.ChsName),
                        new XElement("日文文本", thisLine.OriText),
                        new XElement("简中文本", thisLine.ChsText)
                    )
                );
                XDocument doc = new XDocument();
                doc.Declaration = new XDeclaration("1.0", "UTF-8", "yes");
                doc.Add(toXML);
                doc.Save(fileName);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
