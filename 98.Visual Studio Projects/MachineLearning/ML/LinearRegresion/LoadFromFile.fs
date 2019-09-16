namespace MachineLearning

open System.IO

[<AutoOpen>]
module Load =

    // Suppose you are the CEO of a restaurant franchise and are considering diﬀerent cities for opening a new outlet.
      
    //let filePath = @"C:\Users\dlorente\Desktop\Machine Learning\98.Visual Studio Projects\MachineLearning\MachineLearning\LinearRegresion\Dataset\ex1data1.txt";
   
    let fromFile filepath = 
        File.ReadLines(filepath)   

   



