# 01: Maze generation assessment tasks - Moving the scary man

Nice, so we have a pathfinding algorithm that traces from a `startNode` to an `endNode` and gives us the shortest path between them. But how do we translate that to the **Scary Man** and get him moving?

## Implementing the pathfinding in our maze

First, we'll add some variables for the AI to use... in **AIController** add the following variables:

```csharp
private GameObject monster;
public GameObject Monster 
{
    get { return monster; }
    set { monster = value; }       
}
private GameObject player;
public GameObject Player
{
    get { return player; }
    set { player = value; } 
}
private float hallWidth;
public float HallWidth 
{
    get { return hallWidth; }
    set { hallWidth = value; }
}
[SerializeField] private float monsterSpeed;
private int startRow = -1;
private int startCol = -1;
```

- `monster`, `player` and `hallWidth` are just things we've made in other scripts, and we want to know about them here - so we are doing our `private` and `public` pattern, like we've done many times before.
- `monsterSpeed` is a new variable for how fast **Scary Man** should move - it's `private` but also marked with `[SerializeField]` so we can set it in the inspector.
- `startRow` and `startCol` will be used to figure out the `startNode` for the pathfinding... we are defaulting them to **-1** to start with because this is not a valid position in our graph, so it's a check for later code to say whether or not they have been set properly.

Let's replace `Start` with a new method called `StartAI`:

```csharp
public void StartAI()
{
    startRow = graph.GetUpperBound(0) - 1;
    startCol = graph.GetUpperBound(1) - 1;            
}
```

We'll call this from another script when we're ready to start the AI moving, so we don't need the actual `Start` method (i.e. we don't want this to start when the script loads, only when we purposefully call the method). Here, we set `startRow` and `startCol` to be the **Scary Man's start position** - remember, we are kind of seeking backwards from where we start - so our start position for the AI is the end of the maze and the goal is (currently) the start of the maze! We `- 1` from the bounds limit because of those darn outer walls again - we know the **Scary Man** must start 1 in from the sides.

Now, let's add some code to `Update` to move the **Scary Man** each frame of the game:

```csharp
void Update()
{
    if(startRow != -1 && startCol != -1)
    {            
        int playerCol = (int)Mathf.Round(player.transform.position.x / hallWidth);
        int playerRow = (int)Mathf.Round(player.transform.position.z / hallWidth);
        
        List<Node> path = FindPath(startRow, startCol, playerRow, playerCol);

        if(path != null && path.Count > 1)
        {
            Node nextNode = path[1];
            float nextX = nextNode.y * hallWidth;
            float nextZ = nextNode.x * hallWidth;
            Vector3 endPosition = new Vector3(nextX, 0f, nextZ);
            float step =  monsterSpeed * Time.deltaTime;
            monster.transform.position = Vector3.MoveTowards(monster.transform.position, endPosition, step);
            Vector3 targetDirection = endPosition - monster.transform.position;
            Vector3 newDirection = Vector3.RotateTowards(monster.transform.forward, targetDirection, step, 0.0f);
            monster.transform.rotation = Quaternion.LookRotation(newDirection);
            if(monster.transform.position == endPosition){
                startRow = nextNode.x;
                startCol = nextNode.y;
            }
        }
    }
}
```

We know that `Update` runs every frame from the start of the script, so the first `if(startRow != -1 && startCol != -1)` is just ensuring that the `startRow` and `startCol` have been set before trying to use them in some calculations.

This whole bit of code is really just translating 'real world coordinates' to cells in the data grid, and vice versa:
- We know where the player is *physically* in the game world (the current **X** and **Z** positions of the player), but we need to translate those back to what row and column that corresponds to in our graph; each of our real maze's 'cells' is `hallWidth` wide... so to get where the player is in the graph, we divide by `hallWidth` (and then round to an `int` for the index location).
- Next we `FindPath` (call our pathfinding algorithm) to seek out a path from `startRow` and `startCol` (to begin with, the **last** free cell of the maze) to the player (`playerRow` and `playerCol`).
- If the path is not `null` (i.e., there *is* a path returned by the algorithm), and the path is longer than 1 cell (i.e., the monster is not in the same cell as the player), then we will update the monster's position to move him closer to the player.
- `path[0]` would be the starting node (the node the monster is on), so we want to know where he's supposed to go next; hence, we pull out `path[1]`.
- We figure out where that is in the game world: the Node's `x` and `y` translated into world coordinates by multiplying by `hallWidth` (the reverse of what we did before)... **Note:** the game world coordinates are **X** and **Z** but our Node has properties `x` and `y` - so it's not a typo, just matching up the right axes to the right property :)
- Then we create a `Vector3` from those coordinates.
- The next 5 lines of code are simply moving and rotating the monster towards the next game world point - some of this should look familiar, using `Time.deltaTime` etc... there are some handy methods like `MoveTowards` and `RotateTowards` that take care of some of the Vector work for us.
- Finally, the last `if(monster.transform.position == endPosition)` is checking to see if the Scary Man has reached his destination point (the next cell in the path), if he has, we change `startRow` and `startCol` to the Node we just reached, and let the `Update` code run through with these new values - this way, the Scary Man is constantly reevaluating his goal (i.e., us!) - so no matter where you go, he will find you!

### Rigging all the bits up

In **GameController** add this variable:

```csharp
private AIController aIController;
```

And add this line to the bottom of `Awake`:

```csharp
aIController = GetComponent<AIController>(); 
```

Now we add some things to the `aIController`... **delete** the `CreatePlayer()` and `CreateMonster()` lines from `Start`, and instead add the following lines to the bottom of the method:

```csharp
aIController.Graph = constructor.graph;
aIController.Player = CreatePlayer();
aIController.Monster = CreateMonster(); 
aIController.HallWidth = constructor.hallWidth;         
aIController.StartAI();
```

We are using all those `public` setter methods we set up earlier to set the `Graph`, `Player`, `Monster` and `HallWidth` from info we have available to us here (you'll notice we have reused the `CreatePlayer()` and `CreateMonster()` methods here, but we'll need to make a **change** to them before they will work... we'll do that in a minute). Finally, tell the **AIController** to `StartAI()`.

Now, change the **return type** of `CreatePlayer` from `void` to `GameObject`, and add a `return player;` to the very end of the method... it should look like this when you've done it:

```csharp
private GameObject CreatePlayer()
{
    Vector3 playerStartPosition = new Vector3(constructor.hallWidth, 1, constructor.hallWidth);  
    GameObject player = Instantiate(playerPrefab, playerStartPosition, Quaternion.identity);
    player.tag = "Generated";

    return player;
}
```

By having this method now return the created player, we can pass it into the **AIController**... make the exact same changes to `CreateMonster` so the `monster` is returned by that method now.

And that should be it! Save the scripts, return to the editor and play the scene. Your very scary man should now start seeking you out in the maze, wherever you roam (don't forget to set the **Monster speed** to something other than 0... or he ain't gonna move very fast... or at all)!
