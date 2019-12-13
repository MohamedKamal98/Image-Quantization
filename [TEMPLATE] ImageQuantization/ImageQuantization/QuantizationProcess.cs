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
        MinimumHeap edges;
        MinimumHeap result;
        Edge e;
        // lw get hena tany hafsha5ak
        //private void calculateDistances()
        //{
        //    GetDistinct();
        //    int numberOfDistinctColors = distinct.Count;
        //    edges = new MinimumHeap(((numberOfDistinctColors)*(numberOfDistinctColors-1))/2);
        //    result = new MinimumHeap(numberOfDistinctColors - 1);
        //    for (int i = 0; i < numberOfDistinctColors-1; i++)
        //    {
        //        for (int j = i + 1; j < numberOfDistinctColors; j++)
        //        {
        //            double d = eculideanDistance(distinct.ElementAt(i), distinct.ElementAt(j));
        //            e = new Edge(i, j, d);
        //        }
        //    }
        //}

        /// <summary>
        ///         private SortedList<int, KeyValuePair<KeyValuePair<int, int>, double>> MST()
        //        {
        //            calculateDistances();
        //        int numberOfEdges = edges.Count;
        //        int numberOfVertices;
        //        int x, y, a = 0, b = 0;      
        //            for (int i=0;i<numberOfEdges; i++)
        //            {
        //                x = edges.ElementAt(i).Value.Key.Key;
        //                y = edges.ElementAt(i).Value.Key.Value;
        //                numberOfVertices = Vertices.Count();

        //                for (int j = 0; j<numberOfVertices; j++)
        //                {
        //                    if(Vertices.ElementAt(j).Contains(x))
        //                    {
        //                        a = j;
        //                        break;
        //                    }
        //}
        //                for (int j = 0; j<numberOfVertices; j++)
        //                {
        //                    if (Vertices.ElementAt(j).Contains(y))
        //                    {
        //                        b = j;
        //                        break;
        //                    }
        //                }

        //                if (a == b)
        //                    continue;
        //                else
        //                {
        //                    Vertices.ElementAt(a).UnionWith(Vertices.ElementAt(b));
        //Vertices.Remove(Vertices.ElementAt(b));
        //                    result.Add(index, new KeyValuePair<KeyValuePair<int, int>, double>(new KeyValuePair<int, int>(x, y), edges.ElementAt(i).Value.Value));
        //                    index++;
        //                }
        //            }
        //            return result;

        //        }

        /// </summary>
        /// <returns></returns>

        public void bate5()
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
                for(int j=0; j < numberOfDistinctColors; j++)
                {                
                    if (i == 0)
                    {
                        if (color[j] == 0)
                        {
                            d = eculideanDistance(distinct.ElementAt(source), distinct.ElementAt(j));
                            e = new Edge(source, j, d);
                            min[j] = d;
                            dist[j]=j-1;
                            // insert in the queue
                            edges.Insert(e);
                        }
                    }
                    else
                    {
                        if (color[j] == 0 && i!=j)
                        {
                            d = eculideanDistance(distinct.ElementAt(source), distinct.ElementAt(j));
                            if (d < min[j])
                            {
                                //update the edge with wight min[j] && j distenation
                                //update(dis[j],new wieght,new parent);
                                edges.Updaet(dist[j], d, source);
                                min[j] = d;
                            }

                        }
                    }
                                       
                }
                e = edges.ExtractMinimum();
                source = e.destnation;
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
