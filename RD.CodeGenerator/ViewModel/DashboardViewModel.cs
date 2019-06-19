using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using RDCore.WPF.Common.UserCode;
using System.ComponentModel;
using RD.CodeGenerator.Model;
using System.Windows.Data;

namespace RD.CodeGenerator.ViewModel
{
    public class DashboardViewModel : WorkspaceViewModel
    {
        #region FIELDS
        private ObservableCollection<WorkspaceViewModel> _workSpaces;
        private ReadOnlyCollection<ViewModelItem> _workflowCommands;
        private ViewModelItem _selectedViewModelItem = null;

        ICollectionView collectionView;
        WorkspaceViewModel _activeWorkspace;
        private TileItemModel _dataTileItemModel;

        #endregion

        #region PROPERTIES
        public TileItemModel Data1TileItemModel { get; set; }
        public TileItemModel Data2TileItemModel { get; set; }
        public TileItemModel Data3TileItemModel { get; set; }
        public TileItemModel Data4TileItemModel { get; set; }
        public TileItemModel Data5TileItemModel { get; set; }
        public TileItemModel Data6TileItemModel { get; set; }

        public TileItemModel DataTileItemModel
        {
            get { return _dataTileItemModel; }
            set
            {
                _dataTileItemModel = value;
                OnPropertyChanged("DataTileItemModel");
            }
        }

        public ViewModelItem SelectedViewModelItem
        {
            get { return _selectedViewModelItem; }
            set
            {
                _selectedViewModelItem = value;
                OnPropertyChanged("SelectedViewModelItem");
            }
        }

        public ReadOnlyCollection<ViewModelItem> WorkflowCommands
        {
            get
            {
                if (_workflowCommands == null)
                {
                    List<ViewModelItem> cmds = this.CreateWorkflowCommands();
                    _workflowCommands = new ReadOnlyCollection<ViewModelItem>(cmds);
                }
                return _workflowCommands;
            }
        }

        public ObservableCollection<WorkspaceViewModel> WorkSpaces
        {
            get
            {
                if (_workSpaces == null)
                {
                    _workSpaces = new ObservableCollection<WorkspaceViewModel>();
                    _workSpaces.CollectionChanged += new System.Collections.Specialized.NotifyCollectionChangedEventHandler(OnWorkspacesChanged);
                }
                return _workSpaces;
            }
            set
            {
                _workSpaces = value;
                OnPropertyChanged("WorkSpaces");
            }
        }

        #endregion

        #region CONSTRUCTOR
        public DashboardViewModel()
        {
            DataTileItemModel = new TileItemModel() { Name = "32", Color = "#FF4DAEB5", Header = "Production", CanSlide = true };
            Data1TileItemModel = new TileItemModel() { Name = "44", Color = "#819c79", Header = "Submission", CanSlide = true };
            Data2TileItemModel = new TileItemModel() { Name = "367", Color = "#f6766d", Header = "Groups", CanSlide = true };
            Data3TileItemModel = new TileItemModel() { Name = "197", Color = "#9214ff", Header = "Advisor", CanSlide = true };
            Data4TileItemModel = new TileItemModel() { Name = "38", Color = "#5ea722", Header = "User", CanSlide = true };
            Data5TileItemModel = new TileItemModel() { Name = "2", Color = "#6600ff", Header = "BCC", CanSlide = true };
            Data6TileItemModel = new TileItemModel() { Name = "12", Color = "#cccc33", Header = "Project", CanSlide = true };

        }
        #endregion

        #region METHODS

