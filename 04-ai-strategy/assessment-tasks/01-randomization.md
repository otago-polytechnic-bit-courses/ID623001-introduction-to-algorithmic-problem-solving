# 01: AI Strategy assessment tasks - Randomization

The only drawback with the algorithm as we've currently coded it is that it **always** returns the same list of moves, in the same order, and walks the tree from left to right... so, we will actually replay the same game over and over... not much fun. So we are going to introduce a little **randomness**, but in a way that doesn't impact the algorithm results. Specifically, the results of these kinds of **comparisons** will be adjusted slightly: `(score > maxScore)` - there might be many moves that return an **equivalent** score (i.e., are as 'good' as each other) but the algorithm never considers them because they aren't **greater than** the current score. So, we will, simply, change the order that the moves are evaluated, so there is a chance that a different move gets set first and other equivalent scores are then ignored (but greater scores will still overwrite).

## Shuffle

Add this method into **Minimax.cs**:

```csharp
public List<T> Shuffle<T>(List<T> list)  
{  
    int n = list.Count;  
    while (n > 1) {  
        n--;  
        int k = Random.Range(0,n);  
        T value = list[k];  
        list[k] = list[n];  
        list[n] = value;  
    }  
    return list;
}
```

This shuffles a list (of anything... `T` here is a placeholder for any type) and returns the new list. And to use it, we simply want to insert it **two places** in our code - in the `CalculateMinMax` function, after each of the `List<MoveData> allMoves = GetMoves(...)` lines:

```csharp
allMoves = Shuffle(allMoves);
```

Now your game should be differet each time (this also helps avoid those annoying 'back-and-forth' moves that the AI tends to do when there are no obvious beneficial moves, and it just kind of moves a piece left and then right, over and over again while it waits for something to open up).
