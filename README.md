# MazeRunner
I made this game under a time restriction of 20 hours in Unity using C#. All graphics have been downloaded from external sources, but the code is all mine.  
To generate the maze the Recursive Backtracking algorithm has been implemented: https://en.wikipedia.org/wiki/Maze_generation_algorithm

## How to play
Welcome to MazeRunner! The goal of the game is to collect all the coins before the time is up and your score reaches 0. The faster you collect all coins, the higher your score. You can collect these coins by running around the maze. Of course, it is quite hard to find your way around there, so there is always a camera hovering above the maze. You can view it by pressing the Spacebar: but watch out! When you are using this camera, your score decreases even faster! When you start out, I recommend an 8x8 maze, but anything is possible. The maze does not have to be square.

## Development notes
During the design of the game I always had the time limit in mind, so I had to cut some corners. One of them is that the code to center the top-down camera is not perfect, in big mazes a slight amount of whitespace appears.  
  
There are also a lot of lists storing data about cells: one with all cells, one with the stack and one with visited cells. This is very memory inefficient and should be changed.  
  
Another issue is that the spawning of coins is very inefficient. It would be way more efficient to just pick a few random places from the cells list and spawn a coin there, instead of looping through all of them with a small chance of spawning. It is also possible for coins to spawn close to each other or the player, which of course is not supposed to happen. This could easily be calculated by checking the distance between cells using build in Unity methods.  
  
The UI/UX can also be improved in several ways. The main issue is that it scales very poorly, but I couldn’t quite figure out how to fix this, so I decided to spend my time elsewhere. On the UX front there should be a guide for new players and a hint for a nice maze size. It is also hard to find where you are in the top-down view. This could be fixed by adding a “You are here” arrow. The Play button is blocked if an input field is empty or wrong, but there is just one Boolean for two input fields, so instead of both fields having to be OK only one must be now. And in general, the graphics of the game are still very, very ugly.  
  
There are also quite a few hardcoded variables left, like the spawn height for the coins and walls. These should be calculated automatically based on wall height.  
  
I could also have implemented Interfaces, and should have spawned the maze in its own GameObject parent to keep the game view clean.  
