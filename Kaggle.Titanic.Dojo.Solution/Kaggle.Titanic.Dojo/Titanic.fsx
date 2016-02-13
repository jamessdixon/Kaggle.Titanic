
//Thanks to the following examples
//http://fssnip.net/jb
//http://numl.net/
//https://www.kaggle.com/yildirimarda/titanic/titanic-test3/files

// ------------------------------------------------------------------
// TUTORIAL: Bring in data using type providers 
// ------------------------------------------------------------------

//Load Data from the local file system
#r "../packages/FSharp.Data.2.2.5/lib/net40/FSharp.Data.dll"
open FSharp.Data
[<Literal>]
let trainPath = "../data/train.csv"
type Context = CsvProvider<trainPath>
let rows = Context.GetSample().Rows

//Explore The Data
let first = rows |> Seq.head
first.Name
first.Age

// Print names of surviving children
// (Note - the value of age may be missing, or silly)
for row in rows do
  if row.Survived = true && row.Age <> 0.0 && row.Age < 18.0 then
    printfn "%s (%f)" row.Name row.Age

// TASK: Print names of surviving males 
// who have name longer than 40 characters
// Your Code Here

// ------------------------------------------------------------------
// TUTORIAL: Introdcing higher-order, first-class functions & collections 
// ------------------------------------------------------------------

// Helper functions that extract information from a row 
// they all have a single parameter - passenger
// the type is AFTER the name of the parameter
// and is seperated by a colon
let survived (passenger:Context.Row) = 
  passenger.Survived = true
  
let name (row:Context.Row) = 
  row.Name
   
let hasAge (row:Context.Row) = 
  System.Double.IsNaN(row.Age) = false

let age (row:Context.Row) = 
  row.Age 

// Call them on the first line
survived first
name first
age first

// Seq.* functions can be used to implement LINQ-like queries
// For example, get a sequence of names: 
Seq.map name rows

// Get count of passangers & average age on Titanic
Seq.length rows
Seq.average (Seq.map age (Seq.filter hasAge rows))

// Nicer notation using the pipelining operator
rows
|> Seq.filter hasAge
|> Seq.map age
|> Seq.average

// Or we can use lambda functions, which makes things easier
rows
|> Seq.filter (fun r -> System.Double.IsNaN(r.Age) = false)
|> Seq.averageBy (fun r -> float r.Age)

// TASK #2: Find out whether the average age of those who survived
// is greater/smaller than the average age of those who died

// ------------------------------------------------------------------
// TUTORIAL: Creating an array of records from the CSV Provider
// ------------------------------------------------------------------
//Create a record type
//The title attribute is an option type
type Passenger = {passengerId:int;survived:bool; pClass:int; 
                  name:string; sex:string; age:float; sibSp:int;
                  parch:int; ticket:string; fare:float;
                  cabin:string;embarked:string;familySize:int;
                  title:option<string>; deck:string}

//Create an array of Passengers
//Notice we added 2 additional columns on the end
//The first additional column is the size of the family (adding SibSp + Parch + 1)
//The second is the title of the person (we will populate that later) so it gets a 'None'
let passengers = 
    Context.GetSample().Rows
    |> Seq.map(fun r -> {passengerId=r.PassengerId;survived=r.Survived;
                        pClass=r.Pclass;name=r.Name;sex=r.Sex;
                        age=r.Age;sibSp=r.SibSp;parch=r.Parch;
                        ticket=r.Ticket;fare=float r.Fare;cabin=r.Cabin;
                        embarked=r.Embarked;familySize=r.SibSp + r.Parch + 1;
                        title=None; deck=System.String.Empty})
    |>Seq.toArray

// ------------------------------------------------------------------
// TUTORIAL: Cleaning Of Data 
// ------------------------------------------------------------------

//How many total passengers?
passengers |> Seq.length

//How many are missing an embarked?
passengers
|> Seq.filter(fun p -> System.String.IsNullOrEmpty(p.embarked))
|> Seq.length

//Let's replace the missing with the most common port of embarkment value
//There are 3 possible values (C = Cherbourg; Q = Queenstown; S = Southampton)
//Note that tuples typically use a single letter, but we used the whole word to
//help make it more readable
passengers
|> Seq.groupBy(fun passenger -> passenger.embarked)
|> Seq.map(fun (embarked,passengers) -> embarked, passengers |> Seq.length)
|> Seq.map(fun (embarked,passengerCount) -> embarked,passengerCount, (float)passengerCount/891.0)

//Since 72% came from "S", let's give those 3 rows "S"
//Arrays are mutable (via their index value) and records are not
passengers
|> Seq.iteri(fun idx p -> if System.String.IsNullOrEmpty(p.embarked) then
                            passengers.[idx] <- { p with embarked = "S" })

// TASK: Let's replace any passengers that have a nan fare value 
// with the mean fare value (Seq.averageBy)
// Your Code Here

// TASK2: Let's replace any passengers that have a nan age value 
// with the average avg value (Seq.averageBy)
// Your Code Here

