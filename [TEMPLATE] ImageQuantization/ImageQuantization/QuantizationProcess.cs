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
            GetDistinctColors();
			SetVertices(); 

			Edges = new Edge[(Vertices.Length * (Vertices.Length - 1)) / 2];
			int c = 0;
			
            for (int i = 0; i < Vertices.Length; i++)
            {
                for (int j = i+1; j < Vertices.Length; j++)
                {
						Edges[c] = new Edge(i, j, EculideanDistance(distinct.ElementAt(i), distinct.ElementAt(j)));
						c++;
                }
            }

			//Sorting the array of edges
			heap.Sort(Edges);

        }


		/// <summary>Contains the chosen vertices from the array Vertcies</summary>
		public HashSet<HashSet<int>> ChosesEdges = new HashSet<HashSet<int>>();
        
		/// <summary>
		/// Calculates the Minimum Spanning Tree of the distinct colors
		/// </summary>
		/// <returns>HashSet<Edge> contains the Edges of MST</returns>
        private HashSet<Edge> MST()
        {
			//the summation of weights of final MST
			double sum = 0;
			//a flag used in storing the Edges of final MST in the set 'Result'
			bool storeEdge = false;

            calculateDistances();

			///<summary>
			///x = source of an edge
			///y = distination of an edge
			///a = index of the set that contains the vertex 'x' in the array Vertices or the list 'ChosenEdges' based on the Key(if true then a is index in ChosenEdges)
			///b = index of the set that contains the vertex 'y' in the array Vertices or the list 'ChosenEdges' based on the Key(if true then b is index in ChosenEdges)
			///start= start of the array Vertices
			///end= end of the array Vertices
			/// </summary>
			int x, y,start=0,end=Vertices.Length-1;
			KeyValuePair<bool, int> a = new KeyValuePair<bool, int>(false,0);
			KeyValuePair<bool, int> b  =new KeyValuePair<bool, int>(false,0);

			///<summary> 
			///this loop iterates on the arranged edges and 
			///in each eteration findes the index of source and distination using 'find' method
			/// </summary>
            for (int i=0;i< Edges.Length; i++)
            {
                x = Edges[i].source;
                y = Edges[i].destnation;

				a = Find(x, start, end);
				b = Find(y, start, end);

				///<summary>
				///there are four cases for 'x' & 'y':
				///1: if 'x' and 'y' both found in the array 'Vertices'
				///2: if 'x' and 'y' both found in the set 'chosenEdges'
				///3: if 'x' is found in the set 'chosenEdges' and 'y' is found in the array 'Vertices'
				///4: if 'x' is found in the array 'Vertices' and 'y' is found in the set 'chosenEdges'
				///--------------------------------------------------------------------
				///if 'a or b .key =true' : then it is an index in the set'chosenEdges' 
				///else : then it is an index in the array 'Vertices'
				/// </summary>


				//1: if 'x' and 'y' both found in the array 'Vertices' and not in the same set
				if (!a.Key && !b.Key && a.Value != b.Value)
				{
					//Union the 2 Subsets 
					Vertices[a.Value].UnionWith(Vertices[b.Value]);
					//Add the union subset to the set 'chosenEdges' 
					ChosesEdges.Add(Vertices[a.Value]);

					//===deleting the 2 Subsets from array 'Vertices'===

					//deleting from the greater first to the end of the array then deleteing the second element
					if (a.Value > b.Value)
					{
						delete(a.Value, ref start, ref end);
						delete(b.Value, ref start, ref end);
					}
					else
					{
						delete(b.Value, ref start, ref end);
						delete(a.Value, ref start, ref end);
					}
					//===deleting the 2 Subsets from array 'Vertices'===//

					storeEdge = true;
				}
				//2: if 'x' and 'y' both found in the set 'chosenEdges' and not in the same set
				else if (a.Key && b.Key && a.Value != b.Value)
				{
					//Union the 2 Subsets
					ChosesEdges.ElementAt(a.Value).UnionWith(ChosesEdges.ElementAt(b.Value));
					//Add the union subset to the set 'chosenEdges'
					ChosesEdges.Add(ChosesEdges.ElementAt(a.Value));
					
					//delete the left out subset from the set 'chosenEdges'
					ChosesEdges.Remove(ChosesEdges.ElementAt(b.Value));

					storeEdge = true;
				}
				//3: if 'x' is found in the set 'chosenEdges' and 'y' is found in the array 'Vertices'
				else if (a.Key && !b.Key)
				{
					//Union the 2 Subsets
					ChosesEdges.ElementAt(a.Value).UnionWith(Vertices[b.Value]);
					//Add the union subset to the set 'chosenEdges'
					ChosesEdges.Add(ChosesEdges.ElementAt(a.Value));

					//delete the left out subset from the set 'chosenEdges'
					delete(b.Value,ref start,ref end);

					storeEdge = true;
				}
				//4: if 'x' is found in the array 'Vertices' and 'y' is found in the set 'chosenEdges'
				else if (!a.Key && b.Key)
				{
					//Union the 2 Subsets
					ChosesEdges.ElementAt(b.Value).UnionWith(Vertices[a.Value]);
					//Add the union subset to the set 'chosenEdges'
					ChosesEdges.Add(ChosesEdges.ElementAt(b.Value));

					//delete the left out subset from the set 'chosenEdges'
					delete(a.Value, ref start, ref end);

					storeEdge = true;
				}
				//if 'x' and 'y' are not in the same set
				if (storeEdge)
				{
					result.Add(new Edge(x, y, Edges[i].weight));
					sum += Edges[i].weight;

					storeEdge = false;
				}

				//if the set 'ChosenEdges' contains only one subset that contains all vertices
				if (ChosesEdges.Count==1&&ChosesEdges.ElementAt(0).Count==Vertices.Length)
					break;
            }

		//===============TEST summition in sample cases======================
			MessageBox.Show(distinct.Count.ToString() + "\n"+ sum.ToString());
            return result;
		//===============TEST summition in sample cases======================

		}

		/// <summary>
		/// Remove an element at specific index in the array 'Vertices' by changing the values if 'start' and 'end'
		/// </summary>
		/// <param name="index"></param>
		/// <param name="start"></param>
		/// <param name="end"></param>
		private void delete(int index,ref int start,ref int end)
		{
			
				//if the left out in the end
				if (index == end)
					end--;
				//if the left out in the start
				else if (index == start)
					start++;
				//if the left out in between
				else
				{   // Shifting elements from the end to index
					for (int sheftCounter = index; sheftCounter < end; sheftCounter++)
					{
						Vertices[sheftCounter] = Vertices[sheftCounter + 1];
					}
					end--;
				}
		}

		/// <summary>
		/// searches on the index of the set that contains a specific
		/// vertex in the array 'Vertices' (array of subsets of vertices)
		/// </summary>
		/// <param name="vertix"></param>
		/// <param name="start"></param>
		/// <param name="end"></param>
		/// <returns>index of the set that contains the given vertex</returns>
		private KeyValuePair<bool, int> Find(int vertix, int start, int end)
		{
			KeyValuePair<bool,int> result = new KeyValuePair<bool, int>(false, -1);

			//if the the set 'ChosenEdges' is empty
			if (ChosesEdges.Count!=0)
			{
				//itirate on the set elements an find 'vertix'
				for (int i = 0; i < ChosesEdges.Count; i++)
				{
					//if the current subset contains 'vertix'
					if(ChosesEdges.ElementAt(i).Contains(vertix))
					{
						result = new KeyValuePair<bool, int>(true, i);
						break;
					}
				}
			}
			//if 'vertix' is not found in the set 'ChosenEdges'
			if(! result.Key)
			{
				//Binary Search on 'vertix' in the array 'Vertices'
				for (int i = start; i <= end;)
				{
					int mid = (i + end) / 2;

					if (Vertices[i].Contains(vertix))
					{
						result = new KeyValuePair<bool, int>(false, i);
						break;
					}

					else if (Vertices[end].Contains(vertix))
					{
						result = new KeyValuePair<bool, int>(false, end);
						break;
					}
					else if (Vertices[mid].Contains(vertix))
					{
						result = new KeyValuePair<bool, int>(false, mid);
						break;
					}
					else if (Vertices[mid].ElementAt(0) < vertix)
						i = mid + 1;
					else
						end = mid - 1;
				}

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
