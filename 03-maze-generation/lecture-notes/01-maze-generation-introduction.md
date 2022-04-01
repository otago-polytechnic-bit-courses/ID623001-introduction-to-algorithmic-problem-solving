# 01: Maze Generation - Introduction

## Starter project

Download the **starter code** and open it as a project in Unity.

## Maze generation

Some games lean heavily on **good level design** - carefully thought out and constructed levels with interesting layouts and paths. This can be time-consuming though, which is why a lot of games rely on **procedural generation** of levels - you can create near-infinite variations with (relatively) minimal code. Particularly, for **dungeon crawler** type games, **maze generation** can create interesting (and unique) labyrinths for the player to explore.

A maze typically has a **start** and an **end** and a **perfect** path between them (that is, one correct path to **solve** the maze, and then a number of other paths that lead to dead-ends; there are no 'loops' in a perfect maze); however, in games (especially the 'dungeon' type game we've mentioned before) some looping paths make for more interesting gameplay. So in this project, we will create a maze with a **start**, an **end** and various paths... oh, and a scary monster dude that hunts you down (but more on that later)...
