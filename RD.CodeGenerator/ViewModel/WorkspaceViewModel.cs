using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RDCore.WPF.Common.UserCode;
using System.Windows;
using System.Windows.Input;

namespace RD.CodeGenerator.ViewModel
{
    public abstract class WorkspaceViewModel : ViewModelBase
    {
        #region Fields

        RelayCommand _closeCommand;
        Visibility _closeBtnVisibility = Visibility.Visible;
        RelayCommand _addNewTabCommand;
        Visibility _addNewTabVisibility = Visibility.Collapsed;
        //WorkspaceViewModel _activeWorkspace;

        #endregion // Fields

        #region Constructor

        protected WorkspaceViewModel()
        {
        }

        #endregion // Constructor

        #region CloseCommand

        /// <summary>
        /// Returns the command that, when invoked, attempts
        /// to remove this workspace from the user interface.
        /// </summary>
        public ICommand CloseCommand
        {
            get
            {
                if (_closeCommand == null)
                    _closeCommand = new RelayCommand(param => this.OnRequestClose());

                return _closeCommand;
            }
        }

        #endregion // CloseCommand

        public RelayCommand AddNewTabCommand
        {
            get
            {
                if (_addNewTabCommand == null)
                    _addNewTabCommand = new RelayCommand(param => this.AddNewTabCommandMethod());

                return _addNewTabCommand;
            }
        }
        public event EventHandler AddNewTab;
        public void AddNewTabCommandMethod()
        {
            EventHandler handler = this.AddNewTab;
            if (handler != null)
                handler(this, EventArgs.Empty);
        }
        #region RequestClose [event]

        /// <summary>
        /// Raised when this workspace should be removed from the UI.
        /// </summary>
        public event EventHandler RequestClose;

        public void OnRequestClose()
        {
            EventHandler handler = this.RequestClose;
            if (handler != null)
                handler(this, EventArgs.Empty);
        }

        #endregion // RequestClose [event]

        #region OnTileReques

        public event EventHandler TileRequest;

        public void onTileRequest()
        {
            EventHandler tileHandler = this.TileRequest;
            if (tileHandler != null)
                tileHandler(this, EventArgs.Empty);
        }


        private Visibility _ButtonVisible = Visibility.Collapsed;

        public Visibility ButtonVisible
        {
            get { return _ButtonVisible; }
            set
            {
                _ButtonVisible = value;
                this.OnPropertyChanged("ButtonVisible");
            }
        }

        public void SetButtonVisible()
        {
            ButtonVisible = Visibility.Visible;
        }

        public void SetButtonHidden()
        {
            ButtonVisible = Visibility.Collapsed;
        }
        #endregion

        public Visibility CloseBtnVisibility
        {
            get
            {
                if (this.DisplayName == "")
                    return Visibility.Collapsed;
                else
                    return this._closeBtnVisibility;
            }
        }
        public Visibility AddNewTabVisibility
        {
            get
            {
                if (this.DisplayName == "")
                    return Visibility.Visible;
                else
                    return this._addNewTabVisibility;
            }
        }

        #region Toolbar add/remove button commands
        RelayCommand _toolbarAddCommand;
        public RelayCommand ToolbarAddCommand
        {
            get
            {
                if (_toolbarAddCommand == null)
                    _toolbarAddCommand = new RelayCommand(param => this.ToolbarAddCommandMethod());

                return _toolbarAddCommand;
            }
        }
        protected virtual void ToolbarAddCommandMethod()
        {

        }

        RelayCommand _toolbarRemoveCommand;
        public RelayCommand ToolbarRemoveCommand
        {
            get
            {
                if (_toolbarRemoveCommand == null)
                    _toolbarRemoveCommand = new RelayCommand(param => this.ToolbarRemoveCommandMethod());

                return _toolbarRemoveCommand;
            }
        }
        protected virtual void ToolbarRemoveCommandMethod()
        {

        }
        #endregion Toolbar add/remove button commands

    }
}
