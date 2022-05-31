# 02: AI Strategy - Minimax

With the basic chess game working, we need to replace the rudimentary move system with our Minimax algorithm. We will first code the generic algorithm, and then apply some approaches that make it function a little better. 

## The algorithm

What we're going to write is a script that 'fake' plays all the possible moves for the current player, and all the possible response moves for the opponent, and then all the possible response moves for the player again. Since we're walking up and down a tree, we are going to write a **recursive function**... this function calls itself for each 'level' until it reaches the target 'depth' (3).

First create a new C# script called **Minimax**.
