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

List<TileData> myPieces = new List<TileData>();
List<TileData> opponentPieces = new List<TileData>();
Stack<MoveData> moveStack = new Stack<MoveData>();
MoveHeuristic weight = new MoveHeuristic();

public static Minimax instance;
public static Minimax Instance
{
    get { return instance; }
}

private void Awake()
{
    if (instance == null)        
        instance = this;        
    else if (instance != this)        
        Destroy(this);    
}
```

- The first two are just references to the `BoardManager` and `GameManager` **singletons**.
- `bestMove` will be the returned from the **Minimax** algorithm - each node that is a 'better' move than the previously identified 'best move' will overwrite this variable as we check the different nodes.
- `myScore` and `opponentScore` are used in the **evaluation function** - this is like the +10390 and -10390 we saw in the last lesson.
- `maxDepth` is the signal to our **recursive function** when to stop - as long as we aren't at the `maxDepth` (or the end), keep calling this function for the next level down.
- The two `List<TileData>` variables are used later when the algorithm 'fake plays' the moves, to keep track of the 'board state' each time.
- `Stack<MoveData>` - the **Stack** is a new data structure, and quite important for this particular algorithm... as we walk down the tree, we add the moves (and then children moves, etc) into the **Stack** - the algorithm works **left to right**, and we saw in the example last lesson. A **Stack** is a **LIFO** data structure - of **last in first out**... so when we **pop** off the stack, we get the most recently added node (for example, the end node), then the next node we added *before* that etc... that's why the order is important, we keep adding the nodes in a particular order as we walk the tree, so we need to get them back out in the same order.
- `MoveHeuristic` is a **new class** we haven't made yet... basically, it will contain the **piece weightings**, but could, in a different/bigger game, contain much more data to factor into the **evaluation function**. We'll create it soon.
- Finally, we have the familiar **singleton** instance properties for this class.

We need a few **utility methods** before coding the actual algorithm... first:

```csharp
MoveData CreateMove(TileData from, TileData to)
{
    MoveData tempMove = new MoveData
    {
        firstPosition = from,
        pieceMoved = from.CurrentPiece,
        secondPosition = to
    };

    if (to.CurrentPiece != null)        
        tempMove.pieceKilled = to.CurrentPiece;        

    return tempMove;
}
```

This method creates a `MoveData` based on a hypothetical move - where the move is coming `from`, where it's going `to`, the `piece` moving, and whether any other piece is **killed** after the move is made. (It's very similar to the `CheckAndStoreMove` function from **MoveFunction.cs**.)

Next, we'll add a method to get **all available moves** for a player:

```csharp
List<MoveData> GetMoves(PlayerTeam team)
{
    List<MoveData> turnMove = new List<MoveData>();
    List<TileData> pieces = (team == gameManager.playerTurn) ? myPieces : opponentPieces;      

    foreach (TileData tile in pieces)
    {
        MoveFunction movement = new MoveFunction(board);
        List<MoveData> pieceMoves = movement.GetMoves(tile.CurrentPiece, tile.Position);

        foreach (MoveData move in pieceMoves)
        {
            MoveData newMove = CreateMove(move.firstPosition, move.secondPosition);
            turnMove.Add(newMove);
        }
    }
    return turnMove;
}
```
This method simply iterates through all the available pieces for this player, and gets a `List` of `MoveData` for all legal moves.

This next method is used to perform the 'fake moves' by the algorithm:

```csharp
void DoFakeMove(TileData currentTile, TileData targetTile)
{
    targetTile.SwapFakePieces(currentTile.CurrentPiece);
    currentTile.CurrentPiece = null;
}
```

And, we need something to **undo** the moves again, for moving back up the tree and evaluating previous states:

```csharp
void UndoFakeMove()
{
    MoveData tempMove = moveStack.Pop();
    TileData movedTo = tempMove.secondPosition;
    TileData movedFrom = tempMove.firstPosition;
    ChessPiece pieceKilled = tempMove.pieceKilled;
    ChessPiece pieceMoved = tempMove.pieceMoved;

    movedFrom.CurrentPiece = movedTo.CurrentPiece;
    movedTo.CurrentPiece = (pieceKilled != null) ? pieceKilled : null;      
}
```

In this method, we **pop** the last move off the stack and basically revert the move (position, pieces killed, etc).

Next, we'll add our very simple **evaluation function**, which simply subtracts the opponent's score from our score (like we saw in the examples from last lesson).

```csharp
int Evaluate()
{
    int pieceDifference = myScore - opponentScore;            
    return pieceDifference;
}
```

Next add this method:

```csharp
void GetBoardState()
{
    myPieces.Clear();
    opponentPieces.Clear();
    myScore = 0;
    opponentScore = 0;

    for (int y = 0; y < 8; y++)        
        for (int x = 0; x < 8; x++)
        {
            TileData tile = board.GetTileFromBoard(new Vector2(x, y));
            if(tile.CurrentPiece != null && tile.CurrentPiece.Type != ChessPiece.PieceType.NONE)
            {
                if (tile.CurrentPiece.Team == gameManager.playerTurn)
                {
                    myScore += weight.GetPieceWeight(tile.CurrentPiece.Type);
                    myPieces.Add(tile);
                }
                else
                {
                    opponentScore += weight.GetPieceWeight(tile.CurrentPiece.Type);
                    opponentPieces.Add(tile);
                }
            }
        }     
}
```
This method goes through the pieces on the board and assigns the appropriate pieces to either **myPieces** or **opponentPieces**, and calculates each player's **score**.

Next, we'll add the public method that will be called from `GameManager` to run the algorithm:

```csharp
public MoveData GetMove()
{
    board = BoardManager.Instance;
    gameManager = GameManager.Instance;
    bestMove = CreateMove(board.GetTileFromBoard(new Vector2(0, 0)), board.GetTileFromBoard(new Vector2(0, 0)));

    maxDepth = 3;
    CalculateMinMax(maxDepth, true);

    return bestMove;
} 
```

This method sets the instances for the `board` and `gameManager` and then sets up a `bestMove` - to begin with, it's just a default 'move' for tile **0,0**. This will get overwritten as the algorithm does its thing.

We also set `maxDepth` to 3 - again, you can set this to whatever you want but the higher you set it, the slower your game will run.

Finally, we come to the **recursive function** - `CalculateMinMax`... it takes the `maxDepth` (so it knows when to stop), and a `bool` that will be used to switch between **maximising** and **minimising** the move scores.

```csharp
int CalculateMinMax(int depth, bool max)
{
    GetBoardState();

    if (depth == 0)        
        return Evaluate();

    if (max)
    {
        int maxScore = int.MinValue;
        List<MoveData> allMoves = GetMoves(gameManager.playerTurn);
        foreach (MoveData move in allMoves)
        {
            moveStack.Push(move);

            DoFakeMove(move.firstPosition, move.secondPosition);
            int score = CalculateMinMax(depth - 1, false);
            UndoFakeMove();            

            if(score > maxScore)                
                maxScore = score;                         

            if(score > bestMove.score && depth == maxDepth)
            {
                move.score = score;
                bestMove = move;   
            }
        }
        return maxScore;
    }
    else
    {
        PlayerTeam opponent = gameManager.playerTurn == PlayerTeam.WHITE ? PlayerTeam.BLACK : PlayerTeam.WHITE;
        int minScore = int.MaxValue;
        List<MoveData> allMoves = GetMoves(opponent);
        foreach (MoveData move in allMoves)
        {
            moveStack.Push(move);

            DoFakeMove(move.firstPosition, move.secondPosition);
            int score = CalculateMinMax(depth - 1, true);
            UndoFakeMove();

            if(score < minScore)                
                minScore = score;                            
        }
        return minScore;
    }
}
```

First, it calls `GetBoardState()` for whatever node we're on currently. If we're at `depth == 0` that means we're at an end node (we pass in the `maxDepth` and then we will **-1** each time we move down a level... so eventually, we will reach 0, and that's where we'll stop. When we reach `depth == 0` we return the `Evaluate()` function - add up the weight of our pieces, the opponent's pieces, and calculate this move's score.

The condition `if(max)` is for determing if we are **maximising** or **minimising** this current level. Each time we call this function again, we will pass in the **opposite** boolean.

Both parts of the **if statement** are kind of the same (just reversed), so we'll just kind of cover them once. First we set the **opposite** score of what we're trying to do - e.g., if we're **maximising** we set `int maxScore = int.MinValue;` - the lowest value possible as the default - then as we go, we compare higher scores against this and keep overwriting it with higher scores.

We get all possible moves for the player in question: `List<MoveData> allMoves = GetMoves(gameManager.playerTurn);`

Then we iterate over all the moves and:
- push the move onto the `Stack` (remember, this is so we can walk backwards (**pop**) through the nodes)
- `DoFakeMove(move.firstPosition, move.secondPosition);` to "do" this move
- `int score = CalculateMinMax(depth - 1, false);` call this function again (**recursion**) for the next level down (`depth - 1`), and reverse the `max` boolean.
- `UndoFakeMove();` to get the pieces back when they were before
- then we will set the score of this move to whatever was returned from `CalculateMinMax` - remember, at the end nodes, this will be `Evaluate()`, and at each of the nodes above, it will be one of the returns here - either `maxScore` or `minScore`
- Then do some comparisons - e.g. if `score` is larger than `maxScore`, this is the **new max score** for this level.
- On the **max** turns, there is also this line: `if(score > bestMove.score && depth == maxDepth)` - basically, this is for the **top node** - once we're back up the tree, check each of the scores again the `bestMove.score` - overwrite as necessary.

The **min** side of the condition is the same, but the comparisons are reversed. 

Finally, we need to add some code to **GameManager.cs** to make this all work. First add this variable:

```csharp
Minimax minimax;
```

Then in `Start` add this:

```csharp
minimax = Minimax.Instance;
```

And finally replace all this in `DoAIMove`:

```csharp
MoveFunction movement = new MoveFunction(board);
MoveData move = null;
for (int y = 0; y < 8; y++)                
    for (int x = 0; x < 8; x++)            
    {
        TileData tile = board.GetTileFromBoard(new Vector2(x, y));
        if(tile.CurrentPiece != null && tile.CurrentPiece.Team == playerTurn)
        {
            List<MoveData> pieceMoves = movement.GetMoves(tile.CurrentPiece, tile.Position);
            if(pieceMoves.Count > 0)                        
                move = pieceMoves[0];                        
        }
    }
```

With:

```csharp
MoveData move = minimax.GetMove();
```

And now your chess game should play **much better**!
