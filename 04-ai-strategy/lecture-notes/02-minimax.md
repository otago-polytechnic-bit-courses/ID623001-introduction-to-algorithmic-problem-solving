# 02: AI Strategy - Minimax

With the basic chess game working, we need to replace the rudimentary move system with our Minimax algorithm. We will first code the generic algorithm, and then apply some approaches that make it function a little better. 

## The algorithm

What we're going to write is a script that 'fake' plays all the possible moves for the current player, and all the possible response moves for the opponent, and then all the possible response moves for the player again. Since we're walking up and down a tree, we are going to write a **recursive function**... this function calls itself for each 'level' until it reaches the target 'depth' (3). When it reaches the end nodes at depth 3, it evaluates each game state and returns that up to the previous level (2) - depending on whether it is considering its own move or an opponent's, it will choose the **greater** or **lesser** of the end nodes as the parent value (for us, first time, it's the **greater**). It will then repeat this process (alternating between **maximum** and **minimum** values) up the tree until it reaches the start again... 

So technically it isn't even really considering its **next move**, but making decisions based on a move **3 turns away**... in **Minimax**, your **strongest move** is only as good as your opponent's **weakest move**.

(There is a really **helpful gif** on the Wikipedia page for Minimax if you want to watch the algorithm do its thing step-by-step... I've also put it here for your convenience...)

![](https://upload.wikimedia.org/wikipedia/commons/e/e1/Plminmax.gif)

### Coding Minimax

First create a new C# script called **Minimax**. Add the following variables:
