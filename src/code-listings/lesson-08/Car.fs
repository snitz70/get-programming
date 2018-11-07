module Car

open System

let getDistance (destination) =
    if destination = "Gas" then 10
    elif destination = "Home" then 25
    elif destination = "Office" then 50
    elif destination = "Stadium" then 25
    else failwith "Unknown destination!"

let calculateRemainingPetrol (currentPetrol:int, distance:int) = 
    if distance < currentPetrol then
        currentPetrol - distance
    else
        failwith "Oops!  You've run out of petrol"

/// Drives to a given destination given a starting amount of petrol
let driveTo (petrol, destination) =
    let petrol = calculateRemainingPetrol(petrol, getDistance(destination))
    
    if destination = "Gas" then 
        petrol + 50
    else
        petrol