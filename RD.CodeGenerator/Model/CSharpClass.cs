using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RDCore.WPF.Common.UserCode;
using System.Collections.ObjectModel;
using System.IO;
using RD.CodeGenerator.View;
using System.Windows;

namespace RD.CodeGenerator.Model
{
    public class CSharpClass : ViewModelBase
    {
        Window WinClassPropertyView;
        private string _id = string.Empty;
        private string _nameSpace = string.Empty;
        private string _className = string.Empty;
        private ObservableCollection<ClassProperty> _classProperties = null;
        private CSharpClassFileSetting _cSharpClassFileSettings = null;
        private bool _isIncludePrivateField = false;
        private bool _isIncludetOnPropertyChanged = false;
        private bool _isXmlElement = false;
        private bool _isXmlAttribute = false;
        private bool _isXmlText = false;
        private ClassProperty _classPropertyObj = null;
        private string _classAddUpdateText = "Add";

        public string ClassAddUpdateText
        {
            get { return _classAddUpdateText; }
            set
            {
                _classAddUpdateText = value;
                OnPropertyChanged("ClassAddUpdateText");
            }
        }
        
        public string Id
        {
            get { return _id; }
            set
            {
                _id = value;
                OnPropertyChanged("Id");
            }
        }

        public ClassProperty ClassPropertyObj
        {
            get { return _classPropertyObj; }
            set
            {
                _classPropertyObj = value;
                OnPropertyChanged("ClassPropertyObj");
            }
        }

        public bool IsXmlElement
        {
            get { return _isXmlElement; }
            set
            {
                _isXmlElement = value;
                OnPropertyChanged("IsXmlElement");
                UpdateClassProperties();
            }
        }

        public bool IsXmlAttribute
        {
            get { return _isXmlAttribute; }
            set
            {
                _isXmlAttribute = value;
                OnPropertyChanged("IsXmlAttribute");
                UpdateClassProperties();
            }
        }

        public bool IsXmlText
        {
            get { return _isXmlText; }
            set
            {
                _isXmlText = value;
                OnPropertyChanged("IsXmlText");
                UpdateClassProperties();
            }
        }

        public bool IsIncludetOnPropertyChanged
        {
            get { return _isIncludetOnPropertyChanged; }
            set
            {
                _isIncludetOnPropertyChanged = value;
                OnPropertyChanged("IsIncludetOnPropertyChanged");
            }
        }

        public bool IsIncludePrivateField
        {
            get { return _isIncludePrivateField; }
            set
            {
                _isIncludePrivateField = value;
                OnPropertyChanged("IsIncludePrivateField");
            }
        }

        public string ClassName
        {
            get { return _className; }
            set
            {
                _className = value;
                OnPropertyChanged("ClassName");
            }
        }

        public ObservableCollection<ClassProperty> ClassProperties
        {
            get { return _classProperties; }
            set
            {
                _classProperties = value;
                OnPropertyChanged("ClassProperties");
            }
        }

        public CSharpClassFileSetting CSharpClassFileSettings
        {
            get { return _cSharpClassFileSettings; }
            set
            {
                _cSharpClassFileSettings = value;
                OnPropertyChanged("CSharpClassFileSettings");
            }
        }

        private RelayCommand _addClassPropertyCommand;

        public RelayCommand AddClassPropertyCommand
        {
            get
            {
                if (this._addClassPropertyCommand == null)
                    this._addClassPropertyCommand = new RelayCommand(param => AddClassPropertyCommandMethod());

                return this._addClassPropertyCommand;
            }
        }

        private RelayCommand _addClassPropertiesCommand;

        public RelayCommand AddClassPropertiesCommand
        {
            get
            {
                if (this._addClassPropertiesCommand == null)
                    this._addClassPropertiesCommand = new RelayCommand(param => AddClassPropertiesCommandMethod());

                return this._addClassPropertiesCommand;
            }
        }

        private RelayCommand _editClassPropertiesCommand;

        public RelayCommand EditClassPropertiesCommand
        {
            get
            {
                if (this._editClassPropertiesCommand == null)
                    this._editClassPropertiesCommand = new RelayCommand(param => EditClassPropertiesCommandMethod(param));

                return this._editClassPropertiesCommand;
            }
        }

        public CSharpClass()
        {
            ResetAllProperties(); 
            GetDefaultCSharpClassFileSettings();
        }

        public CSharpClass(CSharpClassFileSetting ParamCSharpClassFileSettings)
        {
            CSharpClassFileSettings = ParamCSharpClassFileSettings;
        }

        void UpdateClassProperties()
        {
            if (ClassProperties != null && ClassProperties.Count > 0)
            {
                ClassProperties.ForEach(prop => prop.IsXmlAttribute = IsXmlAttribute);
                ClassProperties.ForEach(prop => prop.IsXmlElement = IsXmlElement);
                ClassProperties.ForEach(prop => prop.IsXmlText = IsXmlText);
            }
        }

