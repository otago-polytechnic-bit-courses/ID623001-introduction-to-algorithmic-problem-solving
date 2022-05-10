# 02: Maze Generation - Making the game

In the last section we created a random maze and procedurally generated the environment (floor, ceiling and walls). Now we want to actually create a game around the maze, where the player walks around the dungeon to locate the 'treasure' while avoiding the crawling 'Scary Man' that is constantly hunting them.

A lot of things have been included in the **starter code**, including a First Person Controller (FPS) script, materials, the 'Scary Man' model, and various scene lighting etc. What we are going to do next is:

- place the player at the 'start' of the maze;
- instantiate the 'Scary Man' at the end of the maze and start his seeking behaviour;
- create the 'treasure' to be found programatically, and place it at the 'end' of the maze;
- and finally rig up some triggers to restart the maze if the player gets caught by the enemy.

## Setup

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

## Adding the player

Save the script and return to the editor. Open the **GameController** script. We are going to add the player to the scene. The starter code comes with a **player prefab** and we are going to instantiate the player at the start of the maze in a very familiar way. Add these variables to **GameController**:

```csharp
public GameObject playerPrefab;
```

First we will create a method called `CreatePlayer` and add this code to it:

```csharp
private void CreatePlayer()
{
    Vector3 playerStartPosition = new Vector3(constructor.hallWidth, 1, constructor.hallWidth);  
    GameObject player = Instantiate(playerPrefab, playerStartPosition, Quaternion.identity);
    player.tag = "Generated";
}
```

We are adding the player to the **start** of the maze - we are placing the player in square [1,1] (the first empty cell in the maze). Our floor meshes are centred on 0,0; if you recall the `AddQuad` method from **MazeMeshGenerator**, our quad meshes have these vertices:

```csharp
Vector3 vert1 = new Vector3(-.5f, -.5f, 0);
Vector3 vert2 = new Vector3(-.5f, .5f, 0);
Vector3 vert3 = new Vector3(.5f, .5f, 0);
Vector3 vert4 = new Vector3(.5f, -.5f, 0);
```

Since they extend from -.5f to .5f along the two axes, the centre must be at 0,0.

Thus, in figuring out our `playerStartPosition` in **real world coordinates**, we multiply the starting cell values [1,1] by the `hallWidth` value we set in **MazeConstructor**.

We instantiate the player as we have in previous games, passing in a prefab, the start position, and `Quarternion.identity` for the default rotation.

Finally, we tag the `player` with **"Generated"** for later use. Call this method in `Start` below `constructor.GenerateNewMaze(rows,cols);`:

```csharp
void Start() 
{
    constructor.GenerateNewMaze(rows,cols);
        
    CreatePlayer();
}
```

Save the script and return to the editor. Drag the **Player** prefab into the slot on **Game Controller**. Run the game, and you should now be able to walk around your generated maze!

## Adding the enemy

Adding the 'Scary Man' enemy is going to be like adding the player, only on the opposite end of the maze. First, in the **GameController** script, add this variable below `playerPrefab`:

```csharp
public GameObject monsterPrefab;
```

Next, create a new method called `CreateMonster` and add this code to it:

```csharp
private void CreateMonster()
{
    Vector3 monsterPosition = new Vector3(constructor.goalCol * constructor.hallWidth, 0f, constructor.goalRow * constructor.hallWidth);
    GameObject monster = Instantiate(monsterPrefab, monsterPosition, Quaternion.identity);
    monster.tag = "Generated";    
}
```

Virtually the same, except we are using the `goalCol` and `goalRow` from the **MazeConstructor** in our calculations of where the final cell is in the real world. Add `CreateMonster()` to `Start`:

```csharp
void Start() 
{
    constructor.GenerateNewMaze(rows,cols);
        
    CreatePlayer();
    CreateMonster();
}
```

Save the script and return to the editor. Drag the **Monster** prefab into the slot on **Game Controller**. Run the game again, make your way to the end of the maze (or just look at the Scene view) and you should have a very scary man crawling in place in the final cell.
