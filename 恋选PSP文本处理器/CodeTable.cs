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
        Dictionary<Int32, Char> code2character = new Dictionary<Int32, Char>();
        Dictionary<Char, Int32> character2code = new Dictionary<Char, Int32>();

        //设置某个Code的Character
        public void setCharacter(Int32 code, Char character)
        {
            if (!code2character.ContainsKey(code))
                code2character.Add(code, character);
            if (!character2code.ContainsKey(character))
                character2code.Add(character, code);
        }

        //通过Code获取字符
        public Char getCharacter(Int32 code)
        {
            Char thisCharacter = code2character[code];
            return thisCharacter;
        }

        //通过字符获取Code
        public Byte[] getCode(Char character)
        {
            if (!character2code.ContainsKey(character))
                return null;
            Int32 thisCode = character2code[character];
            return new Byte[] { (Byte)(thisCode % 0x100), (Byte)(thisCode / 0x100) };
        }

        /// <summary>
        /// 获取码表中所有字符
        /// </summary>
        /// <returns>字符列表</returns>
        public List<Char> getAllCharAsList()
        {
            List<Char> theList = new List<Char>();
            for (Int32 code = 0; code < this.code2character.Count; code++)
            {
                theList.Add(this.code2character[code]);
            }
            return theList;
        }

        /// <summary>
        /// 原始码表工厂
        /// </summary>
        /// <returns>原始码表</returns>
        public static CodeTable OriCodeTableFactory()
        {
            CodeTable thisOriCodeTable = new CodeTable();
            XDocument doc = XDocument.Load("OriCodeTable.xml");
            var ele =
                from item in doc.Descendants("项目")
                select new
                {
                    code1 = item.Attribute("码1").Value,
                    code2 = item.Attribute("码2").Value,
                    word = item.Attribute("字").Value
                };
            foreach (var att in ele)
                thisOriCodeTable.setCharacter(Int32.Parse(att.code2 + att.code1, System.Globalization.NumberStyles.AllowHexSpecifier), Convert.ToChar(att.word));
            return thisOriCodeTable;
        }

        /// <summary>
        /// 码表工厂
        /// </summary>
        /// <param name="usedChsCharList">使用的字符列表</param>
        /// <returns>码表</returns>
        public static CodeTable CodeTableFactory(List<Char> usedChsCharList)
        {
            usedChsCharList.Insert(0, '　');
            usedChsCharList.Insert(1, '■');
            CodeTable theCodeTable = new CodeTable();
            for (Int32 i = 0; i < usedChsCharList.Count; i++)
            {
                Char thisChar = usedChsCharList[i];
                theCodeTable.setCharacter(i, thisChar);
            }
            return theCodeTable;
        }
    }
}
