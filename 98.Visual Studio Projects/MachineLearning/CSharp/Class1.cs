using MathNet.Numerics.LinearAlgebra;
using System;

namespace CSharp
{
	public class Class1
	{
		Matrix<double> m = Matrix<double>.Build.Random(3, 4);

		public Class1() {


			//Since within an application you often only work with one specific data type, 
			//a common trick to keep this a bit shorter is to define shortcuts to the builders:

			var M = Matrix<double>.Build;
			var V = Vector<double>.Build;

			var m = M.Random(3, 4);
			var v = V.Dense(10);

			//But often we already have data available in some format and need a matrix representing the same data.
			//Whenever a function contains "Of" in its name it does create a copy of the original data.
		}

	}
}