        void OnWorkspacesChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null && e.NewItems.Count != 0)
                foreach (WorkspaceViewModel workspace in e.NewItems)
                {
                    workspace.RequestClose += this.OnWorkspaceRequestClose;
                    workspace.AddNewTab += this.workspace_AddNewTab;
                    //if (collectionView != null)
                    //    collectionView.CurrentChanged += new EventHandler(collectionView_CurrentChanged);
                }
            if (e.OldItems != null && e.OldItems.Count != 0)
                foreach (WorkspaceViewModel workspace in e.OldItems)
                {
                    workspace.RequestClose -= this.OnWorkspaceRequestClose;
                    //collectionView.CurrentChanged -= new EventHandler(collectionView_CurrentChanged);
                    //workspace.SelectionChanged -= this.collectionView_CurrentChanged;
                }
        }

        void workspace_AddNewTab(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// This method trigger when the workspace got closed...
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void OnWorkspaceRequestClose(object sender, EventArgs e)
        {
            WorkspaceViewModel workspace = sender as WorkspaceViewModel;
            //if (workspace.GetType() == typeof(MnaMainViewModel))
            //{
            //    var result = MessageBox.Show ("You want to close Home Deal","MnA", MessageBoxButton.YesNo,MessageBoxImage.Information);
            //    if (result == MessageBoxResult.No)
            //    {
            //        return; 
            //    }
            //}
            workspace.Dispose();
            this.WorkSpaces.Remove(workspace);

            workspace = null;

            GC.Collect();

            var tabsCount = this.WorkSpaces.Count(tab => tab.DisplayName != "");
            if (tabsCount == 0)
            //if (Workspaces.Count == 0)
            {
                this.WorkSpaces.Clear();
                base.OnRequestClose();
            }
            else
            {
                //TODO: Set the correct tab to be active.
                this.SetActiveWorkspace(this.WorkSpaces[tabsCount - 1]);
            }
        }

        private List<ViewModelItem> CreateWorkflowCommands()
        {
            return new List<ViewModelItem>
            {
                new ViewModelItem("C# Class Generator",new RelayCommand(param => this.Workflow_CSharpClassGeneratorView(param))),
                new ViewModelItem("C# Bulk Class Generator",new RelayCommand(param => this.Workflow_CSharpBulkClassGeneratorView(param))),
                new ViewModelItem("Add Production Tasks",new RelayCommand(param => this.Workflow_AddProdTasks())),
                new ViewModelItem("Add BCC Tasks",new RelayCommand(param => this.Workflow_ProdTasksOverview())),
                new ViewModelItem("Submission tasks overview",new RelayCommand(param => this.Workflow_ProdTasksOverview())),
                new ViewModelItem("Submission Advisor overview",new RelayCommand(param => this.Workflow_ProdTasksOverview())),
                new ViewModelItem("Add User Details",new RelayCommand(param => this.Workflow_ProdTasksOverview())),
                new ViewModelItem("Build/Cleanse/Check Tasks Overview",new RelayCommand(param => this.Workflow_ProdTasksOverview())),   
                new ViewModelItem("User And Group Management",new RelayCommand(param => this.Workflow_ProdTasksOverview())),
                new ViewModelItem("BCC Project Details",new RelayCommand(param => this.Workflow_ProdTasksOverview()))
            };
        }

        private void Workflow_CSharpClassGeneratorView(object param)
        {
            CSharpClassGeneratorViewModel _cSharpClassGeneratorViewModel = new CSharpClassGeneratorViewModel();
            this.WorkSpaces.Insert(this.WorkSpaces.Count, _cSharpClassGeneratorViewModel);
            this.SetActiveWorkspace(_cSharpClassGeneratorViewModel);
        }

        private void Workflow_CSharpBulkClassGeneratorView(object param)
        {
            CSharpBulkClassGeneratorViewModel _cSharpBulkClassGeneratorViewModel = new CSharpBulkClassGeneratorViewModel();
            this.WorkSpaces.Insert(this.WorkSpaces.Count, _cSharpBulkClassGeneratorViewModel);
            this.SetActiveWorkspace(_cSharpBulkClassGeneratorViewModel);
        }

        private void Workflow_RandDView()
        {
            //RandDViewViewModel _randDViewViewModel = new CSharpBulkClassGeneratorViewModel();

            //this.WorkSpaces.Insert(this.WorkSpaces.Count, _randDViewViewModel);
            //this.SetActiveWorkspace(_randDViewViewModel);
        }

        private void Workflow_AddProdTasks()
        {
            //AddProductionTaskViewModel _AddProductionTaskViewModel = new AddProductionTaskViewModel(WorkSpaces);
            //this.WorkSpaces.Insert(this.WorkSpaces.Count, _AddProductionTaskViewModel);
            //this.SetActiveWorkspace(_AddProductionTaskViewModel);
        }

        private void Workflow_ProdTasksOverview()
        {
            //ProductionTasksOverviewViewModel _ProductionTasksOverviewViewModel = new ProductionTasksOverviewViewModel();

            //this.WorkSpaces.Insert(this.WorkSpaces.Count, _ProductionTasksOverviewViewModel);
            //this.SetActiveWorkspace(_ProductionTasksOverviewViewModel);
        }

        public void SetActiveWorkspace(WorkspaceViewModel workspace)
        {
            collectionView = CollectionViewSource.GetDefaultView(this.WorkSpaces);

            if (collectionView != null)
                collectionView.MoveCurrentTo(workspace);

            _activeWorkspace = workspace;
        }
        #endregion
    }
}
