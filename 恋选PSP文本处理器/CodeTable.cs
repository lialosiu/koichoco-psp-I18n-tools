using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace 恋选PSP脚本处理器
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

        //获取原始码表
        public static CodeTable getOriCodeTable()
        {
            CodeTable thisOriCodeTable = new CodeTable();
            if (!File.Exists("OriCodeTable.xml")) throw new FileNotFoundException();
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
    }
}
