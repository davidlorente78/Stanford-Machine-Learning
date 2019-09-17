namespace MachineLearning

open MathNet.Numerics.LinearAlgebra.Double
open System

module LinearRegression =

    let theta ( X: Matrix , y : Vector )  =  
        X.TransposeThisAndMultiply(X).PseudoInverse() * X.TransposeThisAndMultiply(y)


    let cost ( X: Matrix , y : Vector , theta : Vector ) = 
        (X.Multiply(theta)-y).L2Norm
        
     
        

       


    

