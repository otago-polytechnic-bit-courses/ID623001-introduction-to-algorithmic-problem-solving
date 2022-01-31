# 04: Introduction to Unity Scripting - Keeping Track of GameObjects

## Lists

Start by creating a new empty Game Object at the root of the **hierarchy** and name it **Sheep Spawn Points** and reset its Transform. Create three empty Game Objects as children **Sheep Spawn Points** and name each one **Spawn Point**.

Set the position of each spawn point to (respectively):

- **(X:-16, Y:0, Z:60)**
- **(X:0, Y:0, Z:60)**
- **(X:16, Y:0, Z:60)**

This spaces them evenly just at the edge of the far end of the field. Now let's write the script that spawns the sheep.

Create a new C# script and name it **SheepSpawner**. Add these variables above `Start`:

```csharp
public bool canSpawn = true; 

public GameObject sheepPrefab; 
public List<Transform> sheepSpawnPositions = new List<Transform>(); 
public float timeBetweenSpawns; 

private List<GameObject> sheepList = new List<GameObject>(); 
```

- The **canSpawn** variable will control whether the script can spawn sheep or not. 
- The **sheepPrefab** variable is, obviously, a reference to the sheep prefab. 
- The next variable is a List that will hold the spawn points we just created before. 
- **timeBetweenSpawns** is the delay before a new sheep will spawn.
- The last List will hold all the 'alive' sheep currently in the scene.

Now add this method:

```csharp
private void SpawnSheep()
{
    Vector3 randomPosition = sheepSpawnPositions[Random.Range(0, sheepSpawnPositions.Count)].position; 
    GameObject sheep = Instantiate(sheepPrefab, randomPosition, sheepPrefab.transform.rotation); 
    sheepList.Add(sheep); 
    sheep.GetComponent<Sheep>().SetSpawner(this); 
}
```

This method spawns a single sheep randomly at one of the spawn points.

Let's break down the first line: this grabs one of the spawn points at random. We can rewrite that line out in longer form to see what's happening:

```csharp
int maxSpawnPoints = sheepSpawnPositions.Count;
int randomNum = Random.Range(0, maxSpawnPoints);
Transform randomSpawnPoint = sheepSpawnPositions[randomNum];
Vector3 randomPosition = randomSpawnPoint.position; 
```

`Random.Range` returns a random int between the provided min and max numbers - min **inclusive** and max **exclusive**. So, here we are returning either 0, 1 or 2. We get the Transform at that index from the List, and then get the position info from that Transform.

The next line instantiates a new sheep: it uses the sheepPrefab reference as its template, the randomPosition we just grabbed as its start position, and its own default rotation as its starting rotation.

Then, add this newly created sheep to the **sheepList**.

The final line won't compile because we are trying to set a reference to **this** spawner script *onto* the Sheep... but we haven't created the `SetSpawner();` method in the **Sheep** script yet. We'll do that now.

Save the **SheepSpawner** script and open the **Sheep** script. Add this variable below the others:

```csharp
private SheepSpawner sheepSpawner;
```

Now add this method to fill in this reference:

```csharp
public void SetSpawner(SheepSpawner spawner)
{
    sheepSpawner = spawner;
}
```

Save the **Sheep** script and switch back to the **SheepSpawner** script. We are going to add a **coroutine** to this script to handle the spawning. **Coroutines**, in Unity, are a way of implementing asynchronous behaviour (note: it isn't *true* asynchronous behaviour, but close enough for us to think of it that way now).

Add this code below the `SpawnSheep` method:

```csharp
private IEnumerator SpawnRoutine() 
{
    while (canSpawn) 
    {
        SpawnSheep(); 
        yield return new WaitForSeconds(timeBetweenSpawns); 
    }
}
```

The first line lets this code run as long as **canSpawn** is true. Next we call the `SpawnSheep();` method to spawn a single sheep at a random point. Then we wait (or yield the execution) for **timeBetweenSpawns**. This line will pause the execution for however many seconds we want before running again - otherwise, our only option for repeatedly running this code would be to put it in the `Update` method and run it **every frame**, which is too fast.

To start the coroutine, we add this code inside `Start`:

```csharp
StartCoroutine(SpawnRoutine());
```

We don't call `SpawnRoutine()` directly, but use this `StartCoroutine()` instead to launch the coroutine.

Next, we want to be able to remove an individual sheep from the list of active sheep when it gets hit. Add this method below `SpawnRoutine`:

```csharp
public void RemoveSheepFromList(GameObject sheep)
{
    sheepList.Remove(sheep);
}
```

This method takes a sheep as its parameter and removes the entry from the sheep list.

Now add this method:

```csharp
public void DestroyAllSheep()
{
    foreach (GameObject sheep in sheepList)
    {
        Destroy(sheep);
    }

    sheepList.Clear();
}
```

This method iterates through the sheepList, destroys each sheep instance, and clears the list of references.

Save the script, and reopen the **Sheep** script. Add this line to the top of the `Drop` and `HitByHay` methods:

```csharp
sheepSpawner.RemoveSheepFromList(gameObject);
```

This removes the sheep from the spawner's list when it either drops off the edge of the world, or gets hit by hay. Now save the script and return to the editor.

Add a new empty Game Object to the **hierarchy**, name it **SheepSpawner** and add a **Sheep Spawner** component.

To configure the spawner, start by dragging a **Sheep** from the prefabs folder onto the Sheep Prefab slot on the **Sheep Spawner**. Next, expand **Sheep Spawn Points** and drag the spawn points one-by-one to the **Sheep Spawn Positions** List. Finally, set the **Time Between Spawns** to 2 and play the scene - you should now have a fully functional game! Stop the sheep from reaching the edge of the world and falling to their doom!
