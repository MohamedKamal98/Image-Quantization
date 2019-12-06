using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Imaging;

namespace ImageQuantization
{
    class QuantizationProcess
    {
        /// <summary>
        /// Get the 2D array of colors (size: Height x Width) from Main Form
        /// </summary>
        /// <returns>2D array of colors</returns>
        public RGBPixel[,] getMatrix()
        {
            return MainForm.ImageMatrix;
        }

    }
    
        
}
