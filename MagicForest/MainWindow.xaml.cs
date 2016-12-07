using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        public static Hero steveTheHero = new Hero();
        private static string m_sWhatsOnTheCell = String.Empty;

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

        public static string VerboseCellContent
        {
            get
            {
                return m_sWhatsOnTheCell;
            }
            set
            {
                m_sWhatsOnTheCell = value;
            }
        }

        public MainWindow()
        {
            steveTheHero.anEvent += new Hero.WatchingExitEvent(On_PortalFound);
            InitializeComponent();
            Inference();
            //CreateForest();
            //PopulateForest(m_afcForest);
            //CreateGUI();
            //ShowForestCellsInGUI();
            //EditTextDetails();
            //Content = mainGrid;
        }

        /// <summary>
        /// Inference cycle that is performed at every click
        /// Makes the decision to undertake an action (move, throw a rock or step into the portal)
        /// </summary>
        public void Inference()
        {
            CreateForest();
            PopulateForest(m_afcForest);
            CreateGUI();
            ShowForestCellsInGUI();
            EditTextDetails();
            Content = mainGrid;
        }
        
        public void IncreaseForestSize()
        {
            m_iForestSize++;
        }
        
        public void EditTextDetails()
        {
            // Initializes the text containing the cell's details (monster, smell, etc ...)
            VerboseCellContent = "";
            ForestCell currentCell = steveTheHero.CurrentCell;
            if (currentCell.HasNothing)
            {
                VerboseCellContent = String.Concat(VerboseCellContent, "- rien de particulier\n");
            }
            if (currentCell.HasHole)
            {
                VerboseCellContent = String.Concat(VerboseCellContent, "- un trou ... Damned ...\n");
            }
            if (currentCell.HasMonster)
            {
                VerboseCellContent = String.Concat(VerboseCellContent, "- un alien ... Game Over\n");
            }
            if (currentCell.HasWind)
            {
                VerboseCellContent = String.Concat(VerboseCellContent, "- un brusque coup de vent\n");
            }
            if (currentCell.HasPoop)
            {
                VerboseCellContent = String.Concat(VerboseCellContent, "- une odeur étrange\n");
            }
            if (currentCell.HasPortal)
            {
                VerboseCellContent = String.Concat(VerboseCellContent, "- une douce lumière\n");
            }
            currentCellText.Text = VerboseCellContent;
        }

        /// <summary>
        /// Creates a grid with "m_iForestSize" rows and columns
        /// This grid is placed left to the button in the main window interface
        /// </summary>
        public void CreateGUI()
        {
            Grid forestGrid = null;
            forestGrid = new Grid();

            // Creates the desired number of rows and columns for the grid
            for (int i = 0; i < m_iForestSize; i++)
            {
                forestGrid.RowDefinitions.Add(new RowDefinition());
                forestGrid.ColumnDefinitions.Add(new ColumnDefinition());
            }

            for (int i = 0; i < m_iForestSize; i++)
            {
                for (int j = 0; j < m_iForestSize; j++)
                {
                    // Address the ForestCell [i,j] to the grid row 'i' and column 'j'
                    Grid.SetRow(m_afcForest[i, j], i);
                    Grid.SetColumn(m_afcForest[i, j], j);
                    forestGrid.Children.Add(m_afcForest[i, j]);
                }
                Grid.SetRow(forestGrid, 0);
                Grid.SetColumn(forestGrid, 0);
                mainGrid.Children.Remove(forestGrid);
                mainGrid.Children.Add(forestGrid);
            }
        }

        /// <summary>
        /// Takes the ForestCell matrix and draw image in the grid accordingly
        /// </summary>
        public void ShowForestCellsInGUI()
        {
            // Initializes the string which details what's on the cell
            for (int i = 0; i < m_iForestSize; i++)
            {
                for (int j = 0; j < m_iForestSize; j++)
                {
                    // If the current cell has ...
                    // ... a hero 
                    if (m_afcForest[i, j].HasHero)
                    {
                        m_afcForest[i, j].AddHeroImage();
                        continue;
                    }
                    // ... nothing
                    if ((m_afcForest[i,j].HasNothing) && !(m_afcForest[i, j].HasHero))
                    {
                        m_afcForest[i, j].RemoveImage();
                        continue;
                    }
                    // ... a portal and no hero
                    if ((m_afcForest[i, j].HasPortal) && !(m_afcForest[i, j].HasHero))
                    {
                        m_afcForest[i, j].AddPortalImage();
                        continue;
                    }
                    // ... a hole and no hero
                    if ((m_afcForest[i, j].HasHole) && !(m_afcForest[i, j].HasHero))
                    {
                        m_afcForest[i, j].AddHoleImage();
                        continue;
                    }
                    // ... wind and no hero
                    if ((m_afcForest[i, j].HasWind) && !(m_afcForest[i, j].HasHero))
                    {
                        m_afcForest[i, j].AddWindImage();
                        continue;
                    }
                    // ... a monster and no hero
                    if ((m_afcForest[i, j].HasMonster) && !(m_afcForest[i, j].HasHero))
                    {
                        m_afcForest[i, j].AddMonsterImage();
                        continue;
                    }
                    // ... poop and no hero
                    if ((m_afcForest[i, j].HasPoop) && !(m_afcForest[i, j].HasHero))
                    {
                        m_afcForest[i, j].AddPoopImage();
                        continue;
                    }
                }
            }
        }

        /// <summary>
        /// Creates a forest matrix with the size "m_iForestSize" with a ForestCell in each cell
        /// </summary>
        public void CreateForest()
        {
            // Nullify the current instances of the objects
            m_afcForest = null;
            m_afcForest = new ForestCell[m_iForestSize, m_iForestSize];
            
            // For every cell we want, we create a new ForestCell
            // and we address it to the ForestCell matrix
            for (int i = 0; i < m_iForestSize; i++)
            {
                for (int j = 0; j < m_iForestSize; j++)
                {
                    // Creates a new ForestCell and gives it the correct attributes for row and column
                    m_afcForest[i, j] = new ForestCell();
                    m_afcForest[i, j].LineIndex = i;
                    m_afcForest[i, j].ColumnIndex = j;
                }
            }
        }

        /// <summary>
        /// Populates a given forest with a portal, a hero, monsters and holes
        /// </summary>
        /// <param name="pForest">The forest to populate, as a 2-dimensional array of ForestCell</param>
        public void PopulateForest(ForestCell[,] pForest)
        {
            steveTheHero.CurrentCell = pForest[0, 0];
            pForest[0, 0].AddHeroOnCell();

            Random random = new Random();
            // We generate a number between 1 and the size of the forest + 1 
            int portalRow = random.Next(0, m_iForestSize);
            int portalColumn = random.Next(0, m_iForestSize);
            pForest[portalRow, portalColumn].AddPortalOnCell();

            // We go through all the cells in the forest ...
            for (int i = 0; i < m_iForestSize; i++)
            {
                for (int j = 0; j < m_iForestSize; j++)
                {
                    if ((pForest[i, j].HasNothing == true) && !((i == 0) && (j == 0)))
                    {
                        // and try to populate it if it's empty (with a monster or a hole)
                        PopulateCellRandomly(pForest[i, j], random);
                    }
                }
            }
        }
       
        /// <summary>
        /// Randomly puts either a monster, a hole, or nothing on the input cell
        /// Also updates the neighboring cells with either smell or wind accordingly
        /// </summary>
        /// <param name="pCell">The input cell to populate</param>
        public void PopulateCellRandomly(ForestCell pCell, Random r)
        {
            // Generates a random number between 0 (included) and 100 (excluded)
            int die = r.Next(0,100);

            // There is 15% of chance for the generated number to be in the following range
            if ((die >= 0) && (die < 10))
            {
                pCell.AddMonsterOnCell();
            }
            // There is 15% of chance for the generated number to be in the following range
            if ((die >= 90) && (die <= 100))
            {
                pCell.AddHoleOnCell();
            }
        }

        public static void StopExecution()
        {
            // STOP EXECUTION : griser bouton + messagebox
        }

        public static void On_PortalFound()
        {
            IncreaseForestSize();
            Inference();
        }

        private void On_DoStuffButtonClick(object sender, RoutedEventArgs e)
        {
            IncreaseForestSize();
            Inference();
        }
    }
}
