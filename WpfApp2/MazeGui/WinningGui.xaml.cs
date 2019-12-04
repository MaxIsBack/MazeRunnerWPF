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
    /// Interaction logic for WinningGui.xaml
    /// </summary>
    public partial class WinningGui : Page, IGuiPage
    {
        public WinningGui()
        {
            InitializeComponent();
        }

        public void OnDisappeared()
        {
        }

        public void OnShown(object passingObj)
        {
            lblScore.Content = "Score: " + new Random().Next(10000);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            GuiMediator.Instance.ShowMazeGui(null);
        }
    }
}
