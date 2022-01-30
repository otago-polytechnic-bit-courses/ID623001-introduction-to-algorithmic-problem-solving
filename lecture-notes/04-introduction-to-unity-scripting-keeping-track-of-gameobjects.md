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
