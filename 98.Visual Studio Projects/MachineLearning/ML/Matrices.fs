namespace MachineLearning

open MathNet.Numerics.LinearAlgebra

module Matrices =

    // create a dense matrix with 3 rows and 4 columns
    // filled with random numbers sampled from the standard distribution       

    let InitializedByFunction = DenseMatrix.init 3 4 (fun i j -> double (i + j))      

    let Theta = Vector<double>.Build.Random(5)

    let Identity = Matrix<double>.Build.DenseIdentity(5)

