namespace MachineLearning

open System
open MathNet.Numerics.LinearAlgebra.Double


module Dataset = 
  
   let private data = MachineLearning.Load.fromFile @"C:\Users\dlorente\Desktop\Machine Learning\98.Visual Studio Projects\MachineLearning\MachineLearning\LinearRegresion\Dataset\ex1data1.txt";     

   let private points = 
          data
          |> Seq.map(fun (a) -> a.Split(',').[0] , a.Split(',').[1])
          |> Seq.map(fun (a,b) -> a.Replace('.',',') ,b.Replace('.',','))
          |> Seq.map(fun (a,b) -> Convert.ToDouble(a) ,Convert.ToDouble(b))
                
   let m = 
    points 
   |> Seq.length
   


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

   let private x_lines = 
         data 
         |> Seq.map(fun (a) ->    Convert.ToDouble(a.Split(',').[0].Replace('.',',')))
         |> Seq.toArray

   let private y_lines = 
         data 
        |> Seq.map(fun (a) ->    Convert.ToDouble(a.Split(',').[1].Replace('.',',')))
        |> Seq.toArray

     
   let private data_x = DenseMatrix.OfColumnArrays x_lines

   let y = DenseVector.OfArray y_lines

   let private ones = DenseVector.Create(data_x.RowCount,1.0)

   let X = data_x.InsertColumn(0,ones)

   let n = X.ColumnCount