# 02: Maze Generation - Making the game

In the last section we created a random maze and procedurally generated the environment (floor, ceiling and walls). Now we want to actually create a game around the maze, where the player walks around the dungeon to locate the 'treasure' while avoiding the crawling 'Scary Man' that is constantly hunting them.

A lot of things have been included in the **starter code**, including a First Person Controller (FPS) script, materials, the 'Scary Man' model, and various scene lighting etc. What we are going to do next is:

- place the player at the 'start' of the maze;
- create the 'treasure' to be found programatically, and place it at the 'end' of the maze;
- rig up some triggers to restart the maze if the player 'finds' the treasure;
- and finally, instantiate the 'Scary Man' at the end of the maze and start his seeking behaviour.

## Adding the player

First, we are going to add a heap of variables to **MazeConstructor** to store references to things for later use:

```csharp
public float hallWidth{ get; private set; }
public int startRow{ get; private set; }
public int startCol{ get; private set; }
public int goalRow{ get; private set; }
public int goalCol{ get; private set; }
```

`hallWidth` will be used later for placing the player and the treasure in the middle of the hallway. `startRow` and `startCol` will be references to where the player starts, and `goalRow` and `goalCol` are references to where the treasure (and Scary Man) will be placed.
