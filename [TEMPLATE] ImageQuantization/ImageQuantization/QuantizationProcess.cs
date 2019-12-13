using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Imaging;

namespace ImageQuantization
{
    
    /// <summary>
	///class Edge has 3 properties source,destination,weight
	/// </summary>
	public class Edge
    {
        public int source, destnation;
        public double weight = 0;
        /// <summary>
        ///Constructor of Edge
        /// </summary>
        /// <param name="src"></param>
        /// <param name="dest"></param>
        /// <param name="w"></param>
        public Edge(int src, int dest, double w)
        {
            source = src;
            destnation = dest;
            weight = w;
            //this.indexInQueue = indexInQueue;
        }

    }


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

        HashSet<int> nonVistedVertices = new HashSet<int>();
        HashSet<RGBPixel> distinct = new HashSet<RGBPixel>();

        private double eculideanDistance(RGBPixel rgb1, RGBPixel rgb2)
        {
            return Math.Sqrt((Math.Pow((rgb1.red - rgb2.red), 2)) + (Math.Pow((rgb1.green - rgb2.green), 2)) + (Math.Pow((rgb1.blue - rgb2.blue), 2)));
        }

        public HashSet<RGBPixel> GetDistinct()
        {
            int counter = 0;
            for (int i = 0; i < getMatrix().GetLength(0); i++)
            {
                for (int j = 0; j < getMatrix().GetLength(1); j++)
                {
                    if (!distinct.Contains(getMatrix()[i, j]))
                    {
                        distinct.Add(getMatrix()[i, j]);
                        nonVistedVertices.Add(counter);
                        counter++;
                    }
                        
                }
            }
            // remove zero as it is my source at the first 
            nonVistedVertices.Remove(0);
            return distinct;
        }
        MinimumHeap edges;
        MinimumHeap result;
        Edge e;

        public void TEST()
        {
            prim();
        }
        public static int[] dist;
        private void prim()
        {
            // calculateDistances();
            GetDistinct();
            
            double s = 0;
            int numberOfDistinctColors = distinct.Count;
            edges = new MinimumHeap(numberOfDistinctColors-1);
            result = new MinimumHeap(numberOfDistinctColors - 1);
            int x = 0;
            KeyValuePair<int, int>[] r = new KeyValuePair<int, int>[numberOfDistinctColors];
            dist = new int[numberOfDistinctColors];
            int[] color = new int[numberOfDistinctColors];
            double[] min = new double[numberOfDistinctColors];
            int source = 0;
            double d;
            int count = 0;
            for (int i=0;i<numberOfDistinctColors;i++)
            {
                if (count == numberOfDistinctColors - 1)
                {
                    break;
                }
                color[source] = 1;
                for(int j=0; j < nonVistedVertices.Count; j++)
                {                
                    if (i == 0)
                    {
                        
                            d = eculideanDistance(distinct.ElementAt(source), distinct.ElementAt(nonVistedVertices.ElementAt(j)));
                            e = new Edge(source, nonVistedVertices.ElementAt(j), d);
                            min[nonVistedVertices.ElementAt(j)] = d;
                            dist[nonVistedVertices.ElementAt(j)] = nonVistedVertices.ElementAt(j) - 1;
                            // insert in the queue
                            edges.Insert(e);
                        
                    }
                    else
                    {
                       // if (color[nonVistedVertices.ElementAt(j)] == 0 && source!=j)
                        //{
                            d = eculideanDistance(distinct.ElementAt(source), distinct.ElementAt(nonVistedVertices.ElementAt(j)));
                            if (d < min[nonVistedVertices.ElementAt(j)])
                            {
                                edges.Updaet(dist[nonVistedVertices.ElementAt(j)], d, source);
                                min[nonVistedVertices.ElementAt(j)] = d;
                            }

                       // }
                    }
                                       
                }
                e = edges.ExtractMinimum();
                source = e.destnation;
                nonVistedVertices.Remove(source);
                color[e.destnation] = 1;
                s += e.weight;
                r[x] = new KeyValuePair<int, int>(e.source, e.destnation);
                x++;
                count++;
            }
                      MessageBox.Show(distinct.Count.ToString() + "\n" + s.ToString());
           
        }


    }
}
