
#r "../packages/numl.0.8.26.0/lib/net40/numl.dll"
open numl

open numl.Model
open numl.Math.Probability
open numl.Supervised.DecisionTree

type Iris = {[<Feature>] SepalLength:decimal; [<Feature>] SepalWidth:decimal; 
                    [<Feature>] PetalLength: decimal; [<Feature>] PetalWidth: decimal;
                    [<StringLabel>]Class:string}

let irises = [|  {SepalLength = 5.1m; SepalWidth = 3.5m; PetalLength = 1.4m; PetalWidth = 0.2m; Class = "Iris-setosa" }; 
                 { SepalLength = 4.9m; SepalWidth = 3m; PetalLength = 1.4m; PetalWidth = 0.2m; Class = "Iris-setosa" }; 
                 { SepalLength = 4.7m; SepalWidth = 3.2m; PetalLength = 1.3m; PetalWidth = 0.2m; Class = "Iris-setosa" }; 
                 { SepalLength = 4.6m; SepalWidth = 3.1m; PetalLength = 1.5m; PetalWidth = 0.2m; Class = "Iris-setosa" }; 
                 { SepalLength = 5m; SepalWidth = 3.6m; PetalLength = 1.4m; PetalWidth = 0.2m; Class = "Iris-setosa" }; 
                 { SepalLength = 5.4m; SepalWidth = 3.9m; PetalLength = 1.7m; PetalWidth = 0.4m; Class = "Iris-setosa" }; 
                 { SepalLength = 4.6m; SepalWidth = 3.4m; PetalLength = 1.4m; PetalWidth = 0.3m; Class = "Iris-setosa" }; 
                 { SepalLength = 5m; SepalWidth = 3.4m; PetalLength = 1.5m; PetalWidth = 0.2m; Class = "Iris-setosa" }; 
                 { SepalLength = 4.4m; SepalWidth = 2.9m; PetalLength = 1.4m; PetalWidth = 0.2m; Class = "Iris-setosa" }; 
                 { SepalLength = 4.9m; SepalWidth = 3.1m; PetalLength = 1.5m; PetalWidth = 0.1m; Class = "Iris-setosa" }; 
                 { SepalLength = 5.4m; SepalWidth = 3.7m; PetalLength = 1.5m; PetalWidth = 0.2m; Class = "Iris-setosa" }; 
                 { SepalLength = 4.8m; SepalWidth = 3.4m; PetalLength = 1.6m; PetalWidth = 0.2m; Class = "Iris-setosa" }; 
                 { SepalLength = 4.8m; SepalWidth = 3m; PetalLength = 1.4m; PetalWidth = 0.1m; Class = "Iris-setosa" }; 
                 { SepalLength = 4.3m; SepalWidth = 3m; PetalLength = 1.1m; PetalWidth = 0.1m; Class = "Iris-setosa" }; 
                 { SepalLength = 5.8m; SepalWidth = 4m; PetalLength = 1.2m; PetalWidth = 0.2m; Class = "Iris-setosa" }; 
                 { SepalLength = 5.7m; SepalWidth = 4.4m; PetalLength = 1.5m; PetalWidth = 0.4m; Class = "Iris-setosa" }; 
                 { SepalLength = 5.4m; SepalWidth = 3.9m; PetalLength = 1.3m; PetalWidth = 0.4m; Class = "Iris-setosa" }; 
                 { SepalLength = 5.1m; SepalWidth = 3.5m; PetalLength = 1.4m; PetalWidth = 0.3m; Class = "Iris-setosa" }; 
                 { SepalLength = 5.7m; SepalWidth = 3.8m; PetalLength = 1.7m; PetalWidth = 0.3m; Class = "Iris-setosa" }; 
                 { SepalLength = 5.1m; SepalWidth = 3.8m; PetalLength = 1.5m; PetalWidth = 0.3m; Class = "Iris-setosa" }; 
                 { SepalLength = 5.4m; SepalWidth = 3.4m; PetalLength = 1.7m; PetalWidth = 0.2m; Class = "Iris-setosa" }; 
                 { SepalLength = 5.1m; SepalWidth = 3.7m; PetalLength = 1.5m; PetalWidth = 0.4m; Class = "Iris-setosa" }; 
                 { SepalLength = 4.6m; SepalWidth = 3.6m; PetalLength = 1m; PetalWidth = 0.2m; Class = "Iris-setosa" }; 
                 { SepalLength = 5.1m; SepalWidth = 3.3m; PetalLength = 1.7m; PetalWidth = 0.5m; Class = "Iris-setosa" }; 
                 { SepalLength = 4.8m; SepalWidth = 3.4m; PetalLength = 1.9m; PetalWidth = 0.2m; Class = "Iris-setosa" }; 
                 { SepalLength = 5m; SepalWidth = 3m; PetalLength = 1.6m; PetalWidth = 0.2m; Class = "Iris-setosa" }; 
                 { SepalLength = 5m; SepalWidth = 3.4m; PetalLength = 1.6m; PetalWidth = 0.4m; Class = "Iris-setosa" }; 
                 { SepalLength = 5.2m; SepalWidth = 3.5m; PetalLength = 1.5m; PetalWidth = 0.2m; Class = "Iris-setosa" }; 
                 { SepalLength = 5.2m; SepalWidth = 3.4m; PetalLength = 1.4m; PetalWidth = 0.2m; Class = "Iris-setosa" }; 
                 { SepalLength = 4.7m; SepalWidth = 3.2m; PetalLength = 1.6m; PetalWidth = 0.2m; Class = "Iris-setosa" }; 
                 { SepalLength = 4.8m; SepalWidth = 3.1m; PetalLength = 1.6m; PetalWidth = 0.2m; Class = "Iris-setosa" }; 
                 { SepalLength = 5.4m; SepalWidth = 3.4m; PetalLength = 1.5m; PetalWidth = 0.4m; Class = "Iris-setosa" }; 
                 { SepalLength = 5.2m; SepalWidth = 4.1m; PetalLength = 1.5m; PetalWidth = 0.1m; Class = "Iris-setosa" }; 
                 { SepalLength = 5.5m; SepalWidth = 4.2m; PetalLength = 1.4m; PetalWidth = 0.2m; Class = "Iris-setosa" }; 
                 { SepalLength = 4.9m; SepalWidth = 3.1m; PetalLength = 1.5m; PetalWidth = 0.1m; Class = "Iris-setosa" }; 
                 { SepalLength = 5m; SepalWidth = 3.2m; PetalLength = 1.2m; PetalWidth = 0.2m; Class = "Iris-setosa" }; 
                 { SepalLength = 5.5m; SepalWidth = 3.5m; PetalLength = 1.3m; PetalWidth = 0.2m; Class = "Iris-setosa" }; 
                 { SepalLength = 4.9m; SepalWidth = 3.1m; PetalLength = 1.5m; PetalWidth = 0.1m; Class = "Iris-setosa" }; 
                 { SepalLength = 4.4m; SepalWidth = 3m; PetalLength = 1.3m; PetalWidth = 0.2m; Class = "Iris-setosa" }; 
                 { SepalLength = 5.1m; SepalWidth = 3.4m; PetalLength = 1.5m; PetalWidth = 0.2m; Class = "Iris-setosa" }; 
                 { SepalLength = 5m; SepalWidth = 3.5m; PetalLength = 1.3m; PetalWidth = 0.3m; Class = "Iris-setosa" }; 
                 { SepalLength = 4.5m; SepalWidth = 2.3m; PetalLength = 1.3m; PetalWidth = 0.3m; Class = "Iris-setosa" }; 
                 { SepalLength = 4.4m; SepalWidth = 3.2m; PetalLength = 1.3m; PetalWidth = 0.2m; Class = "Iris-setosa" }; 
                 { SepalLength = 5m; SepalWidth = 3.5m; PetalLength = 1.6m; PetalWidth = 0.6m; Class = "Iris-setosa" }; 
                 { SepalLength = 5.1m; SepalWidth = 3.8m; PetalLength = 1.9m; PetalWidth = 0.4m; Class = "Iris-setosa" }; 
                 { SepalLength = 4.8m; SepalWidth = 3m; PetalLength = 1.4m; PetalWidth = 0.3m; Class = "Iris-setosa" }; 
                 { SepalLength = 5.1m; SepalWidth = 3.8m; PetalLength = 1.6m; PetalWidth = 0.2m; Class = "Iris-setosa" }; 
                 { SepalLength = 4.6m; SepalWidth = 3.2m; PetalLength = 1.4m; PetalWidth = 0.2m; Class = "Iris-setosa" }; 
                 { SepalLength = 5.3m; SepalWidth = 3.7m; PetalLength = 1.5m; PetalWidth = 0.2m; Class = "Iris-setosa" }; 
                 { SepalLength = 5m; SepalWidth = 3.3m; PetalLength = 1.4m; PetalWidth = 0.2m; Class = "Iris-setosa" }; 
                 { SepalLength = 7m; SepalWidth = 3.2m; PetalLength = 4.7m; PetalWidth = 1.4m; Class = "Iris-versicolor" }; 
                 { SepalLength = 6.4m; SepalWidth = 3.2m; PetalLength = 4.5m; PetalWidth = 1.5m; Class = "Iris-versicolor" }; 
                 { SepalLength = 6.9m; SepalWidth = 3.1m; PetalLength = 4.9m; PetalWidth = 1.5m; Class = "Iris-versicolor" }; 
                 { SepalLength = 5.5m; SepalWidth = 2.3m; PetalLength = 4m; PetalWidth = 1.3m; Class = "Iris-versicolor" }; 
                 { SepalLength = 6.5m; SepalWidth = 2.8m; PetalLength = 4.6m; PetalWidth = 1.5m; Class = "Iris-versicolor" }; 
                 { SepalLength = 5.7m; SepalWidth = 2.8m; PetalLength = 4.5m; PetalWidth = 1.3m; Class = "Iris-versicolor" }; 
                 { SepalLength = 6.3m; SepalWidth = 3.3m; PetalLength = 4.7m; PetalWidth = 1.6m; Class = "Iris-versicolor" }; 
                 { SepalLength = 4.9m; SepalWidth = 2.4m; PetalLength = 3.3m; PetalWidth = 1m; Class = "Iris-versicolor" }; 
                 { SepalLength = 6.6m; SepalWidth = 2.9m; PetalLength = 4.6m; PetalWidth = 1.3m; Class = "Iris-versicolor" }; 
                 { SepalLength = 5.2m; SepalWidth = 2.7m; PetalLength = 3.9m; PetalWidth = 1.4m; Class = "Iris-versicolor" }; 
                 { SepalLength = 5m; SepalWidth = 2m; PetalLength = 3.5m; PetalWidth = 1m; Class = "Iris-versicolor" }; 
                 { SepalLength = 5.9m; SepalWidth = 3m; PetalLength = 4.2m; PetalWidth = 1.5m; Class = "Iris-versicolor" }; 
                 { SepalLength = 6m; SepalWidth = 2.2m; PetalLength = 4m; PetalWidth = 1m; Class = "Iris-versicolor" }; 
                 { SepalLength = 6.1m; SepalWidth = 2.9m; PetalLength = 4.7m; PetalWidth = 1.4m; Class = "Iris-versicolor" }; 
                 { SepalLength = 5.6m; SepalWidth = 2.9m; PetalLength = 3.6m; PetalWidth = 1.3m; Class = "Iris-versicolor" }; 
                 { SepalLength = 6.7m; SepalWidth = 3.1m; PetalLength = 4.4m; PetalWidth = 1.4m; Class = "Iris-versicolor" }; 
                 { SepalLength = 5.6m; SepalWidth = 3m; PetalLength = 4.5m; PetalWidth = 1.5m; Class = "Iris-versicolor" }; 
                 { SepalLength = 5.8m; SepalWidth = 2.7m; PetalLength = 4.1m; PetalWidth = 1m; Class = "Iris-versicolor" }; 
                 { SepalLength = 6.2m; SepalWidth = 2.2m; PetalLength = 4.5m; PetalWidth = 1.5m; Class = "Iris-versicolor" }; 
                 { SepalLength = 5.6m; SepalWidth = 2.5m; PetalLength = 3.9m; PetalWidth = 1.1m; Class = "Iris-versicolor" }; 
                 { SepalLength = 5.9m; SepalWidth = 3.2m; PetalLength = 4.8m; PetalWidth = 1.8m; Class = "Iris-versicolor" }; 
                 { SepalLength = 6.1m; SepalWidth = 2.8m; PetalLength = 4m; PetalWidth = 1.3m; Class = "Iris-versicolor" }; 
                 { SepalLength = 6.3m; SepalWidth = 2.5m; PetalLength = 4.9m; PetalWidth = 1.5m; Class = "Iris-versicolor" }; 
                 { SepalLength = 6.1m; SepalWidth = 2.8m; PetalLength = 4.7m; PetalWidth = 1.2m; Class = "Iris-versicolor" }; 
                 { SepalLength = 6.4m; SepalWidth = 2.9m; PetalLength = 4.3m; PetalWidth = 1.3m; Class = "Iris-versicolor" }; 
                 { SepalLength = 6.6m; SepalWidth = 3m; PetalLength = 4.4m; PetalWidth = 1.4m; Class = "Iris-versicolor" }; 
                 { SepalLength = 6.8m; SepalWidth = 2.8m; PetalLength = 4.8m; PetalWidth = 1.4m; Class = "Iris-versicolor" }; 
                 { SepalLength = 6.7m; SepalWidth = 3m; PetalLength = 5m; PetalWidth = 1.7m; Class = "Iris-versicolor" }; 
                 { SepalLength = 6m; SepalWidth = 2.9m; PetalLength = 4.5m; PetalWidth = 1.5m; Class = "Iris-versicolor" }; 
                 { SepalLength = 5.7m; SepalWidth = 2.6m; PetalLength = 3.5m; PetalWidth = 1m; Class = "Iris-versicolor" }; 
                 { SepalLength = 5.5m; SepalWidth = 2.4m; PetalLength = 3.8m; PetalWidth = 1.1m; Class = "Iris-versicolor" }; 
                 { SepalLength = 5.5m; SepalWidth = 2.4m; PetalLength = 3.7m; PetalWidth = 1m; Class = "Iris-versicolor" }; 
                 { SepalLength = 5.8m; SepalWidth = 2.7m; PetalLength = 3.9m; PetalWidth = 1.2m; Class = "Iris-versicolor" }; 
                 { SepalLength = 6m; SepalWidth = 2.7m; PetalLength = 5.1m; PetalWidth = 1.6m; Class = "Iris-versicolor" }; 
                 { SepalLength = 5.4m; SepalWidth = 3m; PetalLength = 4.5m; PetalWidth = 1.5m; Class = "Iris-versicolor" }; 
                 { SepalLength = 6m; SepalWidth = 3.4m; PetalLength = 4.5m; PetalWidth = 1.6m; Class = "Iris-versicolor" }; 
                 { SepalLength = 6.7m; SepalWidth = 3.1m; PetalLength = 4.7m; PetalWidth = 1.5m; Class = "Iris-versicolor" }; 
                 { SepalLength = 6.3m; SepalWidth = 2.3m; PetalLength = 4.4m; PetalWidth = 1.3m; Class = "Iris-versicolor" }; 
                 { SepalLength = 5.6m; SepalWidth = 3m; PetalLength = 4.1m; PetalWidth = 1.3m; Class = "Iris-versicolor" }; 
                 { SepalLength = 5.5m; SepalWidth = 2.5m; PetalLength = 4m; PetalWidth = 1.3m; Class = "Iris-versicolor" }; 
                 { SepalLength = 5.5m; SepalWidth = 2.6m; PetalLength = 4.4m; PetalWidth = 1.2m; Class = "Iris-versicolor" }; 
                 { SepalLength = 6.1m; SepalWidth = 3m; PetalLength = 4.6m; PetalWidth = 1.4m; Class = "Iris-versicolor" }; 
                 { SepalLength = 5.8m; SepalWidth = 2.6m; PetalLength = 4m; PetalWidth = 1.2m; Class = "Iris-versicolor" }; 
                 { SepalLength = 5m; SepalWidth = 2.3m; PetalLength = 3.3m; PetalWidth = 1m; Class = "Iris-versicolor" }; 
                 { SepalLength = 5.6m; SepalWidth = 2.7m; PetalLength = 4.2m; PetalWidth = 1.3m; Class = "Iris-versicolor" }; 
                 { SepalLength = 5.7m; SepalWidth = 3m; PetalLength = 4.2m; PetalWidth = 1.2m; Class = "Iris-versicolor" }; 
                 { SepalLength = 5.7m; SepalWidth = 2.9m; PetalLength = 4.2m; PetalWidth = 1.3m; Class = "Iris-versicolor" }; 
                 { SepalLength = 6.2m; SepalWidth = 2.9m; PetalLength = 4.3m; PetalWidth = 1.3m; Class = "Iris-versicolor" }; 
                 { SepalLength = 5.1m; SepalWidth = 2.5m; PetalLength = 3m; PetalWidth = 1.1m; Class = "Iris-versicolor" }; 
                 { SepalLength = 5.7m; SepalWidth = 2.8m; PetalLength = 4.1m; PetalWidth = 1.3m; Class = "Iris-versicolor" }; 
                 { SepalLength = 6.3m; SepalWidth = 3.3m; PetalLength = 6m; PetalWidth = 2.5m; Class = "Iris-virginica" }; 
                 { SepalLength = 5.8m; SepalWidth = 2.7m; PetalLength = 5.1m; PetalWidth = 1.9m; Class = "Iris-virginica" }; 
                 { SepalLength = 7.1m; SepalWidth = 3m; PetalLength = 5.9m; PetalWidth = 2.1m; Class = "Iris-virginica" }; 
                 { SepalLength = 6.3m; SepalWidth = 2.9m; PetalLength = 5.6m; PetalWidth = 1.8m; Class = "Iris-virginica" }; 
                 { SepalLength = 6.5m; SepalWidth = 3m; PetalLength = 5.8m; PetalWidth = 2.2m; Class = "Iris-virginica" }; 
                 { SepalLength = 7.6m; SepalWidth = 3m; PetalLength = 6.6m; PetalWidth = 2.1m; Class = "Iris-virginica" }; 
                 { SepalLength = 4.9m; SepalWidth = 2.5m; PetalLength = 4.5m; PetalWidth = 1.7m; Class = "Iris-virginica" }; 
                 { SepalLength = 7.3m; SepalWidth = 2.9m; PetalLength = 6.3m; PetalWidth = 1.8m; Class = "Iris-virginica" }; 
                 { SepalLength = 6.7m; SepalWidth = 2.5m; PetalLength = 5.8m; PetalWidth = 1.8m; Class = "Iris-virginica" }; 
                 { SepalLength = 7.2m; SepalWidth = 3.6m; PetalLength = 6.1m; PetalWidth = 2.5m; Class = "Iris-virginica" }; 
                 { SepalLength = 6.5m; SepalWidth = 3.2m; PetalLength = 5.1m; PetalWidth = 2m; Class = "Iris-virginica" }; 
                 { SepalLength = 6.4m; SepalWidth = 2.7m; PetalLength = 5.3m; PetalWidth = 1.9m; Class = "Iris-virginica" }; 
                 { SepalLength = 6.8m; SepalWidth = 3m; PetalLength = 5.5m; PetalWidth = 2.1m; Class = "Iris-virginica" }; 
                 { SepalLength = 5.7m; SepalWidth = 2.5m; PetalLength = 5m; PetalWidth = 2m; Class = "Iris-virginica" }; 
                 { SepalLength = 5.8m; SepalWidth = 2.8m; PetalLength = 5.1m; PetalWidth = 2.4m; Class = "Iris-virginica" }; 
                 { SepalLength = 6.4m; SepalWidth = 3.2m; PetalLength = 5.3m; PetalWidth = 2.3m; Class = "Iris-virginica" }; 
                 { SepalLength = 6.5m; SepalWidth = 3m; PetalLength = 5.5m; PetalWidth = 1.8m; Class = "Iris-virginica" }; 
                 { SepalLength = 7.7m; SepalWidth = 3.8m; PetalLength = 6.7m; PetalWidth = 2.2m; Class = "Iris-virginica" }; 
                 { SepalLength = 7.7m; SepalWidth = 2.6m; PetalLength = 6.9m; PetalWidth = 2.3m; Class = "Iris-virginica" }; 
                 { SepalLength = 6m; SepalWidth = 2.2m; PetalLength = 5m; PetalWidth = 1.5m; Class = "Iris-virginica" }; 
                 { SepalLength = 6.9m; SepalWidth = 3.2m; PetalLength = 5.7m; PetalWidth = 2.3m; Class = "Iris-virginica" }; 
                 { SepalLength = 5.6m; SepalWidth = 2.8m; PetalLength = 4.9m; PetalWidth = 2m; Class = "Iris-virginica" }; 
                 { SepalLength = 7.7m; SepalWidth = 2.8m; PetalLength = 6.7m; PetalWidth = 2m; Class = "Iris-virginica" }; 
                 { SepalLength = 6.3m; SepalWidth = 2.7m; PetalLength = 4.9m; PetalWidth = 1.8m; Class = "Iris-virginica" }; 
                 { SepalLength = 6.7m; SepalWidth = 3.3m; PetalLength = 5.7m; PetalWidth = 2.1m; Class = "Iris-virginica" }; 
                 { SepalLength = 7.2m; SepalWidth = 3.2m; PetalLength = 6m; PetalWidth = 1.8m; Class = "Iris-virginica" }; 
                 { SepalLength = 6.2m; SepalWidth = 2.8m; PetalLength = 4.8m; PetalWidth = 1.8m; Class = "Iris-virginica" }; 
                 { SepalLength = 6.1m; SepalWidth = 3m; PetalLength = 4.9m; PetalWidth = 1.8m; Class = "Iris-virginica" }; 
                 { SepalLength = 6.4m; SepalWidth = 2.8m; PetalLength = 5.6m; PetalWidth = 2.1m; Class = "Iris-virginica" }; 
                 { SepalLength = 7.2m; SepalWidth = 3m; PetalLength = 5.8m; PetalWidth = 1.6m; Class = "Iris-virginica" }; 
                 { SepalLength = 7.4m; SepalWidth = 2.8m; PetalLength = 6.1m; PetalWidth = 1.9m; Class = "Iris-virginica" }; 
                 { SepalLength = 7.9m; SepalWidth = 3.8m; PetalLength = 6.4m; PetalWidth = 2m; Class = "Iris-virginica" }; 
                 { SepalLength = 6.4m; SepalWidth = 2.8m; PetalLength = 5.6m; PetalWidth = 2.2m; Class = "Iris-virginica" }; 
                 { SepalLength = 6.3m; SepalWidth = 2.8m; PetalLength = 5.1m; PetalWidth = 1.5m; Class = "Iris-virginica" }; 
                 { SepalLength = 6.1m; SepalWidth = 2.6m; PetalLength = 5.6m; PetalWidth = 1.4m; Class = "Iris-virginica" }; 
                 { SepalLength = 7.7m; SepalWidth = 3m; PetalLength = 6.1m; PetalWidth = 2.3m; Class = "Iris-virginica" }; 
                 { SepalLength = 6.3m; SepalWidth = 3.4m; PetalLength = 5.6m; PetalWidth = 2.4m; Class = "Iris-virginica" }; 
                 { SepalLength = 6.4m; SepalWidth = 3.1m; PetalLength = 5.5m; PetalWidth = 1.8m; Class = "Iris-virginica" }; 
                 { SepalLength = 6m; SepalWidth = 3m; PetalLength = 4.8m; PetalWidth = 1.8m; Class = "Iris-virginica" }; 
                 { SepalLength = 6.9m; SepalWidth = 3.1m; PetalLength = 5.4m; PetalWidth = 2.1m; Class = "Iris-virginica" }; 
                 { SepalLength = 6.7m; SepalWidth = 3.1m; PetalLength = 5.6m; PetalWidth = 2.4m; Class = "Iris-virginica" }; 
                 { SepalLength = 6.9m; SepalWidth = 3.1m; PetalLength = 5.1m; PetalWidth = 2.3m; Class = "Iris-virginica" }; 
                 { SepalLength = 5.8m; SepalWidth = 2.7m; PetalLength = 5.1m; PetalWidth = 1.9m; Class = "Iris-virginica" }; 
                 { SepalLength = 6.8m; SepalWidth = 3.2m; PetalLength = 5.9m; PetalWidth = 2.3m; Class = "Iris-virginica" }; 
                 { SepalLength = 6.7m; SepalWidth = 3.3m; PetalLength = 5.7m; PetalWidth = 2.5m; Class = "Iris-virginica" }; 
                 { SepalLength = 6.7m; SepalWidth = 3m; PetalLength = 5.2m; PetalWidth = 2.3m; Class = "Iris-virginica" }; 
                 { SepalLength = 6.3m; SepalWidth = 2.5m; PetalLength = 5m; PetalWidth = 1.9m; Class = "Iris-virginica" }; 
                 { SepalLength = 6.5m; SepalWidth = 3m; PetalLength = 5.2m; PetalWidth = 2m; Class = "Iris-virginica" }; 
                 { SepalLength = 6.2m; SepalWidth = 3.4m; PetalLength = 5.4m; PetalWidth = 2.3m; Class = "Iris-virginica" }; 
                 { SepalLength = 5.9m; SepalWidth = 3m; PetalLength = 5.1m; PetalWidth = 1.8m; Class = "Iris-virginica" }; |]


System.Console.WriteLine("Hello World!")
let description = Descriptor.Create<Iris>()
System.Console.WriteLine(description)
let generator = new DecisionTreeGenerator()
let data = irises |> Array.map box :> System.Collections.Generic.IEnumerable<obj>
let model = generator.Generate(irises)
System.Console.WriteLine("Generated model:")
System.Console.WriteLine(model)
