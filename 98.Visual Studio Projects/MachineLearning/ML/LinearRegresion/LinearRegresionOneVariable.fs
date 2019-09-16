namespace MachineLearning

open MathNet.Numerics.LinearAlgebra
open System.IO

module LinearRegresionOneVariable =

    // Suppose you are the CEO of a restaurant franchise and are considering diﬀerent cities for opening a new outlet.
      
    let filePath = @"C:\Users\dlorente\Desktop\Machine Learning\98.Visual Studio Projects\MachineLearning\MachineLearning\LinearRegresion\Dataset\ex1data1.txt";
   
    //let readLines = new StreamReader(filePath) |> Seq.unfold (fun sr -> 
    //  match sr.ReadLine() with
    //  | null -> sr.Dispose(); None 
    //  | str -> Some(str, sr))

    let readLines = File.ReadLines(filePath)   

   



