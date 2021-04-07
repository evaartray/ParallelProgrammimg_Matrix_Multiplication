using System;
using System.Threading;
using System.Linq;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace ParallelProgrammimg_Matrix_Multiplication
{
	class Program
	{
		class MainClass
		{
			public static void Main(string[] args)
			{
				Console.WriteLine("Matrix multiplication of two 100 X 100 matrices");
				//Change values accordingly:
				MultiplyAll(100, 100, 100);
				Console.WriteLine();

				Console.WriteLine("Matrix multiplication of two 500 X 500 matrices");
				MultiplyAll(500, 500, 500);
				Console.WriteLine();
			}
			public static void MultiplyAll(int a, int b, int c)
			{
				Stopwatch sw = new Stopwatch();

				Matrix matrix = new Matrix();
				matrix.RandomizeMatrix(a, b, c);
				sw = new Stopwatch();

				//Print the duration of sequential multiplication
				sw.Start();
				matrix.SequentialMultiplication();
				sw.Stop();
				Console.WriteLine("Duration of Sequential Multiplication: " + sw.Elapsed);
				TimeSpan ts1 = sw.Elapsed; //value of the time elapsed

				//Print the duration of parallel multiplication
				sw.Restart();
				matrix.ParallelMultiplication();
				sw.Stop();
				Console.WriteLine("Duration of Parallel For Multiplication: " + sw.Elapsed);
				TimeSpan ts2 = sw.Elapsed;

				//The Speedup is a ratio of the sequential to the parallel algorithm
				var speedup = ts1 / ts2;
				Console.WriteLine("\nSpeedup of Parallel multiplication: " + speedup);

				//The efficiency is defined as the ratio of speedup to the number of processors
				var efficiency = speedup / 2;
				Console.WriteLine("Efficiency of Parallel multiplication (2 logical cores): " + efficiency);

			}
		}
		class Matrix
		{
			long[,] matrixA;
			long[,] matrixB;
			long[,] resultMatrix;
			long n, m, o; //Matrix sizes
			
			//Randomizer for the values in the matrix
			public void RandomizeMatrix(int n, int m, int o)
			{
				this.m = m;
				this.n = n;
				this.o = o;
				matrixA = new long[n, m];
				matrixB = new long[m, o];
				resultMatrix = new long[n, o];
				var randomizer = new Random(int.Parse(DateTime.Now.ToString("HHmmssfff")) + (int)(n * o));
				for (long i = 0; i < n; ++i)
				{
					for (long j = 0; j < m; ++j)
					{
						matrixA[i, j] = randomizer.Next(0, n * o);
					}
				}
				for (long i = 0; i < m; ++i)
				{
					for (long j = 0; j < o; ++j)
					{
						matrixB[i, j] = randomizer.Next(0, n * o);
					}
				}
			}

			public void SequentialMultiplication()
			{
				for (long i = 0; i < n; ++i)
				{
					for (long j = 0; j < o; ++j)
					{
						for (long k = 0; k < m; ++k)
						{
							resultMatrix[i, j] += matrixA[i, k] * matrixB[k, j];
						}
					}
				}
			}
			
			//Parallelmultiplication using Parallel library
			public void ParallelMultiplication()
			{
				Parallel.For(0, n, i =>
				{
					Parallel.For(0, o, j =>
					{
						for(int k = 0; k < m; k++)
						{
							resultMatrix[i, j] += matrixA[i, k] * matrixB[k, j];
						}
					});
				});
			}
			public override string ToString()
			{
				string result = "";
				for (long i = 0; i < n; ++i)
				{
					for (long j = 0; j < m; ++j)
					{
						result += (matrixA[i, j] + " ");
					}
					result += "\n";
				}
				result += "\n\n\n";
				for (long i = 0; i < m; ++i)
				{
					for (long j = 0; j < o; ++j)
					{
						result += (matrixB[i, j] + " ");
					}
					result += "\n";
				}
				result += "\n\n\n";
				for (long i = 0; i < n; ++i)
				{
					for (long j = 0; j < o; ++j)
					{
						result += (resultMatrix[i, j] + " ");
					}
					result += "\n";
				}
				return result;
			}
		}
	}
}
