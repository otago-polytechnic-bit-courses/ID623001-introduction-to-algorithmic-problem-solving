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

Look at a very simple example:

<div style="display:grid;width:100px;grid-template-rows:repeat(3, 1fr);">
    <div>s</div>    
    <div></div>
    <div></div>
    <div></div>
    <div></div>
    <div></div>
    <div></div>
    <div></div>
    <div>e</div>
</div>

First, starting at the **s node**, we look at its **neighbours** and attribute something to each of them called the **gCost** - this is how far the node is from the **starting node** - typical gCost calculations use different values for **straight** or **diagonal** moves - we'll use **10** for a straight move and **14** for a diagonal move:

[0 ][10][  ]<br/>
[10][14][  ]<br/>
[  ][  ][e ]

Next, each node gets an **hCost**, which is the **heuristic** - this is the **estimated** distance from the **current node** to the **end node**. So, for example, the initial node is 2 across and 2 up from the end - using a bit of trigonometry: a<sup>2</sup> + b<sup>2</sup> = c<sup>2</sup>. So - 2<sup>2</sup> + 2<sup>2</sup> (we don't actually have to square root the answer - if we are consistent and never square root any of the calculations, we can leave them all in this format) - so 4 + 4 = 8. Our **hCost** for the first node is 8.

Let's do another: the second node to the right (currently with a **gCost** of 1) would have an **hCost** of: 1<sup>2</sup> + 2<sup>2</sup> = 5. So, all the **hCosts** of the nodes would be:

[8][5][4]<br/>
[5][2][1]<br/>
[4][1][0]

Finally, we calculate each node's **fCost** which is simply **fCost = gCost + hCost**. So, after all those calculations, the **fCosts** of all the nodes would be:

[8][6][6]<br/>
[6][4][4]<br/>
[6][4][4]

At each step of the pathfinding, the algorithm takes the **current node** and considers all of its **neighbours** - top and bottom, both sides, and diagonals. First, obviously, if any of the neighbours is the **goal** it moves to it and it's done; if not, it takes the path with the lowest **fCost** and then repeats. In our very simple example, we start at the starting node **s** and consider the neighbours - we have 3, with **fCosts** of **6**, **6**, and **4**... thus, we move diagonally at this step.

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

The constructor for a new `Node` sets the `x` and `y` position, the `isWalkable` to true or false, and sets the `hCost` to a default value of 0.

Finally, we have a method for calculating a node's `fCost`, which is simply its `gCost` plus its `hCost`.

Create a new C# script called **AIController**. Add the following variables above `Start`:

```csharp

```
