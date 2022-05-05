## Implementing the pathfinding in our maze

Nice, so we have a pathfinding algorithm that traces from a `startNode` to an `endNode` and gives us the shortest path between them. But how do we translate that to the **Scary Man** and get him moving?

First, we'll add some variables for the AI to use:

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
    startRow = data.GetUpperBound(0) - 1;
    startCol = data.GetUpperBound(1) - 1;            
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
        Vector3 startPosition = monster.transform.position;            
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
