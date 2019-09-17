open MachineLearning

//Access namespaces as required by the examples. System for the 'Random' type and use of String.Format, FSharp.Data for the HtmlProvider, FSharp.Charting for what is says on the tin
//and System.Drawing for chart styling elements
open System
open FSharp.Charting
open MathNet.Numerics.LinearAlgebra.Double


[<EntryPoint>]
let main argv =
   
   let V = Vector.Build

   let data = Load.fromFile @"C:\Users\dlorente\Desktop\Machine Learning\98.Visual Studio Projects\MachineLearning\MachineLearning\LinearRegresion\Dataset\ex1data1.txt";
   
   data |> Seq.iter(fun x -> printfn  "%s" x)  
   System.Console.ReadLine() |> ignore
   
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

   Console.Write(X.ToString())
   System.Console.ReadLine() |> ignore


   Console.Write(y.ToString())
   System.Console.ReadLine() |> ignore

   
   //Initial theta
   let theta_0 = Vector.Build.Dense(Dataset.n);

   //Initial cost
   let cost_0 = 1.0 / ( 2.0 * Convert.ToDouble(Dataset.m) ) * (X.Multiply(theta_0)-y).Map(fun (x) -> x*x).Sum()
   
   printfn  "Estimated cost for initial Theta 32.073 : %f" cost_0
   //Expected J =  32.073

   let enter_to_continue = 
       System.Console.WriteLine("Program paused. Press enter to continue.")
       System.Console.ReadLine() |> ignore

   enter_to_continue

   

   let theta_check = V.DenseOfEnumerable( [ -1.0 ;  2.0 ])
   
   let cost_check = 1.0 / ( 2.0 * Convert.ToDouble(Dataset.m) ) * (X.Multiply(theta_check)-y).Map(fun (x) -> x*x).Sum()
  
   printfn  "Estimated cost for Theta = [-1,2]  54.24 : %f" cost_check
   enter_to_continue

   let theta = X.TransposeThisAndMultiply(X).PseudoInverse() * X.TransposeThisAndMultiply(y)   
     
    
    //Plotting Regression Line
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
    [Chart.Point(points,Name="DataSet",Title = "Training Data")
     Chart.Line(regression_points,Name="LinearRegression",Title = "Linear Regression") ])
     |>Chart.Show

   
   System.Console.ReadLine() |> ignore



  
   0

