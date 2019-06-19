using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RDCore.WPF.Common.UserCode;
using System.Collections.ObjectModel;

namespace RD.CodeGenerator.Model
{
    public class CSharpClassFileSetting : ViewModelBase
    {
        #region Fileds
        private bool _isIncludeUsings = false;
        private bool _isIncludeNameSpace = false;
        private string _NameSpace = string.Empty;
        private bool _isSerializable = false;
        private bool _isXmlRoot = false;
        private bool _isIncludeDefaultConstructor = true;
        private bool _isIncludeParametrizedConstructor = false;
        private ObservableCollection<ClassProperty> _parameters = null;
        private string _cSharpFilePath = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
        private string _cSharpFileExtension = "cs";
        private bool _isAdditionalCodeSnippet = false;
        private string _additionalCodeSnippet = string.Empty;
        private bool _isClassNameCamelCasing = false;
        private char _separator = '_';
        private ObservableCollection<string> _userDefinedUsings = null;
        #endregion

        #region Properties
        public bool IsIncludeUsings
        {
            get { return _isIncludeUsings; }
            set { _isIncludeUsings = value; OnPropertyChanged("IsIncludeUsings"); }
        }

        public bool IsIncludeNameSpace
        {
            get { return _isIncludeNameSpace; }
            set { _isIncludeNameSpace = value; OnPropertyChanged("IsIncludeNameSpace"); }
        }

        public string NameSpace
        {
            get { return _NameSpace; }
            set { _NameSpace = value; OnPropertyChanged("NameSpace"); }
        }

        public bool IsSerializable
        {
            get { return _isSerializable; }
            set { _isSerializable = value; OnPropertyChanged("IsSerializable"); }
        }

        public bool IsXmlRoot
        {
            get { return _isXmlRoot; }
            set { _isXmlRoot = value; OnPropertyChanged("IsXmlRoot"); }
        }

        public bool IsIncludeDefaultConstructor
        {
            get { return _isIncludeDefaultConstructor; }
            set { _isIncludeDefaultConstructor = value; OnPropertyChanged("IsIncludeDefaultConstructor"); }
        }

        public bool IsIncludeParametrizedConstructor
        {
            get { return _isIncludeParametrizedConstructor; }
            set { _isIncludeParametrizedConstructor = value; OnPropertyChanged("IsIncludeParametrizedConstructor"); }
        }

        public ObservableCollection<ClassProperty> Parameters
        {
            get { return _parameters; }
            set { _parameters = value; OnPropertyChanged("Parameters"); }
        }

        public string CSharpFilePath
        {
            get { return _cSharpFilePath; }
            set { _cSharpFilePath = value; OnPropertyChanged("CSharpFilePath"); }
        }

        public string CSharpFileExtension
        {
            get { return _cSharpFileExtension; }
            set { _cSharpFileExtension = value; OnPropertyChanged("CSharpFileExtension"); }
        }

        public bool IsAdditionalCodeSnippet
        {
            get { return _isAdditionalCodeSnippet; }
            set { _isAdditionalCodeSnippet = value; OnPropertyChanged("IsAdditionalCodeSnippet"); }
        }

        public string AdditionalCodeSnippet
        {
            get { return _additionalCodeSnippet; }
            set { _additionalCodeSnippet = value; OnPropertyChanged("AdditionalCodeSnippet"); }
        }

        public bool IsClassNameCamelCasing
        {
            get { return _isClassNameCamelCasing; }
            set { _isClassNameCamelCasing = value; OnPropertyChanged("IsClassNameCamelCasing"); }
        }

        public char Separator
        {
            get { return _separator; }
            set { _separator = value; OnPropertyChanged("Separator"); }
        }

        public ObservableCollection<string> UserDefinedUsings
        {
            get { return _userDefinedUsings; }
            set { _userDefinedUsings = value; OnPropertyChanged("UserDefinedUsings"); }
        }
        #endregion

        public CSharpClassFileSetting()
        {
            Parameters = new ObservableCollection<ClassProperty>();
            UserDefinedUsings = new ObservableCollection<string>();
        }
    }
}
