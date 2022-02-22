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
  get
  { 
    return gold;
  }
  set
  {
    gold = value;
    goldLabel.GetComponent<Text>().text = "GOLD: " + gold;
  }
}
```

