using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace 恋选PSP文本处理器
{
    public class CodeTable
    {
        Int32 wordCount = 0;
        Char[] word = new Char[0xFFFF];
        public void addWord(Char word)
        {
            this.word[wordCount++] = word;
        }
        public void setWord(Int32 code, Char word)
        {
            wordCount++;
            this.word[code] = word;
        }
        public Char getWord(Int32 code)
        {
            return this.word[code];
        }
        public Byte[] getCode(Char word)
        {
            for (int i=0;i<0xffff;i++)
            {
                if (word == this.word[i])
                    return new Byte[] { (Byte)(i % 0x100), (Byte)(i / 0x100) };
            }
            return null;
        }
        public Boolean loadCodeTable()
        {
            try
            {
                if (wordCount != 0) throw new Exception();
                XDocument doc = XDocument.Load(Assembly.GetExecutingAssembly().GetManifestResourceStream("恋选PSP文本处理器.Files.Ori_CodeTable.xml"));
                var ele =
                    from item in doc.Descendants("项目")
                    select new
                    {
                        code1 = item.Attribute("码1").Value,
                        code2 = item.Attribute("码2").Value,
                        word = item.Attribute("字").Value
                    };
                foreach (var att in ele)
                    this.setWord(Int32.Parse(att.code2 + att.code1, System.Globalization.NumberStyles.AllowHexSpecifier), Convert.ToChar(att.word));
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
