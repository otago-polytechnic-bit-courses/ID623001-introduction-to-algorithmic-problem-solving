# 02: AI Strategy - Minimax

With the basic chess game working, we need to replace the rudimentary move system with our Minimax algorithm. We will first code the generic algorithm, and then apply some approaches that make it function a little better. 

## The algorithm

What we're going to write is a script that 'fake' plays all the possible moves for the current player, and all the possible response moves for the opponent, and then all the possible response moves for the player again. Since we're walking up and down a tree, we are going to write a **recursive function**... this function calls itself for each 'level' until it reaches the target 'depth' (3). When it reaches the end nodes at depth 3, it evaluates each game state and returns that up to the previous level (2) - depending on whether it is considering its own move or an opponent's, it will choose the **greater** or **lesser** of the end nodes as the parent value (for us, first time, it's the **greater**). It will then repeat this process (alternating between **maximum** and **minimum** values) up the tree until it reaches the start again... 

So technically it isn't even really considering its **next move**, but making decisions based on a move **3 turns away**... in **Minimax**, your **strongest move** is only as good as your opponent's **weakest move**.

(There is a really **helpful gif** on the Wikipedia page for Minimax if you want to watch the algorithm do its thing step-by-step... I've also put it here for your convenience...)

![](https://upload.wikimedia.org/wikipedia/commons/e/e1/Plminmax.gif)

### Coding Minimax

First create a new C# script called **Minimax**. Add the following variables:

```csharp
BoardManager board;
GameManager gameManager;
MoveData bestMove;
int myScore = 0;
int opponentScore = 0;
int maxDepth;

List<TileData> tilesWithPieces = new List<TileData>();
List<TileData> myPieces = new List<TileData>();
List<TileData> opponentPieces = new List<TileData>();
Stack<MoveData> moveStack = new Stack<MoveData>();
MoveHeuristic weight = new MoveHeuristic();
TileData[,] localBoard = new TileData[8, 8];

public static MiniMax instance = null;
public static MiniMax Instance
{
    get { return instance; }
}
```

- The first two are just references to the `BoardManager` and `GameManager` **singletons**.
- `bestMove` will be the returned from the **Minimax** algorithm - each node that is a 'better' move than the previously identified 'best move' will overwrite this variable as we check the different nodes.
- `myScore` and `opponentScore` are used in the **evaluation function** - this is like the +10390 and -10390 we saw in the last lesson.
- `maxDepth` is the signal to our **recursive function** when to stop - as long as we aren't at the `maxDepth` (or the end), keep calling this function for the next level down.
- The three `List<TileData>` variables are used later when the algorithm 'fake plays' the moves, to keep track of the 'board state' each time.
- `Stack<MoveData>` - the **Stack** is a new data structure, and quite important for this particular algorithm... as we walk down the tree, we add the moves (and then children moves, etc) into the **Stack** - the algorithm works **left to right**, and we saw in the example last lesson. A **Stack** is a **LIFO** data structure - of **last in first out**... so when we **pop** off the stack, we get the most recently added node (for example, the end node), then the next node we added *before* that etc... that's why the order is important, we keep adding the nodes in a particular order as we walk the tree, so we need to get them back out in the same order.
- `MoveHeuristic` is a **new class** we haven't made yet... basically, it will contain the **piece weightings**, but could, in a different/bigger game, contain much more data to factor into the **evaluation function**. We'll create it soon.
- `localBoard` is a **copy** of the real board that the algorithm uses to 'fake play' (we don't want to actually mess up the real board, so this is our temporary board).
- Finally, we have the **singleton** instance properties for this class.
