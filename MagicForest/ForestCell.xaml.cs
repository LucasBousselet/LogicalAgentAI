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
        /// <summary>
        /// Cell line index.
        /// </summary>
        private int m_iLineIndex;
        /// <summary>
        /// Cell column index.
        /// </summary>
        private int m_iColumnIndex;

        /// <summary>
        /// Cell state for the pathfinding.
        /// </summary>
        private bool m_bClosed = false;
        /// <summary>
        /// Parameter distance to goal for the pathfinding.
        /// </summary>
        private double m_dDistance;
        /// <summary>
        /// Parent cell, used for pathfinding.
        /// </summary>
        private ForestCell m_fcParentCell;

        /// <summary>
        /// Boolean to set if we have a Hero on the cell.
        /// </summary>
        private bool m_bHasHero = false;
        /// <summary>
        /// Boolean to set if we have the portal on the cell.
        /// </summary>
        private bool m_bHasPortal = false;
        /// <summary>
        /// Boolean to set if we have an alien on the cell.
        /// </summary>
        private bool m_bHasAlien = false;
        /// <summary>
        /// Boolean to set if we have a hole on the cell.
        /// </summary>
        private bool m_bHasHole = false;
        /// <summary>
        /// Boolean to set if we have some wind on the cell.
        /// </summary>
        private bool m_bHasWind = false;
        /// <summary>
        /// Boolean to set if we have some radiation on the cell.
        /// </summary>
        private bool m_bHasRadiation = false;
        /// <summary>
        /// Boolean to set if we have nothing on the cell.
        /// </summary>
        private bool m_bHasNothing = true;

        /// <summary>
        /// Mark if the call has already been visited.
        /// </summary>
        private bool m_bAlreadyVisited = false;

        /// <summary>
        /// Creat a new forest cell.
        /// </summary>
        public ForestCell()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Get / set the m_dDistance.
        /// </summary>
        public double Distance
        {
            get
            {
                return m_dDistance;
            }
            set
            {
                m_dDistance = value;
            }
        }

        /// <summary>
        /// Get / set the m_fcParentCell.
        /// </summary>
        public ForestCell ParentCell
        {
            get
            {
                return m_fcParentCell;
            }
            set
            {
                m_fcParentCell = value;
            }
        }

        /// <summary>
        /// Get / set the m_bClosed
        /// </summary>
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
        /// <summary>
        /// Get / set the m_iLineIndex.
        /// </summary>
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

        /// <summary>
        /// Get / set the m_iColumnIndex.
        /// </summary>
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

        /// <summary>
        /// Get / set the m_bHasHero.
        /// </summary>
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

        /// <summary>
        /// Get / set the m_bHasPortal
        /// </summary>
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

        /// <summary>
        /// Get / set the m_bHasAlien
        /// </summary>
        public bool HasAlien
        {
            get
            {
                return m_bHasAlien;
            }
            set
            {
                m_bHasAlien = value;
            }
        }

        /// <summary>
        /// Get / set the m_bHasHole.
        /// </summary>
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

        /// <summary>
        /// Get / set the m_bHasWind.
        /// </summary>
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

        /// <summary>
        /// Get / set the m_bHasRadiation.
        /// </summary>
        public bool HasRadiation
        {
            get
            {
                return m_bHasRadiation;
            }
            set
            {
                m_bHasRadiation = value;
            }
        }

        /// <summary>
        /// Get / set the m_bHasNothing.
        /// </summary>
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

        /// <summary>
        /// Get / set the m_bAlreadyVisited
        /// </summary>
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

        /// <summary>
        /// Get the adjacent cell of a given cell.
        /// </summary>
        /// <returns> A list of the adjacent cells. </returns>
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
        /// Adds an alien to the cell, and poops to the neighboring cells.
        /// </summary>
        public void AddAlienOnCell()
        {
            List<ForestCell> listOfNeighbors = new List<ForestCell>();

            HasAlien = true;
            HasNothing = false;
            listOfNeighbors = getAdjacentCells();
            for (int i = 0; i < listOfNeighbors.Count(); i++)
            {
                listOfNeighbors[i].AddRadiation();
            }
        }

        /// <summary>
        /// Removes the fact that a cell contains a alien (used after throwing a rock at it)
        /// It also updates the neighoring cell and clean them from the radiation, BUT ONLY if
        /// no alien are around anymore in any other neighboring cells.
        /// </summary>
        public void RemoveAlienOnCell()
        {
            // If the cell already does not contain a alien, we don't do anything
            // (the case may arise when the hero throws a rock on an empty case).
            if (!m_bHasAlien)
            {
                return;
            }

            // Neighbors of the cell we removed a alien from.
            List<ForestCell> listOfNeighbors = new List<ForestCell>();
            // Neighbors of the radioactive cell we try to clean up.
            List<ForestCell> listOfRadCellNeighbors = new List<ForestCell>();
            // Boolean used to know if the neighboors cell of radiation still have an alien.
            bool monstersAreStillHere = false;

            m_bHasAlien = false;
            // We know for sure it has nothing after the alien is killed,
            // because a cell can not contain a alien and something else.
            m_bHasNothing = true;
            listOfNeighbors = getAdjacentCells();
            // For each neighbors of the case where we killed the alien...
            for (int i = 0; i < listOfNeighbors.Count(); i++)
            {
                monstersAreStillHere = false;
                // ...we also retrieve its neighbors... 
                listOfRadCellNeighbors = listOfNeighbors[i].getAdjacentCells();
                // ... check in every one of them ...
                for (int j = 0; j < listOfRadCellNeighbors.Count(); j++)
                {
                    // ...if it still has an alien...
                    if (listOfRadCellNeighbors[j].m_bHasAlien)
                    {
                        monstersAreStillHere = true;
                    }
                }
                if (!monstersAreStillHere)
                {
                    // ...before at last removing the radiation !
                    listOfNeighbors[i].RemoveRadiation();
                }
            }
        }

        /// <summary>
        /// Adds a alien image to the cell.
        /// </summary>
        public void AddAlienImage()
        {
            BitmapImage image = new BitmapImage(new Uri("Ressources/alien.jpg", UriKind.Relative));
            cellImage.Source = image;
        }

        /// <summary>
        /// Adds a "danger" image to the cell.
        /// </summary>
        public void AddRadiation()
        {
            HasRadiation = true;
            HasNothing = false;
        }

        /// <summary>
        /// Removes the fact that a cell contains radiation.
        /// </summary>
        public void RemoveRadiation()
        {
            HasRadiation = false;
            if ((HasPortal == false) && (HasWind == false))
            {
                HasNothing = true;
            }
        }

        /// <summary>
        /// Adds a danger image to the cell.
        /// </summary>
        public void AddDangerImage()
        {
            BitmapImage image = new BitmapImage(new Uri("Ressources/danger.jpg", UriKind.Relative));
            cellImage.Source = image;
        }

        /// <summary>
        /// Removes any image from the cell.
        /// </summary>
        public void RemoveImage()
        {
            BitmapImage image = new BitmapImage(new Uri("", UriKind.Relative));
            cellImage.Source = image;
        }

        /// <summary>
        /// Adds a "hole" to the cell, and winds to the neighboring cells.
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
        /// Adds a hole image to the cell.
        /// </summary>
        public void AddHoleImage()
        {
            BitmapImage image = new BitmapImage(new Uri("Ressources/hole.jpg", UriKind.Relative));
            cellImage.Source = image;
        }

        /// <summary>
        /// Adds a "wind" image to the cell.
        /// </summary>
        public void AddWind()
        {
            HasWind = true;
            HasNothing = false;
        }

        /// <summary>
        /// Adds a wind image to the cell.
        /// </summary>
        public void AddWindImage()
        {
            BitmapImage image = new BitmapImage(new Uri("Ressources/wind.jpg", UriKind.Relative));
            cellImage.Source = image;
        }

        /// <summary>
        /// Adds a "portal" to the cell.
        /// </summary>
        public void AddPortalOnCell()
        {
            HasPortal = true;
            HasNothing = false;
        }

        /// <summary>
        /// Adds a portal image to the cell.
        /// </summary>
        public void AddPortalImage()
        {
            BitmapImage image = new BitmapImage(new Uri("Ressources/gate.jpg", UriKind.Relative));
            cellImage.Source = image;
        }

        /// <summary>
        /// Adds a "hero" image to the cell.
        /// </summary>
        public void AddHeroOnCell()
        {
            HasHero = true;
        }

        /// <summary>
        /// Remove the hero from the cell.
        /// </summary>
        public void RemoveHeroFromCell()
        {
            HasHero = false;
        }

        /// <summary>
        /// Adds a hero image to the cell.
        /// </summary>
        public void AddHeroImage()
        {
            BitmapImage image = new BitmapImage(new Uri("Ressources/hero.png", UriKind.Relative));
            cellImage.Source = image;
        }

    }
}
