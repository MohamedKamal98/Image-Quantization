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

        HashSet<RGBPixel> distinct = new HashSet<RGBPixel>();

        private double eculideanDistance(RGBPixel rgb1, RGBPixel rgb2)
        {
            return Math.Sqrt((Math.Pow((rgb1.red - rgb2.red), 2)) + (Math.Pow((rgb1.green - rgb2.green), 2)) + (Math.Pow((rgb1.blue - rgb2.blue), 2)));
        }

        public HashSet<RGBPixel> GetDistinct()
        {
            for (int i = 0; i < getMatrix().GetLength(0); i++)
            {
                for (int j = 0; j < getMatrix().GetLength(1); j++)
                {
                    if (!distinct.Contains(getMatrix()[i, j]))
                        distinct.Add(getMatrix()[i, j]);
                }
            }
            return distinct;
        }
        HashSet<HashSet<int>> Vertices = new HashSet<HashSet<int>>();
        SortedList<double, KeyValuePair<int, int>>edges = new SortedList<double, KeyValuePair<int, int>>();
        SortedList<double, KeyValuePair<int, int>> result = new SortedList<double, KeyValuePair<int, int>>();

        private void calculateDistances()
        {
            GetDistinct();
            int numberOfDistinctColors = distinct.Count;
            for (int i = 0; i < numberOfDistinctColors; i++)
            {
                Vertices.Add(new HashSet<int>());
                Vertices.ElementAt(i).Add(i + 1);
                for (int j = i+1; j < numberOfDistinctColors; j++)
                {
                    edges.Add(eculideanDistance(distinct.ElementAt(i), distinct.ElementAt(j)), new KeyValuePair<int, int>(i, j));
                }
            }

        }


        

        private SortedList<double, KeyValuePair<int, int>> MST()
        {
            calculateDistances();
            int numberOfEdges = edges.Count;
            int numberOfVertices;
            int x, y, a=0, b=0;      
            for (int i=0;i< numberOfEdges; i++)
            {
                x = edges.ElementAt(i).Value.Key;
                y = edges.ElementAt(i).Value.Value;
                numberOfVertices = Vertices.Count();

                for (int j = 0; j < numberOfVertices; j++)
                {
                    if(Vertices.ElementAt(j).Contains(x))
                    {
                        a = j;
                        break;
                    }
                }
                for (int j = 0; j < numberOfVertices; j++)
                {
                    if (Vertices.ElementAt(j).Contains(y))
                    {
                        b = j;
                        break;
                    }
                }

                if (a == b)
                    continue;
                else
                {
                    Vertices.ElementAt(a).UnionWith(Vertices.ElementAt(b));
                    Vertices.Remove(Vertices.ElementAt(b));
                    result.Add(edges.ElementAt(i).Key, new KeyValuePair<int, int>(x, y));
                }
            }
            return result;

        }

        public void bate5()
        {
            MST();

        }

    }
    
        
}
