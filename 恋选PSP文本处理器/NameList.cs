using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 恋选PSP文本处理器
{
    [Serializable]
    public class NameList
    {        
        private Name[] name;
        private Int32 nameCount;

        [Serializable]
        public class Name
        {
            public Int32 NameID;
            public String OriName;
            public String ChsName;
            public Name(Int32 NameID, String OriName, String ChsName)
            {
                this.NameID = NameID;
                this.OriName = OriName;
                this.ChsName = ChsName;
            }
        }

        public NameList()
        {
            name = new Name[300];
            this.nameCount = 0;
        }
        public Boolean add(String OriName, String ChsName)
        {
            if (this.getIDByOriName(OriName) != -1) return false;
            this.name[this.nameCount] = new Name(this.nameCount, OriName, ChsName); 
            this.nameCount++;
            return true;
        }
        public String getOriName(Int32 NameID)
        {
            return name[NameID].OriName;
        }
        public String getChsName(Int32 NameID)
        {
            return name[NameID].ChsName;
        }
        public Int32 getIDByOriName(String OriName)
        {
            for (Int32 NameID = 0; NameID < this.nameCount; NameID++)
            {
                if (this.getOriName(NameID) == OriName)
                    return NameID;
            }
            return -1;
        }
        public Name[] getAll_NOTNULL()
        {
            ArrayList temp = new ArrayList();
            foreach (Name thisOne in name)
            {
                if (thisOne != null)
                    temp.Add(thisOne);
            }
            return (Name[])temp.ToArray(typeof(Name));
        }

        public Int32 getNameCount()
        {
            return this.nameCount;
        }

        public Boolean setChsName(Int32 NameID, String ChsName)
        {
            if (NameID == -1) return false;
            if (name[NameID] == null) return false;
            name[NameID].ChsName = ChsName;
            return true;
        }
    }
}
