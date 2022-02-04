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

Save this script and open the **Sheep** script. Add this line to `HitByHay`:

```csharp
GameStateManager.Instance.SavedSheep();
```

This tells the manager that a sheep was saved. Next, add this code to `Drop`, right above `sheepSpawner.RemoveSheepFromList(gameObject);`:

```csharp
GameStateManager.Instance.DroppedSheep();
```

This tells the manager that a sheep was dropped.

Now, save the script and return to the editor. Create a new empty Game Object as a child of **Managers**, name it **Game State Manager** and add a **Game State Manager** component. Set **Sheep Dropped Before Game Over** to 3 and drag **Sheep Spawner** from the **hierarchy** to the slot with same name.

Now, play the scene and let a few sheep run off the edge. After three sheep have hit the invisible trigger behind the hay machine, the game will end.

### Updating the User Interface

Now that we're keeping track of score and 'lives' we need to relay that information to the player.

Drag **Game UI** from the prefabs folder into the root of the **hierarchy**. This is a pre-made Canvas with some text, a few images and a very simple game over window that is currently disabled. Unfold the Game UI to see all the child elements inside.

Create a new C# script in **Scripts/Managers** and name it **UIManager**. Add this `using` statement below the others:

```csharp
using UnityEngine.UI;
```

With this added, you can start adding references to UI components. Add the following variables above `Start`:

```csharp
public static UIManager Instance; 

public Text sheepSavedText; 
public Text sheepDroppedText; 
public GameObject gameOverWindow; 
```

In order, these are:

- a reference to this **UI Manager**.
- a cached reference to the **Text** component of **SavedSheepText**.
- a references the **Text** component of **DroppedSheepText**.
- a reference to the Game Over Window.

Next change `Start` to `Awake` and add this line to it:

```csharp
Instance = this;
```

This sets the reference to the script inside the **Instance** variable. Now delete the `Update` and add these methods instead:

```csharp
public void UpdateSheepSaved() 
{
    sheepSavedText.text = GameStateManager.Instance.sheepSaved.ToString();
}

public void UpdateSheepDropped() 
{
    sheepDroppedText.text = GameStateManager.Instance.sheepDropped.ToString();
}
```

These methods update the text at the top of the screen. Get the amount of sheep saved (or dropped) from the **Game State Manager**, convert it to a string and use it to set the text of the appropriate **Text** component.

Finally, add this method:

```csharp
public void ShowGameOverWindow()
{
    gameOverWindow.SetActive(true);
}
```

This activates the **Game Over Window**, so it becomes visible.

Save this script and open the **GameStateManager** script. Add the following line to `SavedSheep`:

```csharp
UIManager.Instance.UpdateSheepSaved();
```

This updates the text that shows the amount of sheep saved.

Next, add this line to `DroppedSheep`, right below `sheepDropped++;`:

```csharp
UIManager.Instance.UpdateSheepDropped();
```

This calls the UI Manager to update the dropped sheep text.

Finally, add this line to `GameOver`:

```csharp
UIManager.Instance.ShowGameOverWindow();
```

The game over window gets shown once the game is over with this method call. Save this script and return to the editor.

Add a new empty Game Object as a child of **Managers** and name it **UI Manager**. Add a **UI Manager** component and drag the children of **Game Canvas** to their corresponding slots.

Now, play the game for a bit and notice the UI updates as sheep get saved and fall down. If you let three sheep drop down, a simple game over window pops up. If you press the Escape key, the Title scene loads, but you won't be able to click any of the buttons just yet. We'll do that in the next section.
