using System;
using System.Windows;
using System.Windows.Controls;

namespace MagicForest
{
    /// <summary>
    /// Interface graphique MainWindow.xaml.
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Store the forest size.
        /// </summary>
        private static int m_iForestSize = 3;
        /// <summary>
        /// Matrix of ForestCell i.e. forest.
        /// </summary>
        private static ForestCell[,] m_afcForest = null;
        /// <summary>
        /// The new hero.
        /// </summary>
        private static Hero steveTheHero;
        /// <summary>
        /// Text field used to show the cell contain if several sprites overlap.
        /// </summary>
        private static string m_sWhatsOnTheCell = string.Empty;
        /// <summary>
        /// Grapique grid to represent the forest.
        /// </summary>
        private Grid forestGrid;

        /// <summary>
        /// Get / set the Forest Size.
        /// </summary>
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

        /// <summary>
        /// Get / set the cell content as a string.
        /// </summary>
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

        /// <summary>
        /// Build a new interface.
        /// </summary>
        public MainWindow()
        {
            // Catch exception at start.
            try
            {
                InitializeComponent();

                //Create new forest i.e. matrix of ForestCell.
                CreateForest();
                // Initialize new hero with score at 0.
                steveTheHero = new Hero(m_iForestSize, 0);
                // Add new eventHandler for onExit, onMove and onThrow.
                Actuator.OnExit += new Actuator.dlgExit(OnExit);
                Actuator.OnMove += new Actuator.dlgMove(OnMove);

                Forest[0, 0].AlreadyVisited = true;
                Hero.Memory[0, 0].IsSafe = 1;

                // Randomly populate forest.
                PopulateForest(m_afcForest);
                // Create new grid to display the forest.
                CreateGUI();
                // Update GUI showing the forest.
                UpdateGUI();
            }
            catch (NotSupportedException e)
            {
                e.GetType();
            }
        }

        /// <summary>
        /// Inference cycle that is performed at every click
        /// Makes the decision to undertake an action (move, throw a rock or step into the portal)
        /// </summary>
        public void UpdateGUI()
        {
            // Update GUI sprites.
            ShowForestCellsInGUI();
            // Update content cell text.
            EditTextDetails();
            Content = mainGrid;
        }

        /// <summary>
        /// Increase forest size.
        /// </summary>
        public void IncreaseForestSize()
        {
            m_iForestSize++;
        }

        /// <summary>
        /// Initializes the text containing the cell's details (monster, smell, etc...).
        /// </summary>
        public void EditTextDetails()
        {
            VerboseCellContent = string.Empty;
            ForestCell currentCell = steveTheHero.CurrentForestCell;
            if (currentCell.HasNothing)
            {
                VerboseCellContent = string.Concat(VerboseCellContent, "- Nothing\n");
            }
            if (currentCell.HasHole)
            {
                VerboseCellContent = string.Concat(VerboseCellContent, "- A hole ! Damned !\n");
            }
            if (currentCell.HasAlien)
            {
                VerboseCellContent = string.Concat(VerboseCellContent, "- An Alien ! Game Over\n");
            }
            if (currentCell.HasWind)
            {
                VerboseCellContent = string.Concat(VerboseCellContent, "- Some wind\n");
            }
            if (currentCell.HasRadiation)
            {
                VerboseCellContent = string.Concat(VerboseCellContent, "- Some smell\n");
            }
            if (currentCell.HasPortal)
            {
                VerboseCellContent = string.Concat(VerboseCellContent, "- Some light\n");
            }
            currentCellText.Text = VerboseCellContent;
        }

