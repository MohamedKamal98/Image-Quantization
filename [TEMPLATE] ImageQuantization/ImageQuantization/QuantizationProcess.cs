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

        
		

		/// <summary>
		/// Get the weight (eculidean Distance) between 2 vertices
		/// </summary>
		/// <param name="rgb1"></param>
		/// <param name="rgb2"></param>
		/// <returns>double weight </returns>
        private double eculideanDistance(RGBPixel rgb1, RGBPixel rgb2)
        {
			double res;
            return  res = Math.Sqrt((Math.Pow((rgb1.red - rgb2.red), 2)) + (Math.Pow((rgb1.green - rgb2.green), 2)) + (Math.Pow((rgb1.blue - rgb2.blue), 2)));
        }


	//contains distinct colors
		HashSet<RGBPixel> distinct = new HashSet<RGBPixel>();
		/// <summary>
		/// Get the list of Distinct Colors
		/// </summary>
		/// <returns>HashSet of distinct colors</returns>
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


		///<summary>Array of HashSets<int> to contain the sets of vertices</summary>
		HashSet<int>[] Vertices;
		/// <summary>
		/// sets all vertices in sets
		/// </summary>
		private void SetVertices()
		{
			HashSet<int> NewV;
			Vertices = new HashSet<int>[distinct.Count];
			for (int i = 0; i < Vertices.Length; i++)
			{
				NewV = new HashSet<int>();
				NewV.Add(i);
				Vertices[i] = NewV;
			}
		}

		///<summary>HashSet<Edge> to contain the MST edges</summary>
		HashSet<Edge> result = new HashSet<Edge>();
		///<summary>Array of Edges to contain All edges</summary>
		Edge[] Edges;
		///<summary>Binary Heap used for sorting the array of edges</summary>
		HeapSort heap = new HeapSort();


		/// <summary>
		/// Calculate the weights of each edge and fill the array of edges 
		/// </summary>
		private void calculateDistances()
        {
            GetDistinct();
			SetVertices(); 

			Edges = new Edge[(Vertices.Length * (Vertices.Length - 1)) / 2];
			int c = 0;
			
            for (int i = 0; i < Vertices.Length; i++)
            {
                for (int j = i+1; j < Vertices.Length; j++)
                {
						Edges[c] = new Edge(i, j, eculideanDistance(distinct.ElementAt(i), distinct.ElementAt(j)));
						c++;
                }
            }

			//Sorting the array of edges
			heap.Sort(Edges);

        }


        
		/// <summary>
		/// Calculates the Minimum Spanning Tree of the distinct colors
		/// </summary>
		/// <returns>HashSet<Edge> contains the Edges of MST</returns>
        private HashSet<Edge> MST()
        {
			//for testing summition in sample cases
			double sum = 0;


            calculateDistances();
			
			///<summary>
			///x = source of an edge
			///y = distination of an edge
			///a = index of the set contains the vertex 'x' in the array Vertices
			///b = index of the set contains the vertex 'y' in the array Vertices
			///start= start of the array Vertices
			///end= end of the array Vertices
			/// </summary>
			int x, y, a=0, b=0,start=0,end=Vertices.Length-1;  
			
			///<summary> 
			///this loop iterates on the arranged edges and 
			///in each eteration findes the index of source and
			///the distination using binary search
			/// </summary>
            for (int i=0;i< Edges.Length; i++)
            {
                x = Edges[i].source;
                y = Edges[i].destnation;

				a = find(x, start, end);
				b = find(y, start, end);
				if (a != b)
				{
					//Union the 2 Subsets
					Vertices[a].UnionWith(Vertices[b]);

				//===deleting the Left out Subset===
					//if the left out in the end
					if (b == end)
						end--;
					//if the left out in the start
					else if (b == start)
						start++;
					//if the left out in the between
					else
					{   // Shifting elements from the end to b
						for (int k = b; k < end; k++)
						{
							Vertices[k] = Vertices[k + 1];
						}
						end--;
					}
			    //===deleting the Left out Subset===

					result.Add(new Edge(x, y, Edges[i].weight));
					sum += Edges[i].weight;
				}

				if (Vertices[a].Count == Vertices.Length)
					break;
            }

		//for testing summition in sample cases
			MessageBox.Show(sum.ToString());
            return result;

        }

		/// <summary>
		/// searches on the index of the set that contains a specific
		/// vertex in the array of subsets of vertices using binary search
		/// </summary>
		/// <param name="v"></param>
		/// <param name="start"></param>
		/// <param name="end"></param>
		/// <returns>index of the set that contains the vertex v</returns>
		private int find(int v, int start, int end)
		{
			int result=0;
			for (int i = start; i < end; )
			{
				int mid = (i + end) / 2;

				if (Vertices[i].Contains(v))
				{
					result = start;
					break;
				}

				else if (Vertices[end].Contains(v))
				{
					result = end;
					break;
				}
				else if (Vertices[mid].Contains(v))
				{
					result = mid;
					break;
				}
				else if (Vertices[mid].ElementAt(0) < v)
					i = mid+1;
				else
					end = mid-1;
			}
			return result;
		}

		/// <summary>
		/// :) دي بطيخ عادي 
		/// </summary>
        public void bate5()
        {
           MST();

        }

    }
    
        
}
