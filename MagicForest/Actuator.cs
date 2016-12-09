namespace MagicForest
{
    /// <summary>
    /// Class to handle the hero actions.
    /// </summary>
    public static class Actuator
    {
        /// <summary>
        /// Implement OnMove event with starting and destination cell.
        /// </summary>
        /// <param name="prevForestCell"> Starting cell. </param>
        /// <param name="NewForestCell"> Destination cell. </param>
        public delegate void dlgMove(ForestCell prevForestCell, ForestCell NewForestCell);
        /// <summary>
        /// New celegate onMove.
        /// </summary>
        public static dlgMove OnMove;

        /// <summary>
        /// Implement OnExit event.
        /// </summary>
        public delegate void dlgExit();
        /// <summary>
        /// New delegate event for exit (portal found).
        /// </summary>
        public static dlgExit OnExit;

        /// <summary>
        /// Move the hero to a new cell.
        /// </summary>
        /// <param name="p_hHero"> Hero. </param>
        /// <param name="p_fcDestinationCell"> Destination cell. </param>
        /// <param name="cost"> Cost of the move. </param>
        public static void Move(Hero p_hHero, ForestCell p_fcDestinationCell, int cost)
        {
            // Update previous cell (used to know where we come and not throw rock there if we meet some radiation).
            p_hHero.PreviousForestCell = p_hHero.CurrentForestCell;
            // update hero current cell.
            p_hHero.CurrentForestCell = p_fcDestinationCell;
            // Update current MemoryCell.
            p_hHero.CurrentMemoryCell = Hero.Memory[p_fcDestinationCell.LineIndex, p_fcDestinationCell.ColumnIndex];

            p_hHero.CurrentForestCell.AlreadyVisited = true;
            p_hHero.CurrentMemoryCell.IsSafe = 1;
            // Remove calculated cost from score.
            p_hHero.Score -= cost;
            // Throw new on move event.
            OnMove(p_hHero.PreviousForestCell, p_hHero.CurrentForestCell);
        }

        /// <summary>
        /// Throw rock on the left cell.
        /// </summary>
        /// <param name="p_hHero"> Hero. </param>
        public static void ThrowRockLeft(Hero p_hHero)
        {
            if (p_hHero.CurrentForestCell.LineIndex - 1 > 0)
            {
                // Set target
                ForestCell p_fcTarget = MainWindow.Forest[p_hHero.CurrentForestCell.LineIndex, p_hHero.CurrentForestCell.ColumnIndex - 1];
                // Kill alien and remove radiation.
                p_fcTarget.RemoveAlienOnCell();
                // Update memory
                Hero.Memory[p_fcTarget.LineIndex, p_fcTarget.ColumnIndex].HasNoAlien = 1;
                Hero.Memory[p_fcTarget.LineIndex, p_fcTarget.ColumnIndex].MayContainAlien = -1;
                // Mark cell as OK
                p_hHero.CellsOK.Add(p_fcTarget);
                p_hHero.Score -= 10;
            }
        }

        /// <summary>
        /// Throw rock on right cell.
        /// </summary>
        /// <param name="p_hHero"> Hero. </param>
        public static void ThrowRockRight(Hero p_hHero)
        {
            if (p_hHero.CurrentForestCell.LineIndex + 1 > 0)
            {
                ForestCell p_fcTarget = MainWindow.Forest[p_hHero.CurrentForestCell.LineIndex, p_hHero.CurrentForestCell.ColumnIndex + 1];
                p_fcTarget.RemoveAlienOnCell();
                Hero.Memory[p_fcTarget.LineIndex, p_fcTarget.ColumnIndex].HasNoAlien = 1;
                Hero.Memory[p_fcTarget.LineIndex, p_fcTarget.ColumnIndex].MayContainAlien = -1;
                p_hHero.CellsOK.Add(p_fcTarget);
                p_hHero.Score -= 10;
            }
        }

        /// <summary>
        /// Throw rock on top cell.
        /// </summary>
        /// <param name="p_hHero"> Hero. </param>
        public static void ThrowRockTop(Hero p_hHero)
        {
            if (p_hHero.CurrentForestCell.ColumnIndex - 1 > 0)
            {
                ForestCell p_fcTarget = MainWindow.Forest[p_hHero.CurrentForestCell.LineIndex - 1, p_hHero.CurrentForestCell.ColumnIndex];
                p_fcTarget.RemoveAlienOnCell();
                Hero.Memory[p_fcTarget.LineIndex, p_fcTarget.ColumnIndex].HasNoAlien = 1;
                Hero.Memory[p_fcTarget.LineIndex, p_fcTarget.ColumnIndex].MayContainAlien = -1;
                p_hHero.CellsOK.Add(p_fcTarget);
                p_hHero.Score -= 10;
            }
        }

        /// <summary>
        /// Throw Rock bottom.
        /// </summary>
        /// <param name="p_hHero"> Hero. </param>
        public static void ThrowRockBottom(Hero p_hHero)
        {
            if (p_hHero.CurrentForestCell.ColumnIndex + 1 > 0)
            {
                ForestCell p_fcTarget = MainWindow.Forest[p_hHero.CurrentForestCell.LineIndex + 1, p_hHero.CurrentForestCell.ColumnIndex];
                p_fcTarget.RemoveAlienOnCell();
                Hero.Memory[p_fcTarget.LineIndex, p_fcTarget.ColumnIndex].HasNoAlien = 1;
                Hero.Memory[p_fcTarget.LineIndex, p_fcTarget.ColumnIndex].MayContainAlien = -1;
                p_hHero.CellsOK.Add(p_fcTarget);
                p_hHero.Score -= 10;
            }
        }

        /// <summary>
        /// Exit the current map.
        /// </summary>
        /// <param name="p_hHero"></param>
        public static void Exit(Hero p_hHero)
        {
            OnExit?.Invoke();
        }
    }
}
