using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagicForest
{
    /// <summary>
    /// Class to handle character perception of the environment.
    /// </summary>
    public static class Sensor
    {
        /// <summary>
        /// Test if cell is empty.
        /// </summary>
        /// <param name="p_fcCell"> Current cell. </param>
        /// <returns> Tru if cell is empty, false otherwise. </returns>
        public static bool HasNothing(ForestCell p_fcCell)
        {
            return p_fcCell.HasNothing;
        }

        /// <summary>
        /// Check if cell has light.
        /// </summary>
        /// <param name="p_fcCell"> Current cell. </param>
        /// <returns> True if cell has portal, false otherwise. </returns>
        public static bool HasLight(ForestCell p_fcCell)
        {
            return p_fcCell.HasPortal;
        }

        /// <summary>
        /// Check if cell has wind.
        /// </summary>
        /// <param name="p_fcCell"> Current cell. </param>
        /// <returns> True if cell has wind, false otherwise. </returns>
        public static bool HasWind(ForestCell p_fcCell)
        {
            return p_fcCell.HasWind;
        }

        /// <summary>
        /// Check if cell has radiation.
        /// </summary>
        /// <param name="p_fcCell"> Current cell. </param>
        /// <returns> True if cell has radiation, false otherwise. </returns>
        public static bool HasRadiation(ForestCell p_fcCell)
        {
            return p_fcCell.HasRadiation;
        }

    }
}
