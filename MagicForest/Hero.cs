using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagicForest
{
    public class Hero
    {
        private string m_sDirectionFacing;
        public readonly string[] m_asPossibleDirections = new string[4] { "top", "right", "bottom", "left" };
        private int m_iScore;
        private ForestCell m_fcCurrentCell;
        private bool m_bStillAlive = true;
        private bool m_bSmellDetected = false;
        private bool m_bWindDetected = false;
        private bool m_bLightDetected = false;
        private string m_sGoal = "GETOUTOMG!";

        public string DirectionFacing
        {
            get
            {
                return m_sDirectionFacing;
            }
            set
            {
                m_sDirectionFacing = value;
            }
        }

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

        public ForestCell CurrentCell
        {
            get
            {
                return m_fcCurrentCell;
            }
            set
            {
                m_fcCurrentCell = value;
                // UPDATE SPRITE EMPLACEMENT
            }
        }

        public bool StillAlive
        {
            set
            {
                MainWindow.StopExecution();
            }
        }

        public Hero()
        {
            m_sDirectionFacing = m_asPossibleDirections[1];
            m_iScore = 0;
            //m_fcCurrentCell = MainWindow.Forest[0, 0];
        }

        public ForestCell getFrontCell()
        {
            ForestCell fcResult = null;
            switch (m_sDirectionFacing)
            {
                case "top":
                    fcResult = MainWindow.Forest[m_fcCurrentCell.LineIndex - 1, m_fcCurrentCell.LineIndex];
                    break;
                case "right":
                    fcResult = MainWindow.Forest[m_fcCurrentCell.LineIndex, m_fcCurrentCell.LineIndex + 1];
                    break;
                case "bottom":
                    fcResult = MainWindow.Forest[m_fcCurrentCell.LineIndex + 1, m_fcCurrentCell.LineIndex];
                    break;
                case "left":
                    fcResult = MainWindow.Forest[m_fcCurrentCell.LineIndex, m_fcCurrentCell.LineIndex - 1];
                    break;
                default:
                    break;
            }
            return fcResult;
        }

        public ForestCell getBackCell()
        {
            ForestCell fcResult = null;
            switch (m_sDirectionFacing)
            {
                case "top":
                    fcResult = MainWindow.Forest[m_fcCurrentCell.LineIndex + 1, m_fcCurrentCell.LineIndex];
                    break;
                case "right":
                    fcResult = MainWindow.Forest[m_fcCurrentCell.LineIndex, m_fcCurrentCell.LineIndex - 1];
                    break;
                case "bottom":
                    fcResult = MainWindow.Forest[m_fcCurrentCell.LineIndex - 1, m_fcCurrentCell.LineIndex];
                    break;
                case "left":
                    fcResult = MainWindow.Forest[m_fcCurrentCell.LineIndex, m_fcCurrentCell.LineIndex + 1];
                    break;
                default:
                    break;
            }
            return fcResult;
        }

        public bool AmIAlive()
        {
            return m_bStillAlive;
        }

        public void GetEnvironmentState()
        {
            m_bLightDetected = Sensor.HasLight(m_fcCurrentCell);
            m_bSmellDetected = Sensor.HasSmell(m_fcCurrentCell);
            m_bWindDetected = Sensor.HasWind(m_fcCurrentCell);
        }

        public int UpdateMyState()
        {
            int iResultState = 0;

            if (m_bSmellDetected == true)
            {
                iResultState = 1;
            }
            if (m_bWindDetected == true)
            {
                iResultState = 2;
            }
            if (m_bLightDetected == true)
            {
                iResultState = 3;
            }
            if (m_fcCurrentCell.HasMonster == true)
            {
                iResultState = 4;
            }
            if (m_fcCurrentCell.HasHole == true)
            {
                iResultState = 5;
            }

            return iResultState;
        }

        private List<PossibleAction> ActionDeclenchable(int p_iStateEnv)
        {
            //TurnLeft apActToTurnLeft = new TurnLeft(this);
            //TurnRight apActToTurnRIght = new TurnRight(this);
            GoBackward apActToGoBackward = new GoBackward(this);
            GoForward apActToGoForward = new GoForward(this);
            ThrowRock apActToThrowRock = new ThrowRock(this);
            Exit apActToExit = new Exit(this);

            List<PossibleAction> aListActionPossible = new List<PossibleAction>();

            // Empty cell
            if (p_iStateEnv == 0)
            {
                
            }
            // Smell
            if (p_iStateEnv == 1)
            {
                
            }
            // Wind
            if (p_iStateEnv == 2)
            {
                
            }
            // Light
            if (p_iStateEnv == 3)
            {
                
            }
            // Monster
            if (p_iStateEnv == 4)
            {
                m_bStillAlive = false;
                MainWindow.StopExecution();
            }
            // Hole
            if (p_iStateEnv == 5)
            {
                m_bStillAlive = false;
                MainWindow.StopExecution();
            }

            return aListActionPossible;
        }

        public PossibleAction DetermineActionUponMyGoal(int p_iStateEnv)
        {
            List<PossibleAction> lapListPossibleAction = ActionDeclenchable(p_iStateEnv);

            // Initialises the index used to keep track of the best action to perform.
            int iIndexActionToDo = -1;

            // Each iteration, initialises the worthiness of the action currently evaluated.
            int iWorthiness = -1;

            for (int i = 0; i < lapListPossibleAction.Count; i++)
            {
                if (iWorthiness < CalculateWorthiness(lapListPossibleAction[i], m_sGoal, p_iStateEnv))
                {
                    iWorthiness = CalculateWorthiness(lapListPossibleAction[i], m_sGoal, p_iStateEnv);
                    // If the current action is the most relevant, we keep its index in the list.
                    iIndexActionToDo = i;
                }
            }
            // When we went through the whole list, the index returned is the one of the most relevant action.
            return lapListPossibleAction[iIndexActionToDo];
        }

        public void DoAction(PossibleAction p_paAction)
        {
            p_paAction.Act();
        }

        private int CalculateWorthiness(PossibleAction p_paAction, string p_sMyGoal, int p_iStateEnv)
        {
            int iWorthiness = -1;

            /* Depending on the state of the environment and the goal, we logically attibute a number of point to each counter
             * This way, a particularly interesting action to perform will be set a high value of worthiness */
            switch (p_iStateEnv)
            {
                // Empty
                case 0:
                    if (p_paAction.Name() == "GoBackward")
                    {
                        iWorthiness = 1;
                    }
                    if (p_paAction.Name() == "GoForward")
                    {
                        iWorthiness = 1;
                    }
                    if (p_paAction.Name() == "ThrowRock")
                    {
                        iWorthiness = 0;
                    }
                    if (p_paAction.Name() == "Exit")
                    {
                        iWorthiness = 0;
                    }
                    break;
                // Smell
                case 1:
                    if (p_paAction.Name() == "GoBackward")
                    {
                        iWorthiness = 0;
                    }
                    if (p_paAction.Name() == "GoForward")
                    {
                        iWorthiness = 0;
                    }
                    if (p_paAction.Name() == "ThrowRock")
                    {
                        iWorthiness = 1;
                    }
                    if (p_paAction.Name() == "Exit")
                    {
                        iWorthiness = 0;
                    }
                    break;
                // Wind
                case 2:
                    if (p_paAction.Name() == "GoBackward")
                    {
                        iWorthiness = 1;
                    }
                    if (p_paAction.Name() == "GoForward")
                    {
                        iWorthiness = 0;
                    }
                    if (p_paAction.Name() == "ThrowRock")
                    {
                        iWorthiness = 0;
                    }
                    if (p_paAction.Name() == "Exit")
                    {
                        iWorthiness = 0;
                    }
                    break;
                // Light
                case 3:
                    if (p_paAction.Name() == "GoBackward")
                    {
                        iWorthiness = 0;
                    }
                    if (p_paAction.Name() == "GoForward")
                    {
                        iWorthiness = 0;
                    }
                    if (p_paAction.Name() == "ThrowRock")
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

    }
}