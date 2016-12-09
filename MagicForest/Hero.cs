using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace MagicForest
{
    /// <summary>
    /// Class used to handle the behavior of the hero.
    /// </summary>
    public class Hero
    {
        /// <summary>
        /// Hero score.
        /// </summary>
        private int m_iScore;
        /// <summary>
        /// Hero current cell.
        /// </summary>
        private ForestCell m_fcCurrentForestCell;
        /// <summary>
        /// Hero previous cell.
        /// </summary>
        private ForestCell m_fcPreviousForestCell;
        /// <summary>
        /// If hero is still alive.
        /// </summary>
        private bool m_bStillAlive = true;
        /// <summary>
        /// Result of checking the cell content : hasNothing.
        /// </summary>
        private bool m_bNothing = false;
        /// <summary>
        /// Result of checking the cell content : hasRadiation.
        /// </summary>
        private bool m_bRadiationDetected = false;
        /// <summary>
        /// Result of checking the cell content : hasWind.
        /// </summary>
        private bool m_bWindDetected = false;
        /// <summary>
        /// Result of checking the cell content : hasLight.
        /// </summary>
        private bool m_bLightDetected = false;
        /// <summary>
        /// Set hero goal.
        /// </summary>
        private string m_sGoal = "GETOUTOMG!";

        /// <summary>
        /// 2-dimensional array of the knowledge our hero has of the current environment.
        /// </summary>
        private static MemoryCell[,] m_lmcMemory;
        /// <summary>
        /// Set hero current memory cell.
        /// </summary>
        /// <summary>
        /// Current memory size.
        /// </summary>
        private int m_iMemorySize;
        private MemoryCell m_mcCurrentMemoryCell;
        /// <summary>
        /// List of OK cells.
        /// </summary>
        private List<ForestCell> m_lfcCellsOK = new List<ForestCell>();
        /// <summary>
        /// List of suspicious cells.
        /// </summary>
        private List<ForestCell> m_lfcCellsSuspicous = new List<ForestCell>();
        /// <summary>
        /// List of cells with radiation.
        /// </summary>
        private List<ForestCell> m_lfcCellsWithRadiation = new List<ForestCell>();

        /// <summary>
        /// Get / set the m_mcCurrentMemoryCell.
        /// </summary>
        public MemoryCell CurrentMemoryCell
        {
            get
            {
                return m_mcCurrentMemoryCell;
            }
            set
            {
                m_mcCurrentMemoryCell = value;
            }
        }

        /// <summary>
        /// Get / set the m_fcPreviousForestCell.
        /// </summary>
        public ForestCell PreviousForestCell
        {
            get
            {
                return m_fcPreviousForestCell;
            }
            set
            {
                m_fcPreviousForestCell = value;
            }
        }

        /// <summary>
        /// Get / set the m_lfcCellsOK.
        /// </summary>
        public List<ForestCell> CellsOK
        {
            get
            {
                return m_lfcCellsOK;
            }
        }

        /// <summary>
        /// Get / set the m_lfcCellsSuspicous.
        /// </summary>
        public List<ForestCell> CellsSuspicous
        {
            get
            {
                return m_lfcCellsSuspicous;
            }
        }

        /// <summary>
        /// Get / set the m_lmcMemory.
        /// </summary>
        public static MemoryCell[,] Memory
        {
            get
            {
                return m_lmcMemory;
            }
            set
            {
                m_lmcMemory = value;
            }
        }

        /// <summary>
        /// Get / set the m_iMemorySize.
        /// </summary>
        public int MemorySize
        {
            get
            {
                return m_iMemorySize;
            }
            set
            {
                m_iMemorySize = value;
            }
        }

        /// <summary>
        /// Get / set the m_iScore.
        /// </summary>
        public int Score
        {
            get
            {
                return m_iScore;
            }
            set
            {
                m_iScore = value;
            }
        }

        /// <summary>
        /// Get / set the m_fcCurrentForestCell.
        /// </summary>
        public ForestCell CurrentForestCell
        {
            get
            {
                return m_fcCurrentForestCell;
            }
            set
            {
                m_fcCurrentForestCell = value;
            }
        }

        /// <summary>
        /// Create a new hero.
        /// </summary>
        /// <param name="memSize"> Hero's Memory size. </param>
        /// <param name="Score"> Hero's score. </param>
        public Hero(int memSize, int Score)
        {
            m_iScore = 0;
            m_iMemorySize = memSize;
            m_iScore = Score;
            PopulateMemoryCellMatrix();
            m_mcCurrentMemoryCell = Memory[0, 0];
        }

        /// <summary>
        /// Puts MemoryCells inside the matrix of MemoryCell the hero has of his environment.
        /// </summary>
        public void PopulateMemoryCellMatrix()
        {
            m_lmcMemory = new MemoryCell[m_iMemorySize, m_iMemorySize];

            for (int i = 0; i < m_iMemorySize; i++)
            {
                for (int j = 0; j < m_iMemorySize; j++)
                {
                    Memory[i, j] = new MemoryCell();
                    Memory[i, j].LineIndex = i;
                    Memory[i, j].ColumnIndex = j;
                }
            }
        }

        /// <summary>
        /// Check if agent is still alive.
        /// </summary>
        /// <returns> true if the agent is alive, false otherwise. </returns>
        public bool AmIAlive()
        {
            return m_bStillAlive;
        }

        /// <summary>
        /// Get the environment state from sensors.
        /// </summary>
        public void GetEnvironmentState()
        {
            m_bNothing = Sensor.HasNothing(m_fcCurrentForestCell);
            m_bLightDetected = Sensor.HasLight(m_fcCurrentForestCell);
            m_bRadiationDetected = Sensor.HasRadiation(m_fcCurrentForestCell);
            m_bWindDetected = Sensor.HasWind(m_fcCurrentForestCell);
        }

        /// <summary>
        /// Update memory cell.
        /// </summary>
        /// <returns> The state we are currently in. </returns>
        public int UpdateMyState()
        {
            int iResultState = -1;

            // If the cell is empty.
            if (m_bNothing)
            {
                // Remove from list of cells with radiation.
                if (m_lfcCellsWithRadiation.Contains(CurrentForestCell))
                {
                    m_lfcCellsWithRadiation.Remove(CurrentForestCell);
                }

                // Update Cell with knowledge that it's empty.
                List<MemoryCell> MemoryCells = m_lmcMemory[m_mcCurrentMemoryCell.LineIndex, m_mcCurrentMemoryCell.ColumnIndex].getAdjacentMemoryCells();
                foreach (MemoryCell mcItem in MemoryCells)
                {
                    m_lfcCellsOK.AddRange(m_fcCurrentForestCell.getAdjacentCells());
                    // Remove duplicates.
                    m_lfcCellsOK = m_lfcCellsOK.Distinct().ToList();

                    // Note that we are sure that there is no alien here.
                    mcItem.HasNoAlien = 1;
                    mcItem.MayContainAlien = -1;
                    // Note that we are sure that there is no hole here.
                    mcItem.HasNoHole = 1;
                    mcItem.MayContainHole = -1;
                }

                iResultState = 0;
            }

            if (m_bRadiationDetected && !m_bWindDetected && !m_bLightDetected)
            {
                // Add cell to radList.
                m_lfcCellsWithRadiation.Add(m_fcCurrentForestCell);

                // Get the neighboor cells from current cell.
                List<MemoryCell> MemoryCells = m_lmcMemory[m_mcCurrentMemoryCell.LineIndex, m_mcCurrentMemoryCell.ColumnIndex].getAdjacentMemoryCells();
                for (int i = MemoryCells.Count - 1; i >= 0; i--)
                {
                    // Check in neighboor cell isn't already ok.
                    List<ForestCell> fcNeighboorForestCells = m_fcCurrentForestCell.getAdjacentCells();

                    foreach (ForestCell fcItem in fcNeighboorForestCells)
                    {
                        if (!m_lfcCellsOK.Contains(fcItem))
                        {
                            m_lfcCellsSuspicous.Add(fcItem);
                            m_lfcCellsSuspicous = m_lfcCellsSuspicous.Distinct().ToList();

                            // Note that there may be an alien around.
                            Memory[fcItem.LineIndex, fcItem.ColumnIndex].MayContainAlien = 1;
                            Memory[fcItem.LineIndex, fcItem.ColumnIndex].HasNoAlien = -1;

                            // Note that we are sure that there is no hole here.
                            Memory[fcItem.LineIndex, fcItem.ColumnIndex].HasNoHole = 1;
                            Memory[fcItem.LineIndex, fcItem.ColumnIndex].MayContainHole = -1;
                        }
                    }
                }

                iResultState = 1;
            }
            if (m_bWindDetected && !m_bRadiationDetected && !m_bLightDetected)
            {
                // Get the neighboor cells from current cell
                List<MemoryCell> MemoryCells = m_lmcMemory[m_mcCurrentMemoryCell.LineIndex, m_mcCurrentMemoryCell.ColumnIndex].getAdjacentMemoryCells();
                for (int i = MemoryCells.Count - 1; i >= 0; i--)
                {
                    // Check in neighboor cell isn't already ok
                    List<ForestCell> fcNeighboorForestCells = m_fcCurrentForestCell.getAdjacentCells();

                    foreach (ForestCell fcItem in fcNeighboorForestCells)
                    {
                        if (!m_lfcCellsOK.Contains(fcItem))
                        {
                            // Add the cell neighboors as suspicious.
                            m_lfcCellsSuspicous.Add(fcItem);
                            m_lfcCellsSuspicous = m_lfcCellsSuspicous.Distinct().ToList();

                            // Note that there may be a hole around.
                            Memory[fcItem.LineIndex, fcItem.ColumnIndex].MayContainHole = 1;
                            Memory[fcItem.LineIndex, fcItem.ColumnIndex].HasNoHole = -1;

                            // Note that we are sure that there is no alien here.
                            Memory[fcItem.LineIndex, fcItem.ColumnIndex].HasNoAlien = 1;
                            Memory[fcItem.LineIndex, fcItem.ColumnIndex].MayContainAlien = -1;
                        }
                    }
                }

                iResultState = 2;
            }
            if (m_bWindDetected && m_bRadiationDetected && !m_bLightDetected)
            {
                // Get the neighboor cells from current cell.
                List<MemoryCell> MemoryCells = m_lmcMemory[m_mcCurrentMemoryCell.LineIndex, m_mcCurrentMemoryCell.ColumnIndex].getAdjacentMemoryCells();
                for (int i = MemoryCells.Count - 1; i >= 0; i--)
                {
                    // Check in neighboor cell isn't already ok.
                    List<ForestCell> fcNeighboorForestCells = m_fcCurrentForestCell.getAdjacentCells();

                    foreach (ForestCell fcItem in fcNeighboorForestCells)
                    {
                        if (!m_lfcCellsOK.Contains(fcItem))
                        {
                            // Add the cell neighboors as suspicious.
                            m_lfcCellsSuspicous.Add(fcItem);
                            m_lfcCellsSuspicous = m_lfcCellsSuspicous.Distinct().ToList();

                            // Note that there may be a hole around.
                            Memory[fcItem.LineIndex, fcItem.ColumnIndex].MayContainHole = 1;
                            Memory[fcItem.LineIndex, fcItem.ColumnIndex].HasNoHole = -1;

                            // Note that there may be an alien around.
                            Memory[fcItem.LineIndex, fcItem.ColumnIndex].MayContainAlien = 1;
                            Memory[fcItem.LineIndex, fcItem.ColumnIndex].HasNoAlien = -1;

                        }
                    }
                }

                iResultState = 3;
            }
            // If we detect a portal.
            if (m_bLightDetected)
            {
                iResultState = 4;
            }

            return iResultState;
        }

        /// <summary>
        /// Infere the possible action from the memory and the current cell state.
        /// </summary>
        /// <param name="p_iStateEnv"> Current cell state. </param>
        /// <returns> A list of PossibleAction. </returns>
        private List<PossibleAction> ActionDeclenchable(int p_iStateEnv)
        {
            ThrowRockLeft paActToThrowRockLeft = new ThrowRockLeft(this);
            ThrowRockRight paActToThrowRockRight = new ThrowRockRight(this);
            ThrowRockTop paActToThrowRockTop = new ThrowRockTop(this);
            ThrowRockBottom paActToThrowRockBottom = new ThrowRockBottom(this);
            Exit paActToExit = new Exit(this);

            List<PossibleAction> aListPossibleAction = new List<PossibleAction>();

            // if the current cell is empty cell or with wind
            if (p_iStateEnv == 0 || p_iStateEnv == 2)
            {
                bool bFlagOK = false;
                // If there are some remaining safe cells, move there.
                if (m_lfcCellsOK.Any())
                {
                    int iCost = int.MaxValue;
                    ForestCell fcDestination = m_lfcCellsOK[new Random().Next(0, m_lfcCellsOK.Count)];
                    foreach (ForestCell fcOK in m_lfcCellsOK)
                    {
                        if (!fcOK.AlreadyVisited)
                        {
                            Pathfinding.InitCost(this, fcOK);
                            // Find the fastest safe path to the destination cell.
                            List<ForestCell> PathFound = Pathfinding.FindPath(this, fcOK);
                            Pathfinding.ResetGridCost();

                            if (PathFound.Count < iCost)
                            {
                                fcDestination = fcOK;
                                iCost = PathFound.Count;
                                bFlagOK = true;
                            }
                        }
                    }
                    if (bFlagOK)
                    {
                        Move paActToMove = new Move(this, fcDestination, iCost);
                        aListPossibleAction.Add(paActToMove);
                    }
                }

                bool bFlagSuspicious = false;
                // Else go to a cell with smell to kill monster.
                if (m_lfcCellsWithRadiation.Any() && !bFlagOK)
                {
                    int iCost = int.MaxValue;
                    ForestCell dest = null;
                    foreach (ForestCell fcRadiation in m_lfcCellsWithRadiation)
                    {
                        Pathfinding.InitCost(this, fcRadiation);
                        // Find the fastest safe path to the destination cell.
                        List<ForestCell> lfcPathFound = Pathfinding.FindPath(this, fcRadiation);
                        Pathfinding.ResetGridCost();

                        // If the new path is better than the old one.
                        if (lfcPathFound.Count < iCost)
                        {
                            dest = fcRadiation;
                            iCost = lfcPathFound.Count;
                            bFlagSuspicious = true;
                        }
                    }
                    if (bFlagSuspicious)
                    {
                        Move paActToMove = new Move(this, dest, iCost);
                        aListPossibleAction.Add(paActToMove);
                    }
                }
                if (!bFlagSuspicious && !bFlagOK)
                {
                    // Else try the closest suspicious cell.
                    if (m_lfcCellsSuspicous.Any())
                    {
                        bool bPathFound = false;
                        int iCost = int.MaxValue;
                        ForestCell fcDestination = null;
                        foreach (ForestCell fcSuspicious in m_lfcCellsSuspicous)
                        {
                            if (!fcSuspicious.AlreadyVisited)
                            {
                                Pathfinding.InitCost(this, fcSuspicious);
                                // Find the fastest safe path to the destination cell.
                                List<ForestCell> lfcPathFound = Pathfinding.FindPath(this, fcSuspicious);
                                Pathfinding.ResetGridCost();

                                // If the new path is better than the old one.
                                if (lfcPathFound.Count < iCost)
                                {
                                    fcDestination = fcSuspicious;
                                    iCost = lfcPathFound.Count;
                                    bPathFound = true;
                                }
                            }
                        }
                        if (bPathFound)
                        {
                            Move paActToMove = new Move(this, fcDestination, iCost);
                            aListPossibleAction.Add(paActToMove);
                        }
                    }
                }
            }

            // If the current cell contains radiation.
            if (p_iStateEnv == 1)
            {
                bool bCellOKFlag = false;
                // If there are some remaining safe cells, move there.
                if (m_lfcCellsOK.Any())
                {
                    int iCost = int.MaxValue;
                    ForestCell fcDestination = null;
                    foreach (ForestCell fcOK in m_lfcCellsOK)
                    {
                        if (!fcOK.AlreadyVisited)
                        {
                            Pathfinding.InitCost(this, fcOK);
                            // Find the fastest safe path to the destination cell.
                            List<ForestCell> lfcPathFound = Pathfinding.FindPath(this, fcOK);
                            Pathfinding.ResetGridCost();

                            // If the new path is better than the old one.
                            if (lfcPathFound.Count < iCost)
                            {
                                fcDestination = fcOK;
                                iCost = lfcPathFound.Count;
                                bCellOKFlag = true;
                            }
                        }
                    }
                    // If we found a path.
                    if (bCellOKFlag)
                    {
                        Move paActToMove = new Move(this, fcDestination, iCost);
                        aListPossibleAction.Add(paActToMove);
                    }

                }
                // Else try to kill the alien.
                if (!bCellOKFlag)
                {
                    // Try to find a valid target i.e. cell in m_lfcCellsSuspicous and not the cell we come from.
                    List<ForestCell> lfcTargets = CurrentForestCell.getAdjacentCells();
                    // Remove cell visited just before from targets.
                    for (int i = lfcTargets.Count - 1; i >= 0; i--)
                    {
                        if (Memory[lfcTargets[i].LineIndex, lfcTargets[i].ColumnIndex].HasNoAlien == 1 ||
                            Memory[lfcTargets[i].LineIndex, lfcTargets[i].ColumnIndex].IsSafe == 1)
                        {
                            lfcTargets.Remove(lfcTargets[i]);
                        }
                    }

                    // Randomly choose target from remaining cells.
                    Random r = new Random();
                    int iTargetIndex = r.Next(0, lfcTargets.Count);

                    if (lfcTargets[iTargetIndex].ColumnIndex < m_fcCurrentForestCell.ColumnIndex)
                    {
                        aListPossibleAction.Add(paActToThrowRockLeft);
                    }
                    else
                    {
                        if (lfcTargets[iTargetIndex].ColumnIndex > m_fcCurrentForestCell.ColumnIndex)
                        {
                            aListPossibleAction.Add(paActToThrowRockRight);
                        }
                        else
                        {
                            if (lfcTargets[iTargetIndex].LineIndex < m_fcCurrentForestCell.LineIndex)
                            {
                                aListPossibleAction.Add(paActToThrowRockTop);
                            }
                            else
                            {
                                if (lfcTargets[iTargetIndex].LineIndex > m_fcCurrentForestCell.LineIndex)
                                {
                                    aListPossibleAction.Add(paActToThrowRockBottom);
                                }
                            }
                        }
                    }
                }
            }

            // Smell + Wind
            if (p_iStateEnv == 3)
            {
                bool bCellOKFlag = false;
                // If there are some remaining safe cells, move there
                if (m_lfcCellsOK.Any())
                {
                    int iCost = int.MaxValue;
                    ForestCell fcDestination = null;
                    foreach (ForestCell fcOK in m_lfcCellsOK)
                    {
                        if (!fcOK.AlreadyVisited)
                        {
                            Pathfinding.InitCost(this, fcOK);
                            // Find the fastest safe path to the destination cell.
                            List<ForestCell> lfcPathFound = Pathfinding.FindPath(this, fcOK);
                            Pathfinding.ResetGridCost();

                            // If the new path is better than the old one.
                            if (lfcPathFound.Count < iCost)
                            {
                                fcDestination = fcOK;
                                iCost = lfcPathFound.Count;
                                bCellOKFlag = true;
                            }
                        }
                    }
                    if (bCellOKFlag)
                    {
                        Move paActToMove = new Move(this, fcDestination, iCost);
                        aListPossibleAction.Add(paActToMove);
                    }

                }
                // Else try to kill monster and wait for result.
                if (!bCellOKFlag)
                {
                    List<ForestCell> lfcTargets = CurrentForestCell.getAdjacentCells();
                    // Remove cell visited just before from targets.
                    for (int i = lfcTargets.Count - 1; i >= 0; i--)
                    {
                        if (Memory[lfcTargets[i].LineIndex, lfcTargets[i].ColumnIndex].HasNoAlien == 1)
                        {
                            lfcTargets.Remove(lfcTargets[i]);
                        }
                    }
                    // if no more target, try random cell.
                    if (!lfcTargets.Any())
                    {
                        if (m_lfcCellsSuspicous.Any())
                        {
                            bool bPathFound = false;
                            int iCost = int.MaxValue;
                            ForestCell fcDestination = null;
                            foreach (ForestCell fcSuspicious in m_lfcCellsSuspicous)
                            {
                                if (!fcSuspicious.AlreadyVisited)
                                {
                                    Pathfinding.InitCost(this, fcSuspicious);
                                    List<ForestCell> PathFound = Pathfinding.FindPath(this, fcSuspicious);
                                    Pathfinding.ResetGridCost();

                                    if (PathFound.Count < iCost)
                                    {
                                        fcDestination = fcSuspicious;
                                        iCost = PathFound.Count;
                                        bPathFound = true;
                                    }
                                }
                            }
                            if (bPathFound)
                            {
                                Move paActToMove = new Move(this, fcDestination, iCost);
                                aListPossibleAction.Add(paActToMove);
                            }

                        }
                    }
                    else
                    {
                        // Randomly choose target from remaining cells.
                        Random r = new Random();
                        int iTargetIndex = r.Next(0, lfcTargets.Count);

                        if (lfcTargets[iTargetIndex].ColumnIndex < m_fcCurrentForestCell.ColumnIndex)
                        {
                            aListPossibleAction.Add(paActToThrowRockLeft);
                        }
                        else
                        {
                            if (lfcTargets[iTargetIndex].ColumnIndex > m_fcCurrentForestCell.ColumnIndex)
                            {
                                aListPossibleAction.Add(paActToThrowRockRight);
                            }
                            else
                            {
                                if (lfcTargets[iTargetIndex].LineIndex < m_fcCurrentForestCell.LineIndex)
                                {
                                    aListPossibleAction.Add(paActToThrowRockTop);
                                }
                                else
                                {
                                    if (lfcTargets[iTargetIndex].LineIndex > m_fcCurrentForestCell.LineIndex)
                                    {
                                        aListPossibleAction.Add(paActToThrowRockBottom);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            // Light
            if (p_iStateEnv == 4)
            {
                // GET OUT !
                aListPossibleAction.Add(paActToExit);
            }

            return aListPossibleAction;
        }

        /// <summary>
        /// Determine action upon my goal.
        /// No really used but here for the BDI implementation.
        /// </summary>
        /// <param name="p_iStateEnv"> Environment state. </param>
        /// <returns></returns>
        public PossibleAction DetermineActionUponMyGoal(int p_iStateEnv)
        {
            List<PossibleAction> lapListPossibleAction = ActionDeclenchable(p_iStateEnv);

            if (lapListPossibleAction != null)
            {
                // Initialises the index used to keep track of the best action to perform.
                int iIndexActionToDo = -1;

                // Each iteration, initialises the worthiness of the action currently evaluated.
                int iWorthiness = -1;

                for (int i = 0; i < lapListPossibleAction.Count; i++)
                {
                    int temp = CalculateWorthiness(lapListPossibleAction[i], m_sGoal, p_iStateEnv);
                    if (iWorthiness < temp)
                    {
                        iWorthiness = temp;
                        // If the current action is the most relevant, we keep its index in the list.
                        iIndexActionToDo = i;
                    }
                }
                // When we went through the whole list, the index returned is the one of the most relevant action.
                return lapListPossibleAction[iIndexActionToDo];
            }
            return null;
        }

        /// <summary>
        /// Do the most appropriate action.
        /// </summary>
        /// <param name="p_paAction"> Action to do. </param>
        public void DoAction(PossibleAction p_paAction)
        {
            if (p_paAction != null)
            {
                p_paAction.Act();
            }
        }

        /// <summary>
        /// Calculate worthitness of an action
        /// </summary>
        /// <param name="p_paAction"> Action. </param>
        /// <param name="p_sMyGoal"> Hero goal. </param>
        /// <param name="p_iStateEnv"> Environment state. </param>
        /// <returns></returns>
        private int CalculateWorthiness(PossibleAction p_paAction, string p_sMyGoal, int p_iStateEnv)
        {
            int iWorthiness = -1;

            /* Depending on the state of the environment and the goal, we logically attibute a number of point to each counter
             * This way, a particularly interesting action to perform will be set a high value of worthiness.
             */
            switch (p_iStateEnv)
            {
                // Empty cell.
                case 0:
                    if (p_paAction.Name() == "Move")
                    {
                        iWorthiness = 1;
                    }
                    if (p_paAction.Name() == "ThrowRockLeft")
                    {
                        iWorthiness = 0;
                    }
                    if (p_paAction.Name() == "ThrowRockRight")
                    {
                        iWorthiness = 0;
                    }
                    if (p_paAction.Name() == "ThrowRockTop")
                    {
                        iWorthiness = 0;
                    }
                    if (p_paAction.Name() == "ThrowRockBottom")
                    {
                        iWorthiness = 0;
                    }
                    break;
                // Cell with rad.
                case 1:
                    if (p_paAction.Name() == "Move")
                    {
                        iWorthiness = 0;
                    }
                    if (p_paAction.Name() == "ThrowRockLeft")
                    {
                        iWorthiness = 1;
                    }
                    if (p_paAction.Name() == "ThrowRockRight")
                    {
                        iWorthiness = 1;
                    }
                    if (p_paAction.Name() == "ThrowRockTop")
                    {
                        iWorthiness = 1;
                    }
                    if (p_paAction.Name() == "ThrowRockBottom")
                    {
                        iWorthiness = 1;
                    }
                    break;
                // Cell with Wind.
                case 2:
                    if (p_paAction.Name() == "Move")
                    {
                        iWorthiness = 1;
                    }
                    if (p_paAction.Name() == "ThrowRockLeft")
                    {
                        iWorthiness = 0;
                    }
                    if (p_paAction.Name() == "ThrowRockRight")
                    {
                        iWorthiness = 0;
                    }
                    if (p_paAction.Name() == "ThrowRockTop")
                    {
                        iWorthiness = 0;
                    }
                    if (p_paAction.Name() == "ThrowRockBottom")
                    {
                        iWorthiness = 0;
                    }
                    break;
                // Cell with wind + rad.
                case 3:
                    if (p_paAction.Name() == "Move")
                    {
                        iWorthiness = 1;
                    }
                    if (p_paAction.Name() == "ThrowRockLeft")
                    {
                        iWorthiness = 1;
                    }
                    if (p_paAction.Name() == "ThrowRockRight")
                    {
                        iWorthiness = 1;
                    }
                    if (p_paAction.Name() == "ThrowRockTop")
                    {
                        iWorthiness = 1;
                    }
                    if (p_paAction.Name() == "ThrowRockBottom")
                    {
                        iWorthiness = 1;
                    }
                    break;
                // Cell with light.
                case 4:
                    if (p_paAction.Name() == "Move")
                    {
                        iWorthiness = 0;
                    }
                    if (p_paAction.Name() == "ThrowRockLeft")
                    {
                        iWorthiness = 0;
                    }
                    if (p_paAction.Name() == "ThrowRockRight")
                    {
                        iWorthiness = 0;
                    }
                    if (p_paAction.Name() == "ThrowRockTop")
                    {
                        iWorthiness = 0;
                    }
                    if (p_paAction.Name() == "ThrowRockBottom")
                    {
                        iWorthiness = 0;
                    }
                    if (p_paAction.Name() == "Exit")
                    {
                        iWorthiness = 1;
                    }
                    break;

                default:
                    break;
            }

            // Then, we return the counter associated with the current goal of the agent.
            int iResult = -1;
            if (p_sMyGoal == "GETOUTOMG!")
            {
                iResult = iWorthiness;
            }
            return iResult;
        }

        /// <summary>
        /// Start the inference process to determine the action to do.
        /// </summary>
        public void Inference()
        {
            if (AmIAlive())
            {
                /* The function call execute the BDI model.
                 * - First we call GetEnvironmentState() which return the state of the environment.
                 * - The we call DetermineActionUponMyGoal() which determines which action will bring 
                 * the robot to its goal.
                 * Finally we call DoAction() which executes the action which has been chosen.
                 */
                GetEnvironmentState();
                DoAction(DetermineActionUponMyGoal(UpdateMyState()));
            }
        }
    }
}