// ------------------------------------------------------------------
// TUTORIAL: Cleaning Of The Data: Part 2
// ------------------------------------------------------------------
//Let's give each person a title
//There are 3 possible values (Mr; Mrs; Miss)
//To do that, we need to search the name attribute
//And then locate certain key words
//The FSharp Map type is perfect for this kind of thing
let titleMap = Map.ofList [("Lady","Miss"); 
                            ("Countess","Miss");
                            ("Mlle","Miss"); 
                            ("Mme","Miss"); 
                            ("Mee","Miss"); 
                            ("Ms","Miss");
                            ("Miss","Miss");
                            ("Dona","Mrs");
                            ("Mrs","Mrs");
                            ("Capt","Mr");
                            ("Don","Mr");
                            ("Major","Mr");
                            ("Sir","Mr");
                            ("Col","Mr");
                            ("Jonkheer","Mr");
                            ("Rev","Mr");
                            ("Dr","Mr");
                            ("Master","Mr");
                            ("Mr","Mr")]

//Create a helper function that
//splits each name into individual words
//then look up each word in the titleMap
//if the key is found, the value will be returned as Some<t> else None
//if more than match happens, take the first one
//With the assumption that the title is in the beginning of the name
let getTitle (name:string) =
    let name' = name.Replace('.',' ')
    let words = name'.Split(' ') 
    let titles = 
        words 
        |> Array.map(fun w -> w.Trim())
        |> Seq.map(fun w -> titleMap.TryFind(w))
        |> Seq.filter(fun t -> t.IsSome)
    if titles |> Seq.length > 0 then titles |> Seq.head
    else None

//Assign the Title
passengers
|> Seq.iteri(fun idx p -> passengers.[idx] <- { p with title = getTitle p.name })

//Note that there was 1 woman doctor on the titanic, passenger number 797 -> index 796
passengers.[796] <- {passengers.[796] with title = Some "Mrs" }

// TASK: Let's assign a value to the deck
// You can look at the first letter of a person's cabin ("C85";"C123";"E46")
// To detemine their deck.  If they do not have a cabin, 
// you can assume they were in steerage
// You don't need a map, but a supporting function would be helpful

// ------------------------------------------------------------------
// TUTORIAL: Determining If Someone Lived Or Died
// ------------------------------------------------------------------

//Using numl
#r "../packages/numl.0.8.26.0/lib/net40/numl.dll"
open numl

open numl.Model
open numl.Math.Probability
open numl.Supervised.DecisionTree

//Create a numl class
//Note that the survived is mutable
//so that numl can alter its value when it gets to the training phase
type NumlPassenger = {[<Feature>] sex:string; [<Feature>] enbarked:string; [<Label>] mutable survived: bool}

//Create a dataframe populated with instances of the numl classes
let dataFrame = 
    passengers 
    |> Seq.map(fun p -> {NumlPassenger.sex = p.sex; enbarked = p.embarked; survived=p.survived})
    |> Seq.map box

//Create desscriptor so the output is readable
//Set the seed to help when the samples are randomized
let descriptor = Descriptor.Create<NumlPassenger>()
Sampling.SetSeedFromSystemTime() 
//Create a Decision Tree Generator
//If you want to try a different model, select a 
//different Generator
let generator = new DecisionTreeGenerator(descriptor)
generator.SetHint(false)

//Have the global learner learn from the data and the generator
//to create the model
let model = Learner.Learn(dataFrame, 0.80, 25, generator)

//Prinout out the results
printfn "%A" model.Model

//With the model trained
//We can now see what scores we get with the test data
//Load Data from the local file system
[<Literal>]
let testPath = "../data/test.csv"
type testContext = CsvProvider<testPath>
let testRows = testContext.GetSample().Rows

//The test data is identitcal to the train data
//with the exception that the survived is not included
//numl wants the same data structured passed in for calculation
//so we just default it to false
//We then have the model predict survival 
//And since numl retuns the result as an object
//we have to cast it back into a NumlPassenger class
let testFrame = 
    testRows 
    |> Seq.map(fun tr -> {NumlPassenger.sex = tr.Sex; enbarked = tr.Embarked; survived=false})
    |> Seq.map(fun np -> model.Model.Predict(np))
    |> Seq.map(fun o -> unbox o)
   
//With the test frame ready, we need to combine it
//with the original testRows
//As long as it was not run in parallel, order is preserved
//Seq.zip "glues" the two sequences together
//Next, in the map
//we take the passengerId and the survival prediction
//and turn it into a single comma-seperated string
//Finally, we turn it into a list
let submission =
    Seq.zip testRows testFrame
    |> Seq.map(fun (tr,np) -> sprintf "%i,%A" tr.PassengerId np.survived)
    |> List.ofSeq

//Write it to the file system
//We add a header row of "PassengerId,Survived"
//Because that is what Kaggle wants fot its submission
//Go ahead and check the data folder and see if there is the .csv
//in it.  When you open it, it should have
//a single header row and 418 lines of predictions
//You can then upload it to Kaggle and see your score
open System.IO
let writeResults =
    let outputPath = __SOURCE_DIRECTORY__ + "../../data/submission.csv"
    File.WriteAllLines(outputPath, "PassengerId,Survived" :: submission)

// TASK: Create another tree with two varaibles: sex, and class
// Once you have a model, predict the test set
// and submit it to Kaggle
// Good luck!


//If you still have some time
//Go back to this:
//  TASK: Let's replace any passengers that have a nan fare value 
//  with the mean fare value (Seq.averageBy)
//And replace the nan fare with a more precise value than the avg
//Perhaps the median?

//Go back to this:
//  TASK2: Let's replace any passengers that have a nan age value 
//  with the average avg value (Seq.averageBy)
//And replace the nan age with a more precise value than the avg
//Perhaps a logistic regression?