        public void GenerateCSharpClassFile(bool IsUseDefaultSettings)
        {
            if (IsUseDefaultSettings == true)
                GetDefaultCSharpClassFileSettings();

            ClassName = CSharpClassFileSettings.IsClassNameCamelCasing ? ClassName.ToCamelCase(CSharpClassFileSettings.Separator) : ClassName;

            StringBuilder codeFileData = new StringBuilder();

            if (CSharpClassFileSettings.IsIncludeUsings)
            {
                codeFileData.AppendLine("using System;");
                codeFileData.AppendLine("using System.Collections.Generic;");
                codeFileData.AppendLine("using System.Linq;");
                codeFileData.AppendLine("using System.Text;");

                if (CSharpClassFileSettings.IsSerializable || CSharpClassFileSettings.IsXmlRoot)
                    codeFileData.AppendLine("using System.Xml.Serialization;");

                if (CSharpClassFileSettings.UserDefinedUsings != null && CSharpClassFileSettings.UserDefinedUsings.Count > 0)
                {
                    foreach (string udUsing in CSharpClassFileSettings.UserDefinedUsings)
                    {
                        codeFileData.AppendLine(udUsing);
                    }
                }
                codeFileData.AppendLine("");
            }

            if (CSharpClassFileSettings.IsIncludeNameSpace)
            {
                codeFileData.AppendLine("namespace " + CSharpClassFileSettings.NameSpace);
                codeFileData.AppendLine("{");
            }

            if (CSharpClassFileSettings.IsSerializable)
            {
                codeFileData.AppendLine("\t[Serializable]");
            }

            if (CSharpClassFileSettings.IsXmlRoot)
            {
                codeFileData.AppendLine("\t[XmlRoot(ElementName = \"" + ClassName.ToLower() + "\")]");
            }

            if (IsIncludetOnPropertyChanged)
                codeFileData.AppendLine("\tpublic class " + ClassName + " : ViewModelBase");
            else
                codeFileData.AppendLine("\tpublic class " + ClassName + "");
            codeFileData.AppendLine("\t{");

            if (ClassProperties != null && ClassProperties.Count > 0)
            {
                if (IsIncludePrivateField)
                {
                    foreach (var prop in ClassProperties)
                    {
                        string proptype = prop.PropType;

                        if (prop.IsGenericListType)
                        {
                            if (!string.IsNullOrEmpty(prop.GenericListType))
                            {
                                proptype = prop.GenericListType + "<" + prop.PropType + ">";
                            }
                        }
                        codeFileData.AppendLine("\t\tprivate " + proptype + " _" + prop.PropName.FirstCharToLower() + ";");
                    }
                }

                foreach (ClassProperty prop in ClassProperties)
                {
                    string proptype = prop.PropType;

                    if (prop.IsGenericListType)
                    {
                        if (!string.IsNullOrEmpty(prop.GenericListType))
                        {
                            proptype = prop.GenericListType + "<" + prop.PropType + ">";
                        }
                    }
                    if (IsIncludePrivateField)
                    {
                        
                        if (prop.IsXmlAttribute)
                        {
                            codeFileData.AppendLine("\t\t[XmlAttribute(AttributeName = \"" + prop.PropName.ToLower() + "\")]");
                        }
                        if (prop.IsXmlElement)
                        {
                            codeFileData.AppendLine("\t\t[XmlElement(ElementName = \"" + prop.PropName.ToLower() + "\")]");
                        }
                        if (prop.IsXmlText)
                        {
                            codeFileData.AppendLine("\t\t[XmlText]");
                        }
                        codeFileData.AppendLine("\t\tpublic " + proptype + " " + prop.PropName);
                        codeFileData.AppendLine("\t\t{");
                        codeFileData.AppendLine("\t\t\tget { return _" + prop.PropName.FirstCharToLower() + "; }");
                        codeFileData.AppendLine("\t\t\tset");
                        codeFileData.AppendLine("\t\t\t{");
                        codeFileData.AppendLine("\t\t\t\t_" + prop.PropName.FirstCharToLower() + " = value;");
                        if (IsIncludetOnPropertyChanged)
                            codeFileData.AppendLine("\t\t\t\tOnPropertyChanged(\"" + prop.PropName + "\");");

                        codeFileData.AppendLine("\t\t\t}");
                        codeFileData.AppendLine("\t\t}");
                        codeFileData.AppendLine("");
                    }
                    else
                    {
                        if (prop.IsXmlAttribute)
                        {
                            codeFileData.AppendLine("\t\t[XmlAttribute(AttributeName = \"" + prop.PropName.ToLower() + "\")]");
                        }
                        if (prop.IsXmlElement)
                        {
                            codeFileData.AppendLine("\t\t[XmlElement(ElementName = \"" + prop.PropName.ToLower() + "\")]");
                        }
                        if (prop.IsXmlText)
                        {
                            codeFileData.AppendLine("\t\t[XmlText]");
                        }
                        codeFileData.AppendLine("\t\tpublic " + proptype + " " + prop.PropName + " { get; set; }");
                    }
                }                
            }

            codeFileData.AppendLine("");

            if (CSharpClassFileSettings.IsIncludeDefaultConstructor)
            {
                codeFileData.AppendLine("\t\tpublic " + ClassName + "()");
                codeFileData.AppendLine("\t\t{");
                codeFileData.AppendLine("\t\t}");
                codeFileData.AppendLine("");
            }

            if (CSharpClassFileSettings.IsIncludeParametrizedConstructor)
            {
                if (CSharpClassFileSettings.Parameters.Count > 0)
                {
                    string parameters = string.Empty;
                    IEnumerable<string> paramList = from p in CSharpClassFileSettings.Parameters
                                                    select p.PropType + " " + p.PropName;

                    parameters = String.Join(",", paramList);

                    codeFileData.AppendLine("\t\tpublic " + ClassName + "(" + parameters + ")");
                    codeFileData.AppendLine("\t\t{");
                    codeFileData.AppendLine("\t\t\t\\To do Implementation");
                    codeFileData.AppendLine("\t\t}");
                    codeFileData.AppendLine("");
                }
            }
            
            if (CSharpClassFileSettings.IsAdditionalCodeSnippet)
            {
                codeFileData.AppendLine(CSharpClassFileSettings.AdditionalCodeSnippet);
                codeFileData.AppendLine("");
            }

            codeFileData.AppendLine("\t}");

            if (CSharpClassFileSettings.IsIncludeNameSpace)
            {
                codeFileData.AppendLine("}");
            }


            string filePath = Path.Combine(CSharpClassFileSettings.CSharpFilePath, ClassName);
            filePath = filePath + "." + CSharpClassFileSettings.CSharpFileExtension;
            File.WriteAllText(filePath, codeFileData.ToString());
            //GetDefaultCSharpClassFileSettings();
            //ResetAllProperties();
        }

