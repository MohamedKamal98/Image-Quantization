using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Imaging;
using System.Collections;
using System.Collections.Specialized;

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
		/// Calculates eculidean distance between the given two edges
		/// </summary>
		/// <param name="rgb1"></param>
		/// <param name="rgb2"></param>
		/// <returns>double Eculidean Distance</returns>
        private double EculideanDistance(RGBPixel rgb1, RGBPixel rgb2)
        {
            return Math.Sqrt((Math.Pow((rgb1.red - rgb2.red), 2)) + (Math.Pow((rgb1.green - rgb2.green), 2)) + (Math.Pow((rgb1.blue - rgb2.blue), 2)));
        }
        // Hashtable that contains all unique colors 
        public static Hashtable distinctHashtable;
        // Array of distincit Colors 
        public static RGBPixel[] distinctColors;
        /// <summary>
        /// Gets the distinct colors from the matrix of colors
        /// </summary>
        /// <returns>Set of colors</returns>
        public static void GetDistinct(RGBPixel[,] M)
        {
            distinctHashtable = new Hashtable();
            distinctColors = new RGBPixel[M.GetLength(1) * M.GetLength(0)];
            int hashFunctionKey = 0;
            int Counter = 0;
            for (int i = 0; i < M.GetLength(0); i++)
            {
                for (int j = 0; j < M.GetLength(1); j++)
                {
                    // Calculates the Key of the hashtable from three integers (red,blue,green) 
                    hashFunctionKey = 8 * (M[i, j].green + M[i, j].blue) * (M[i, j].green + M[i, j].blue + 1) + M[i, j].blue;
                    hashFunctionKey = 5 * (hashFunctionKey + M[i, j].red) * (hashFunctionKey + M[i, j].red + 1) + M[i,j].red;

                    if (!distinctHashtable.ContainsKey(hashFunctionKey))
                    {
                        distinctHashtable.Add(hashFunctionKey, Counter);
                        distinctColors[Counter] = M[i, j];
                        Counter++;
                    }
                }
            }
        }

        /// Contains index of each edge in the queue
        public static int[] indeciesInQueue;
        
		/// <summary>
		/// Gets the minimum span tree using Prim's algorithm
		/// </summary>
		private void MSTPrim()
        {
            
			//Sumation of wieghts of the MST
            double MSTEdgesSum = 0;

			//Number of distinct colors
			int numberOfDistinctColors = distinctHashtable.Count;

			// result edges of MST
			MSTresult = new MinimumHeap(numberOfDistinctColors - 1);

			/// Temporary edge to inqueue in 'edges'
			Edge tmpEdge;

			/// Priority queue contains edges in order
			MinimumHeap edges = new MinimumHeap(numberOfDistinctColors - 1);


            indeciesInQueue = new int[numberOfDistinctColors];

			//Contains minimum wieght of each vertix
            double[] minimumWieght = new double[numberOfDistinctColors];
            // List of all non Visted Nodes 
            List<int> nonVisted = new List<int>();
            int tmpSource = 0;
            double tmpWieght;
            int tmpDistination;
            /// Loop to fill the minimum wieght of each vertex with the distination between each one of them and the first node which is the tmpSource O(D)
            for ( tmpDistination = 1; tmpDistination < numberOfDistinctColors; tmpDistination++)
            {
                        nonVisted.Add(tmpDistination);
                        tmpWieght = EculideanDistance(distinctColors[tmpSource], distinctColors[tmpDistination]);
                        tmpEdge = new Edge(tmpSource, tmpDistination, tmpWieght);
                        minimumWieght[tmpDistination] = tmpWieght;
                        indeciesInQueue[tmpDistination] = tmpDistination - 1;
                        // insert in the queue
                        edges.Insert(tmpEdge);
            }
            // take the min edge in the queue
            tmpEdge = edges.ExtractMinimum();
            // insert the min edge in the result MST queue
            MSTresult.Insert(tmpEdge);
            // the next source will be the min edge distination 
            tmpSource = tmpEdge.destnation;
            // remove the min edge distination from the nonVisted list as it become Vistied 
            nonVisted.Remove(tmpSource);
            // add the min edge Weight to the Summution of MST edges
            MSTEdgesSum += tmpEdge.weight;
            /// Nested Loop that Calculate the MST  O(Elog(V))
            /// first Loop O(E) that takes all edges in the queue 
            while (edges.HeapSize!=0)
            {
                /// Second Loop that Calculate the distance between the tmpSource and all of the non visted nodes O(Log(V))
                for (int i = 0; i < nonVisted.Count; i++)
                {
                    tmpDistination = nonVisted[i];
                    tmpWieght = EculideanDistance(distinctColors[tmpSource], distinctColors[tmpDistination]);
                    if (tmpWieght < minimumWieght[tmpDistination])
                    {
                        //Update edge at the index found in 'indeciesInQueue[tmpDistination]' with 'tmpSource' and 'tmpWieght'
                        edges.Update(indeciesInQueue[tmpDistination], tmpWieght, tmpSource);
                        //Update the minimum wieght of 'tmpDistination' with tmpWieght
                        minimumWieght[tmpDistination] = tmpWieght;
                    }
                }
                // take the min edge in the queue
                tmpEdge = edges.ExtractMinimum();
                // insert the min edge in the result MST queue
                MSTresult.Insert(tmpEdge);
                // the next source will be the min edge distination 
                tmpSource = tmpEdge.destnation;
                // remove the min edge distination from the nonVisted list as it become Vistied 
                nonVisted.Remove(tmpSource);
                // add the min edge Weight to the Summution of MST edges
                MSTEdgesSum += tmpEdge.weight;
            }
              
			//========================== TEST ===============================================
                      MessageBox.Show(distinctHashtable.Count.ToString() + "\n" + MSTEdgesSum.ToString());
			//========================== TEST ===============================================           
		}
		/// priority queue contains edges of MST
		MinimumHeap MSTresult;

		//colors palett
		RGBPixel[] P;

		public void Cluster(int K)
		{
			P = new RGBPixel[K];
			int[] RGBCount = new int[K]; 
			Hashtable T = new Hashtable();
			Edge edge;
			int RV = distinctHashtable.Count, RC = K, Pi = 0, src, dis, tmpIndex;
			bool srcFound = false, disFound = false;

			//O(K)
			while(RC != 0)
			{
				//O(logn)
				edge = MSTresult.ExtractMinimum();
				src = edge.source;
				dis = edge.destnation;
				//O(1)
				srcFound = T.ContainsKey(src);
				disFound = T.ContainsKey(dis);
				if(!srcFound || !disFound)
				{
					if (RV > RC)
					{
						if (!srcFound && !disFound)
						{
							RGBCount[Pi] = 2;

							//add the first 2 pixles in P[Pi]
							P[Pi].red += distinctColors[src].red;
							P[Pi].green += distinctColors[src].green;
							P[Pi].blue += distinctColors[src].blue;

							//divide the result over 2 to get the avarege
							P[Pi].red =(byte)((P[Pi].red + distinctColors[dis].red) / RGBCount[Pi]);
							P[Pi].green = (byte)((P[Pi].green + distinctColors[dis].green) / RGBCount[Pi]);
							P[Pi].blue = (byte)((P[Pi].blue + distinctColors[dis].blue) / RGBCount[Pi]);

							T.Add(src, Pi);
							T.Add(dis, Pi);
							Pi++;
							RV -= 2;
							RC--;
						}
						else if(srcFound && !disFound)
						{
							if (RV != RC)
							{
								tmpIndex = (int)T[src];

								//Accessing Hashtable O(1)
								P[tmpIndex].red = (byte)((P[tmpIndex].red*RGBCount[tmpIndex] + distinctColors[src].red)/++RGBCount[tmpIndex]);
								P[tmpIndex].green = (byte)((P[tmpIndex].green * RGBCount[tmpIndex] + distinctColors[src].green) / ++RGBCount[tmpIndex]);
								P[tmpIndex].blue = (byte)((P[tmpIndex].blue * RGBCount[tmpIndex] + distinctColors[src].blue) / ++RGBCount[tmpIndex]);

								T.Add(dis,(int)T[src]);
								RV--;
							}
						}
						else
						{
							if (RV != RC)
							{
								tmpIndex = (int)T[dis];

								P[tmpIndex].red = (byte)((P[tmpIndex].red * RGBCount[tmpIndex] + distinctColors[dis].red) / ++RGBCount[tmpIndex]);
								P[tmpIndex].green = (byte)((P[tmpIndex].green * RGBCount[tmpIndex] + distinctColors[dis].green) / ++RGBCount[tmpIndex]);
								P[tmpIndex].blue = (byte)((P[tmpIndex].blue * RGBCount[tmpIndex] + distinctColors[dis].blue) / ++RGBCount[tmpIndex]);


								T.Add(src, (int)T[dis]);
								RV--;
							}
						}
					}
					else
					{
						if (!srcFound)
						{
							P[Pi].red += distinctColors[src].red;
							P[Pi].green += distinctColors[src].green;
							P[Pi].blue += distinctColors[src].blue;

							T.Add(src, Pi);
							Pi++;
							RV--;
							RC--;
						}
						else
						{
							P[Pi].red += distinctColors[dis].red;
							P[Pi].green += distinctColors[dis].green;
							P[Pi].blue += distinctColors[dis].blue;

							T.Add(dis, Pi);

							Pi++;
							RV--;
							RC--;
						}
					}
				}

			}

		}


		public void TEST()
		{
			
			MSTPrim();
			Cluster(3);
		}


	}
}
