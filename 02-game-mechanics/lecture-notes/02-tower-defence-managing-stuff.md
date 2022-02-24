# 02: Game Mechanics - Managing stuff (resources, enemies)

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

### Managing gold

We need to access the **GameManagerBehaviour** from the **PlaceMonsters** script - we've already seen how to access things by dragging them into the **Inspector**, but here we will use a different approach.

Add the following variable to the **PlaceMonster** script:

```csharp
private GameManagerBehaviour gameManager;
```

You'll notice that the gameManager is `private`, which means we can't just drag it in from the editor. Instead, we will 'find' it in the code. Add the following line to the `Start` method:

```csharp
gameManager = GameObject.Find("GameManager").GetComponent<GameManagerBehaviour>();
```

You get the GameObject named **GameManager** using `GameObject.Find()`, which returns the first game object it finds with the given name. Then, retrieve its `GameManagerBehaviour` component and store it for later. 

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

## Managing enemies

In a tower defence game, enemies continually march towards the objective while you set up defences to try and stop them. The time has come to set up those enemies.

### Waypoints

We are going to implement the most rudimentary system for an enemy 'AI' to follow - **waypoints** that map out a course for them to follow.

Right-click in the **hierarchy** and create a new empty Game Object. Name it **Road** and reset its transform so its position is at **(0, 0, 0)**. Then create a new empty Game Object as a child of **Road** and name it **Waypoint0** - set its position to **(X:-12, Y:2, Z:0)** - this is where the enemies will start coming from.

Create five more waypoints as children of **Road** with the following names and positions:

- **Waypoint1: (X:7, Y:2, Z:0)**
- **Waypoint2: (X:7, Y:-1, Z:0)**
- **Waypoint3: (X:-7.3, Y:-1, Z:0)**
- **Waypoint4: (X:-7.3, Y:-4.5, Z:0)**
- **Waypoint5: (X:7, Y:-4.5, Z:0)**

### Spawning enemies

The Prefabs folder contains an Enemy prefab - it's set up much like the Monster prefab, with an `AudioSource` and a child `Sprite`. Set the position of the **Enemy prefab** to **(X:-20, Y:0, Z:0)**, so new instances will spawn off screen.

Create a new C# script named **MoveEnemy** and add it to the **Enemy** prefab as a component. Add the following variables to the script:

```csharp
[HideInInspector]
public GameObject[] waypoints;
private int currentWaypoint = 0;
private float lastWaypointSwitchTime;
public float speed = 1.0f;
```

`waypoints` stores a copy of the waypoints in an array, while `[HideIninspector]` above waypoints ensures you cannot accidentally change the field in the inspector, but you can still access it from other scripts.

`currentWaypoint` tracks which waypoint the enemy is currently walking away from, and `lastWaypointSwitchTime` stores the time when the enemy passed over it. Finally, you store the enemy's `speed`.

Add this line to `Start`:

```csharp
lastWaypointSwitchTime = Time.time;
```

This initializes `lastWaypointSwitchTime` to the current time.

To make the enemy move along the path, add the following code to `Update`:

```csharp
Vector3 startPosition = waypoints [currentWaypoint].transform.position;
Vector3 endPosition = waypoints [currentWaypoint + 1].transform.position;

float pathLength = Vector3.Distance (startPosition, endPosition);
float totalTimeForPath = pathLength / speed;
float currentTimeOnPath = Time.time - lastWaypointSwitchTime;
gameObject.transform.position = Vector2.Lerp (startPosition, endPosition, currentTimeOnPath / totalTimeForPath);

if (gameObject.transform.position.Equals(endPosition)) 
{
  if (currentWaypoint < waypoints.Length - 2)
  {
    currentWaypoint++;
    lastWaypointSwitchTime = Time.time;
    // TODO: Rotate into move direction
  }
  else
  {
    Destroy(gameObject);

    AudioSource audioSource = gameObject.GetComponent<AudioSource>();
    AudioSource.PlayClipAtPoint(audioSource.clip, transform.position);
    // TODO: deduct health
  }
}
```

This code does the following:

1. First, we get the **start position** and the **end position** for the current segment of the path - e.g. the first and second waypoints; then next time the second and third waypoints.
2. Then we figure out the length of that segment, how long it will take the enemy to walk it (at its speed), and how long it has been currently walking it (`Time.time - lastWaypointSwitchTime`).
3. With `Vector2.Lerp` we are saying: move the enemy along the vector (the line) created between `startPosition` and `endPosition` according to its time on the path - e.g. if it should take 2 seconds to walk it, and it's been 1 second, then the enemy should be halfway along.
4. Check whether the enemy has reached the endPosition. If yes, handle these two possible scenarios:

  - The enemy is not yet at the last waypoint, so increase `currentWaypoint` and update `lastWaypointSwitchTime`. Later, you'll add code to rotate the enemy so it points in the direction it's moving, too.
  - The enemy reached the last waypoint, so this destroys it and triggers a sound effect. Later you'll add code to decrease the player's `health`, too.

Save the script and return to the editor.

Create a new C# script called **SpawnEnemy** and add it as a component to **Road**. Add the following variable:

```csharp
public GameObject[] waypoints;
```

Save the script and return to the editor. Set the **Size** of the **waypoints** array to 6, and then drag each waypoint into the slots in turn - **Waypoint0** to **Element 0**, and so on...

Add this variable to **SpawnEnemy**:

```csahrp
public GameObject testEnemyPrefab;
```

And add the following code to `Start`:

```csharp
Instantiate(testEnemyPrefab).GetComponent<MoveEnemy>().waypoints = waypoints;
```

This instantiates a new copy of the prefab stored in `testEnemy` and assigns it waypoints to follow.

Save the script and return to the editor. Select **Road** in the **hierarchy** and set the **Test Enemy** to the **Enemy** prefab.

Run the project and the enemy should follow the road!
