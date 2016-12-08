using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace MagicForest
{
    /// <summary>
    /// Logique d'interaction pour ForestCell.xaml
    /// </summary>
    public partial class ForestCell : UserControl
    {
        private int m_iLineIndex;
        private int m_iColumnIndex;
        private bool m_bClosed = false;

        public double F { get { return G + H; } set {; } }
        public double G;
        public double H;
        public ForestCell ParentCell;

        private bool m_bHasHero = false;
        private bool m_bHasPortal = false;
        private bool m_bHasMonster = false;
        private bool m_bHasHole = false;
        private bool m_bHasWind = false;
        private bool m_bHasPoop = false;
        private bool m_bHasNothing = true;

        private bool m_bAlreadyVisited = false;

        public ForestCell()
        {
            InitializeComponent();
        }

        public bool Closed
        {
            get
            {
                return m_bClosed;
            }
            set
            {
                m_bClosed = value;
            }
        }

        public int LineIndex
        {
            get
            {
                return m_iLineIndex;
            }
            set
            {
                m_iLineIndex = value;
            }
        }

        public int ColumnIndex
        {
            get
            {
                return m_iColumnIndex;
            }
            set
            {
                m_iColumnIndex = value;
            }
        }

        public bool HasHero
        {
            get
            {
                return m_bHasHero;
            }
            set
            {
                m_bHasHero = value;
            }
        }

        public bool HasPortal
        {
            get
            {
                return m_bHasPortal;
            }
            set
            {
                m_bHasPortal = value;
            }
        }

        public bool HasMonster
        {
            get
            {
                return m_bHasMonster;
            }
            set
            {
                m_bHasMonster = value;
            }
        }

        public bool HasHole
        {
            get
            {
                return m_bHasHole;
            }
            set
            {
                m_bHasHole = value;
            }
        }

        public bool HasWind
        {
            get
            {
                return m_bHasWind;
            }
            set
            {
                m_bHasWind = value;
            }
        }

        public bool HasPoop
        {
            get
            {
                return m_bHasPoop;
            }
            set
            {
                m_bHasPoop = value;
                // UPDATE SPRITE ! (REMOVE IF MONSTER IS KILLED)
            }
        }

        public bool HasNothing
        {
            get
            {
                return m_bHasNothing;
            }
            set
            {
                m_bHasNothing = value;
            }
        }

        public bool AlreadyVisited
        {
            get
            {
                return m_bAlreadyVisited;
            }
            set
            {
                m_bAlreadyVisited = value;
            }
        }

        public List<ForestCell> getAdjacentCells()
        {
            List<ForestCell> lfcResult = new List<ForestCell>();

            if (m_iLineIndex - 1 >= 0)
            {
                lfcResult.Add(MainWindow.Forest[m_iLineIndex - 1, m_iColumnIndex]);
            }
            if (m_iLineIndex + 1 < MainWindow.ForestSize)
            {
                lfcResult.Add(MainWindow.Forest[m_iLineIndex + 1, m_iColumnIndex]);
            }
            if (m_iColumnIndex - 1 >= 0)
            {
                lfcResult.Add(MainWindow.Forest[m_iLineIndex, m_iColumnIndex - 1]);
            }
            if (m_iColumnIndex + 1 < MainWindow.ForestSize)
            {
                lfcResult.Add(MainWindow.Forest[m_iLineIndex, m_iColumnIndex + 1]);
            }

            return lfcResult;
        }

        /// <summary>
        /// Adds a monster to the cell, and poops to the neighboring cells
        /// </summary>
        public void AddMonsterOnCell()
        {
            List<ForestCell> listOfNeighbors = new List<ForestCell>();

            HasMonster = true;
            HasNothing = false;
            listOfNeighbors = getAdjacentCells();
            for (int i = 0; i < listOfNeighbors.Count(); i++)
            {
                listOfNeighbors[i].AddPoop();
            }
        }

        /// <summary>
        /// Removes the fact that a cell contains a monster (used after throwing a rock at it)
        /// It also updates the neighoring cell and clean them from the smell, BUT ONLY if
        /// no monster are around anymore in any other neighboring cells
        /// </summary>
        public void RemoveMonsterOnCell()
        {
            // If the cell already does not contain a monster, we don't do anything
            // (the case may arise when the hero throws a rock on an empty case)
            if (!m_bHasMonster)
            {
                return;
            }

            // Neighbors of the cell we removed a monster from
            List<ForestCell> listOfNeighbors = new List<ForestCell>();
            // Neighbors of the smelly cell we try to clean up
            List<ForestCell> listOfSmellyCaseNeighbors = new List<ForestCell>();
            bool monstersAreStillHere = false;

            m_bHasMonster = false;
            // We know for sure it has nothing after the monster is killed,
            // because a cell can not contain a monster and something else
            m_bHasNothing = true;
            listOfNeighbors = getAdjacentCells();
            // For each neighbors of the case where we killed the monster ...
            for (int i = 0; i < listOfNeighbors.Count(); i++)
            {
                // ... we also retrieve its neighbors ... 
                listOfSmellyCaseNeighbors = listOfNeighbors[i].getAdjacentCells();
                // ... check in every one of them ...
                for (int j = 0; j < listOfSmellyCaseNeighbors.Count(); j++)
                {
                    // ... if it still has a monster ...
                    if (listOfSmellyCaseNeighbors[j].m_bHasMonster)
                    {
                        monstersAreStillHere = true;
                    }
                }
                if (!monstersAreStillHere)
                {
                    // ... before at last removing the poop !
                    listOfNeighbors[i].RemovePoop();
                }
            }
        }

        /// <summary>
        /// Adds a monster image to the cell
        /// </summary>
        public void AddMonsterImage()
        {
            BitmapImage image = new BitmapImage(new Uri("Ressources/alien.jpg", UriKind.Relative));
            cellImage.Source = image;
        }

        /// <summary>
        /// Adds a "danger" image to the cell
        /// </summary>
        public void AddPoop()
        {
            HasPoop = true;
            HasNothing = false;
        }

        /// <summary>
        /// Removes the fact that a cell contains poops
        /// </summary>
        public void RemovePoop()
        {
            HasPoop = false;
            if ((HasPortal == false) && (HasWind == false))
            {
                HasNothing = true;
            }
        }

        /// <summary>
        /// Adds a poop image to the cell
        /// </summary>
        public void AddPoopImage()
        {
            BitmapImage image = new BitmapImage(new Uri("Ressources/danger.jpg", UriKind.Relative));
            cellImage.Source = image;
        }

        /// <summary>
        /// Removes any image from the cell
        /// </summary>
        public void RemoveImage()
        {
            BitmapImage image = new BitmapImage(new Uri("", UriKind.Relative));
            cellImage.Source = image;
        }

        /// <summary>
        /// Adds a "hole" to the cell, and winds to the neighboring cells
        /// </summary>
        public void AddHoleOnCell()
        {
            List<ForestCell> listOfneighbors = new List<ForestCell>();

            HasHole = true;
            HasNothing = false;
            listOfneighbors = getAdjacentCells();
            for (int i = 0; i < listOfneighbors.Count(); i++)
            {
                listOfneighbors[i].AddWind();
            }
        }

        /// <summary>
        /// Adds a hole image to the cell
        /// </summary>
        public void AddHoleImage()
        {
            BitmapImage image = new BitmapImage(new Uri("Ressources/hole.jpg", UriKind.Relative));
            cellImage.Source = image;
        }

        /// <summary>
        /// Adds a "wind" image to the cell
        /// </summary>
        public void AddWind()
        {
            HasWind = true;
            HasNothing = false;
        }

        /// <summary>
        /// Adds a wind image to the cell
        /// </summary>
        public void AddWindImage()
        {
            BitmapImage image = new BitmapImage(new Uri("Ressources/wind.jpg", UriKind.Relative));
            cellImage.Source = image;
        }

        /// <summary>
        /// Adds a "portal"  to the cell
        /// </summary>
        public void AddPortalOnCell()
        {
            HasPortal = true;
            HasNothing = false;
        }

        /// <summary>
        /// Adds a portal image to the cell
        /// </summary>
        public void AddPortalImage()
        {
            BitmapImage image = new BitmapImage(new Uri("Ressources/gate.jpg", UriKind.Relative));
            cellImage.Source = image;
        }

        /// <summary>
        /// Adds a "hero" image to the cell
        /// </summary>
        public void AddHeroOnCell()
        {
            HasHero = true;
        }

        public void RemoveHeroFromCell()
        {
            HasHero = false;
        }

        /// <summary>
        /// Adds a hero image to the cell
        /// </summary>
        public void AddHeroImage()
        {
            BitmapImage image = new BitmapImage(new Uri("Ressources/hero.png", UriKind.Relative));
            cellImage.Source = image;
        }

    }
}
