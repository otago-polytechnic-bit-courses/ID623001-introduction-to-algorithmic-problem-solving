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
    private MazeConstructor generator;

    void Start()
    {
        generator = GetComponent<MazeConstructor>();
    }
}
```

The `RequireComponent` attribute ensures that a **MazeConstructor** component will also be added when you add this script to a GameObject - we need a **MazeConstructor** always in this game.

The `generator` variable gets a reference to that component (which will be added automatically in a moment).

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
