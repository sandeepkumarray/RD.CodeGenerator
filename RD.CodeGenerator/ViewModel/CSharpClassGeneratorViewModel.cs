using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RDCore.WPF.Common.UserCode;
using RD.CodeGenerator.Model;
using System.Windows.Controls;
using System.Windows;
using System.Collections.ObjectModel;

namespace RD.CodeGenerator.ViewModel
{
    public class CSharpClassGeneratorViewModel : WorkspaceViewModel
    {
        private CSharpClassFileModel _cSharpClassFileModel = null;
        private ContextMenu _lBClassPropertiesContextMenu = null;
        private bool _isUseDefaultSettings = false;
        private Visibility _settingsVisibility = Visibility.Hidden;
        private bool _isXmlElement = false;
        private bool _isXmlAttribute = false;
        private bool _isXmlText = false;

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

        public Visibility SettingsVisibility
        {
            get { return _settingsVisibility; }
            set
            {
                _settingsVisibility = value;
                OnPropertyChanged("SettingsVisibility");
            }
        }

        public bool IsUseDefaultSettings
        {
            get { return _isUseDefaultSettings; }
            set
            {
                _isUseDefaultSettings = value;
                if (value == false)
                    SettingsVisibility = Visibility.Visible;
                else
                    SettingsVisibility = Visibility.Hidden;
                OnPropertyChanged("IsUseDefaultSettings");
            }
        }

        public ContextMenu LBClassPropertiesContextMenu
        {
            get { return _lBClassPropertiesContextMenu; }
            set
            {
                _lBClassPropertiesContextMenu = value;
                OnPropertyChanged("LBClassPropertiesContextMenu");
            }
        }

        public CSharpClassFileModel CSharpClassFileModelObj
        {
            get { return _cSharpClassFileModel; }
            set
            {
                _cSharpClassFileModel = value;
                OnPropertyChanged("CSharpClassFileModel");
            }
        }

        public CSharpClassGeneratorViewModel()
        {
            this.DisplayName = "C# Class Generator";
            LBClassPropertiesContextMenu = CreateListBoxContextMenu("Paste");
            CSharpClassFileModelObj = new CSharpClassFileModel();
            IsUseDefaultSettings = true;
        }

        public CSharpClassGeneratorViewModel(string ClassName)
            : this()
        {
            CSharpClassFileModelObj = new CSharpClassFileModel(ClassName);
        }

        private RelayCommand _cSharpClassFileCommand;

        public RelayCommand CSharpClassFileCommand
        {
            get
            {
                if (this._cSharpClassFileCommand == null)
                    this._cSharpClassFileCommand = new RelayCommand(param => CSharpClassFileModelObj.GenerateCSharpClassFile(IsUseDefaultSettings),
                        (param) => CSharpClassFileModelObj.CanGenerateCSharpClassFile());

                return this._cSharpClassFileCommand;
            }
        }

        private RelayCommand _addUserDefinedUsingsCommand;

        public RelayCommand AddUserDefinedUsingsCommand
        {
            get
            {
                if (this._addUserDefinedUsingsCommand == null)
                    this._addUserDefinedUsingsCommand = new RelayCommand(param => CSharpClassFileModelObj.AddUserDefinedUsingsMethod(param));

                return this._addUserDefinedUsingsCommand;
            }
        }

        private RelayCommand _deleteUserDefinedUsingsCommand;

        public RelayCommand DeleteUserDefinedUsingsCommand
        {
            get
            {
                if (this._deleteUserDefinedUsingsCommand == null)
                    this._deleteUserDefinedUsingsCommand = new RelayCommand(param => CSharpClassFileModelObj.DeleteUserDefinedUsingsCommand(param));

                return this._deleteUserDefinedUsingsCommand;
            }
        }

        private ContextMenu CreateListBoxContextMenu(string Action)
        {
            ContextMenu ContextMenuObj = new ContextMenu();
            switch (Action)
            {
                case "Paste":
                    MenuItem pasteItem = new MenuItem();
                    pasteItem.Header = "Paste";
                    pasteItem.Click += new RoutedEventHandler(pasteItem_Click);
                    ContextMenuObj.Items.Add(pasteItem);
                    break;
                default:
                    break;
            }
            return ContextMenuObj;
        }

        void pasteItem_Click(object sender, RoutedEventArgs e)
        {
            var copyData = Clipboard.GetDataObject();
            if (copyData != null)
            {
                var datos = (string)copyData.GetData(DataFormats.Text);
                var stringRows = datos.Split(new Char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
                var table = new List<ClipExcelRow>(stringRows.Length);

                foreach (string stringRow in stringRows)
                {
                    table.Add(new ClipExcelRow(stringRow));
                }

                List<string> lbValues = new List<string>();
                int index = 0;
                CSharpClassFileModelObj.ClassProperties = new ObservableCollection<ClassProperty>();
                foreach (ClipExcelRow cer in table)
                {
                    lbValues.Add(Convert.ToString(cer[index]));
                    ClassProperty classProperty = new ClassProperty(Convert.ToString(cer[index]), "string");
                    CSharpClassFileModelObj.ClassProperties.Add(classProperty);
                }
            }
        }

        void UpdateClassProperties()
        {
            if (CSharpClassFileModelObj.ClassProperties != null && CSharpClassFileModelObj.ClassProperties.Count > 0)
            {
                CSharpClassFileModelObj.ClassProperties.ForEach(prop => prop.IsXmlAttribute = IsXmlAttribute);
                CSharpClassFileModelObj.ClassProperties.ForEach(prop => prop.IsXmlElement = IsXmlElement);
                CSharpClassFileModelObj.ClassProperties.ForEach(prop => prop.IsXmlText = IsXmlText);
            }
        }
    }
}
