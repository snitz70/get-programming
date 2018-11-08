open System

/// Gets the distance to a given destination 
let getDistance (destination) =
    if destination = "Gas" then 10
    elif destination = "Home" then 25
    elif destination = "Office" then 50
    elif destination = "Stadium" then 25
    else failwith "Unknown destination!"

let calculateRemainingPetrol (currentPetrol, distance) = 
    if currentPetrol > distance then currentPetrol - distance
    else failwith "Oops!  You've run out of petrol!"

let driveTo (petrol, destination) = 
    let petrol = calculateRemainingPetrol (petrol, getDistance(destination))
    if destination = "Gas" then petrol + 50
    else petrol

// Couple of quick tests
getDistance("Home") = 25
getDistance("Stadium") = 25
calculateRemainingPetrol (100, 50)
calculateRemainingPetrol (10, 50)
driveTo(100, "Office")
driveTo(100, "Home")

let a = driveTo(100, "Office")
let b = driveTo(a, "Stadium")
let c = driveTo(b, "Gas")
let answer = driveTo(c, "Home")

