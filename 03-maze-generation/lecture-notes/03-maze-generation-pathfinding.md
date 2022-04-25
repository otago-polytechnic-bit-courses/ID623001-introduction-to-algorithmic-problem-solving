# 03: Maze Generation - Pathfinding

So - we have a very scary man who is supposed to hunt you down as you wander around the maze... but at the moment, he just sort of crawls in place - not very scary! Let's do something about it!

## AIController

Ok, we are going to implement a very common pathfinding algorithm called **A* Search** - it is used to traverse **graphs** using the shortest possible path between nodes. Let's take a moment to talk about **graphs**...

**Graphs** (and **trees**) are comprised of **nodes** and **edges**. A **tree** is a recursive structure of parent/child nodes, while a **graph** is more complex and contrains **loops** in its structure. See below for examples of trees and graphs:

![](https://techdifferences.com/wp-content/uploads/2018/03/Untitled-1.jpg)
