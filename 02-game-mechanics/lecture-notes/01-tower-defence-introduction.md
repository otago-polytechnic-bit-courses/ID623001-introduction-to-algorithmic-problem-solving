# 01: Game Mechanics - Tower defence introduction

## Starter project

Download the **starter code** and open it as a project in Unity.

## Tower defence

We are going to build a 2D tower defence game, and implement functionality such as **gaining and using resources**, **upgrading units**, and rudimentary **enemy wayfinding**.

Open the **GameScene** from the scenes folder and set the Game view aspect ratio to **4:3** - this will ensure the labels and background line up correctly. The object is to place monsters in defensive positions around the path to stop the bugs from reaching the cookie at the end. You will earn gold as you play and use it to ugrade the monsters, defeating multiple waves of enemy bugs!

### Unit placement

The first step is to set up the functionality to place your defensive monsters. Monsters can only be places on the spots marked with an **X**. Drag and drop **Images\Objects\Openspot** into the Scene view. Select **Openspot** in the **hierarchy tab** and add a **Box Collider 2D** component to it. Next add an **Audio\Audio Source** component to **Openspot** and set the Audio Source's **AudioClip** to **Audio\tower_place**. Deactivate **Play On Awake**.

Create a prefab from what you just created: drag and drop **Openspot** from the **hierarchy** to the **prefabs** folder. You can now create more placement spots using the prefab. Drag 11 more spots into the scene (don't worry about their position now, we'll adjust those in a minute). You should have a total of 12 placement spots in the **hierarchy**.

Now we will set the positions of each of the spots... the 12 position values are as follows:

- **(X:-5.2, Y:3.5, Z:0)**
- **(X:-2.2, Y:3.5, Z:0)**
- **(X:0.8, Y:3.5, Z:0)**
- **(X:3.8, Y:3.5, Z:0)**
- **(X:-3.8, Y:0.4, Z:0)**
- **(X:-0.8, Y:0.4, Z:0)**
- **(X:2.2, Y:0.4, Z:0)**
- **(X:5.2, Y:0.4, Z:0)**
- **(X:-5.2, Y:-3.0, Z:0)**
- **(X:-2.2, Y:-3.0, Z:0)**
- **(X:0.8, Y:-3.0, Z:0)**
- **(X:3.8, Y:-3.0, Z:0)**

You should now see 12 **Xs** spaced around the path on the map, 3 rows of 4.

### Placing monsters

Create a new C# script in the scripts folder and name it **PlaceMonster**. Open the script in Visual Studio. Add these two variables above `Start`:

```csharp
public GameObject monsterPrefab;
private GameObject monster;
```

The first is a reference to the monster prefab, and the second is a variable that holds a monster on this spot, when you have created one. 

And add the following method:

```csharp
private bool CanPlaceMonster()
{
  return monster == null;
}
```

This method checks if the `monster` variable is **null**. **Null** means no monster has been created here yet, so it is ok to create one. If `monster` is NOT null, then it means we have already created one, so this method will help us prevent duplicate placement on one spot.

Add the following method to create a monster on this spot:

```csharp
void OnMouseUp()
{
  if (CanPlaceMonster())
  {
    monster = (GameObject)Instantiate(monsterPrefab, transform.position, Quaternion.identity);

    AudioSource audioSource = gameObject.GetComponent<AudioSource>();
    audioSource.PlayOneShot(audioSource.clip);

    // TODO: Deduct gold
  }
}
```

This is a built-in Unity method called `OnMouseUp` which listens for a **mouseUp** event on the **Openspot** - basically that means the player clicked the spot. The method then calls `CanPlaceMonster()` to check if a monster has already been placed here or not. If the spot is clear, then a new monster is instantiated from the prefab. Let's take a look at this line more closely:

```csharp
monster = (GameObject)Instantiate(monsterPrefab, transform.position, Quaternion.identity);
```

First we have the `Instantiate` method that we've used before... this takes the `monsterPrefab` as its reference, the `transform.position` of the **Openspot** as its position (that is, place the monster at the same location), and no rotation (the default `Quarternion.identity`). 

The `(GameObject)` at the start is called a **cast**... If you remember your inheritance lessons from Programming 2, you'll know instances of classes can be cast as different types of that class based on inheritance rules: in this case, `Instantiate` creates an instance of the base class `Object`, but we want to access methods of the descendant class `GameObject`, so we cast it and store it *as* a `GameObject`. It's like how *dogs* are *animals*, and so you can think of *Snoopy* as either a *dog* OR an *animal* - it doesn't change what *Snoopy* is, just how you think of him.

Finally, we are assigning the instantiated object into the `monster` variable... now, if we try to click here again, `CanPlaceMonster()` would return NOT null, and thus this placement code would not run a second time.

The last two lines of the method are just getting the **AudioSource** of the **Openspot** and playing the placement sound effect. At this point you can remove the `Start` and `Update` methods - we won't be using them.

Save the script and switch back to the editor. Select **Openspot** in the prefabs folder, add the **PlaceMonster** script as a component, and then click the circle to the right of the **PlaceMonster** script's **Monster Prefab** field - select **Monster** from the dialog box that appears.

Run the scene and you should be able to place monsters on the **X** spots.

### Upgrading monsters

A script acts as the basis for implementing a leveling system for the monsters. It tracks how powerful the monster should be on each level, and of course, the current level of a monster.

