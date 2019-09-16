open MachineLearning

//Access namespaces as required by the examples. System for the 'Random' type and use of String.Format, FSharp.Data for the HtmlProvider, FSharp.Charting for what is says on the tin
//and System.Drawing for chart styling elements
open System
open FSharp.Charting
open MathNet.Numerics.LinearAlgebra.Double


[<EntryPoint>]
let main argv =
   
   let data = Load.fromFile @"C:\Users\dlorente\Desktop\Machine Learning\98.Visual Studio Projects\MachineLearning\MachineLearning\LinearRegresion\Dataset\ex1data1.txt";
   
   data |> Seq.iter(fun x -> printfn  "%s" x)  
   
   let points = 
        data
        |> Seq.map(fun (a) -> a.Split(',').[0] , a.Split(',').[1])
        |> Seq.map(fun (a,b) -> a.Replace('.',',') ,b.Replace('.',','))
        |> Seq.map(fun (a,b) -> Convert.ToDouble(a) ,Convert.ToDouble(b))
  
   Chart.Point (points,"DataSet")
    |> Chart.WithYAxis(Title = "Profit", Max = (25.0), Min = (-5.0))
    |> Chart.WithXAxis(Title = "Population", Max = (25.0), Min = (-5.0))
    |> Chart.Show  

   let X = Dataset.X
   let y = Dataset.y

   //Normal Equation
   let theta =  X.TransposeThisAndMultiply(X).PseudoInverse() * X.TransposeThisAndMultiply(y)
       
    //https://numerics.mathdotnet.com/Matrix.html

   let x_min_vector_list = [1.0; Dataset.x_min]
   let x_max_vector_list = [1.0; Dataset.x_max]  

   let x_min_vector = Matrix.Build.DenseOfRows([x_min_vector_list])
   let x_max_vector = Matrix.Build.DenseOfRows([x_max_vector_list])
          
   let prediction_x_min = x_min_vector.Multiply(theta)
   let prediction_x_max = x_max_vector.Multiply(theta)  

   let regression_points = 
    //[ (0,0) ; (25,25) ]
    [ (Dataset.x_min,prediction_x_min.[0]) ; (Dataset.x_max,prediction_x_max.[0]) ]
    |> List.toSeq
       
   Chart.Combine(
    [Chart.Point(points,Name="DataSet")
     Chart.Line(regression_points,Name="LinearRegression") ])
     |>Chart.Show

   
   System.Console.ReadLine() |> ignore



  
   0

