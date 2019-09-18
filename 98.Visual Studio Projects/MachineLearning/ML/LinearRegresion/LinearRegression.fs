namespace MachineLearning

open MathNet.Numerics.LinearAlgebra.Double
open System

module LinearRegression =

    let theta ( X: Matrix ) ( y : Vector )  =  
        X.TransposeThisAndMultiply(X).PseudoInverse() * X.TransposeThisAndMultiply(y)

    let cost ( X: DenseMatrix ) ( y : Vector ) ( theta : Vector  ) = 
        let j = 1.0 / ( 2.0 * Convert.ToDouble(Dataset.m) ) * (X.Multiply(theta)-y).Map(fun (x) -> x*x).Sum()
        j

        
     
        

       


    

