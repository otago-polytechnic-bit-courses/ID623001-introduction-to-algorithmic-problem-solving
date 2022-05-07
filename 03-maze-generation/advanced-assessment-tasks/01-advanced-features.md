# 01: Maze generation Advanced Assessment Tasks

## Extra features

Implement this advanced functionality. These tasks are self-directed and will require some independent research and problem-solving.

1. Implement what happens if the monster reaches you (collides with you) before you reach the treasure: 
- You should print "Gotcha!" to the console (like the "You Win" message from before)...
- You should start a new maze (it's like a neverending nightmare of mazes until you finally reach the treasure!)
- **Hint 1**: you will need to follow a similar pattern as `OnTreasureTrigger`, but for the `monster`... think about where the `monster` is generated, and where you should rig up the `TriggerEventRouter` for him.
- **Hint 2**: how will you start a new maze? Do you already have a method, maybe, that does what you need??

2. We are going to implement a 'helper' function for the player - some of these mazes could be huge, and virtually impossible to navigate - we are going to:
- Implement a feature where if the player presses the **F key** a 'path' of spheres will show the way to the treasure.
