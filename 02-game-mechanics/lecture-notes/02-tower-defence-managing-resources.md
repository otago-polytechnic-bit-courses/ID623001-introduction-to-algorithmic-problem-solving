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
