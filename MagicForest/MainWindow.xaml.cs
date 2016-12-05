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
        private static Hero steveTheHero = null;

        public static int ForestSize
        {
            get
            {
                return m_iForestSize;
            }
            set
            {
                m_iForestSize = value;
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
            Grid forestGrid = new Grid();
            
            for (int i = 0; i < 9; i++)
            {
                forestGrid.RowDefinitions.Add(new RowDefinition());
                forestGrid.ColumnDefinitions.Add(new ColumnDefinition());
            }
            int a = 2;
            //Grid.SetColumn(_ListOfCells[k], j);
            //_SudokuGrid.Children.Add(_ListOfCells[k]);
            //PopulateForest(m_afcForest);
        }

        /// <summary>
        /// Populates a given forest with a portal, a hero, monsters and holes
        /// </summary>
        /// <param name="pForest">The forest to populate, as a 2-dimensional array of ForestCell</param>
        public void PopulateForest(ForestCell[,] pForest)
        {
            Random random = new Random();
            int portalRow = random.Next(0, m_iForestSize + 1);
            int portalColumn = random.Next(0, m_iForestSize + 1);
            pForest[portalRow, portalColumn].HasPortal = true;

            steveTheHero.CurrentCell = pForest[0, 0];

            for (int i = 0; i < m_iForestSize; i++)
            {
                for (int j = 0; j < m_iForestSize; j++)
                {
                    if (pForest[i, j].HasNothing == true)
                    {
                        PopulateCellRandomly(pForest[i, j]);
                    }
                }
            }
        }

        /// <summary>
        /// Randomly puts either a monster, a hole, or nothing on the input cell
        /// Also updates the neighboring cells with either smell or wind accordingly
        /// </summary>
        /// <param name="pCell">The input cell to populate</param>
        public void PopulateCellRandomly(ForestCell pCell)
        {
            List<ForestCell> neighborCells = null;

            // Generates a random number between 0 (included) and 100 (excluded)
            Random random = new Random();
            int die = random.Next(0,100);
            // There is 15% of chance for the generated number to be in the following range
            if ((die >= 0) && (die < 15))
            {
                pCell.HasMonster = true;
                neighborCells = pCell.getAdjacentCells();
                for (int i = 0; i < neighborCells.Count(); i++)
                {
                    neighborCells[i].HasPoop = true;
                }
            }
            // There is 15% of chance for the generated number to be in the following range
            if ((die >= 85) && (die <= 100))
            {
                pCell.HasHole = true;
                neighborCells = pCell.getAdjacentCells();
                for (int i = 0; i < neighborCells.Count(); i++)
                {
                    neighborCells[i].HasWind = true;
                }
            }
        }

        public static void StopExecution()
        {
            // STOP EXECUTION : griser bouton + messagebox
        }
    }
}
