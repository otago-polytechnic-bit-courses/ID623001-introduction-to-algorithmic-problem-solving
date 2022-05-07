# 03: Maze Generation - Pathfinding

So - we have a very scary man who is supposed to hunt you down as you wander around the maze... but at the moment, he just sort of crawls in place - not very scary! Let's do something about it!

## A\*

We are going to implement a very common pathfinding algorithm called **A\* Search** - it is used to traverse **graphs** using the shortest possible path between nodes. Let's take a moment to talk about **graphs**...

**Graphs** (and **trees**) are comprised of **nodes** and **edges**. A **tree** is a recursive structure of parent/child nodes, while a **graph** is more complex and contrains **loops** in its structure. See below for examples of trees and graphs:

![](https://techdifferences.com/wp-content/uploads/2018/03/Untitled-1.jpg)

Our maze can be thought of as a graph - if you visualise the cells as nodes, and only connect those nodes that aren't restricted by a wall, you can produce as graph of all possible paths through the maze:

![](http://www.cs.umd.edu/class/spring2019/cmsc132-020X-040X/Project8/maze.png)

Some algorithms (for example, **Dijkstra’s**) will calculate all paths from the start to the end node, and then simply take the path with the lowest total traversal cost - this can be really inefficient. 

**A\*** is a little different - it continually estimates the distance of a node from the end goal (using something called a **heuristic**) and prioritises nodes that have the lowest **estimated** cost of reaching the goal.

![](https://miro.medium.com/max/300/1*iSt-urlSaXDABqhXX6xveQ.png)

### fCost = gCost + hCost

So how does this algorithm really work, and what is this **heuristic**?

First, from the **start node** we look at its **neighbours** and attribute something to each of them called the **gCost** - this is how far the node is from the **starting node** - typical gCost calculations use different values for **straight** or **diagonal** moves - we'll use **10** for a straight move and **14** for a diagonal move.

Next, each node gets an **hCost**, which is the **heuristic** - this is the **estimated** distance from the **current node** to the **end node**. You can count these off as moves horizontally, vertically or diagonally.

Finally, we calculate each node's **fCost** which is simply **fCost = gCost + hCost**. 

At each step of the pathfinding, the algorithm takes the **current node** and considers all of its **neighbours** - top and bottom, both sides, and diagonals. First, obviously, if any of the neighbours is the **goal** it moves to it and it's done; if not, it takes the path with the lowest **fCost** and then repeats. If there is a choice of **equal fCost** values, it will then choose the one with the lowest **hCost** (or closest to the goal). If everything is equal between two nodes, it will evaluate both sets of neighbours.

Adon will walk through the example here: https://codepen.io/dfenders/pen/NWyKpgg

### Coding the algorithm

Now that we have a basic understanding of how the algorithm works, let's start coding it. First, we need a representation of our maze data as **nodes**. Create a new C# script called **Node**. This will be a utility class, and we won't be using any of the Unity-specific things - so replace everything in the file with the following:

```csharp
public class Node
{
    public int x;
    public int y;
    
    public int gCost;
    public int hCost;
    public int fCost;      

    public Node cameFromNode;

    public bool isWalkable;

    public Node(int x, int y, bool isWalkable)
    {
        this.x = x;
        this.y = y;
        hCost = 0;
        this.isWalkable = isWalkable;
    }
    
    public void CalculateFCost(){
        fCost = gCost + hCost;
    } 
}
```

We have variables for this node's `x` and `y` position (essentially its **row** and **col** in the grid). We then have a series of 'cost' variables - **g**, **h** and **f** costs... how these are used will become clear later. We have a reference to what node preceded this one in the graph - we need to remember this later when our algorithm finally reaches the end node, so we can backtrack the 'path' back to the start. We have a boolean `isWalkable` so we don't try to calculate paths across the walls.

The constructor for a new `Node` sets the `x` and `y` position, the `isWalkable` to true or false, and sets the `hCost` to a default value of 0 (we'll set this as we make each node - we need a method for this).

Finally, we have a method for calculating a node's `fCost`, which is simply its `gCost` plus its `hCost`.

Open the script **MazeConstructor**. First, add this variable to hold our new graph of Nodes:

```csharp
public Node[,] graph;
```

And inside `GenerateNewMaze`, **before** the `DisplayMaze()` function call, add this code:

```csharp
graph = new Node[sizeRows,sizeCols];

for (int i = 0; i < sizeRows; i++)        
    for (int j = 0; j < sizeCols; j++)            
        graph[i, j] = data[i,j] == 0 ? new Node(i, j, true) : new Node(i, j, false);
```

Here we are iterating through our maze data and creating a new `Node` for each cell - for every slot marked **0** we create a Node with `isWalkable` set to **true** and for every slot marked **1** we set `isWalkable` to **false**. All the nodes are saved into our new 2D array called `graph`.

Ok - here comes the meaty stuff. Create a new C# script called **AIController**, and add it to the **Controller** Game Object. Add the following variables above `Start`:

```csharp
private const int MOVE_STRAIGHT_COST = 10;
private const int MOVE_DIAGONAL_COST = 140;

private Node[,] graph;
public Node[,] Graph 
{
    get { return graph; }
    set { graph = value; }
}
```

These constants hold the move costs like we used above - **10** for horizontal/vertical moves, and **140** for diagonal moves (**NB:** in this particular scenario, with the way the man 'moves' around the 3D space, I have found a larger diagonal cost works much better - this gets the enemy to prioritise straighter moves around corners etc - it looks better. In larger open rooms, he should still move diagonally though, with no obstacles in the way... you can play with this value if you want different results).

We also have a referene to the `graph` created in the **MazeConstructor** - we have a pattern we've seen before, a `private` variable with a `public` version for getting and setting it from other scripts.

We are going to fill in large chunks of the code here and not focus too much on the individual lines - instead, we'll get a good overview of what these methods do.

Add the following method to the **AIController**:

```csharp
private int CalculateDistanceCost(Node a, Node b){
    int xDistance = Mathf.Abs(a.x - b.x);
    int yDistance = Mathf.Abs(a.y - b.y);
    int remaining = xDistance - yDistance;
    return MOVE_DIAGONAL_COST * Mathf.Min(xDistance, yDistance) + MOVE_STRAIGHT_COST * remaining;
}
```

This is a 'distance' function, that we will use to calculate both **gCosts** (distance of Node from start) and **hCosts** (distance of Node from goal). The code uses some math to calculate what we were doing visually - combining vertical/horizontal moves and diagonal moves.

Next, we'll add a utility method for finding the lowest **fCost** Node from a List:

```csharp
private Node GetLowestFCostNode(List<Node> pathNodeList){
    Node lowestFCostNode = pathNodeList[0];
    for(int i = 1; i < pathNodeList.Count; i++)
        if(pathNodeList[i].fCost < lowestFCostNode.fCost)
            lowestFCostNode = pathNodeList[i];
                    
    return lowestFCostNode;
}
```

A pretty standard programming pattern - take the first thing in the List and **assume** it is the lowest. Then, walk through the rest of the List, and if you find something with a **lower value**, assign *that* to be the new lowest... continue until the end of the List, and you will have found the lowest value.

The next utility method we add is for finding all the neighbours of a given Node:

```csharp
private List<Node> GetNeighbourList(Node currentNode){
    List<Node> neighbourList = new List<Node>();

    if(currentNode.x - 1 >= 0)
    {
        neighbourList.Add(graph[currentNode.x - 1,currentNode.y]);

        if(currentNode.y - 1 >= 0)
            neighbourList.Add(graph[currentNode.x - 1, currentNode.y - 1]);
        if(currentNode.y + 1 < graph.GetLength(1))
            neighbourList.Add(graph[currentNode.x - 1, currentNode.y + 1]);
    }

    if(currentNode.x + 1 < graph.GetLength(0))
    {
        neighbourList.Add(graph[currentNode.x + 1, currentNode.y]);
            
        if(currentNode.y - 1 >= 0) 
            neighbourList.Add(graph[currentNode.x + 1, currentNode.y - 1]);
        if(currentNode.y + 1 < graph.GetLength(1)) 
            neighbourList.Add(graph[currentNode.x + 1, currentNode.y + 1]);
    }

    if(currentNode.y - 1 >= 0) 
        neighbourList.Add(graph[currentNode.x, currentNode.y - 1]);
    if(currentNode.y + 1 < graph.GetLength(1)) 
        neighbourList.Add(graph[currentNode.x, currentNode.y + 1]);
        
    return neighbourList;
}
```

For the given Node, we run it through a series of **if statements** to check where on our grid it is - if it is along any of the edges, we *won't* add the neighbours that would be outside the grid. Otherwise, we add the neighbours on all **four straight sides** (above, below, left and right) and the **diagonals**.

Next add this method:

```csharp
private List<Node> CalculatePath(Node endNode)
{
    List<Node> path = new List<Node>();
    path.Add(endNode);
    Node currentNode = endNode;
    while(currentNode.cameFromNode != null)
    {
        path.Add(currentNode.cameFromNode);
        currentNode = currentNode.cameFromNode;
    }
    path.Reverse();
    return path;
}
```

Once we have evaluated our way **to the endNode**, we will walk back through the path that led there, using the `cameFromNode` property to find our way back to the start. This method makes a List of the Nodes from the **end** back to the **start**, hence why we call `path.Reverse()` right at the end.

Ok, now comes the biggie - the actual **A\* pathfinding** logic, as we talked about earlier:

```csharp
public List<Node> FindPath(int startX, int startY, int endX, int endY)
{
    Node startNode = graph[startX,startY];
    Node endNode = graph[endX, endY];

    List<Node> openList = new List<Node> { startNode };
    List<Node> closedList = new List<Node>();

    int graphWidth = graph.GetLength(0);
    int graphHeight = graph.GetLength(1);

    for(int x = 0; x < graphWidth; x++)
        for(int y = 0; y < graphHeight; y++)
        {
            Node pathNode = graph[x, y];
            pathNode.gCost = int.MaxValue;
            pathNode.CalculateFCost();
            pathNode.cameFromNode = null;
        }

    startNode.gCost = 0;
    startNode.hCost = CalculateDistanceCost(startNode, endNode);
    startNode.CalculateFCost();

    while(openList.Count > 0)
    {
        Node currentNode = GetLowestFCostNode(openList);
        if(currentNode == endNode)            
            return CalculatePath(endNode);            

        openList.Remove(currentNode);
        closedList.Add(currentNode);

        foreach(Node neighbourNode in GetNeighbourList(currentNode)){
            if(closedList.Contains(neighbourNode)) continue;

            if(!neighbourNode.isWalkable){
                closedList.Add(neighbourNode);
                continue;
            }

            int tentativeGCost = currentNode.gCost + CalculateDistanceCost(currentNode, neighbourNode);
            if(tentativeGCost < neighbourNode.gCost){
                neighbourNode.cameFromNode = currentNode;
                neighbourNode.gCost = tentativeGCost;
                neighbourNode.hCost = CalculateDistanceCost(neighbourNode, endNode);  
                neighbourNode.CalculateFCost();                  

                if(!openList.Contains(neighbourNode))
                    openList.Add(neighbourNode);
            }
        }
    }

    //out of nodes on the open list
    return null;
}
```

- First, we pull out the `startNode` and `endNode` from the graph - the Nodes we want to find a path between. We instantiate 2 Lists: the `openList` and `closedList` - this is simply *Nodes we haven't visited yet* and *ones we have*. The idea is to not revisit Nodes that have already been evaluated, as this is inefficient. As we choose Nodes based on their **fCost** and look at their **neighbours** we will constantly remove them from the **openList** and add them to the **closedList**. We start with the `startNode` in the **openList**.
- The `graphWidth` and `graphHeight` are simply holding the outer bounds of the whole graph (the max row and col, if you like).
- We first walk the graph with a **double for loop**, visiting every Node and attributing some values to its properties: at this stage, we have no way of knowing what the **gCost** of a Node might be (we'll only know that as we start to measure the distance between Nodes), so we set it to the **highest possible value** - as we start evaluating neighbours, we will attribute them **actual gCosts** as we see how far from the start they are on a given path... but sometimes we will *reevaluate* this cost (as we saw in the example), and come up with a lower value - we'll see this in practice later; we calculate the current **fCost** (which is inaccurate, but will be refined as we go); and we set the `cameFromNode` to `null` (we haven't started any paths so we don't yet know *what* Node has led to any other Node).
- Once we've assigned some start values to all the Nodes, we start on the actual path. We start with the `startNode` and assign a **gCost** of **0** (obviously, this is the closest to the start), calculate its distance from the `endNode` (**hCost**), and calculate its **fCost**. We now have 1 actually evaluated Node in the **openList**... so, let's start evaluating **neighbours**.
- `while(openList.Count > 0)` says: "Keep doing this code as long as there are open Nodes to evaluate".
- First, the Node with the **lowest fCost** is looked at first (very first time around, there is only one candidate - the `startNode` - but as we fill the List, we will have decisions to make, like we looked at with the example earlier). If the currently examined Node *is* the `endNode` then we're done (and we can track back the path we took).
- If we aren't at the `endNode` yet, we do a few things to this node: first, we remove it from the **openList** and add it to the **closedList**. Then we check out its **neighbours** using the `GetNeighbourList` method we added earlier.
- For each **neighbour**, we check first if the **closedList** contains that Node. If it does, we've already checked it out, so we don't need to again; the `continue` keyword will **break us out of the current iteration of the loop** but not **stop the loop**.
- We also check if the **neighbour in question** is **walkable** or not (i.e., is it a wall?) If it is **not walkable** then add it to the **closedList** and, again, break out of this iteration of the loop.
- So, for any neighbours that we haven't checked out already and are walkable, we will move onto the next bit of code... we add together the current **gCost** and the cost of moving to this neighbour (this is the neighbour's **new gCost**... for now... it's called `tentativeGCost` because, if you remember the example from earlier, sometimes we *reevaluate* a Node's **gCost** if we've come at it from a different path... if we ever find that a **neighbour** can have a **lower gCost** because we've come at it from a new path, we will recalculate its **fCost** and, if need be, readd it to the **openList**.
- And that's it... continually look at neighbour Nodes, evaluate if they are good candidates or not, and add them to the **openList** - as you check out new Nodes, remove them from the **openList** and you will eventually reach the endNode!