        /// <summary>
        /// Creates a grid with "m_iForestSize" rows and columns.
        /// This grid is placed left to the button in the main window interface.
        /// </summary>
        public void CreateGUI()
        {
            if (forestGrid != null)
            {
                forestGrid.Children.Clear();
            }
            mainGrid.Children.Remove(forestGrid);

            forestGrid = new Grid();

            // Creates the desired number of rows and columns for the grid.
            for (int i = 0; i < m_iForestSize; i++)
            {
                forestGrid.RowDefinitions.Add(new RowDefinition());
                forestGrid.ColumnDefinitions.Add(new ColumnDefinition());
            }

            for (int i = 0; i < m_iForestSize; i++)
            {
                for (int j = 0; j < m_iForestSize; j++)
                {
                    // Address the ForestCell [i,j] to the grid row 'i' and column 'j'.
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
        /// Takes the ForestCell matrix and draw image in the grid accordingly.
        /// </summary>
        public void ShowForestCellsInGUI()
        {
            // Initializes the string which details what's on the cell.
            for (int i = 0; i < m_iForestSize; i++)
            {
                for (int j = 0; j < m_iForestSize; j++)
                {
                    // If the current cell has...
                    // ...a hero 
                    if (m_afcForest[i, j].HasHero)
                    {
                        m_afcForest[i, j].AddHeroImage();
                        continue;
                    }
                    // ...nothing
                    if ((m_afcForest[i, j].HasNothing) && !(m_afcForest[i, j].HasHero))
                    {
                        m_afcForest[i, j].RemoveImage();
                        continue;
                    }
                    // ...a portal and no hero
                    if ((m_afcForest[i, j].HasPortal) && !(m_afcForest[i, j].HasHero))
                    {
                        m_afcForest[i, j].AddPortalImage();
                        continue;
                    }
                    // ...a hole and no hero
                    if ((m_afcForest[i, j].HasHole) && !(m_afcForest[i, j].HasHero))
                    {
                        m_afcForest[i, j].AddHoleImage();
                        continue;
                    }
                    // ...wind and no hero
                    if ((m_afcForest[i, j].HasWind) && !(m_afcForest[i, j].HasHero))
                    {
                        m_afcForest[i, j].AddWindImage();
                        continue;
                    }
                    // ...an alien and no hero
                    if ((m_afcForest[i, j].HasAlien) && !(m_afcForest[i, j].HasHero))
                    {
                        m_afcForest[i, j].AddAlienImage();
                        continue;
                    }
                    // ...radiation and no hero
                    if ((m_afcForest[i, j].HasRadiation) && !(m_afcForest[i, j].HasHero))
                    {
                        m_afcForest[i, j].AddDangerImage();
                        continue;
                    }
                }
            }
        }

        /// <summary>
        /// Creates a forest matrix with the size "m_iForestSize" with a ForestCell in each cell.
        /// </summary>
        public void CreateForest()
        {
            // Nullify the current instances of the objects.
            m_afcForest = null;
            m_afcForest = new ForestCell[m_iForestSize, m_iForestSize];

            // For every cell we want, we create a new ForestCell
            // and we address it to the ForestCell matrix.
            for (int i = 0; i < m_iForestSize; i++)
            {
                for (int j = 0; j < m_iForestSize; j++)
                {
                    // Creates a new ForestCell and gives it the correct attributes for row and column.
                    m_afcForest[i, j] = new ForestCell();
                    m_afcForest[i, j].LineIndex = i;
                    m_afcForest[i, j].ColumnIndex = j;
                }
            }
        }

        /// <summary>
        /// Populates a given forest with a portal, a hero, monsters and holes.
        /// </summary>
        /// <param name="pForest"> The forest to populate, as a 2-dimensional array of ForestCell. </param>
        public void PopulateForest(ForestCell[,] pForest)
        {
            steveTheHero.CurrentForestCell = pForest[0, 0];
            pForest[0, 0].AddHeroOnCell();

            Random random = new Random();
            // We generate a number between 1 and the size of the forest + 1.
            int portalRow = random.Next(1, m_iForestSize);
            int portalColumn = random.Next(1, m_iForestSize);
            pForest[portalRow, portalColumn].AddPortalOnCell();

            // We go through all the cells in the forest ...
            for (int i = 0; i < m_iForestSize; i++)
            {
                for (int j = 0; j < m_iForestSize; j++)
                {
                    if ((pForest[i, j].HasNothing) && !((i == 0) && (j == 0)))
                    {
                        // and try to populate it if it's empty (with a monster or a hole).
                        PopulateCellRandomly(pForest[i, j], random);
                    }
                }
            }
        }

        /// <summary>
        /// Randomly puts either a monster, a hole, or nothing on the input cell
        /// Also updates the neighboring cells with either smell or wind accordingly.
        /// </summary>
        /// <param name="pCell"> The input cell to populate </param>
        public void PopulateCellRandomly(ForestCell pCell, Random r)
        {
            // Generates a random number between 0 (included) and 100 (excluded).
            int die = r.Next(0, 100);

            // There is 8 % of chance for the generated number to be in the following range.
            if ((die >= 0) && (die < 8))
            {
                pCell.AddAlienOnCell();
            }
            // There is 8 % of chance for the generated number to be in the following range.
            if ((die >= 92) && (die <= 100))
            {
                pCell.AddHoleOnCell();
            }
        }

        /// <summary>
        /// Event thrown when the Hero move, update sprite emplacement.
        /// Start the cycle if the Hero dies.
        /// </summary>
        /// <param name="prevForestCell"> Previous cell.  </param>
        /// <param name="NewForestCell"> Cell to move to. </param>
        public void OnMove(ForestCell prevForestCell, ForestCell NewForestCell)
        {
            for (int i = 0; i < m_iForestSize; i++)
            {
                for (int j = 0; j < m_iForestSize; j++)
                {
                    if (m_afcForest[i, j] == prevForestCell)
                    {
                        m_afcForest[i, j].HasHero = false;
                    }
                    if (m_afcForest[i, j] == NewForestCell)
                    {
                        m_afcForest[i, j].HasHero = true;
                    }
                }
            }
            if (steveTheHero.CurrentForestCell.HasAlien || steveTheHero.CurrentForestCell.HasHole)
            {
                UpdateGUI();
                OnDeath();
            }
        }

        /// <summary>
        /// Event thrown when the hero reaches the portal.
        /// </summary>
        public void OnExit()
        {
            // Store current number of hero points with update from finding the exit.
            int score = steveTheHero.Score + 10 * (int)Math.Pow(m_iForestSize, 2);

            IncreaseForestSize();
            // Create a new forest.
            CreateForest();

            // Create new Hero with the caracteristics of the previous one
            // (we have to do this to free some memory and because updating hero
            // caracteristics led to errors).
            steveTheHero = new Hero(m_iForestSize, score);

            Forest[0, 0].AlreadyVisited = true;
            Hero.Memory[0, 0].IsSafe = 1;

            // Populate the new forest.
            PopulateForest(m_afcForest);
            CreateGUI();

            steveTheHero.CurrentForestCell = Forest[0, 0];
            Forest[0, 0].AlreadyVisited = true;
            Hero.Memory[0, 0].IsSafe = 1;
        }

        /// <summary>
        /// Event thrown when we try to do an action and we are in a hole or on a monster.
        /// </summary>
        public void OnDeath()
        {
            int score = steveTheHero.Score - 10 * (int)Math.Pow(m_iForestSize, 2);

            // Show messagebox with fnal agent score.
            MessageBox.Show("You unfortunatly dies in agony, maybe next time...\nScore final : " + score,
                "You died !",
                MessageBoxButton.OK,
                MessageBoxImage.Warning);

            // Reset program.
            m_iForestSize = 3;
            CreateForest();

            steveTheHero = new Hero(m_iForestSize, 0);
            Forest[0, 0].AlreadyVisited = true;
            Hero.Memory[0, 0].IsSafe = 1;

            PopulateForest(m_afcForest);
            CreateGUI();
        }

        /// <summary>
        /// Start the BDI model when clicking on a button.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void On_GetOutButtonClick(object sender, RoutedEventArgs e)
        {
            steveTheHero.Inference();
            UpdateGUI();
        }
    }
}
