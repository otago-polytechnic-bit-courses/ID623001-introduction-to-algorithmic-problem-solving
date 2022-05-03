# 01: Maze Generation - Introduction

## Starter project

Download the **starter code** and open it as a project in Unity. *We may need to do a couple things to fix some missing references before we get started*.

- Open the **Player** prefab. If you have a 'missing script' error, select the **FpsMovement** script. Drag the **Main Camera** onto the **Head Cam** slot. Save the prefab.
- Open the **Monster** prefab. The **Animator** > **Controller** should be 'Low Crawl'. Select the **WhiteClown** in the hierarchy. The **Mesh** should be **WhiteClown**. Materials > Element 0 should be **whiteclown_diffuse**.

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

    void Awake()
    {
        constructor = GetComponent<MazeConstructor>();
    }
    
    void Start()
    {
    
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

In the **GameController** script add the following variables above `Awake`:

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
- Next, we are assigning a value into a variable called `b`. It's the exact same pattern as before... if `a != 0` (i.e. a is **-1 or 1**), then we assign `b = 0` - this is because we don't want to place a **diagonal** wall... if *both* `a` and `b` are **non-zero** we will move in a diagonal direction (we'll see this in a second).
- So, if `a == 0` (the **false/else** part of this), then we do our `Random.value < .5` trick again, and get either a **-1 or 1** to assign to `b`. At the end of this, `b` will be either **0, 1 or -1**, but will also be **opposite to a**, as in zero if a is non-zero or vice versa.

Save the script and return to the editor. Run the game and you should see a random maze displayed on the GUI, different each time you run the game!

### Generating the 3D Maze Mesh

This next part isn't really related to the maze generation algorithm, so we won't spend too long on the details of it - it is simply a way to procedurally generate the 3D walls and floor from the maze data.

Before we get into the code, we need to link some materials that will be used to texture our generated mesh. Select the **Graphics** folder in the Project window, and then Select **Controller** in the hierarchy to expose the **Maze Constructor**. Drag each of the materials from the **Graphics** folder over to the material slots in **Maze Constructor**: 

- **floor-mat** for Maze Mat 1
- **wall-mat** for Maze Mat 2
- **treasure** for Treasure Mat

We will also create a new tag called **Generated** - click on the Tag menu at the top of the Inspector and select **Add Tag**. When we generate the meshes, we will assign this tag to them to identify them later.

Create a new C# script called **MazeMeshGenerator** and replace the entire contents with this code:

```csharp
using System.Collections.Generic;
using UnityEngine;

public class MazeMeshGenerator
{    
    // generator params
    public float width;     // how wide are hallways
    public float height;    // how tall are hallways

    public MazeMeshGenerator()
    {
        width = 3.75f;
        height = 3.5f;
    }

    public Mesh FromData(int[,] data)
    {
        Mesh maze = new Mesh();
        List<Vector3> newVertices = new List<Vector3>();
        List<Vector2> newUVs = new List<Vector2>();

        maze.subMeshCount = 2;
        List<int> floorTriangles = new List<int>();
        List<int> wallTriangles = new List<int>();

        int rMax = data.GetUpperBound(0);
        int cMax = data.GetUpperBound(1);
        float halfH = height * .5f;

        for (int i = 0; i <= rMax; i++)        
            for (int j = 0; j <= cMax; j++)            
                if (data[i, j] != 1)
                {
                    // floor
                    AddQuad(Matrix4x4.TRS(
                        new Vector3(j * width, 0, i * width),
                        Quaternion.LookRotation(Vector3.up),
                        new Vector3(width, width, 1)
                    ), ref newVertices, ref newUVs, ref floorTriangles);

                    // ceiling
                    AddQuad(Matrix4x4.TRS(
                        new Vector3(j * width, height, i * width),
                        Quaternion.LookRotation(Vector3.down),
                        new Vector3(width, width, 1)
                    ), ref newVertices, ref newUVs, ref floorTriangles);

                    if (i - 1 < 0 || data[i-1, j] == 1)                    
                        AddQuad(Matrix4x4.TRS(
                            new Vector3(j * width, halfH, (i-.5f) * width),
                            Quaternion.LookRotation(Vector3.forward),
                            new Vector3(width, height, 1)
                        ), ref newVertices, ref newUVs, ref wallTriangles);                    

                    if (j + 1 > cMax || data[i, j+1] == 1)                    
                        AddQuad(Matrix4x4.TRS(
                            new Vector3((j+.5f) * width, halfH, i * width),
                            Quaternion.LookRotation(Vector3.left),
                            new Vector3(width, height, 1)
                        ), ref newVertices, ref newUVs, ref wallTriangles);                    

                    if (j - 1 < 0 || data[i, j-1] == 1)                    
                        AddQuad(Matrix4x4.TRS(
                            new Vector3((j-.5f) * width, halfH, i * width),
                            Quaternion.LookRotation(Vector3.right),
                            new Vector3(width, height, 1)
                        ), ref newVertices, ref newUVs, ref wallTriangles);                    

                    if (i + 1 > rMax || data[i+1, j] == 1)                    
                        AddQuad(Matrix4x4.TRS(
                            new Vector3(j * width, halfH, (i+.5f) * width),
                            Quaternion.LookRotation(Vector3.back),
                            new Vector3(width, height, 1)
                        ), ref newVertices, ref newUVs, ref wallTriangles);                    
                }               

        maze.vertices = newVertices.ToArray();
        maze.uv = newUVs.ToArray();
        
        maze.SetTriangles(floorTriangles.ToArray(), 0);
        maze.SetTriangles(wallTriangles.ToArray(), 1);
        maze.RecalculateNormals();

        return maze;
    }

    private void AddQuad(Matrix4x4 matrix, ref List<Vector3> newVertices,
        ref List<Vector2> newUVs, ref List<int> newTriangles)
    {
        int index = newVertices.Count;

        Vector3 vert1 = new Vector3(-.5f, -.5f, 0);
        Vector3 vert2 = new Vector3(-.5f, .5f, 0);
        Vector3 vert3 = new Vector3(.5f, .5f, 0);
        Vector3 vert4 = new Vector3(.5f, -.5f, 0);

        newVertices.Add(matrix.MultiplyPoint3x4(vert1));
        newVertices.Add(matrix.MultiplyPoint3x4(vert2));
        newVertices.Add(matrix.MultiplyPoint3x4(vert3));
        newVertices.Add(matrix.MultiplyPoint3x4(vert4));

        newUVs.Add(new Vector2(1, 0));
        newUVs.Add(new Vector2(1, 1));
        newUVs.Add(new Vector2(0, 1));
        newUVs.Add(new Vector2(0, 0));

        newTriangles.Add(index+2);
        newTriangles.Add(index+1);
        newTriangles.Add(index);

        newTriangles.Add(index+3);
        newTriangles.Add(index+2);
        newTriangles.Add(index);
    }
}
```

The code looks long and complicated, but a lot of it is simply repeated. The `AddQuad` function creates a 2D plane - either the ceiling, floor or a wall face. A quad is made up of 2 trianges (imagine slicing a square diagonally). And the main body of the code simply walks through the maze data, and for each cell: places a floor and ceiling quad in the scene, and then places the appropriate walls based on whether this cell's neighbours are supposed to be blocked or open.

Save the script and open **MazeConstructor**. Add the following variable:

```csharp
private MazeMeshGenerator meshGenerator;
```

And add the following code to the top of the `Awake` method:

```csharp
meshGenerator = new MazeMeshGenerator();
```

Next, add the following method called `DisplayMaze`:

```csharp
private void DisplayMaze()
{
    GameObject go = new GameObject();
    go.transform.position = Vector3.zero;
    go.name = "Procedural Maze";
    go.tag = "Generated";

    MeshFilter mf = go.AddComponent<MeshFilter>();
    mf.mesh = meshGenerator.FromData(data);
    
    MeshCollider mc = go.AddComponent<MeshCollider>();
    mc.sharedMesh = mf.mesh;

    MeshRenderer mr = go.AddComponent<MeshRenderer>();
    mr.materials = new Material[2] {mazeMat1, mazeMat2};
}
```

And finally, add this line to the end of `GenerateNewMaze`:

```csharp
DisplayMaze();
```

By itself, a **Mesh** is simply data. It isn't visible until assigned to an object (more specifically, the object's **MeshFilter**) in the scene. Thus `DisplayMaze()` doesn't only call `MazeMeshGenerator.FromData()`, but rather inserts that call in the middle of instantiating a new GameObject, setting the **Generated** tag, adding **MeshFilter** and the generated mesh, adding **MeshCollider** for colliding with the maze, and finally adding **MeshRenderer** and materials.

Save the script and return to the editor. Run the scene, and you should have a rendered 3D maze!
