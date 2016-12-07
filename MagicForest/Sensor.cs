using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagicForest
{
    public static class Sensor
    {
        public static bool HasNothing(ForestCell p_fcCell)
        {
            return p_fcCell.HasNothing;
        }

        public static bool HasLight(ForestCell p_fcCell)
        {
            return p_fcCell.HasPortal;
        }

        public static bool HasWind(ForestCell p_fcCell)
        {
            return p_fcCell.HasWind;
        }

        public static bool HasSmell(ForestCell p_fcCell)
        {
            return p_fcCell.HasPoop;
        }

    }
}
