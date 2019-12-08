using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace ImageQuantization
{
	public static class Graph
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
	}
}
// This code is contributed by Aakash Hasija 

