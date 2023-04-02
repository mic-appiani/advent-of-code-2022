using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Utils
{
    public static class Utils
    {
        public static int Convert2DIndexToLinear(int row, int column, int totalColumns)
        {
            return row * totalColumns + column;
        }

        public static (int row, int column) ConvertLinearIndexTo2D(int linearIndex, int totalColumns)
        {
            int row = linearIndex / totalColumns;
            int column = linearIndex % totalColumns;
            return (row, column);
        }
    }
}
