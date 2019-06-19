using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace RD.CodeGenerator.View
{
    /// <summary>
    /// Interaction logic for CSharpClassGeneratorView.xaml
    /// </summary>
    public partial class CSharpClassGeneratorView : UserControl
    {
        public CSharpClassGeneratorView()
        {
            InitializeComponent();
            CoreEvents.ClearTextFieldEvent += new EventHandler<ClearTextFieldEventArg>(CoreEvents_ClearTextFieldEvent);
        }

        void CoreEvents_ClearTextFieldEvent(object sender, ClearTextFieldEventArg e)
        {
            txtUserDefinedUsings.Text = string.Empty;
        }
    }
}