Create a new C# script called **MonsterData** and add it as a component to the **Monster** prefab in the prefabs folder.

Add the following code *above* the `MonsterData` class:

```csharp
[System.Serializable]
public class MonsterLevel
{
  public int cost;
  public GameObject visualization;
}
```

This creates the `MonsterLevel` class, that will be used to group the cost (in gold, which you’ll support later) and the visual representation for a specific monster level.

You add `[System.Serializable]` at the top to make instances of the class editable from the inspector. This allows you to quickly change all values in the Level class — even while the game is running. It’s incredibly useful for balancing your game.

At the very top of the script make sure this `using` statement is present:

```csharp
using System.Collections.Generic;
```

Then add the following variable to the `MonsterData` class above the `Start` method:

```csharp
public List<MonsterLevel> levels;
```

Save the script and return to the editor.

Select the **Monster** prefab and in the **inspector** set its **Size** to 3. Set the **Cost** for each level to the following values:

- **Element 0: 200**
- **Element 1: 110**
- **Element 2: 120**

Double-click on **Prefabs\Monster** in the prefabs folder and drag and drop the children to the corresponding **visualization** fields of **Monster Data**: **Monster0** to **Element 0**, **Monster1** to **Element 1**, and **Monster2** to **Element 2**.

Switch back to the **MonsterData** script and add this variable to `MonsterData`:

```csharp
private MonsterLevel currentLevel;
```

Now add the following to `MonsterData`:

```csharp
public MonsterLevel CurrentLevel
{

  get { return currentLevel; }

  set
  {
    currentLevel = value;
    int currentLevelIndex = levels.IndexOf(currentLevel);

    GameObject levelVisualization = levels[currentLevelIndex].visualization;
    
    for (int i = 0; i < levels.Count; i++)
    {
      if (levelVisualization != null) 
        if (i == currentLevelIndex)         
          levels[i].visualization.SetActive(true);        
        else        
          levels[i].visualization.SetActive(false);           
    }
  }
  
}
```

So, what's going on here? Well, we had a `private` variable called `currentLevel`, and have now made a `public` variable called `CurrentLevel` (notice the capitalisation of the first letter?) By doing this, we can create a **getter** and **setter** method for reading and writing the private `currentLevel` - why? A couple reasons:

- first, we could control the read/write access of a variable - add only a **getter** and the variable is *read-only*; add only a **setter** and the variable is *write-only*.
- we can also add custom behaviour into the **getter** and **setter** methods - instead of just returning the variable, or setting it, we can say "everytime we get the variable do this stuff first", or "everytime we set the variable, run this code".

Here, the **getter** just returns the variable - when we access `CurrentLevel` we will get whatever is in `currentLevel`. But in the **setter** method we say, whenever someone *sets* `currentLevel`, we will automatically get the correct visualization and activate it as well.

Next, add the following code to the `MonsterData` class:

```csharp
void OnEnable()
{
  CurrentLevel = levels[0];
}
```

This starts the monsters at level 1 when they are first placed. At this point you can remove the `Start` and `Update` methods - we won't be using them.

Add the following method to `MonsterData`:

```csharp
public MonsterLevel GetNextLevel()
{
  int currentLevelIndex = levels.IndexOf(currentLevel);
  int maxLevelIndex = levels.Count - 1;
  if (currentLevelIndex < maxLevelIndex)
  {
    return levels[currentLevelIndex+1];
  } 
  else
  {
    return null;
  }
}
```

This gets the current level and compares it to the last index possible in the list (notice the `levels.Count - 1` - this is because Lists are zero-based, so the last index is one less than the length of the List). If it is possible to upgrade the monster (i.e. it is not at its highest level yet) then return the level data, otherwise return `null`.

Add the following method to increase a monster's level:

```csharp
public void IncreaseLevel()
{
  int currentLevelIndex = levels.IndexOf(currentLevel);
  if (currentLevelIndex < levels.Count - 1)
  {
    CurrentLevel = levels[currentLevelIndex + 1];
  }
}
```

This *sets* the `currentLevel` to whatever it currently is **plus 1** (i.e. the next level).

Save the script and open the **PlaceMonster** script. Add this method below `CanPlaceMonster`:

```csharp
private bool CanUpgradeMonster()
{
  if (monster != null)
  {
    MonsterData monsterData = monster.GetComponent<MonsterData>();
    MonsterLevel nextLevel = monsterData.GetNextLevel();
    if (nextLevel != null)
    {
      return true;
    }
  }
  return false;
}
```

This checks that:

- first there is a monster to upgrade (there should be, but it's good to check),
- then we get the **next level** from the `MonsterData`,
- and finally, if we *have* a next level (i.e. can upgrade) return `true`.

To actually upgrade the monsters, add this `else if` condition to the statement in `OnMouseUp`:

```csharp
if (CanPlaceMonster())
{
  // Your code here stays the same as before
}
else if (CanUpgradeMonster())
{
  monster.GetComponent<MonsterData>().IncreaseLevel();
  AudioSource audioSource = gameObject.GetComponent<AudioSource>();
  audioSource.PlayOneShot(audioSource.clip);
  // TODO: Deduct gold
}
```

So now your code checks if you can place a monster (the spot is empty), or (if the spot is not empty) are you able to upgrade a monster (it is not at its highest level). The next step will be to add a condition to check if the player has enough gold to place or upgrade a monster.
