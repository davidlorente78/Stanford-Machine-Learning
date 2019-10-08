// Learn more about F# at http://fsharp.org
open System
open FSharp.Data
open FSharp.Charting

open MathNet
open MathNet.Numerics.LinearAlgebra
open MathNet.Numerics.LinearAlgebra.Double

//Here one must write the full file path
type Data = CsvProvider<"C:\Users\dlorente\Desktop\Stanford-Machine-Learning\98.Visual Studio Projects\MachineLearning\GradientDescent\Dataset\day.csv">
type Obs = Data.Row
type Model = Obs -> float

type V = Vector<float>
type M = Matrix<float>

//Simplifying creation of models
//At this point what we want is to easily create various models by incluiding or removing features
//Convert an observation into a list of features
type Featurizer = Obs -> float list

[<EntryPoint>]
let main argv =
    printfn "Our focus here will be create a model to predict cnt, the count of people who rented a bicycle on a particular day.!"

    //Here one must write the full file path again
    let dataset = Data.Load("C:\Users\dlorente\Desktop\Stanford-Machine-Learning\98.Visual Studio Projects\MachineLearning\GradientDescent\Dataset\day.csv")
    let headers = dataset.Headers
    let data = dataset.Rows 

    let all = Chart.Line [for obs in data -> obs.Cnt]    

    //Feature Scaling
    //in order to avoid taking a long time till flobal minimum make sure features are on similar scale

    //Mean Normalization
    //To make features have zero mean 
    let average = 
        data
        |> Seq.map (fun x -> (float) x.Cnt)
        |> Seq.average

    let variance (values : float[]) = 
        let sqr x = x*x
        let avg = values |> Array.average
        let sigma2 = values |> Array.averageBy (fun x -> sqr (x-avg))
        sigma2

    let standardDeviation values = 
        sqrt (variance values)

    let stdDeviation = standardDeviation (data |> Seq.map (fun x -> (float) x.Cnt) |> Seq.toArray)

    //TODO
           
    let windowed n (series:float seq) = 
        series
        |> Seq.windowed n 
        |> Seq.map (fun xs -> xs |> Seq.average)
        |> Seq.toList

    let combinedChart = Chart.Combine [
        Chart.Line [for obs in data -> obs.Cnt]
        Chart.Line (windowed 30 [for obs in data -> (float) obs.Cnt])
        Chart.Line (windowed 50 [for obs in data -> (float) obs.Cnt])]        
    combinedChart|> Chart.Show     
   
    //alpha : Learning Rate
    let update alpha (theta0, theta1) (obs:Obs) =
        let y = float obs.Cnt
        let x = float obs.Instant
        let theta0_ = theta0 - 2.0 * alpha * 1.0 * (theta0 + theta1 * x - y)
        let theta1_ = theta1 - 2.0 * alpha * x * (theta0 + theta1 * x - y)
        theta0_,theta1_

    //Batch : Term to refer to each step of gradient descent uses all the training examples
    let batchUpdate rate (theta0, theta1) (data:Obs seq) = 
        let updates = 
            data
            |> Seq.map (update rate (theta0,theta1))
        //Simultaneously update all theta parameters.
        let theta0_ = updates |> Seq.averageBy fst
        let theta1_ = updates |> Seq.averageBy snd
        theta0_,theta1_
   
    let batch rate iters = 
        let rec search (t0,t1) i = 
            if i = 0 then (t0,t1)
            else 
                search(batchUpdate rate (t0,t1) data) (i-1)
        search (0.0,0.0) iters

    let cost (data:Obs seq) (m:Model) = 
        data
        |> Seq.sumBy (fun x -> pown (float x.Cnt - m x ) 2)
        |> sqrt

    //Hypothesis Function
    //Theta0 and Theta1 : Parameters of the model
    //Just one variable Instant
    let model (theta0,theta1) (obs:Obs) = 
        theta0 + theta1 * (float obs.Instant) 

    let model0 = model (4504.0,0.0)
    let model1 = model (6000.0, -4.5) //Theta0 = 6000 and Theta1 = -4.5        

    //Computing the cost for a particular choice of theta
    let overallCost = cost data
    overallCost model0 |> printfn "Cost model0 : %.0f"
    overallCost model1 |> printfn "Cost model1 : %.0f"

    let batched_error rate = 
        Seq.unfold (fun (t0,t1) ->
            let (t0_,t1_) = batchUpdate rate (t0,t1) data
            let err = model (t0,t1) |> overallCost
            Some (err, (t0_,t1_))) (0.0,0.0)
        |> Seq.take 250
        |> Seq.toList
        |> Chart.Line

    //let data = [0;1;2;3;4]

    //Basic Example of fold
    //let sum = data |> Seq.fold (fun total x -> total + x) 0

    batched_error 0.000001
           |>Chart.Show

    //For sufficiently small alpha J(theta) should decrease on every iteration
    //if alpha is too low slow convergence
    batched_error 0.00000000001
        |>Chart.Show       

    //Repeat until convergence
    let g = batch 0.000001 200 //{(0,260681895294291, 10,6752516673409)}

    let model3 = model (0.260681895294291, 10.6752516673409)
    overallCost model3 |> printfn "Cost model3 : %.0f"
    
    let regressionChart = Chart.Combine [
          Chart.Line [for obs in data -> obs.Cnt]
          Chart.Line [for obs in data -> model3 obs]]
          
    regressionChart |>Chart.Show

    //To avoid clear seasonal patterns we will simply shuffle the sample first
    //Fisher Yates random Shuffle Algorithm

    let seed = 314159
    let rng = System.Random seed

    let shuffle (arr:' a[]) = 
        let arr = Array.copy arr
        let l = arr.Length
        for i in (l-1) .. -1 .. 1 do
            let temp = arr.[i]
            let j = rng.Next (0,i+1)
            arr.[i] <- arr.[j]
            arr.[j] <- temp
        arr

    let arrayToShuffle = [|1;2;3;4;5|]
   
    //We cut the dataset and used the first 70% of observations for training and used the last 30% for validation

    let training,validation = 
        let shuffled = 
            data
            |> Seq.toArray
            |> shuffle
        let size = 
            0.7 * float (Array.length shuffled) |> int
        shuffled.[..size],shuffled.[size+1..]
    

    //Here is how we could recreate our simple straight-line model with a constant dummy variable that always takes a value 1.0 and extract the Instant from an observation
    let featurizer_00 (obs:Obs) = 
        [1.0;
        float obs.Instant;]


    // >> Composes two functions

    let predictor (f:Featurizer) (theta:V) = 
        f>> vector >> (*) theta

    let evaluate (model:Model)  (data:Obs seq) = 
        data
        |> Seq.averageBy (fun obs -> abs (model obs - float obs.Cnt)) //Mean Absolute Error

    let estimate (Y:V) (X:M) = 
        (X.Transpose() * X).Inverse() * X.Transpose() * Y


    //the model function takes in a Featurizer describing what features we want to extract from an observation and a dataset that we will use to train the model
    //First we extract out from the dataset the output Yt and the matrix Xt
    //Then we estimate Theta using Yt and Xt and return the vector Theta, bundled together with the corresponding predictor function. 

    let model_features (f : Featurizer) (data:Obs seq) = 
        let Yt, Xt = 
            data
            |> Seq.toList
            |> List.map (fun obs -> float obs.Cnt, f obs)
            |> List.unzip
        let theta = estimate (vector Yt) (matrix Xt)
        let predict = predictor f theta
        theta, predict

    let (theta_00,model_00) = model_features featurizer_00 training

    //We can now evaluate the quality of our model, on the training and validation sets
    printfn "Model 00"
    evaluate model_00 training      |> printfn "Training: %.0f"
    evaluate model_00 validation    |> printfn "Evaluation: %.0f"


    let featurizer_01 (obs:Obs) = 
        [1.0;
        float obs.Instant;
        float obs.Atemp;
        float obs.Hum;
        float obs.Temp;
        float obs.Windspeed;]

    let (theta_01,model_01) = model_features featurizer_01 training

    printfn "Model 01"
    evaluate model_01 training      |> printfn "Training: %.0f"
    evaluate model_01 validation    |> printfn "Evaluation: %.0f"

    let visualization = Chart.Combine [
        Chart.Line [for obs in data -> float obs.Cnt]
        Chart.Line [for obs in data -> model_00 obs]   
        Chart.Line [for obs in data -> model_01 obs]   
    ]
    visualization |> Chart.Show

    //Plot the actual data against the predicted one
    //The scatterplot if perfect will follows a 45 degree stright line
    let actualvsPredicted = Chart.Point [for obs in data -> float obs.Cnt , model_01 obs]
    actualvsPredicted |> Chart.Show


    overallCost model_00 |> printfn "Cost model_00 : %.0f"
    overallCost model_01 |> printfn "Cost model_01 : %.0f"


    //Refining predictions with more features
    let featurizer_02 (obs:Obs) = 
        [1.0;
        float obs.Instant;
        float obs.Atemp;
        float obs.Hum;
        float obs.Temp;
        float obs.Windspeed;
        (if obs.Weekday = 1 then 1.0 else 0.0);
        (if obs.Weekday = 2 then 1.0 else 0.0);
        (if obs.Weekday = 3 then 1.0 else 0.0);
        (if obs.Weekday = 4 then 1.0 else 0.0);
        (if obs.Weekday = 5 then 1.0 else 0.0);
        (if obs.Weekday = 6 then 1.0 else 0.0);
        ]

    let (theta_02,model_02) = model_features featurizer_02 training

    printfn "Model 02"
    evaluate model_02 training      |> printfn "Training: %.0f"
    evaluate model_02 validation    |> printfn "Evaluation: %.0f"

    0 // return an integer exit code
