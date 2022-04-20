# 02: Maze Generation - Making the game

In the last section we created a random maze and procedurally generated the environment (floor, ceiling and walls). Now we want to actually create a game around the maze, where the player walks around the dungeon to locate the 'treasure' while avoiding the crawling 'Scary Man' that is constantly hunting them.

A lot of things have been included in the **starter code**, including a First Person Controller (FPS) script, materials, the 'Scary Man' model, and various scene lighting etc. What we are going to do next is:

- place the player at the 'start' of the maze;
- create the 'treasure' to be found programatically, and place it at the 'end' of the maze;
- rig up some triggers to restart the maze if the player 'finds' the treasure;
- and finally, instantiate the 'Scary Man' at the end of the maze and start his seeking behaviour.

## Adding the player

First, we are going to add a few variables to **MazeConstructor** to store references to things for later use:

```csharp
public float hallWidth{ get; private set; }
public int goalRow{ get; private set; }
public int goalCol{ get; private set; }
```

`hallWidth` will be used later for placing the player and the treasure in the middle of the hallway. `goalRow` and `goalCol` are references to where the treasure (and Scary Man) will be placed. 

We can set `hallWidth` by putting this code in `Awake`, below the creation of the `meshGenerator`:

```csharp
hallWidth = meshGenerator.width;
```

And we can set `goalRow` and `goalCol` by adding these 2 lines to `GenerateNewMaze`, just before `DisplayMaze()`:

```csharp
goalRow = data.GetUpperBound(0) - 1;
goalCol = data.GetUpperBound(1) - 1;
```

This gets both the last row and column from the maze data, but subtracts 1 (since the actual last row and column will be a wall).

Next, we are going to add a utility method to destroy all the generated GameObjects when we need to create a new maze:

```csharp
public void DisposeOldMaze()
{
    GameObject[] objects = GameObject.FindGameObjectsWithTag("Generated");
    foreach (GameObject go in objects) {
        Destroy(go);
    }
}
```

This method simply finds all the GameObjects with the **Generated** tag and destroys them one by one. We want to call it before we create a new maze, so we'll add it to the very start of `GenerateNewMaze`... your entire `GenerateNewMaze` method should now look like this:

```csharp
public void GenerateNewMaze(int sizeRows, int sizeCols)
{        
    DisposeOldMaze();  
    
    if (sizeRows % 2 == 0 && sizeCols % 2 == 0)
        Debug.LogError("Odd numbers work better for dungeon size.");
        
    data = FromDimensions(sizeRows, sizeCols);

    goalRow = data.GetUpperBound(0) - 1;
    goalCol = data.GetUpperBound(1) - 1;                                    

    DisplayMaze();            
}  
```
