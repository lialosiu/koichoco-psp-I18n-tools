using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 恋选PSP文本处理器
{
    [Serializable]
    class TextList
    {
        private Text[] text;
        private Int32 textCount;

        [Serializable]
        public class Text
        {
            public Int32 Status = -1;
            public Boolean Changed = false;

            readonly public Int32 LineID;
            readonly public Int32 NameID;
            readonly public String OriText;
            public String ChsText;

            public Text(Int32 LineID, Int32 NameID, String OriText, String ChsText)
            {
                this.LineID = LineID;
                this.NameID = NameID;
                this.OriText = SomeToolkit.ToSBC(OriText);
                this.ChsText = SomeToolkit.ToSBC(ChsText);
                this.checkStatus();
            }
            public Int32 checkStatus()
            {
                this.Status = 0;
                if (this.ChsText.Length > this.OriText.Length) this.Status = 1; //文本过长
                if (this.ChsText.Length == 0 || this.ChsText == this.OriText) this.Status = 2;  //未翻译
                if (this.ChsText.Length != 0 && (this.ChsText.Split('…').Length == ChsText.Length + 1)) this.Status = 0;    //已完成
                return this.Status;
            }
        }

        public TextList()
        {
            text = new Text[50000];
            textCount = 0;
        }

        public Boolean add(Int32 LineID, Int32 NameID, String OriText, String ChsText)
        {
            if (text[LineID] == null)
            {
                text[LineID] = new Text(LineID, NameID, OriText, ChsText);
                textCount++;
                return true;
            }
            else return false;
        }

        public Boolean change(Int32 LineNum, String ChsText)
        {
            if (text[LineNum] == null) return false;
            else
            {
                text[LineNum].ChsText = SomeToolkit.ToSBC(ChsText);
                text[LineNum].Changed = true;
                text[LineNum].checkStatus();
                return true;
            }
        }

        public Int32 getStatus(Int32 LineNum)
        {
            return text[LineNum].Status;
        }
        public Int32 getNameID(Int32 LineNum)
        {
            return text[LineNum].NameID;
        }
        public String getOriText(Int32 LineNum)
        {
            return text[LineNum].OriText;
        }
        public String getChsText(Int32 LineNum)
        {
            return text[LineNum].ChsText;
        }
        public Int32 getTextCount()
        {
            return this.textCount;
        }
        public Text getText(Int32 LineNum)
        {
            return text[LineNum];
        }
        public Text[] getAll_NOTNULL()
        {
            ArrayList temp = new ArrayList();
            foreach (Text thisOne in text)
            {
                if (thisOne != null)
                    temp.Add(thisOne);
            }
            return (Text[])temp.ToArray(typeof(Text));
        }
        public Text[] getAll_NOT2()
        {
            ArrayList temp = new ArrayList();
            foreach (Text thisLine in text)
            {
                if (thisLine != null && thisLine.Status < 2)
                    temp.Add(thisLine);
            }
            return (Text[])temp.ToArray(typeof(Text));
        }
    }
}
