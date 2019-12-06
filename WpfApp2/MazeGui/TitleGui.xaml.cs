using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MazeRunnerWPF.Controller;

namespace MazeRunnerWPF.MazeGui
{
    /// <summary>
    /// Interaction logic for TitleGui.xaml
    /// </summary>
    public partial class TitleGui : Page, IGuiPage
    {
        public TitleGui()
        {
            InitializeComponent();
        }

        public void OnShown(object passingObj)
        {
        }

        public void OnDisappeared()
        {
        }

        private void btnStartGame1_Click(object sender, RoutedEventArgs e)
        {
            btnStartGame2.Visibility = lblPrompt.Visibility = cbDifficulty.Visibility = Visibility.Visible;
            btnStartGame1.Visibility = Visibility.Hidden;
        }

        private void btnStartGame2_Click(object sender, RoutedEventArgs e)
        {
            //  if (SAVEGAME_WAS_FOUND) // TODO
            {
                var window = Window.GetWindow(this);
                if (MessageBox.Show(
                    window,
                    "Load previous save game?",
                    "Saved Game Detected",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    // TODO: load savegame here
                }
            }
            GuiMediator.Instance.ShowMazeGui(cbDifficulty.SelectedIndex);
        }
    }
}
