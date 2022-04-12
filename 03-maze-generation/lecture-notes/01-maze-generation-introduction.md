# 01: Maze Generation - Introduction

## Starter project

Download the **starter code** and open it as a project in Unity.

## Maze generation

Some games lean heavily on **good level design** - carefully thought out and constructed levels with interesting layouts and paths. This can be time-consuming though, which is why a lot of games rely on **procedural generation** of levels - you can create near-infinite variations with (relatively) minimal code. Particularly, for **dungeon crawler** type games, **maze generation** can create interesting (and unique) labyrinths for the player to explore.

A maze typically has a **start** and an **end** and a **perfect** path between them (that is, one correct path to **solve** the maze, and then a number of other paths that lead to dead-ends; there are no 'loops' in a perfect maze); however, in games (especially the 'dungeon' type game we've mentioned before) some looping paths make for more interesting gameplay. So in this project, we will create a maze with a **start**, an **end** and various paths... oh, and a scary monster dude that hunts you down (but more on that later)...

### Code architecture

Start by adding an empty **Game Object** to the scene, and name it **Controller**. Reset its transform so its position is at **(X:0, Y:0, Z:0)**. This object is simply an attachment point for the scripts that control the game.

In the **Scripts** folder, create a new C# script named **GameController**, and another script named **MazeConstructor**. The first script will manage the overall game, while the second will specifically handle the maze generation.

Replace everything in **GameController** with the following code

```csharp
using System;
using UnityEngine;

[RequireComponent(typeof(MazeConstructor))]           

public class GameController : MonoBehaviour
{
    private MazeConstructor constructor;

    void Start()
    {
        constructor = GetComponent<MazeConstructor>();
    }
}
```

The `RequireComponent` attribute ensures that a **MazeConstructor** component will also be added when you add this script to a GameObject - we need a **MazeConstructor** always in this game.

The `constructor` variable gets a reference to that component (which will be added automatically in a moment).

Save the script and return to the editor. Drag the **GameController** script from the folder onto the **Controller** Game Object in the **hierarchy** - it adds **GameController** *and* **MazeConstructor** as components of the object.

In the **MazeContstructor** script, replace everything with the following code:

```csharp
using UnityEngine;

public class MazeConstructor : MonoBehaviour
{
    public bool showDebug;
    
    [SerializeField] private Material mazeMat1;
    [SerializeField] private Material mazeMat2;
    [SerializeField] private Material startMat;
    [SerializeField] private Material treasureMat;

    public int[,] data
    {
        get; private set;
    }

    void Awake()
    {
        // default to walls surrounding a single empty cell
        data = new int[,]
        {
            {1, 1, 1},
            {1, 0, 1},
            {1, 1, 1}
        };
    }
}
```

The `showDebug` boolean will be used to toggle the maze data on or off in the UI (that is, if we want to see the maze that was created on the screen).

The four **Material** variables will be used to texture the parts of the maze (walls, floor, etc) - they are `private` so they can't be accessed through code outside this script, but marked `[SerializeField]` so we can still access them from the Inspector.

The next property - `data` - is a **2D array** (or **matrix**) of numbers (**int**) - `int[,]`. Our maze data is just a grid of rows and columns, and the data is just a **0** or a **1** to indicate if the spot is **open** or **blocked** (wall). Also, we have made the variable `public`, but then also specified a `private set`, which indicates this property is **read-only** outside of this class.

In the `Awake` method, we are simply defaulting `data` to a 'single room' with walls all around.

Add the following method:

```csharp
void OnGUI()
{
    if (!showDebug)
        return;

    int[,] maze = data;
    int rMax = maze.GetUpperBound(0);
    int cMax = maze.GetUpperBound(1);

    string msg = "";

    for (int i = rMax; i >= 0; i--)
    {
        for (int j = 0; j <= cMax; j++)
            msg += maze[i, j] == 0 ? "...." : "==";
        msg += "\n";
    }

    GUI.Label(new Rect(20, 20, 500, 500), msg);
}
```

This code simply prints a 'maze' onto the screen so we can see that the generation works - it is only for **debug purposes** and not used in the actual generation code.

Save the script and return to the editor. Make sure **Show Debug** is ticked on the **MazeConstructor**, and hit **Play**: the default maze should be shown on the screen.

### Generating the maze data

In the **GameController** script add the following variables above `Start`:

```csharp
[SerializeField] private int rows;
[SerializeField] private int cols;
```

This is how big our maze grid will be, in terms of rows and columns; we've made the variables `private` so we don't accidentally overwrite them somewhere in the code, but then marked them `[SerializeField]` so we can easily set them in the editor.

Add the following code to `Start`:

```csharp
constructor.GenerateNewMaze(rows, cols);
```

We are calling a method on the **MazeConstructor** script (`constructor`) called `GenerateNewMaze` and passing in the **rows** and **cols** values (note: this method doesn't exist yet, but we'll make it soon).

Open the **MazeConstructor** script and add the following method below `Awake`:

```csharp
public void GenerateNewMaze(int sizeRows, int sizeCols)
{
    if (sizeRows % 2 == 0 && sizeCols % 2 == 0)
        Debug.LogError("Odd numbers work better for dungeon size.");

    data = FromDimensions(sizeRows, sizeCols);
}
```

This is the method we called before in **GameController** that takes the **rows** and **cols** - we are going to add a lot more to this method, but for now it doesn't do much. First, it checks to see if the **sizeRows** and **sizeCols** arguments are both even (`% 2 == 0`) - because of the way walls are placed in this dungeon, having both even numbers for the rows and cols doesn't work as well for generating the maze, so this simply prints out a warning. The next part gets the maze data from another method called `FromDimensions`, which we will add now...

```csharp
public int[,] FromDimensions(int sizeRows, int sizeCols)
{
    int[,] maze = new int[sizeRows, sizeCols];
    // stub to fill in
    return maze;
}
```

Again, there is a lot more to add to this method, but for now it simply creates a new **2D array** of **int** to the row and col size we have specified, and returns it.

Save the script and return to the editor. Specify some numbers in the **rows** and **cols** on **GameController** and hit **Play** - you should see a 'maze' generated to those sizes printed on the screen (note that the 'maze' is currently all 'empty space' - that's because the default value for our array slots is `0`, and we haven't put the logic in yet to place walls).

Before we code the **maze generation algorithm**, we'll talk generally about how it works:

- the algorithm iterates over every other space in the grid (not every single space) to both **place a wall** and **choose an adjacent space to block** as well. 
- the algorithm also randomly decides if **the space should be skipped instead**, resulting in open spaces to vary the maze. 
 
This is an **extremely simple algorithm** that doesn't need to know anything about the rest of the maze, such as a list of branch points to iterate over (other implementations of maze generation algorithms will utilise **tree and graph structures** to trace back paths etc - generally needed for 'perfect mazes').

First add this variable to **MazeConstructor**:

```csharp
public float placementThreshold = 0.1f;   // chance of empty space
```

We will use this later to determine if we should place a wall in a space or not.

Add the following code to `FromDimensions`, replacing the line that says ``// stub to fill in``:

```csharp
int rMax = maze.GetUpperBound(0);
int cMax = maze.GetUpperBound(1);

for (int i = 0; i <= rMax; i++)        
    for (int j = 0; j <= cMax; j++)            
        if (i == 0 || j == 0 || i == rMax || j == cMax)                
            maze[i, j] = 1;                                    
        else if (i % 2 == 0 && j % 2 == 0 && Random.value > placementThreshold)                                    
        {
            maze[i, j] = 1;

            int a = Random.value < .5 ? 0 : (Random.value < .5 ? -1 : 1);
            int b = a != 0 ? 0 : (Random.value < .5 ? -1 : 1);
            maze[i+a, j+b] = 1;
        }  
```

That's all there is to the maze generation! Extremely **small** code... but a **little complex** - let's break it down:
- `int rMax = maze.GetUpperBound(0);` and `int cMax = maze.GetUpperBound(1);` are simply getting the upper indices of the maze (each dimension in turn). We'll use them to 'walk' through the grid.
- Next is a common programming pattern, the **double for loop**. Since we have a 2D array, we need to loop twice: once over all the rows, and then over each column (or cell) in that row.
- The `if` condition is checking for the boundaries of the maze - basically, if we are anywhere in the **first row**, **first column**, **last row** or **last column**, this *must* be a wall. Like our default 'room' above, we are setting the walls to be **1**.
- The `else if` is determining for all other **non-boundary spaces** if they should be a wall or a blank space (corridor). First, you'll notice the `% 2 == 0`, which we've seen a few times now. A refresher: this is calculating a **remainder** for a division by 2, which will give either a **0** or a **1**. In a nutshell, this code says **every second one** - we aren't looking at every space, but every **2nd space** (after the outer walls). This is because we want to always leave at least 1 blank space between inner walls: e.g. 101... 
- So, these are the first **2 conditions** - is this a space that *could potentially* be a wall? The final condition is randomly determining if the space *should be* a wall. `Random.value` is a quick **static** way of generating a number between 0 and 1. If that random value is greater than our `placementThreshold` variable (set at 0.1) then we will place a wall: `maze[i, j] = 1;`

The last 3 lines of code say that **as well as placing a wall here** we should **also place a wall next to here** in one of four directions: North, South, East or West. The first line is a **shorthand** way of writing if statements called a **ternary statement**. Let's break it down:
- `int a =` means that we are assigning into a variable called `a` something as a result of this if statement. Regardless of which condition is met (the if or else) we will assign some value into `a`.
- `Random.value < .5 ?` is the **condition** - it's the same as having `if(Random.value < .5){ ... }` - so we are generating a random number (between 0 and 1) and checking if it is less than 0.5.
- Next comes the **true** and **false** parts of the if statement: `0 : (Random.value < .5 ? -1 : 1);` - The `:` is like the `else` in this scenario... *if* the condition is **true** then assign `a = 0`... *else* assign `a = (Random.value < .5 ? -1 : 1)`. And what does that look like? Why, *another ternary!*
- So... we're in the **false** part of `(Random.value < .5)` (i.e. `Random.value` came out to be equal to or greater than 0.5)... Now we generate **a second random value** and see if *that* is less than 0.5... *if it is* then we are assigning `a = -1`, *else* we are assigning `a = 1`.
- After all that, `a` will be one of **0, 1 or -1**.