        public void ResetAllProperties()
        {
            IsIncludetOnPropertyChanged = false;
            IsIncludePrivateField = false;
            ClassName = default(string);
            ClassProperties = new ObservableCollection<ClassProperty>();
        }

        private void GetDefaultCSharpClassFileSettings()
        {
            this.CSharpClassFileSettings = new CSharpClassFileSetting();
        }

        private void AddClassPropertyCommandMethod()
        {
            if (string.IsNullOrEmpty(ClassPropertyObj.Id))
            {
                ClassPropertyObj.Id = _lib.GetUniqueIdentifier(new UniqueIdentifierSetting() { UniqueIDType = UniqueIDType.TimeStamp });
                ClassProperties.Add(ClassPropertyObj);
            }
            else
            {
                ClassProperty updateClass = ClassProperties.ToList().Find(c => c.Id == ClassPropertyObj.Id);
                updateClass = ClassPropertyObj;
            }
            WinClassPropertyView.Close();
        }

        private void AddClassPropertiesCommandMethod()
        {
            WinClassPropertyView = new Window();
            ClassPropertyView ClassDetailsViewObj = new ClassPropertyView();
            ClassPropertyObj = new ClassProperty();
            ClassDetailsViewObj.DataContext = this;
            WinClassPropertyView.Content = ClassDetailsViewObj;
            WinClassPropertyView.WindowState = WindowState.Normal;
            WinClassPropertyView.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            WinClassPropertyView.ResizeMode = ResizeMode.NoResize;
            WinClassPropertyView.Height = 330;
            WinClassPropertyView.Width = 500;
            WinClassPropertyView.ShowDialog();
        }

        private void EditClassPropertiesCommandMethod(object param)
        {
            if (param != null)
            {
                WinClassPropertyView = new Window();
                ClassPropertyView ClassDetailsViewObj = new ClassPropertyView();
                ClassPropertyObj = (ClassProperty)(param);
                ClassPropertyObj.PropAddUpdateText = "Update";
                ClassDetailsViewObj.DataContext = this;
                WinClassPropertyView.Content = ClassDetailsViewObj;
                WinClassPropertyView.WindowState = WindowState.Normal;
                WinClassPropertyView.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                WinClassPropertyView.ResizeMode = ResizeMode.NoResize;
                WinClassPropertyView.Height = 330;
                WinClassPropertyView.Width = 500;
                WinClassPropertyView.ShowDialog();
            }
        }
    }
}
