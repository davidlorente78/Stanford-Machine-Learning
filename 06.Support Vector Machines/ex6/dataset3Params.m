function [C, sigma] = dataset3Params(X, y, Xval, yval)
%DATASET3PARAMS returns your choice of C and sigma for Part 3 of the exercise
%where you select the optimal (C, sigma) learning parameters to use for SVM
%with RBF kernel
%   [C, sigma] = DATASET3PARAMS(X, y, Xval, yval) returns your choice of C and 
%   sigma. You should complete this function to return the optimal C and 
%   sigma based on a cross-validation set.
%

% You need to return the following variables correctly.
C = 1;
sigma = 0.3;

% ====================== YOUR CODE HERE ======================
% Instructions: Fill in this function to return the optimal C and sigma
%               learning parameters found using the cross validation set.
%               You can use svmPredict to predict the labels on the cross
%               validation set. For example, 
%                   predictions = svmPredict(model, Xval);
%               will return the predictions on the cross validation set.
%
%  Note: You can compute the prediction error using 
%        mean(double(predictions ~= yval))
%

values_C = [0.003 0.01 0.03 0.1 0.3 1 3 10 30 100];
values_sigma = [0.003 0.01 0.03 0.1 0.3 1 3 10 30 100];

error = 1000;

for Ci=1:length(values_C),
    this_C = values_C(Ci);
    for sigmai=1:length(values_sigma),
        this_sigma = values_sigma(sigmai);
        
        model = svmTrain(X, y, this_C, @(x1, x2) gaussianKernel(x1, x2, this_sigma));
        predictions = svmPredict(model, Xval);
        this_error = mean(double(predictions ~= yval));
        
        if this_error < error,
            fprintf('\nPrediction error %f for C %f and sigma %f\n', this_error, this_C, this_sigma);
            error = this_error;
            C = this_C;
            sigma = this_sigma;
        end
    end
end


  fprintf('\nMinimun error occurs when C : %f and sigma : %f\n', C, sigma);


% =========================================================================

end
