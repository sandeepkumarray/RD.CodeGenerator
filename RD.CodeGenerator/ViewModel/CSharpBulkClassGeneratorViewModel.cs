using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows;
using RD.CodeGenerator.Model;
using RDCore.WPF.Common.UserCode;
using System.Collections.ObjectModel;
using RD.CodeGenerator.View;

namespace RD.CodeGenerator.ViewModel
{
    public class CSharpBulkClassGeneratorViewModel : WorkspaceViewModel
    {
        Window WinClassDetailsView = null;
        private ObservableCollection<CSharpClass> _classDatas = null;
        private ContextMenu _lBClassPropertiesContextMenu = null;
        private bool _isUseDefaultSettings = false;
        private Visibility _settingsVisibility = Visibility.Hidden;
        private CSharpClassFileSetting _cSharpClassFileSettings = null;
        private CSharpClass _cSharpClassObj = null;

        public CSharpClass CSharpClassObj
        {
            get { return _cSharpClassObj; }
            set
            {
                _cSharpClassObj = value;
                OnPropertyChanged("CSharpClassObj");
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

        public ObservableCollection<CSharpClass> ClassDatas
        {
            get { return _classDatas; }
            set
            {
                _classDatas = value;
                OnPropertyChanged("ClassDatas");
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

        private RelayCommand _addClassDataCommand;

        public RelayCommand AddClassDataCommand
        {
            get
            {
                if (this._addClassDataCommand == null)
                    this._addClassDataCommand = new RelayCommand(param => AddClassDataCommandMethod());

                return this._addClassDataCommand;
            }
        }

        private RelayCommand _addCSharpClassDataCommand;

        public RelayCommand AddCSharpClassDataCommand
        {
            get
            {
                if (this._addCSharpClassDataCommand == null)
                    this._addCSharpClassDataCommand = new RelayCommand(param => AddCSharpClassDataCommandMethod());

                return this._addCSharpClassDataCommand;
            }
        }

        private RelayCommand _editClassDataCommand;

        public RelayCommand EditClassDataCommand
        {
            get
            {
                if (this._editClassDataCommand == null)
                    this._editClassDataCommand = new RelayCommand(param => EditClassDataCommandMethod(param));

                return this._editClassDataCommand;
            }
        }

        private RelayCommand _cSharpBulkClassFileCommand;

        public RelayCommand CSharpBulkClassFileCommand
        {
            get
            {
                if (this._cSharpBulkClassFileCommand == null)
                    this._cSharpBulkClassFileCommand = new RelayCommand(param => CSharpBulkClassFileCommandMethod());

                return this._cSharpBulkClassFileCommand;
            }
        }

        public CSharpBulkClassGeneratorViewModel()
        {
            ClassDatas = new ObservableCollection<CSharpClass>();
            this.DisplayName = "C# Bulk Class Generator";
            CSharpClassObj = new CSharpClass();
            IsUseDefaultSettings = true;
            CSharpClassFileSettings = new CSharpClassFileSetting();
            LBClassPropertiesContextMenu = CreateListBoxContextMenu("Paste");
        }

        private void AddCSharpClassDataCommandMethod()
        {
            if (string.IsNullOrEmpty(CSharpClassObj.Id))
            {
                CSharpClassObj.Id = _lib.GetUniqueIdentifier(new UniqueIdentifierSetting() { UniqueIDType = UniqueIDType.TimeStamp });
                ClassDatas.Add(CSharpClassObj);
            }
            else
            {
                CSharpClass updateClass = ClassDatas.ToList().Find(c => c.Id == CSharpClassObj.Id);
                updateClass = CSharpClassObj;
            }
            WinClassDetailsView.Close();
        }

        private void AddClassDataCommandMethod()
        {
            WinClassDetailsView = new Window();
            ClassDetailsView ClassDetailsViewObj = new ClassDetailsView();
            CSharpClassObj = new CSharpClass();
            ClassDetailsViewObj.DataContext = this;
            WinClassDetailsView.Content = ClassDetailsViewObj;
            WinClassDetailsView.WindowState = WindowState.Normal;
            WinClassDetailsView.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            WinClassDetailsView.ResizeMode = ResizeMode.NoResize;
            WinClassDetailsView.Height = 600;
            WinClassDetailsView.Width = 500;
            WinClassDetailsView.ShowDialog();
        }

        private void EditClassDataCommandMethod(object param)
        {
            if (param != null)
            {
                WinClassDetailsView = new Window();
                ClassDetailsView ClassDetailsViewObj = new ClassDetailsView();
                CSharpClassObj = (CSharpClass)(param);
                CSharpClassObj.ClassAddUpdateText = "Update";
                ClassDetailsViewObj.DataContext = this;
                WinClassDetailsView.Content = ClassDetailsViewObj;
                WinClassDetailsView.WindowState = WindowState.Normal;
                WinClassDetailsView.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                WinClassDetailsView.ResizeMode = ResizeMode.NoResize;
                WinClassDetailsView.Height = 600;
                WinClassDetailsView.Width = 500;
                WinClassDetailsView.ShowDialog();
            }
        }

        private void CSharpBulkClassFileCommandMethod()
        {
            ClassDatas.ForEach(cd => cd.GenerateCSharpClassFile(IsUseDefaultSettings));
            ClassDatas = new ObservableCollection<CSharpClass>();
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
                CSharpClassObj.ClassProperties = new ObservableCollection<ClassProperty>();
                foreach (ClipExcelRow cer in table)
                {
                    lbValues.Add(Convert.ToString(cer[index]));
                    ClassProperty classProperty = new ClassProperty(Convert.ToString(cer[index]), "string");
                    classProperty.Id = _lib.GetUniqueIdentifier(new UniqueIdentifierSetting() { UniqueIDType = UniqueIDType.TimeStamp });
                    CSharpClassObj.ClassProperties.Add(classProperty);
                }
            }
        }

    }
}
