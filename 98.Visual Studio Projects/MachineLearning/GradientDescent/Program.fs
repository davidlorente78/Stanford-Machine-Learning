// Learn more about F# at http://fsharp.org

open System
open FSharp.Data
open FSharp.Charting

//Here one must write the full file path
type Data = CsvProvider<"C:\Users\dlorente\Desktop\Stanford-Machine-Learning\98.Visual Studio Projects\MachineLearning\GradientDescent\Dataset\day.csv">
type Obs = Data.Row
type Model = Obs -> float

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
        |> Seq.take 1000
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

    0 // return an integer exit code
