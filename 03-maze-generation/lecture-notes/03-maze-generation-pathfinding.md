# 03: Maze Generation - Pathfinding

So - we have a very scary man who is supposed to hunt you down as you wander around the maze... but at the moment, he just sort of crawls in place - not very scary! Let's do something about it!

## AIController

We are going to implement a very common pathfinding algorithm called **A\* Search** - it is used to traverse **graphs** using the shortest possible path between nodes. Let's take a moment to talk about **graphs**...

**Graphs** (and **trees**) are comprised of **nodes** and **edges**. A **tree** is a recursive structure of parent/child nodes, while a **graph** is more complex and contrains **loops** in its structure. See below for examples of trees and graphs:

![](https://techdifferences.com/wp-content/uploads/2018/03/Untitled-1.jpg)

Our maze can be thought of as a graph - if you visualise the cells as nodes, and only connect those nodes that aren't restricted by a wall, you can produce as graph of all possible paths through the maze:

![](http://www.cs.umd.edu/class/spring2019/cmsc132-020X-040X/Project8/maze.png)

The simplest explanation of our **pathfinding algorithm** is that it takes a **starting node** and an **end node** and calculates the **cost** of moving towards the goal taking various paths; after calculating all the **costs** it takes the shortest (least cost) path.

![](https://miro.medium.com/max/300/1*iSt-urlSaXDABqhXX6xveQ.png)

Look at a very simple example:

[s][ ][ ]<br/>
[ ][ ][ ]<br/>
[ ][ ][e]

If we attribute **cost** to the cells (how far each cell is from our goal) we get this:

[4][3][2]<br/>
[3][2][1]<br/>
[2][1][0]

Now, starting at our starting node ("s", which is currently 4 squares away from the goal) we look at all the neighbours - we have 3 choices, with diagonals allowed:
- we can go **right** for a cost of **3**
- **down** for a cost of **3**
- **diagonally down/right** for a cost of **2**

Thus, the algorithm goes diagonally down/right. On the next move, the choices are **1** (right), **1** (down) or **0** (diagonal again) - and it takes the **0** path as this is the lowest cost.

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



Create a new C# script called **AIController**. Add the following variables above `Start`:

```csharp

```
