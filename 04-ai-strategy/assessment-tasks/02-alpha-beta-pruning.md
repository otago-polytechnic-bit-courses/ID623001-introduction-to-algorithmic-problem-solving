# 02: AI Strategy assessment tasks - Alpha-Beta pruning

As discussed, this algorithm gets expensive due to the sheer number of nodes it needs to evaluate in the tree - if we could add more levels, we could make the AI 'smarter'. One method of improving the efficiency of the algorithm is called **Alpha-Beta pruning**.

## Alpha-Beta pruning

The concept behind alpha beta pruning is to essentially maintain candidates for the **maximum** (alpha) and **minimum** values (beta) at each level, and stop checking **a subtree** when you realize nothing down that branch can beat your current candidate (this is called **pruning**). Check out the following example image:

![](https://static.javatpoint.com/tutorial/ai/images/alpha-beta-pruning-step7.png)

This tree has already had some branches pruned (the 'x' marks on the tree), but this later step illustrates the benefit of this approach... 

- Node A is looking to pick the **maximum** option from B and C
- Node B's current value is already 3
- Node C is looking to pick the **minimum** option from Node F and G
- Node F is already 1, which is already lower than 3 - thus, **it doesn't matter what G ends up being** - it would have to be lower than the current value of Node F to even be chosen as the value for Node C - which means the algorithm **already knows** that C can only have a **maximum** value of 1... and it **already knows** 3 is higher than 1
- So the algorithm prunes the whole **subtree** of Node G and we've saved evaluating 3 nodes at this step (which doesn't seem like much, but the algorithm also earlier pruned another node, so there are a total of 4 nodes here that it didn't need to evaluate... and this tree only has 15 nodes total - so even in this trivial example we've improved the efficiency by 27%)
