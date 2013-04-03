using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 恋选PSP文本处理器
{
    [Serializable]
    public class GameText
    {
        [Serializable]
        public class Line
        {
            readonly public Int32 id;
            readonly public String oriName;
            private String _chsName;
            public String chsName
            {
                get { return _chsName; }
                set { _chsName = Toolkit.ToSBC(value); }
            }
            readonly public String oriSentences;
            private String _chsSentences;
            public String chsSentences
            {
                get { return _chsSentences; }
                set { _chsSentences = Toolkit.ToSBC(value); checkStatus(); }
            }
            public Int32 status;
            public Boolean changedFlag;

            public Line(Int32 id, String oriName, String chsName, String oriSentences, String chsSentences)
            {
                this.id = id;
                this.oriName = Toolkit.ToSBC(oriName);
                this.chsName = chsName;
                this.oriSentences = Toolkit.ToSBC(oriSentences);
                this.chsSentences = chsSentences;
                this.changedFlag = false;
                checkStatus();
            }

            public Int32 checkStatus()
            {
                this.status = 0;
                if (this.chsSentences.Length > this.oriSentences.Length) this.status = 1; //文本过长
                if (this.chsSentences.Length == 0 || this.chsSentences == this.oriSentences) this.status = 2;  //未翻译
                if (this.chsSentences.Length != 0 && (this.chsSentences.Split('…').Length == chsSentences.Length + 1)) this.status = 0;    //已完成
                return this.status;
            }

            public override String ToString()
            {
                return this.id.ToString();
            }
        }

        Dictionary<Int32, Line> line = new Dictionary<Int32, Line>();
        Dictionary<String, List<Int32>> oriNameSentences2ids = new Dictionary<String, List<Int32>>();
        Dictionary<String, List<Int32>> oriSentences2ids = new Dictionary<String, List<Int32>>();

        public void addLine(Int32 id, String oriName, String chsName, String oriSentences, String chsSentences)
        {
            Line thisLine = new Line(id, oriName, chsName, oriSentences, chsSentences);
            line.Add(id, thisLine);
            if (oriNameSentences2ids.ContainsKey('[' + oriName + ']' + oriSentences))
            {
                List<Int32> IDList = oriNameSentences2ids['[' + oriName + ']' + oriSentences];
                IDList.Add(id);
            }
            else
            {
                List<Int32> IDList = new List<Int32>();
                IDList.Add(id);
                oriNameSentences2ids.Add('[' + oriName + ']' + oriSentences, IDList);
            }

            if (oriSentences2ids.ContainsKey(oriSentences))
            {
                List<Int32> IDList = oriSentences2ids[oriSentences];
                IDList.Add(id);
            }
            else
            {
                List<Int32> IDList = new List<Int32>();
                IDList.Add(id);
                oriSentences2ids.Add(oriSentences, IDList);
            }
        }

        //用ID搜索Line
        public Line getLinebyID(Int32 id)
        {
            Line thisLine = line[id];
            return thisLine;
        }

        /*
        //用原名和原文搜索匹配的第一个Line
        public Line getLinebyOriNameSentences(String oriName, String oriSentences)
        {
            try
            {
                return getLinebyID(oriNameSentences2id['[' + oriName + ']' + oriSentences]);
            }
            catch (KeyNotFoundException)
            {
                return null;
            }
        }
         * */

        /// <summary>
        /// 将所有的Line的changeFlag设为false
        /// </summary>
        public void setAllLineChangeFlagToFalse()
        {
            foreach (var thisLine in this.line)
            {
                thisLine.Value.changedFlag = false;
            }
        }

        /// <summary>
        /// 通过原名、原文匹配并设置译名、译文
        /// </summary>
        /// <param name="oriName">原名</param>
        /// <param name="oriSentences">原文</param>
        /// <param name="chsName">译名</param>
        /// <param name="chsSentences">译文</param>
        /// <returns>成功设置计数</returns>
        public Int32 setChsbyOriNameSentences(String oriName, String oriSentences, String chsName, String chsSentences, StreamWriter logFile)
        {
            Int32 successCount = 0;
            List<Int32> _IDs;
            if (oriNameSentences2ids.ContainsKey('[' + oriName + ']' + oriSentences))
            {
                _IDs = oriNameSentences2ids['[' + oriName + ']' + oriSentences];
                foreach (Int32 thisID in _IDs)
                {
                    Line thisLine = getLinebyID(thisID);
                    if (thisLine.changedFlag == false)
                    {
                        thisLine.chsName = chsName;
                        thisLine.chsSentences = chsSentences;
                        thisLine.changedFlag = true;
                        successCount++;
                    }
                }
            }
            else
            {
                if (logFile != null)
                    logFile.WriteLine("[" + oriName + "][" + chsName + "][" + oriSentences + "][" + chsSentences + "]");
            }

            return successCount;
        }

        /// <summary>
        /// 通过原文匹配并设置译文
        /// </summary>
        /// <param name="oriSentences">原文</param>
        /// <param name="chsSentences">译文</param>
        /// <returns>成功设置计数</returns>
        public Int32 setChsbyOriSentences(String oriSentences, String chsSentences, StreamWriter logFile)
        {
            Int32 successCount = 0;
            List<Int32> _IDs;

            if (oriSentences2ids.ContainsKey(oriSentences))
            {
                _IDs = oriSentences2ids[oriSentences];
                foreach (Int32 thisID in _IDs)
                {
                    Line thisLine = getLinebyID(thisID);
                    if (thisLine.changedFlag == false)
                    {
                        thisLine.chsSentences = chsSentences;
                        thisLine.changedFlag = true;
                        successCount++;
                    }
                }
            }
            else
            {
                if (logFile != null)
                    logFile.WriteLine("[" + oriSentences + "][" + chsSentences + "]");
            }

            return successCount;
        }


        //用ID设置中文名
        public Boolean setChsNamebyID(Int32 id, String chsName)
        {
            Line thisLine = getLinebyID(id);
            thisLine.chsName = chsName;
            return true;
        }

        //用ID设置中文文本
        public Boolean setChsSentencesbyID(Int32 id, String chsSentences)
        {
            Line thisLine = getLinebyID(id);
            thisLine.chsSentences = chsSentences;
            return true;
        }

        //获取原始名
        public String getOriNamebyID(Int32 id)
        {
            Line thisLine = getLinebyID(id);
            return thisLine.oriName;
        }

        //获取中文名
        public String getChsNamebyID(Int32 id)
        {
            Line thisLine = getLinebyID(id);
            return thisLine.chsName;
        }

        //获取原始文本
        public String getOriSentencesbyID(Int32 id)
        {
            Line thisLine = getLinebyID(id);
            return thisLine.oriSentences;
        }

        //获取中文文本
        public String getChsSentencesbyID(Int32 id)
        {
            Line thisLine = getLinebyID(id);
            return thisLine.chsSentences;
        }

        //获取行数
        public Int32 getLineCount()
        {
            return line.Count;
        }

        //输出所有为DataTable格式
        public DataTable getAllAsDataTable()
        {
            DataTable gameTextDataTable = new DataTable();

            gameTextDataTable.Columns.Add("ID");
            gameTextDataTable.Columns.Add("原名");
            gameTextDataTable.Columns.Add("翻名");
            gameTextDataTable.Columns.Add("原文");
            gameTextDataTable.Columns.Add("译文");

            foreach (var item in this.line)
            {
                Line thisLine = item.Value;
                gameTextDataTable.Rows.Add(thisLine, thisLine.oriName, thisLine.chsName, thisLine.oriSentences, thisLine.chsSentences);
            }
            return gameTextDataTable;
        }

        //输出状态非0的文本为DataTable格式
        public DataTable getStatusNotZeroAsDataTable()
        {
            DataTable gameTextDataTable = new DataTable();

            gameTextDataTable.Columns.Add("ID");
            gameTextDataTable.Columns.Add("原名");
            gameTextDataTable.Columns.Add("翻名");
            gameTextDataTable.Columns.Add("原文");
            gameTextDataTable.Columns.Add("译文");

            foreach (var item in this.line)
            {
                Line thisLine = item.Value;
                if (thisLine.status != 0)
                    gameTextDataTable.Rows.Add(thisLine, thisLine.oriName, thisLine.chsName, thisLine.oriSentences, thisLine.chsSentences);
            }
            return gameTextDataTable;
        }

        /// <summary>
        /// 输出所有文本
        /// </summary>
        /// <returns>所有的文本为List</returns>
        public List<Line> getAllAsList()
        {
            List<Line> gameTextList = new List<Line>();

            foreach (var item in this.line)
            {
                Line thisLine = item.Value;
                gameTextList.Add(thisLine);
            }
            return gameTextList;
        }

        /// <summary>
        /// 输出所有文本，除了未翻译的
        /// </summary>
        /// <returns>所有除了未翻译的文本List</returns>
        public List<Line> getAllNot2AsList()
        {
            List<Line> gameTextList = new List<Line>();

            foreach (var item in this.line)
            {
                Line thisLine = item.Value;
                if (thisLine.status == 2) continue;
                gameTextList.Add(thisLine);
            }
            return gameTextList;
        }

        //从TXT文件读入
        public Int32 readFromTXT(String fileName)
        {
            Int32 successCount = 0;
            StreamReader thisFile = new StreamReader(fileName);
            while (!thisFile.EndOfStream)
            {
                String thisLine = thisFile.ReadLine();
                Match thisLineString_Match = Regex.Match(thisLine, @"O(\d{5})\[(.*)](.*)");
                if (thisLineString_Match.Success)
                {
                    Int32 _ID = Int32.Parse(thisLineString_Match.Groups[1].ToString());
                    String _OriName = thisLineString_Match.Groups[2].ToString();
                    String _OriSentences = thisLineString_Match.Groups[3].ToString();

                    thisLine = thisFile.ReadLine();
                    thisLineString_Match = Regex.Match(thisLine, "C" + _ID.ToString("D5") + @"\[(.*)](.*)");
                    if (thisLineString_Match.Success)
                    {
                        String _ChsName = thisLineString_Match.Groups[1].ToString();
                        String _ChsSentences = thisLineString_Match.Groups[2].ToString();
                        this.setChsSentencesbyID(_ID, _ChsSentences);
                        successCount++;
                    }
                }
            }
            thisFile.Close();
            return successCount;
        }

        //输出为TXT文件
        public Boolean writeToTXT(String fileName)
        {
            try
            {
                StreamWriter thisFile = new StreamWriter(fileName);
                foreach (Line thisLine in this.getAllAsList())
                {
                    thisFile.WriteLine("O" + thisLine.id.ToString("D5") + "[" + thisLine.oriName + "]" + thisLine.oriSentences);
                    thisFile.WriteLine("C" + thisLine.id.ToString("D5") + "[" + thisLine.chsName + "]" + thisLine.chsSentences);
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

        /// <summary>
        /// 读入PC版TXT文件
        /// </summary>
        /// <param name="fileURL">TXT文件路径</param>
        /// <param name="matchOriName">是否用原名进行匹配</param>
        /// <returns>成功计数</returns>
        public Int32 readFromPCTXT(String fileURL, Boolean matchOriName, StreamWriter _LogFile1 = null, StreamWriter _LogFile2 = null)
        {
            Int32 successCount = 0;
            StreamReader thisFile = new StreamReader(fileURL);

            String[,] _StrType = new String[10, 2];

            //○46○裕樹「よいしょっと」
            _StrType[0, 0] = @"○\d+○(.*)「(.+)」";
            _StrType[0, 1] = @"●\d+●(.*)「(.+)」";

            //○46○裕樹「よいしょっと
            _StrType[1, 0] = @"○\d+○(.*)「(.+)";
            _StrType[1, 1] = @"●\d+●(.*)「(.+)";

            //○46○裕樹/ょっと」
            _StrType[2, 0] = @"○\d+○(.*)\/(.+)」";
            _StrType[2, 1] = @"●\d+●(.*)\/(.+)」";

            //○xx○/ょっと
            _StrType[3, 0] = @"○\d+○\/(.+)";
            _StrType[3, 1] = @"●\d+●\/?(.+)";

            //○47○家の自転車を引っ張り出して、またがった。
            _StrType[4, 0] = @"○\d+○(.+)";
            _StrType[4, 1] = @"●\d+●(.+)";

            Int32 _Type = -1;

            while (!thisFile.EndOfStream)
            {
                String thisLineString = thisFile.ReadLine();
                Match thisLineString_Match;

                if (thisLineString == "") continue;
                else if (thisLineString == "\t") continue;   //这坑爹啊，为什么会有'\t' _(:3」∠)_
                else if (Regex.Match(thisLineString, _StrType[0, 0]).Success) _Type = 0;
                else if (Regex.Match(thisLineString, _StrType[1, 0]).Success) _Type = 1;
                else if (Regex.Match(thisLineString, _StrType[2, 0]).Success) _Type = 2;
                else if (Regex.Match(thisLineString, _StrType[3, 0]).Success) _Type = 3;
                else if (Regex.Match(thisLineString, _StrType[4, 0]).Success) _Type = 4;

                thisLineString_Match = Regex.Match(thisLineString, _StrType[_Type, 0]);
                if (thisLineString_Match.Success)
                {
                    String _OriName;
                    String _OriSentences;
                    if (_Type < 3)
                    {
                        _OriName = thisLineString_Match.Groups[1].ToString();
                        _OriSentences = thisLineString_Match.Groups[2].ToString();
                    }
                    else
                    {
                        _OriName = "";
                        _OriSentences = thisLineString_Match.Groups[1].ToString();
                    }

                    thisLineString = thisFile.ReadLine();
                    thisLineString_Match = Regex.Match(thisLineString, _StrType[_Type, 1]);
                    if (thisLineString_Match.Success)
                    {
                        String _ChsName;
                        String _ChsSentences;

                        if (_Type < 3)
                        {
                            _ChsName = thisLineString_Match.Groups[1].ToString();
                            _ChsSentences = thisLineString_Match.Groups[2].ToString();
                        }
                        else
                        {
                            _ChsName = "";
                            _ChsSentences = thisLineString_Match.Groups[1].ToString();
                        }

                        Int32 thisTimeSuccessCount = 0;
                        if (matchOriName == true)
                        {
                            thisTimeSuccessCount = this.setChsbyOriNameSentences(_OriName, _OriSentences, _ChsName, _ChsSentences, _LogFile1);
                        }
                        else
                        {
                            thisTimeSuccessCount = this.setChsbyOriSentences(_OriSentences, _ChsSentences, _LogFile2);
                        }
                        successCount += thisTimeSuccessCount;
                    }
                }
            }
            thisFile.Close();
            return successCount;
        }

        /// <summary>
        /// 生成输入的TextList中译名和译文所用的字符List
        /// </summary>
        /// <param name="theTextList">要输入的文本List</param>
        /// <returns>所有译名译文所使用过的已排序字符List</returns>
        public static List<Char> UsedChsCharListFactory(List<Line> theTextList)
        {
            List<Char> usedChar = new List<Char>();

            foreach (Line thisLine in theTextList)
            {
                foreach (Char thisChar in thisLine.chsName + thisLine.chsSentences)
                {
                    if (!usedChar.Exists(delegate(Char c) { return thisChar == c; }) && thisChar >= '一' && thisChar <= '龙')
                    {
                        usedChar.Add(thisChar);
                    }
                }
            }
            usedChar.Sort();
            return usedChar;
        }


        //生成原始文本
        public static GameText InitGameTextFactory()
        {
            GameText text = new GameText();
            CodeTable oriCodeTable = CodeTable.getOriCodeTable();

            FileStream thisFileStream = File.Open(@"sc.bin", FileMode.Open, FileAccess.Read);
            Int64 offsetTextPoint = 0x90;
            Int64 offsetText = 0;

            Byte[] _Bytes;

            Boolean normalTextEnd = false;
            Boolean startAChoose = true;

            while (true)
            {
                thisFileStream.Seek(offsetTextPoint, SeekOrigin.Begin);
                _Bytes = new Byte[4];
                thisFileStream.Read(_Bytes, 0, 4);
                offsetTextPoint = thisFileStream.Position;

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

                Int32 _ID = 0;
                Int32 _Code = 0;
                Char _Character = '\0';
                StringBuilder _Name = new StringBuilder();
                StringBuilder _Sentences = new StringBuilder();

                if (normalTextEnd == false)
                {
                    //处理普通文本

                    //换算文本指针所指地址
                    offsetText = 0x30000 + Convert.ToInt32(_Bytes[3]) * 0x100 * 0x100 * 0x100 + Convert.ToInt32(_Bytes[2]) * 0x100 * 0x100 + Convert.ToInt32(_Bytes[1]) * 0x100 + Convert.ToInt32(_Bytes[0]);
                    //根据当前文本指针跳至文本位置
                    thisFileStream.Seek(offsetText, SeekOrigin.Begin);

                    _Bytes = new Byte[2];

                    while (true)
                    {
                        thisFileStream.Read(_Bytes, 0, 2);
                        if (_Bytes[0] == 0xF0 && _Bytes[1] == 0xFF)       //入口标记
                        {
                            thisFileStream.Read(_Bytes, 0, 2);              //读取ID
                            _ID = Convert.ToInt32(_Bytes[1]) * 0x100 + Convert.ToInt32(_Bytes[0]);
                            while (true)                                        //读取姓名
                            {
                                thisFileStream.Read(_Bytes, 0, 2);
                                if (_Bytes[0] == 0xFF && _Bytes[1] == 0xFF) break;    //正文入口标记，结束姓名处理
                                _Code = Convert.ToInt32(_Bytes[1]) * 0x100 + Convert.ToInt32(_Bytes[0]);
                                _Character = oriCodeTable.getCharacter(_Code);
                                _Name.Append(_Character);
                            }
                            continue;
                        }

                        //强制换行标记？貌似在那个小游戏上面是换行的……
                        if (_Bytes[0] == 0xFE && _Bytes[1] == 0xFF)
                        {
                            //_Sentences.Append('\n');
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

                        //正文处理
                        _Code = Convert.ToInt32(_Bytes[1]) * 0x100 + Convert.ToInt32(_Bytes[0]);
                        _Character = oriCodeTable.getCharacter(_Code);
                        _Sentences.Append(_Character);
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
                        thisFileStream.Read(_Bytes, 0, 4);
                        offsetTextPoint = thisFileStream.Position;
                        offsetText = 0x30000 + Convert.ToInt32(_Bytes[3]) * 0x100 * 0x100 * 0x100 + Convert.ToInt32(_Bytes[2]) * 0x100 * 0x100 + Convert.ToInt32(_Bytes[1]) * 0x100 + Convert.ToInt32(_Bytes[0]);
                        startAChoose = false;
                    }

                    //根据当前文本指针跳至文本位置
                    thisFileStream.Seek(offsetText, SeekOrigin.Begin);

                    while (true)
                    {
                        thisFileStream.Read(_Bytes, 0, 2);
                        if (_Bytes[0] == 0xFF && _Bytes[1] == 0xFF)     //结束标记
                        {
                            thisFileStream.Read(_Bytes, 0, 4);

                            //自定义ID，读取的ID号+9W
                            _ID = 90000 + Convert.ToInt32(_Bytes[3]) * 0x100 + Convert.ToInt32(_Bytes[2]);
                            break;
                        }
                        _Code = Convert.ToInt32(_Bytes[1]) * 0x100 + Convert.ToInt32(_Bytes[0]);
                        _Character = oriCodeTable.getCharacter(_Code);
                        _Sentences.Append(_Character);
                    }
                }
                //添加数据
                text.addLine(_ID, _Name.ToString(), _Name.ToString(), _Sentences.ToString(), _Sentences.ToString());
            }

            //完成
            thisFileStream.Close();

            return text;
        }

    }
}
