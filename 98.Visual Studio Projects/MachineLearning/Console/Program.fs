// Learn more about F# at http://fsharp.org

open System
open MachineLearning
open FSharp.Charting

[<EntryPoint>]
let main argv =
   
   let I = MachineLearning.Matrices.Identity

   let Theta = MachineLearning.Matrices.Theta

   let U = MachineLearning.Matrices.InitializedByFunction

   let R = I.Multiply(Theta)

   let data = MachineLearning.LinearRegresionOneVariable.readLines 

   //data |> Seq.iter(fun x -> printfn  "%s" x)

   let randomPoints = [ for i in 0 .. 1000 -> i, i/2 ]

   //let points = data |> Seq.map(fun (a) -> Chart.Point (a.Split(',').[0] , a.Split(',').[1])

   Chart.Point randomPoints |> ignore
  
   0

