# 01: AI Strategy - Introduction to chess

In this module, we are going to look at **decision trees** which are a means for **AI** to make decisions about what actions to take. The application of these trees can take many forms depending on the context of the game; we are going to look at a very simple example of an AI weighing up all its options before making its 'move' - **Chess**! Now, Chess itself is not simple... and to play it well, you need a lot of knowledge and practice. But it provides a good example for us for a couple reasons: (1) the rules of the game are very well-defined - there are only certain legal moves that can be made at any turn, so this makes the options available easy to evaluate; and (2) it is turn-based, which will be easier for us to deal with than, say, a real-time game where the state-of-play is constantly updating.

## Starter project

Download the **starter code** for this game, which is a fully coded chess game, with all the legal moves etc, already prepared for you :)

## Decision trees

As the name suggests, the idea of **decision trees** are just trees (remember from last module: nodes and edges, in a hierarchical structure) that evaluate different scenarios and assign some sort of **weighting** to those scenarios, positive or negative. Let's consider a very basic example: the **AI** is deciding whether to move **left** or **right**. These form the nodes of the tree:

![](../../dec_tree1.png)

Now, in this scenario, to the left is a trap that will kill the AI instantly. On the right is a treasure that will reward the AI with 100 pieces of gold. So, the AI is considering a few variables in this move - its **health** and its **amount of gold**. A very simple **evaluation** of this could be something like this: moving **left** will **damage** the AI **100 health** (**-100**) and also yield **no new gold** (**+0**); moving **right** will **not damage** the AI at all (**-0**) and will yield **100 gold** (**+100**). So, the evaluation of each path might look like this to the AI:

![](../../dec_tree2.png)