//Give F# Interactive awareness of the packages folder (it will allow easier loading/references of resources below)
#I "..\packages"
 
//Read, compile and run the FSharpt.Charting.fsx file (to access the functionality of FSharp.Charting.dll)
#load "C:\Users\dlorente\Desktop\Machine Learning\98.Visual Studio Projects\MachineLearning\Charts\InteractiveChart.fsx"
 
//Read in the FSharp.Data and FSharp.Data.TypeProviders assemblies (so we can make use of the HtmlProvider)
#r @"C:\Users\dlorente\Desktop\Machine Learning\98.Visual Studio Projects\MachineLearning\packages\FSharp.Charting.2.1.0\lib\net45\FSharp.Charting.dll"
#r @"C:\Users\dlorente\Desktop\Machine Learning\98.Visual Studio Projects\MachineLearning\packages\FSharp.Data.TypeProviders.5.0.0.6\lib\net40\FSharp.Data.TypeProviders.dll"
 
//Access namespaces as required by the examples. System for the 'Random' type and use of String.Format, FSharp.Data for the HtmlProvider, FSharp.Charting for what is says on the tin
//and System.Drawing for chart styling elements
open System
open FSharp.Data
open FSharp.Charting
open System.Drawing
 

//Define an array of resting heart rate values
let restingHeartRateValues = [69; 74; 76; 71; 68; 65; 64; 79; 80; 75; 72; 65; 63; 62; 59; 65; 80; 61; 59] 
//Produce a line chart to visualise this data
Chart.Line(restingHeartRateValues)
|> Chart.Show 

System.Console.ReadLine()