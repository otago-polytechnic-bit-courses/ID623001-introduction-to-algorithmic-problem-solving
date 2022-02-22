# 02: Game Mechanics - Managing resources

## Game manager

We want to implement a gold system for buying monsters and upgrades. But we will need to pass the gold information around to different objects: we will implement a shared object that will store this data, the **Game Manager**.

Right-click in the **hierarchy** and create an empty GameObject; rename it **GameManager**.

Create a new C# script named **GameManagerBehaviour** and add it as a component to **GameManager**.

Add the following line to the top of the script:

```csharp
using UnityEngine.UI;
```

This lets you access UI-specific classes like **Text**, which the project uses for the labels. Now add the following variable to the class:

```csharp
public Text goldLabel;
```

This will store a reference to the **Text** component used to display how much gold the player owns.

Add the following code to the `GameManagerBehaviour` class:

```csharp
private int gold;
public int Gold {

  get { return gold; }
  set
  {
    gold = value;
    goldLabel.GetComponent<Text>().text = "GOLD: " + gold;
  }
  
}
```

This code is similar to the **getter** and **setter** methods we defined in `CurrentLevel`. This **getter** simply returns the private `gold` variable. In the **setter**, though, not only do we set the gold value, but we also automatically update the `goldLabel` to display the new amount of gold.

Add the following line to `Start`:

```csharp
Gold = 1000;
```

Save the script and return to the editor.

In the **hierarchy tab** select **GameManager**. In the **Inspector** click the circle to the right of **Gold Label** and in the **Select Text** dialog select the **Scene** tab and choose **GoldLabel**.

Run the scene, and the label should display **Gold: 1000**.

### Using gold

We need to access the **GameManagerBehaviour** from the **PlaceMonsters** script - we've already seen how to access things by dragging them into the **Inspector**, but here we will use a different approach.

Add the following variable to the **PlaceMonster** script:

```csharp
private GameManagerBehavior gameManager;
```

You'll notice that the gameManager is `private`, which means we can't just drag it in from the editor. Instead, we will 'find' it in the code. Add the following line to the `Start` method:

```csharp
gameManager = GameObject.Find("GameManager").GetComponent<GameManagerBehavior>();
```

You get the GameObject named **GameManager** using `GameObject.Find()`, which returns the first game object it finds with the given name. Then, retrieve its `GameManagerBehavior` component and store it for later. 

Now add this line inside `OnMouseUp()`, replacing each of the comments that read `// TODO: Deduct gold` (note: there are **two** places to add this line):

```csharp
gameManager.Gold -= monster.GetComponent<MonsterData>().CurrentLevel.cost;
```

Save the file and switch back to the editor. Now when you upgrade monsters, the **Gold** readout should update.

Now we need to fix our `CanPlaceMonster()` code to actually check if the player has enough money or not. Replace this code:

```csharp
private bool CanPlaceMonster()
{
  return monster == null;
}
```

With this:

```csharp
private bool CanPlaceMonster()
{
  int cost = monsterPrefab.GetComponent<MonsterData>().levels[0].cost;
  return monster == null && gameManager.Gold >= cost;
}
```

This code first gets the cost of a **first level** monster from the `MonsterData`. Then it returns a **boolean** (true/false) that evaluates if:

- `monster` is not null; **AND**
- the amount of gold in the game manager is more than or equal to the current placement cost

If those conditions are true (i.e. the player has enough gold to place a new monster) then the method returns true; otherwise it will return false.

Tweak the `CanUpgradeMonster()` method by replacing this line:

```csharp
return true; 
```

With this:

```csharp
return gameManager.Gold >= nextLevel.cost;
```

Save the script and return to the editor. Run the scene and try placing and upgrading monsters. You should only be able to do so as long as you have enough gold!
