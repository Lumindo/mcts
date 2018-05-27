# mcts
The Monte-Carlo Tree Search is an algorithm for building and expanding a game tree that is based on statistics instead on heuristic evaluation functions. Mcts avoids using a heuristic by building its tree as guided by playing games of random move sequences. While a sequence of random moves by itself has a very low playing strength, in aggregate random games tend to favor the player that is in a better position.

For theoretical details see:

> Cameron Brown et. al. - A Survey of Monte-Carlo Tree Search Methods
> Roelofs - Monte-Carlo Tree Search in a Modern Board Game Framework.
> Schadd - Monte-Carlo Search Techniques in the Modern Board Game "Thurn and Taxis".
> Silver - Reinforcement Learning and Simulation-Based Search in Computer Go.
> Haute - Search in Trees with Chance Nodes.


This repository contains the following project files:

> GameTreeCore:		a library for creating game trees. 
> MctsCore: 		a library of the mcts algorithm. 
> HusCore: 		a library of the Hus game system 
			(see https://en.wikipedia.org/wiki/%C7%81Hus)
> TestMctsWithHus: 	a simple demo program to show how the libraries are used.

Builts and documentations of the above mentioned projects can be found under /bin/Release/ .
