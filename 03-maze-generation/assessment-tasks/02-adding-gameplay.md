# 02: Maze generation assessment tasks - Adding gameplay

So - we have our level (the procedurally generated maze), our protagonist (the player) and an antagonist (the Scary Man) - but there is still no actual gameplay - you can try to avoid the Scary Man forever, but there's no way to actually win. And, if the Scary Man reaches you...... nothing happens! So, let's add some gameplay!

## Adding a goal

In **MazeConstructor**, we've already set up some of what we need for placing the goal; remember, we have already set `goalRow` and `goalCol` previously. Add the following method to the bottom of **MazeConstructor**:

```csharp
private void PlaceGoal(TriggerEventHandler treasureCallback)
{            
    GameObject treasure = GameObject.CreatePrimitive(PrimitiveType.Cube);
    treasure.transform.position = new Vector3(goalCol * hallWidth, .5f, goalRow * hallWidth);
    treasure.name = "Treasure";
    treasure.tag = "Generated";

    treasure.GetComponent<BoxCollider>().isTrigger = true;
    treasure.GetComponent<MeshRenderer>().sharedMaterial = treasureMat;

    TriggerEventRouter tc = treasure.AddComponent<TriggerEventRouter>();
    tc.callback = treasureCallback;
}
```

This method will create a cube, texture it with a nice gold colour, and place it at the end of the maze. We are using an included script called `TriggerEventRouter` to fire an event when the player collides with the 'treasure'. It takes a **callback method** (which we haven't written yet), and will fire that method when the collission happens.

In `GenerateNewMaze`, add this line to the very bottom:

```csharp
PlaceGoal(treasureCallback); 
```

This places the treasure and passes in the `treasureCallback` - but where does it come from? Well, we're going to write it in our **GameController** and pass it into `GenerateNewMaze`... so we'll make a little adjustment to the method signature, and add a new argument:

```csharp
public void GenerateNewMaze(int sizeRows, int sizeCols, TriggerEventHandler treasureCallback)
```

Now `GenerateNewMaze` takes 3 arguments: the row size, the column size, and a callback for colliding with the treasure. 

Save this script, and open **GameController**. Add this method to the bottom:

```csharp
private void OnTreasureTrigger(GameObject trigger, GameObject other)
{ 
    Debug.Log("You Won!");
    aIController.StopAI();
}
```

A very simple method (you can make this more complex if you wish) - this simply prints "You Won" to the console, and calls a method on **AIController** called `StopAI()` (which we haven't written yet). 

Now, just pass in the name of this method (not a method call, though) to `constructor.GenerateNewMaze` in `Start`... it should look like this:

```csharp
constructor.GenerateNewMaze(rows, cols, OnTreasureTrigger);
```

Now, the last detail is to add a method to the **AIController** script. Open it, and add this method:

```csharp
public void StopAI()
{
    startRow = -1;
    startCol = -1;
    Destroy(monster);
}
```

First we set `startRow` and `startCol` back to **-1** (the condition that makes the `Update` stop trying to do things), and we destroy the actual `monster` from the game.

Now - what happens when if the monster gets you? Check out the **Advanced Assessment Tasks** to find out!
