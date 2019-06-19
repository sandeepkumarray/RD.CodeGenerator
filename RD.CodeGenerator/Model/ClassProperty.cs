using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RDCore.WPF.Common.UserCode;

namespace RD.CodeGenerator.Model
{
    public class ClassProperty : ViewModelBase
    {
        private string _propName;
        private string _propType;
        private bool _isXmlElement = false;
        private bool _isXmlAttribute = false;
        private bool _isXmlText = false;
        private bool _isGenericListType = false;
        private string _genericListType;
        private List<string> _propTypeList = null;
        private List<string> _genericListTypeList = null;
        private string _propAddUpdateText = "Add";
        private string _id;

        public string Id
        {
            get { return _id; }
            set
            {
                _id = value;
                OnPropertyChanged("Id");
            }
        }

        public string PropAddUpdateText
        {
            get { return _propAddUpdateText; }
            set
            {
                _propAddUpdateText = value;
                OnPropertyChanged("PropAddUpdateText");
            }
        }

        public List<string> GenericListTypeList
        {
            get { return _genericListTypeList; }
            set
            {
                _genericListTypeList = value;
                OnPropertyChanged("GenericListTypeList");
            }
        }

        public List<string> PropTypeList
        {
            get { return _propTypeList; }
            set
            {
                _propTypeList = value;
                OnPropertyChanged("PropTypeList");
            }
        }

        public string GenericListType
        {
            get { return _genericListType; }
            set
            {
                _genericListType = value;
                OnPropertyChanged("GenericListType");
            }
        }

        public string PropName
        {
            get { return _propName; }
            set
            {
                _propName = value;
                OnPropertyChanged("PropName");
            }
        }

        public string PropType
        {
            get { return _propType; }
            set
            {
                _propType = value;
                OnPropertyChanged("PropType");
            }
        }

        public bool IsXmlElement
        {
            get { return _isXmlElement; }
            set
            {
                _isXmlElement = value;
                OnPropertyChanged("IsXmlElement");
            }
        }

        public bool IsXmlAttribute
        {
            get { return _isXmlAttribute; }
            set
            {
                _isXmlAttribute = value;
                OnPropertyChanged("IsXmlAttribute");
            }
        }

        public bool IsXmlText
        {
            get { return _isXmlText; }
            set
            {
                _isXmlText = value;
                OnPropertyChanged("IsXmlText");
            }
        }

        public bool IsGenericListType
        {
            get { return _isGenericListType; }
            set
            {
                _isGenericListType = value;
                OnPropertyChanged("IsGenericListType");
            }
        }

        public ClassProperty()
        {
            PropTypeList = new List<string>();
            PropTypeList.Add("bool");
            PropTypeList.Add("byte");
            PropTypeList.Add("sbyte");
            PropTypeList.Add("char");
            PropTypeList.Add("decimal");
            PropTypeList.Add("double");
            PropTypeList.Add("float");
            PropTypeList.Add("int");
            PropTypeList.Add("uint");
            PropTypeList.Add("long");
            PropTypeList.Add("ulong");
            PropTypeList.Add("object");
            PropTypeList.Add("short");
            PropTypeList.Add("ushort");
            PropTypeList.Add("string");

            GenericListTypeList = new List<string>();
            GenericListTypeList.Add("Comparer");
            GenericListTypeList.Add("EqualityComparer");
            GenericListTypeList.Add("HashSet");
            GenericListTypeList.Add("KeyedByTypeCollection");
            GenericListTypeList.Add("LinkedList");
            GenericListTypeList.Add("LinkedListNode");
            GenericListTypeList.Add("List");
            GenericListTypeList.Add("Queue");
            GenericListTypeList.Add("SortedSet");
            GenericListTypeList.Add("Stack");
            GenericListTypeList.Add("SynchronizedCollection");
            GenericListTypeList.Add("SynchronizedReadOnlyCollection");
        }

        public ClassProperty(string propName, string propType)
            : this()
        {
            PropName = propName;
            PropType = propType;
        }
    }
}
