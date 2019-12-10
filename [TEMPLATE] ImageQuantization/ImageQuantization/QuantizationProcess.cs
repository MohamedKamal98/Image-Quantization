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
		public RGBPixel[,] GetMatrix()
        {
            return MainForm.ImageMatrix;
        }

        
		

		/// <summary>
		/// Get the weight (eculidean Distance) between 2 vertices
		/// </summary>
		/// <param name="rgb1"></param>
		/// <param name="rgb2"></param>
		/// <returns>double weight </returns>
        private double EculideanDistance(RGBPixel rgb1, RGBPixel rgb2)
        {
			double res;
			return res = Math.Sqrt(Math.Pow(rgb1.red - rgb2.red, 2) + Math.Pow(rgb1.green - rgb2.green, 2) + Math.Pow(rgb1.blue - rgb2.blue, 2));
        }


	//contains distinct colors
		HashSet<RGBPixel> distinct = new HashSet<RGBPixel>();
	//contains the parent of each vertix(the parent of each vertix is intialized with itself)
		int[] parent;
	//contains the rank(depth) of each vertix(initialized by zero)
		int[] rank;

		/// <summary>
		/// Get the list of Distinct Colors
		/// </summary>
		/// <returns>HashSet of distinct colors</returns>
		public HashSet<RGBPixel> GetDistinctColors()
        {
            for (int i = 0; i < GetMatrix().GetLength(0); i++)
            {
                for (int j = 0; j < GetMatrix().GetLength(1); j++)
                {
                    if (!distinct.Contains(GetMatrix()[i, j]))
                        distinct.Add(GetMatrix()[i, j]);
                }
            }
            return distinct;
        }
		/// <summary>
		/// sets all vertices in sets
		/// </summary>
		private void SetVertices()
		{
			parent = new int[distinct.Count];
			rank = new int[distinct.Count];
			for (int i = 0; i < distinct.Count; i++)
			{
				parent[i] = i;
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
            GetDistinctColors();
			SetVertices(); 

			Edges = new Edge[(distinct.Count * (distinct.Count - 1)) / 2];
			int c = 0;
			
            for (int i = 0; i < distinct.Count; i++)
            {
                for (int j = i+1; j < distinct.Count; j++)
                {
						Edges[c] = new Edge(i, j, EculideanDistance(distinct.ElementAt(i), distinct.ElementAt(j)));
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
			//the summation of weights of final MST
			double sum = 0;

			calculateDistances();
		// 'x', 'y' are the source and distination of the current edge
		// 'rootX', 'rootY' are the roots of 'x' and 'y' in the tree of desjoint set
		// 'i' is the counter of the loop
			int rootX, rootY, x, y, i=0;

			while (result.Count != distinct.Count-1)
			{
				x = Edges[i].source;
				y = Edges[i].destnation;

				rootX = find(x);
				rootY = find(y);

				// if 'x' and 'y' are not in the same group(don't have the same root)
				if (rootX != rootY)
				{
					//the parent of the root with lower rank = the root with the greater rank
					if (rank[rootX] < rank[rootY])
						parent[rootX] = rootY;

					else if (rank[rootY] < rank[rootX])
						parent[rootY] = rootX;
					
					//if both roots has has the same rank
					//put one of them as a parent of the other, then increment the rank of the parent
					else
					{
						parent[rootY] = rootX;
						rank[rootX]++;
					}
					result.Add(new Edge(x, y, Edges[i].weight));
					sum += Edges[i].weight;
				}
				i++;
			}

			//===============TEST summition in sample cases======================
			MessageBox.Show(distinct.Count.ToString() + "\n" + sum.ToString());
			//===============TEST summition in sample cases======================
			return result;
		}

		/// <summary>
		/// Finds the parent of the given vertix
		/// </summary>
		/// <param name="v"></param>
		/// <returns>the parent of 'v'</returns>
		private int find(int v)
		{
			//if parent of 'v' is not 'v'
			if (parent[v] != v)
				//recurcively find the parent of 'v' 
				parent[v] = find(parent[v]);

			return parent[v];
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
