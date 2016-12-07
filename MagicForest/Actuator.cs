using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagicForest
{
    public static class Actuator
    {
        public static void TurnLelf(Hero p_hHero)
        {
            int iCurrentDirectionIndex = p_hHero.m_asPossibleDirections.ToList().IndexOf(p_hHero.DirectionFacing);
            int iNewDirection = (iCurrentDirectionIndex - 1) < 0 ? iCurrentDirectionIndex + 3 : iCurrentDirectionIndex - 1;
            p_hHero.DirectionFacing = p_hHero.m_asPossibleDirections[iNewDirection];
            //p_hHero.Score--;
        }

        public static void TurnRight(Hero p_hHero)
        {
            int iCurrentDirectionIndex = p_hHero.m_asPossibleDirections.ToList().IndexOf(p_hHero.DirectionFacing);
            p_hHero.DirectionFacing = p_hHero.m_asPossibleDirections[(iCurrentDirectionIndex + 1) % 4];
            //p_hHero.Score--;
        }

        public static void GoForward(Hero p_hHero)
        {
            p_hHero.CurrentCell = p_hHero.getFrontCell();
            p_hHero.CurrentCell.AlreadyVisited = true;
            p_hHero.Score--;
        }

        public static void GoBackward(Hero p_hHero)
        {
            p_hHero.CurrentCell = p_hHero.getBackCell();
            p_hHero.CurrentCell.AlreadyVisited = true;
            p_hHero.Score--;
        }

        public static void ThrowRock(Hero p_hHero)
        {
            ForestCell fcTargetCell = p_hHero.getFrontCell();
            fcTargetCell.HasMonster = false;

            List<ForestCell> lfcAdjacentForectCells = fcTargetCell.getAdjacentCells();
            foreach (ForestCell fcForectCell in lfcAdjacentForectCells)
            {
                fcForectCell.HasPoop = false;
            }
            p_hHero.Score -= 10;
        }

        public static void Exit(Hero p_hHero)
        {
            p_hHero.Score = p_hHero.Score + 10 * p_hHero.MemorySize * p_hHero.MemorySize;

            
        }
    }
}
