# 01: Maze generation Advanced Assessment Tasks

## Extra features

Implement this advanced functionality. These tasks are self-directed and will require some independent research and problem-solving.

1. Implement what happens if the monster reaches you (collides with you) before you reach the treasure: 
- You should print "Gotcha!" to the console (like the "You Win" message from before)...
- You should start a new maze (it's like a neverending nightmare of mazes until you finally reach the treasure!)
- **Hint 1:** you will need to follow a similar pattern as `OnTreasureTrigger`, but for the `monster`... think about where the `monster` is generated, and where you should rig up the `TriggerEventRouter` for him.
- **Hint 2:** how will you start a new maze? Do you already have a method, maybe, that does what you need??

2. We are going to implement a 'helper' function for the player - some of these mazes could be huge, and virtually impossible to navigate - we are going to:
- Implement a feature where if the player presses the **F key** a 'path' of spheres will show the way to the treasure from where they are.
- **Hint 1:** you can implement the spheres in a *similar* way as you made the treasure cube...
- **Hint 2:** you can call the `FindPath` method from the **AIController** to do the pathfinding... just need to figure out the right values to pass into it...
- **Hint 3:** you should destroy any previous path spheres before showing the path (so you don't get confused by multiple spheres on the screen).
- **Hint 4:** here is some starter code for how you would write this (this goes in **GameController**):

```csharp
void Update()
{
    if(Input.GetKeyDown("f"))
    {
        // put your code here
    }
}
```

All these things we've done in one form or another! It's your job to puzzle out where we've already written some similar code that can be adapated to these tasks!
