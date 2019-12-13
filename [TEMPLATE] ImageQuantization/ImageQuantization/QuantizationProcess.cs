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
		/// <summary>
		/// Calculates eculidean distance between the given two edges
		/// </summary>
		/// <param name="rgb1"></param>
		/// <param name="rgb2"></param>
		/// <returns>double Eculidean Distance</returns>
        private double EculideanDistance(RGBPixel rgb1, RGBPixel rgb2)
        {
            return Math.Sqrt((Math.Pow((rgb1.red - rgb2.red), 2)) + (Math.Pow((rgb1.green - rgb2.green), 2)) + (Math.Pow((rgb1.blue - rgb2.blue), 2)));
        }
		/// <summary>
		/// Gets the distinct colors from the matrix of colors
		/// </summary>
		/// <returns>Set of colors</returns>
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


		/// Contains index of each edge in the queue
		public static int[] indeciesInQueue;

		/// <summary>
		/// Gets the minimum span tree using Prim's algorithm
		/// </summary>
		private void MSTPrim()
        {

			//Sumation of wieghts of the MST
            double sum = 0;

			//Number of distinct colors
			int numberOfDistinctColors = distinct.Count;

			/// Temporary edge to inqueue in 'edges'
			Edge tmpEdge;

			/// Priority queue contains edges in order
			MinimumHeap edges = new MinimumHeap(numberOfDistinctColors - 1);

			/// priority queue contains edges of MST
			MinimumHeap result = new MinimumHeap(numberOfDistinctColors - 1);

            indeciesInQueue = new int[numberOfDistinctColors];
			
			//To check if vertix is visited or not
			//each element is 1 if the vertix of this element is visited, or 0 if is not visited
            int[] color = new int[numberOfDistinctColors];
			
			//Contains minimum wieght of each vertix
            double[] minimumWieght = new double[numberOfDistinctColors];

            int tmpSource = 0;
            double tmpWieght;
			//Loops on all vertices unless the last vertix O(V-1)
            for (int i=0;i<numberOfDistinctColors-1;i++)
            {
                color[tmpSource] = 1;
				//Loops on all edges connected to the current vertix 'tmpSource'
                for(int tmpDistination=0; tmpDistination < numberOfDistinctColors; tmpDistination++)
                {
					//If the current vertix is the initial vertix
					if (i == 0)
                    {
						//If the distination vertix is not visited
						if (color[tmpDistination] == 0)
                        {
                            tmpWieght = EculideanDistance(distinct.ElementAt(tmpSource), distinct.ElementAt(tmpDistination));
                            tmpEdge = new Edge(tmpSource, tmpDistination, tmpWieght);
                            minimumWieght[tmpDistination] = tmpWieght;
                            indeciesInQueue[tmpDistination]=tmpDistination-1;
                            // insert in the queue
                            edges.Insert(tmpEdge);
                        }
                    }
					//If the current vertix is NOT the initial
                    else
                    {
						//If the distination vertix is NOT visited and the source is not the distination
                        if (color[tmpDistination] == 0 && tmpSource!=tmpDistination)
                        {
                            tmpWieght = EculideanDistance(distinct.ElementAt(tmpSource), distinct.ElementAt(tmpDistination));
                            if (tmpWieght < minimumWieght[tmpDistination])
                            {
                                //Update edge at the index found in 'indeciesInQueue[tmpDistination]' with 'tmpSource' and 'tmpWieght'
                                edges.Update(indeciesInQueue[tmpDistination], tmpWieght, tmpSource);
								//Update the minimum wieght of 'tmpDistination' with tmpWieght
                                minimumWieght[tmpDistination] = tmpWieght;
                            }
                        }
                    }           
                }
				//Moving to the next minimum vertix 
                tmpEdge = edges.ExtractMinimum();
                tmpSource = tmpEdge.destnation;
                color[tmpEdge.destnation] = 1;
                sum += tmpEdge.weight;
            }

			//========================== TEST ===============================================
                      MessageBox.Show(distinct.Count.ToString() + "\n" + sum.ToString());
			//========================== TEST ===============================================           
		}
		public void TEST()
		{
			GetDistinct();
			MSTPrim();
		}


	}
}
