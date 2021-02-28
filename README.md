# WetherPredictionGameB
back End

This is a simple app based on guesswork. Anyone who could guess the temperature of the cities would set a better record.
The game is played as description blow :
The first preparations for the starting the game are done by using " InitGame" service. 
By using " InitUserGuess" service, the guesses of the user, as well as the real temperatures of the cities are seved. 
" Calculate Result Game " service will calculate the result of the game all the user. 
"AllGameRecord " service is for displaying all the records. 

The source of the project is written with ASP. Net Core 3.1 and " 3 layer architecture " is used. 
Efforts have been made to follow the clean code. 
Principles of "ocp" and "liskof" are used. 
The pattern of "singleton " is implemented. 
Also "caching " has been used in some parts and "Exception handling" is done. 
If you want to use the source of the program you should specify the "String connection " in the "appsettings " file.
It is now conected to the local database.
The Api from "metaweather.com "  has been used to receive weather information for the cities.
Data architecture is in "code first".
