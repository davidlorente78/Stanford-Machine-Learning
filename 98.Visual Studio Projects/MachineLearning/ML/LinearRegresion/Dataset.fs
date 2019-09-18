namespace MachineLearning

open System
open MathNet.Numerics.LinearAlgebra.Double

module Dataset =   

   let private data = MachineLearning.Load.fromFile @"C:\Users\dlorente\Desktop\Machine Learning\98.Visual Studio Projects\MachineLearning\MachineLearning\LinearRegresion\Dataset\ex1data1.txt";     
   let private data2 = MachineLearning.Load.fromFile2 @"C:\Users\dlorente\Desktop\Machine Learning\98.Visual Studio Projects\MachineLearning\MachineLearning\LinearRegresion\Dataset\ex1data1.txt";     
      
   let points = 
          data
          |> Seq.map(fun (a) -> a.Split(',').[0] , a.Split(',').[1])
          |> Seq.map(fun (a,b) -> a.Replace('.',',') ,b.Replace('.',','))
          |> Seq.map(fun (a,b) -> Convert.ToDouble(a) ,Convert.ToDouble(b))
         
   let x_max = 
         data 
         |> Seq.map(fun (a) ->    Convert.ToDouble(a.Split(',').[0].Replace('.',',')))
         |> Seq.max

   let x_min = 
      data 
      |> Seq.map(fun (a) ->    Convert.ToDouble(a.Split(',').[0].Replace('.',',')))
      |> Seq.min
      
   let y_min=
      data 
      |> Seq.map(fun (a) ->    Convert.ToDouble(a.Split(',').[1].Replace('.',',')))
      |> Seq.min

   let y_max=
       data 
       |> Seq.map(fun (a) ->    Convert.ToDouble(a.Split(',').[1].Replace('.',',')))
       |> Seq.max

   let m = 
       data2 
      |> Seq.length
      |> float

   let private ones = DenseVector.Create(int m , 1.0)
   
   let n = 1
   
   let X =            
       let M = DenseMatrix.OfRowArrays data2
       M.RemoveColumn(n).InsertColumn(0,ones)
       
   let y = 
       data 
       |> Seq.map(fun (a) ->    Convert.ToDouble(a.Split(',').[1].Replace('.',',')))
       |> Seq.toArray
       |> DenseVector.OfArray
     