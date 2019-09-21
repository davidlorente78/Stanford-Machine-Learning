// Learn more about F# at http://fsharp.org

open System
open System.IO

type Observation = {Label : String ; Pixels : float []}

[<EntryPoint>]
let main argv =
    let toObservation (csvData : string) = 
        let columns = csvData.Split(',')
        let label = columns.[0]
        let pixels = columns.[1..] |>  Array.map float
        {Label = label; Pixels = pixels}

    let reader path =
        let data = File.ReadAllLines path
        data.[1..]
        |>Array.map toObservation

    let datasetPath = @"C:\Users\Sibon\Desktop\Stanford-Machine-Learning\98.Visual Studio Projects\MachineLearning\DigitsRecognizer\Dataset\digits.csv"
       
    let data = reader datasetPath
          

    //There are 5000 training examples in ex3data1.mat, where each training
    //example is a 20 pixel by 20 pixel grayscale image of the digit. Each pixel is
    //represented by a floating point number indicating the grayscale intensity at
    //that location. The 20 by 20 grid of pixels is “unrolled” into a 400-dimensional
    //vector. Each of these training examples becomes a single row in our data
    //matrix X. This gives us a 5000 by 400 matrix X where every row is a training
    //example for a handwritten digit image.

    //The second part of the training set is a 5000-dimensional vector y that
    //contains labels for the training set. To make things more compatible with
    //Octave/MATLAB indexing, where there is no zero index, we have mapped
    //the digit zero to the value ten. Therefore, a “0” digit is labeled as “10”, while
    //the digits “1” to “9” are labeled as “1” to “9” in their natural order.


    let m = data.Length

    let number = data.[34].Label

    //Separate data in training data and validation data

    let trainingdata = data

    let validationdata = data



    let manhattanDistance (pixels1 : float [], pixels2 : float []) = 
        Array.zip pixels1 pixels2
        |> Array.map (fun (x,y) -> abs (x-y))
        |> Array.sum

    let train (trainingset : Observation []) =
        let classify (pixels : float []) = 
            trainingset
            |> Array.minBy(fun x-> manhattanDistance (x.Pixels, pixels))
            |> fun x -> x.Label
        classify

    let classifier = train trainingdata



    validationdata
    |> Array.averageBy (fun x-> if classifier x.Pixels = x.Label then 1. else 0.)
    |> printfn "Correct %.3f"
    
    0 // return an integer exit code
