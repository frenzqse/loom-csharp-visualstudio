using System.Windows;
using System.Windows.Forms;
using Org.OpenEngSB.Loom.Csharp.VisualStudio.Plugins.Assistants.Common;

namespace Org.OpenEngSB.Loom.Csharp.VisualStudio.Plugins.Assistants.UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class BrowserWindow : Window, IWizardStep
    {
        private Wizard _wizard;

        public BrowserWindow(Wizard wizards)
        {
            InitializeComponent();
            _wizard = wizards;

            label_path.Content = _wizard.Configuration.Path;
        }

        private void button_cancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            Close();
        }

        private void button_download_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
            saveSelectedItems();
            Close();
        }

        public bool DoStep()
        {
            this.ShowDialog();

            if (this.DialogResult.HasValue)
                return this.DialogResult.Value;

            return false;
        }

        private void saveSelectedItems()
        {
            foreach (UIArtifact a in treeView_artifacts.Items)
            {
                foreach (UIItemVersion v in a.Versions)
                {
                    foreach (UIItem i in v.Items)
                    {
                        if (i.IsChecked)
                        {
                            _wizard.Configuration.Items.Add(i.ItemModel);
                        }
                    }
                }
            }
        }

        private void button_browse_Click(object sender, RoutedEventArgs e)
        {
            FolderBrowserDialog dia = new FolderBrowserDialog();
            DialogResult result = dia.ShowDialog();
            
            if (result != System.Windows.Forms.DialogResult.OK)
                return;

            _wizard.Configuration.Path = dia.SelectedPath;
            label_path.Content = _wizard.Configuration.Path;
        }
    }
}
