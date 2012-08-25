using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CarmineEngine
{

    public static class Util
    {

        public static int Clamp(int integer, int maxValue, int minValue)
        {
            if (integer > maxValue)
                return maxValue;
            if (integer < minValue)
                return minValue;
            return integer;
        }
    }
}
