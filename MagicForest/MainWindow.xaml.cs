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

namespace MagicForest
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private static int m_iForestSize = 3;
        private static ForestCell[,] m_afcForest = null;

        public static int ForestSize
        {
            get
            {
                return m_iForestSize;
            }
        }

        public static ForestCell[,] Forest
        {
            get
            {
                return m_afcForest;
            }
        }

        public MainWindow()
        {
            InitializeComponent();
            m_afcForest = new ForestCell[m_iForestSize, m_iForestSize];
        }

        public static void StopExecution()
        {
            // STOP EXECUTION : griser bouton + messagebox
        }
    }
}
