# 02: AI Strategy assessment tasks - Alpha-Beta pruning

As discussed, this algorithm gets expensive due to the sheer number of nodes it needs to evaluate in the tree - if we could add more levels, we could make the AI 'smarter'. One method of improving the efficiency of the algorithm is called **Alpha-Beta pruning**.

## Alpha-Beta pruning

The concept behind alpha beta pruning is to essentially maintain candidates for the **maximum** (alpha) and **minimum** values (beta) at each level, and stop checking **a subtree** when you realize nothing down that branch can beat your current candidate (this is called **pruning**). Check out the following example image:

![](https://static.javatpoint.com/tutorial/ai/images/alpha-beta-pruning-step7.png)

This tree has already had a branch pruned (the 'x' mark coming down from Node E), but this is a later step that illustrates the benefit of this approach... 

- Node A is looking to pick the **maximum** option from B and C
- Node B's current value is already 3
- Node C is looking to pick the **minimum** option from Node F and G
- Node F is already 1, which is already lower than 3 - thus, **it doesn't matter what G ends up being** - it would have to be lower than the current value of Node F to even be chosen as the value for Node C - which means the algorithm **already knows** that C can only have a **maximum** value of 1... and it **already knows** 3 is higher than 1
- So the algorithm prunes the whole **subtree** of Node G and we've saved evaluating 3 nodes at this step (which doesn't seem like much, but the algorithm also earlier pruned another node, so there are a total of 4 nodes here that it didn't need to evaluate... and this tree only has 15 nodes total - so even in this trivial example we've improved the efficiency by 27%)

Obviously this approach works best when the number of branches that can be pruned is higher. And that depends heavily on the order that the nodes are evaluated in - imagine Node D and E in the image were swapped and the algorithm evaluated E first... then the candidate would be 9, and it would have to check both options of D for possible lower values. In a **real chess engine** the moves would be ordered in certain ways to make the tree more 'prunable' and thus more efficient (e.g. certain pieces at certain positions will be known to be better moves for attacking or defending, etc... there are usually lookup tables of moves that can help prioritise a certain order to the tree - this is beyond the scope of this class, but it's interesting to know about).

### Coding Alph-Beta pruning into our game

All we need to do is keep track of the **alpha** and **beta** candidates as we traverse the tree. The rules are

- on a **maximizing** level, if a **score** is greater than the current **alpha** it becomes the new **alpha**; and if a **score** is greater than the current **beta** you can stop checking children of this node
- on a **minimizing** level, if a **score** is less than the current **beta** it becomes the new **beta**; and if a **score** is less than or equal to the current **alpha** we can stop checking children of this node

The first change we will make is to the signature of `CalculateMinMax`:

```csharp
int CalculateMinMax(int depth, int alpha, int beta, bool max)
```

So, it's mostly the same (`depth` and `max`), but we've added the `alpha` and `beta` variables as well to keep passing up and down the tree.

Everywhere you call `CalculateMinMax` you need to add `alpha` and `beta` to the signature as well.

Next, you can get rid of these two lines, as we won't be using these variables anymore: 
- `int maxScore = int.MinValue;`
- `int minScore = int.MaxValue;`

And since we aren't using those variables anymore, these sections of code can go as well:

```csharp
if(score > maxScore)                
    maxScore = score;
```

And:

```csharp
if(score < minScore)                
    minScore = score;
```

And we can change the returns of each side of the condition; instead of `return maxScore` we will `return alpha`, and instead of `return minScore` we will `return beta`.
