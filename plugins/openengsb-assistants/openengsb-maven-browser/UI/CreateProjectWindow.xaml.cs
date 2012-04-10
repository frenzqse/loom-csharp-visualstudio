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
using System.Collections.ObjectModel;
using System.Windows.Forms;
using Org.OpenEngSB.Loom.Csharp.VisualStudio.Plugins.Assistants.Common;

namespace Org.OpenEngSB.Loom.Csharp.VisualStudio.Plugins.Assistants.UI
{
    /// <summary>
    /// Interaction logic for CreateProjectWindow.xaml
    /// </summary>
    public partial class CreateProjectWindow : Window, IWizardStep
    {
        public Wizard SolutionWizard { get; set; }
        public ObservableCollection<string> ProjectReferences { get; set; }

        private IWizardStep _nextStep;

        public CreateProjectWindow(Wizard wizard)
        {
            InitializeComponent();
            SolutionWizard = wizard;
            DataContext = this;
            _nextStep = null;
            ProjectReferences = new ObservableCollection<string>();
        }

        private void referenceItems()
        {
            foreach (Item i in SolutionWizard.Configuration.Items)
            {
                ProjectReferences.Add(i.Path);
            }
        }

        public void DoStep()
        {
            referenceItems();
            this.ShowDialog();

            if (this._nextStep == null)
                return;

            if (this.DialogResult.HasValue && this.DialogResult.Value == true)
                _nextStep.DoStep();
        }

        public void SetNextStep(IWizardStep step)
        {
            _nextStep = step;
        }
        private void button_add_Click(object sender, RoutedEventArgs e)
        {
        }

        private void button_remove_Click(object sender, RoutedEventArgs e)
        {
            ProjectReferences.Remove((string)listBox_files.SelectedItem);
        }

        private void button_finish_Click(object sender, RoutedEventArgs e)
        {
            if (textBox_solution.Text == string.Empty)
            {
                System.Windows.MessageBox.Show("Please enter a name for the solution.");
                return;
            }

            if (textBox_project.Text == string.Empty)
            {
                System.Windows.MessageBox.Show("Please enter a name for the solution.");
                return;
            }

            SolutionWizard.Configuration.ProjectReferences = new List<string>(ProjectReferences);
            SolutionWizard.Configuration.ProjectName = textBox_project.Text;
            SolutionWizard.Configuration.SolutionName = textBox_solution.Text;
            SolutionWizard.CreateSolution();
            Close();
        }

        private void button_browse_Click(object sender, RoutedEventArgs e)
        {
            FolderBrowserDialog dia = new FolderBrowserDialog();
            DialogResult result = dia.ShowDialog();

            if (result != System.Windows.Forms.DialogResult.OK)
                return;

            SolutionWizard.Configuration.SolutionDirectory = dia.SelectedPath;
            label_destination.Content = SolutionWizard.Configuration.SolutionDirectory;
        }
    }
}
