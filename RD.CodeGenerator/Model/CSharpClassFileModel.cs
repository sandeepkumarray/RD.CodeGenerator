using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using RDCore.WPF.Common.UserCode;
using System.Collections.ObjectModel;

namespace RD.CodeGenerator.Model
{
    public class CSharpClassFileModel : ViewModelBase
    {
        private string _nameSpace = string.Empty;
        private string _className = string.Empty;
        private ObservableCollection<ClassProperty> _classProperties = null;
        private CSharpClassFileSetting _cSharpClassFileSettings = null;
        private bool _isIncludePrivateField = false;
        private bool _isIncludetOnPropertyChanged = false;

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

        public CSharpClassFileModel()
        {
            ClassProperties = new ObservableCollection<ClassProperty>();
            GetDefaultCSharpClassFileSettings();
        }

        public CSharpClassFileModel(string ClassName)
            : this()
        {
            this.ClassName = ClassName;
        }

        public CSharpClassFileModel(string ClassName, CSharpClassFileSetting CSharpClassFileSettings)
            : this(ClassName)
        {
            this.CSharpClassFileSettings = CSharpClassFileSettings;
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
                        codeFileData.AppendLine("\t\tprivate " + prop.PropType + " _" + prop.PropName.FirstCharToLower() + ";");
                    }
                }

                foreach (ClassProperty prop in ClassProperties)
                {
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
                        codeFileData.AppendLine("\t\tpublic " + prop.PropType + " " + prop.PropName);
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
                        codeFileData.AppendLine("\t\tpublic " + prop.PropType + " " + prop.PropName + " { get; set; }");
                    }
                }

                codeFileData.AppendLine("");
            }

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
            GetDefaultCSharpClassFileSettings();
            ResetAllProperties();
        }

        private void ResetAllProperties()
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

        public bool CanGenerateCSharpClassFile()
        {
            if (!string.IsNullOrEmpty(this.ClassName))
                return true;
            else
                return false;
        }

        public void AddUserDefinedUsingsMethod(object param)
        {
            string value = Convert.ToString(param);
            value = ValidateAndFormat(value);

            if (!this.CSharpClassFileSettings.UserDefinedUsings.Contains(value))
                this.CSharpClassFileSettings.UserDefinedUsings.Add(value);
            CoreEvents.OnClearTextFieldEvent(param, new ClearTextFieldEventArg());
        }

        private string ValidateAndFormat(string value)
        {
            string returnvalue = string.Empty;
            string[] allVals = value.Split(' ');
            char lastChar = value[value.Length - 1];

            if (allVals[0].ToLower() != "using")
            {
                value = value.ToCamelCaseWithSeparator('.');
                returnvalue += "using ";
            }
            else
            {
                value = value.Remove(0, "using".Length + 1).ToCamelCaseWithSeparator('.');
                returnvalue += "using ";
            }

            returnvalue += value;

            if (lastChar != ';')
                returnvalue += ";";

            return returnvalue;
        }

        public void DeleteUserDefinedUsingsCommand(object param)
        {
            string value = Convert.ToString(param);
            this.CSharpClassFileSettings.UserDefinedUsings.Remove(value);
        }
    }
}
