# 02: AI Strategy assessment tasks - Alpha-Beta pruning

As discussed, this algorithm gets expensive due to the sheer number of nodes it needs to evaluate in the tree - if we could add more levels, we could make the AI 'smarter'. One method of improving the efficiency of the algorithm is called **Alpha-Beta pruning**.

## Alpha-Beta pruning

The concept behind alpha beta pruning is to essentially maintain candidates for the **maximum** (alpha) and **minimum** values (beta) at each level, and stop checking **a subtree** when you realize nothing down that branch can beat your current candidate (this is called **pruning**). Check out the following example image:

![](https://static.javatpoint.com/tutorial/ai/images/alpha-beta-pruning-step8.png)
