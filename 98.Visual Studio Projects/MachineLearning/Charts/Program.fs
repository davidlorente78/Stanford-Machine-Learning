open MachineLearning

//Access namespaces as required by the examples. System for the 'Random' type and use of String.Format, FSharp.Data for the HtmlProvider, FSharp.Charting for what is says on the tin
//and System.Drawing for chart styling elements
open System
open FSharp.Data
open FSharp.Charting
open System.Drawing
open MathNet.Numerics.LinearAlgebra
open MathNet.Numerics.LinearAlgebra.Double


[<EntryPoint>]
let main argv =
   
   let I = Matrices.Identity

   let Theta = Matrices.Theta

   let U = Matrices.InitializedByFunction

   let R = I.Multiply(Theta)

   let data = MachineLearning.LinearRegresionOneVariable.readLines 
   data |> Seq.iter(fun x -> printfn  "%s" x)
         
   let points = 
    data 
    |> Seq.map(fun (a) ->  ( Convert.ToDouble(a.Split(',').[0].Replace('.',',')) , Convert.ToDouble(a.Split(',').[1].Replace('.',','))))
    //|> Seq.toList


   let points2 = 
        data
        |> Seq.map(fun (a) -> a.Split(',').[0] , a.Split(',').[1])
        |> Seq.map(fun (a,b) -> a.Replace('.',',') ,b.Replace('.',','))
        |> Seq.map(fun (a,b) -> Convert.ToDouble(a) ,Convert.ToDouble(b))

              
   let m = points2 |> Seq.length;

   let x_max = 
       data 
       |> Seq.map(fun (a) ->    Convert.ToDouble(a.Split(',').[0].Replace('.',',')))
       |> Seq.max

   let x_min = 
    data 
    |> Seq.map(fun (a) ->    Convert.ToDouble(a.Split(',').[0].Replace('.',',')))
    |> Seq.min
    

   let y_max=
       data 
         |> Seq.map(fun (a) ->    Convert.ToDouble(a.Split(',').[1].Replace('.',',')))
         |> Seq.max
     
    //(float*float) list
   Chart.Point (points2,"DataSet")
    |> Chart.WithYAxis(Title = "Profit", Max = (25.0), Min = (-5.0))
    |> Chart.WithXAxis(Title = "Population", Max = (25.0), Min = (-5.0))
    |> Chart.Show

   //Define an array of resting heart rate values
   //let restingHeartRateValues = [69; 74; 76; 71; 68; 65; 64; 79; 80; 75; 72; 65; 63; 62; 59; 65; 80; 61; 59]
    
   //Produce a line chart to visualise this data
   //Chart.Line(restingHeartRateValues)
   //|> Chart.Show 

   // Draw scatter plot  of points
   //let rnd = new Random()
   //let rand() = rnd.NextDouble()
   //let randomPoints = [ for i in 0 .. 1000 -> rand(), rand() ]
   //Chart.Point randomPoints      
   //|> Chart.WithYAxis(Title = "Random", Max = (1.0), Min = (0.0))
   //|> Chart.WithXAxis(Title = "Random", Max = (1.0), Min = (0.0))
   //|> Chart.Show

   //https://fslab.org/FSharp.Charting/PointAndLineCharts.html

   let x_lines = 
       data 
       |> Seq.map(fun (a) ->    Convert.ToDouble(a.Split(',').[0].Replace('.',',')))
       |> Seq.toArray

   let y_lines = 
       data 
      |> Seq.map(fun (a) ->    Convert.ToDouble(a.Split(',').[1].Replace('.',',')))
      |> Seq.toArray

   
        
         

   let data_x = DenseMatrix.OfColumnArrays x_lines

   let y = DenseVector.OfArray y_lines

   let ones = DenseVector.Create(data_x.RowCount,1.0)

   let X = data_x.InsertColumn(0,ones)
    

   //-3.6303  1.1664

   let XT = X.Transpose()
   
   let partial = (XT.Multiply(X))
   let inverse = partial.PseudoInverse()

   let theta =  X.TransposeThisAndMultiply(X).PseudoInverse() * X.TransposeThisAndMultiply(y)
       
    //https://numerics.mathdotnet.com/Matrix.html

   let x_min_vector_list = [1.0; x_min]
   let x_max_vector_list = [1.0; x_max]  

   let x_min_vector = Matrix.Build.DenseOfRows([x_min_vector_list])
   let x_max_vector = Matrix.Build.DenseOfRows([x_max_vector_list])
          
   let prediction_x_min = x_min_vector.Multiply(theta)
   let prediction_x_max = x_max_vector.Multiply(theta)

  

   let regression_points = 
    //[ (0,0) ; (25,25) ]
    [ (x_min,prediction_x_min.[0]) ; (x_max,prediction_x_max.[0]) ]
    |> List.toSeq
    
   //Chart.Line (regression_points,"LinearRegression")
   
   Chart.Combine(
    [Chart.Point(points2,Name="DataSet")
     Chart.Line(regression_points,Name="LinearRegression") ])
     |>Chart.Show

   
   System.Console.ReadLine() |> ignore



  
   0

