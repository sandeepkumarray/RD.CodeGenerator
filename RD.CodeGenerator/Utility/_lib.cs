using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.IO;
using System.Text.RegularExpressions;

namespace RD.CodeGenerator
{
    public static class _lib
    {
        /// <summary>
        /// It takes the object and Serializes it into an XML
        /// </summary>
        /// <param name="obj">Object</param>
        /// <returns>XML Data</returns>
        public static string ToXml(object obj)
        {
            XmlSerializer s = new XmlSerializer(obj.GetType());
            using (StringWriterWithEncoding writer = new StringWriterWithEncoding(Encoding.UTF8))
            {
                s.Serialize(writer, obj);
                return writer.ToString();
            }
        }

        /// <summary>
        /// It takes the XML data and deserializes it into an object
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="data">XML data</param>
        /// <returns>object</returns>
        public static object FromXml<T>(this TextReader data)
        {
            XmlSerializer s = new XmlSerializer(typeof(T));
            //using (StringReader reader = new StringReader(data))
            //{

            object obj = s.Deserialize(data);
            return (T)obj;
            //}
        }

        /// <summary>
        /// Extends StringWriter class for changing Encoding type.
        /// </summary>
        class StringWriterWithEncoding : StringWriter
        {
            public StringWriterWithEncoding(StringBuilder sb, Encoding encoding)
                : base(sb)
            {
                this.m_Encoding = encoding;
            }

            public StringWriterWithEncoding(Encoding encoding)
                : base()
            {
                this.m_Encoding = encoding;
            }

            private readonly Encoding m_Encoding;
            public override Encoding Encoding
            {
                get
                {
                    return this.m_Encoding;
                }
            }
        }

        public static string RemoveSpecialCharacters2(string str)
        {
            StringBuilder sb = new StringBuilder();
            foreach (char c in str)
            {
                if ((c >= '0' && c <= '9') || (c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z') || c == '.' || c == '_')
                {
                    sb.Append(c);
                }
            }
            return sb.ToString();
        }

        public static string GetUniqueIdentifier(UniqueIdentifierSetting UniqueIdentifierSetting)
        {
            string result = string.Empty;
            switch (UniqueIdentifierSetting.UniqueIDType)
            {
                case UniqueIDType.TimeStamp:
                    long ticks = DateTime.Now.Ticks;
                    byte[] bytes = BitConverter.GetBytes(ticks);
                    if (UniqueIdentifierSetting.ToBase64)
                        result = Convert.ToBase64String(bytes)
                                                .Replace('+', '_')
                                                .Replace('/', '-')
                                                .TrimEnd('=');
                    else
                        result = Convert.ToString(ticks);
                    break;
                case UniqueIDType.IntegerIncreamental:
                    if (Convert.ToInt32(UniqueIdentifierSetting.PreviousValue) == 0)
                        throw new Exception("PreviousValue Must be greater than 0");
                    break;
                case UniqueIDType.AlphaNumeric:
                    if (Convert.ToInt32(UniqueIdentifierSetting.PreviousValue) == 0)
                        throw new Exception("PreviousValue Must be greater than 0");
                    else
                    {
                        var numAlpha = new Regex("(?<Alpha>[a-zA-Z]*)(?<Numeric>[0-9]*)");
                        var match = numAlpha.Match("sa00001");
                        var alpha = match.Groups["Alpha"].Value;
                        var num = match.Groups["Numeric"].Value;
                        var number = Convert.ToInt32(num);
                        ++number;
                        var nextId = alpha;
                        var length = num.Length;
                        if (length != 0)
                        {
                            var format = "D" + length;
                            nextId += number.ToString(format);
                        }
                        else
                        {
                            nextId += number.ToString();
                        }
                    }
                    break;
                case UniqueIDType.GUID:
                    result = new Guid("dddddddd-dddd-dddd-dddd-dddddddddddd").ToString();
                    break;
                case UniqueIDType.TimeStampWithSize:
                    if (UniqueIdentifierSetting.Size == 0)
                        throw new Exception("Size Must be greater than 0");
                    else
                    {
                        result = GenerateRandomNumber(UniqueIdentifierSetting.Size);
                    }
                    break;
                default:
                    break;
            }
            return result;
        }

        public static string GenerateRandomNumber(int size)
        {
            Random random = new Random((int)DateTime.Now.Ticks);
            StringBuilder builder = new StringBuilder();
            string s;
            for (int i = 0; i < size; i++)
            {
                s = Convert.ToString(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65)));
                builder.Append(s);
            }

            return builder.ToString();
        }
    }

    public class UniqueIdentifierSetting
    {
        public UniqueIDType UniqueIDType;
        public String PreviousValue = "0";
        public bool ToBase64 = true;
        public int Size = 20;
    }

    public enum UniqueIDType
    {
        TimeStamp = 1,
        IntegerIncreamental = 2,
        AlphaNumeric = 3,
        GUID = 4,
        TimeStampWithSize = 5,
    }
}
