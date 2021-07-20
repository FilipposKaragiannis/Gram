Hi, 

To begin with I would like to apologise for submitting an incomplete task. Unfortunately I am going through a really hectic period and didn't manage to complete the project in time, with the structured solution approach I took. 


The Game is not functional, as the gameplay component and the UI hooks are missing. In this solution I try to demonstrate codebase architecture, coding principles and design patterns, but I understand that this is a not complete project. Bellow I will also try to explain how I intented to complete the task and structure every aspect of the game framework. 

I consider this mini game a data oriented game. The codebase is structured with the onion architecture in mind, and is split up into different project for each layer of the framework. 

The C# solution is ported into unity as generated .dlls and there is one unity script that serves as a bridge between unity and the Gram.sln. You can find the script attached to the only gameObject entry point. 

For the structuring of this project I borrowed some core features I've written for re-using in personal projects. These include Ioc container registrations, MessageBus, Logging, Events, some Models and extensions and can be found inside Gram.Rpg.Client.Core project. 

When the game starts the state of the world is requested, which will initialise the player (either load from storage or create a new player and allocate heroes).

Created a domain entity that represents the player and holds any relevant player data. This includes the Hero Inventory, and some Player Stats (intented to display those in the main menu.)

Implemented data persistence using file storage. In the editor you can find the saved player profile under unity/localStorage as soon as you play the game in the editor. 

I have implemented all the Application logic. Have approached the game actions as single use cases which will update the player based on the action invoked. There are two application use cases that were intented to be invoked after a battle result. You can find PlayerLosesBattle/PlayerWinsBattle to see the logic. These should be updating the player profile and perform the saving using a player gateway. Also wrote tests for these use cases which can be found in the Gram.Rpg.Client.Tests project. 




Not completed: 

The UI structure in the presentation layer is not complete. I intented to create a ScreenManager to control screen transitions between menu and in-game, and generate all the UI elements procedurally. 

For the gameplay my design would include a BattleSession that takes in 2 player models, a LocalPlayer and an Opponent. These models would hold game related information - (heroes inventory with hero stats), and a battle config that would hold game play related information (i.e. firstToAttack, turnTime etc).

Would create a Scorer object that would track all the in game events and create an eventHistory object to be used on the Application layer as soon as the battle would end.

Given that this is a turn based game, for the core gameplay I would use a hierarchical state machine (Master/Slave) and establish communication with the presentation layer via the container by registering a scoped registration. 


PS. To compile the solution and port the dlls into unity you can run the bash script from `scripts/buildAndDeploy.sh`