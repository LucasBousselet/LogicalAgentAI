using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagicForest
{
    class Hero
    {
        private bool lightSensor;

        private bool stenchSensor;

        private bool windSensor;

        public int score;

        public Hero()
        {
            this.lightSensor = false;
            this.stenchSensor = false;
            this.windSensor = false;
            this.score = 0;
        }

        public bool HasLight
        {
            get
            {
                return lightSensor;
            }
            set
            {
                lightSensor = value;
            }
        }

        public bool HasStench
        {
            get
            {
                return stenchSensor;
            }
            set
            {
                stenchSensor = value;
            }
        }

        public bool HasWind
        {
            get
            {
                return windSensor;
            }
            set
            {
                windSensor = value;
            }
        }

        public void ThrowRock()
        {

        }
    }
}