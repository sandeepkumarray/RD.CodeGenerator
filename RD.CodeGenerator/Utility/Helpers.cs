using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RD.CodeGenerator
{
    class Helpers
    {
    }


    public class ClipExcelField
    {
        public string Value { get; set; }
    }

    public class ClipExcelRow
    {
        public List<ClipExcelField> Fields { get; set; }

        public ClipExcelRow(string value)
        {
            Fields = new List<ClipExcelField>();
            var fieldsString = value.Split(new char[] { '\t' });
            foreach (string f in fieldsString)
            {
                Fields.Add(new ClipExcelField { Value = f });
            }
        }

        public object this[int index]
        {
            get
            {
                if (index < this.Fields.Count)
                    return this.Fields[index].Value;
                else
                    return null;
            }
            set
            {
                this.Fields[index].Value = (string)value;
            }
        }

    }

}
