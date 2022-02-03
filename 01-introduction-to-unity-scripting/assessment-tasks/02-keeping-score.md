# 02: Introduction to Unity Scripting Assessment Tasks

## Keeping Score

The game has no failure state at the moment, no matter how many sheep drop to their demise, the player can keep going. Of course, there needs some kind of visual feedback so the player knows how he/she is doing.

Before you can add the visuals, you need to store the amount of sheep saved and dropped somewhere. Another manager is ideal for this!

Create a new C# script in **Scripts/Managers** and name it **GameStateManager**. Add this below the other `using` statements:

```csharp
using UnityEngine.SceneManagement;
```

This allows you to use scene related methods.

Now, add these variable declarations right above `Start`:

```csharp
public static GameStateManager Instance; 

[HideInInspector]
public int sheepSaved; 

[HideInInspector]
public int sheepDropped; 

public int sheepDroppedBeforeGameOver; 
public SheepSpawner sheepSpawner; 
```

- The first **Instance** varialbe saves a reference of the script itself, which can be called from any other script.
- **sheepSaved** is the amount of sheep that were saved by giving them hay. The `[HideInInspector]` attribute makes it so Unity won't show the variable in the editor, but it can still be accessed from other scripts. Doing this for public variables that are fully managed by scripts is good practice!
- **sheepDropped** is the number of sheep that fell down.
- **sheepDroppedBeforeGameOver** is the amount of sheep that can be dropped before the game is over.
- And finally **sheepSpawner** is a reference to a **Sheep Spawner** component.

Next, replace `Start` with `Awake` and add this line to the method:

```csharp
Instance = this;
```

This caches the current script so it can be accessed from other scripts.

Now add this method:

```csharp
public void SavedSheep()
{
    sheepSaved++;
}
```

This increments **sheepSaved** every time the method is invoked. Next add:

```csharp
private void GameOver()
{
    sheepSpawner.canSpawn = false; 
    sheepSpawner.DestroyAllSheep(); 
}
```

This method will get called when too many sheep drop and the game is over. It will stop the sheep spawner spawning, and destroy all sheep that are still running around.

Finally, add this method:

```csharp
public void DroppedSheep()
{
    sheepDropped++; 

    if (sheepDropped == sheepDroppedBeforeGameOver) 
    {
        GameOver();
    }
}
```

This will be called everytime a sheep drops. It increments the **sheepDropped** variable, and then compares that number to the total number of allowed dropped sheep. If the numbers are the same, it will call `GameOver();`

Add this code to `Update`:

```csharp
if (Input.GetKeyDown(KeyCode.Escape))
{
    SceneManager.LoadScene("Title");
}
```

If the **Escape** key is pressed, **SceneManager** will load the title screen.

Save this script and open the **Sheep** script.

### Updating the User Interface

