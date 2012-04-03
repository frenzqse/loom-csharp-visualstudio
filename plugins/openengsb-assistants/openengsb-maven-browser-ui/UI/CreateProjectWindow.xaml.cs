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
using System.Windows.Shapes;
using Org.OpenEngSB.Loom.Csharp.VisualStudio.Plugins.Assistants.Common;

namespace Org.OpenEngSB.Loom.Csharp.VisualStudio.Plugins.Assistants.UI
{
    /// <summary>
    /// Interaction logic for CreateProjectWindow.xaml
    /// </summary>
    public partial class CreateProjectWindow : Window, IWizardStep
    {
        private Wizard _wizard { get; set; }

        public CreateProjectWindow(Wizard wizard)
        {
            InitializeComponent();
            _wizard = wizard;
            referenceItems();
        }

        private void referenceItems()
        {
            foreach (Item i in _wizard.Configuration.Items)
            {
                _wizard.Configuration.ProjectReferences.Add(i.Path);
            }
        }

        public bool DoStep()
        {
            this.ShowDialog();

            if (this.DialogResult.HasValue)
                return this.DialogResult.Value;

            return false;
        }

        private void button_add_Click(object sender, RoutedEventArgs e)
        {

        }

        private void button_remove_Click(object sender, RoutedEventArgs e)
        {

        }

        private void button_finish_Click(object sender, RoutedEventArgs e)
        {
            if (textBox_solution.Text == string.Empty)
            {
                MessageBox.Show("Please enter a name for the solution.");
                return;
            }

            if (textBox_project.Text == string.Empty)
            {
                MessageBox.Show("Please enter a name for the solution.");
                return;
            }

            _wizard.Configuration.ProjectName = textBox_project.Text;
            _wizard.Configuration.SolutionName = textBox_solution.Text;
            _wizard.CreateSolution();
        }
    }
}
