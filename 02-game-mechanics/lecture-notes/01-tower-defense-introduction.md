# 01: Game Mechanics - Tower defense introduction

## Starter project

Download the **starter code** and open it as a project in Unity.

## Tower defense

We are going to build a 2D tower defense game, and implement functionality such as **gaining and using resources**, **upgrading units**, and rudimentary **enemy wayfinding**.

Open the **GameScene** from the scenes folder and set the Game view aspect ratio to **4:3** - this will ensure the labels and background line up correctly. The object is to place monsters in defensive positions around the path to stop the bugs from reaching the cookie at the end. You will earn gold as you play and use it to ugrade the monsters, defeating multiple waves of enemy bugs!

### Unit placement

The first step is to set up the functionality to place your defensive monsters. Monsters can only be places on the spots marked with an **X**. Drag and drop **Images\Objects\Openspot** into the Scene view. Select **Openspot** in the **hierarchy tab** and add a **Box Collider 2D** component to it. Next add an **Audio\Audio Source** component to **Openspot** and set the Audio Source's **AudioClip** to **Audio\tower_place**. Deactivate **Play On Awake**.

Create a prefab from what you just created: drag and drop **Openspot** from the **hierarchy** to the **prefabs** folder. You can now create more placement spots using the prefab. Drag 11 more spots into the scene (don't worry about their position now, we'll adjust those in a minute). You should have a total of 12 placement spots in the **hierarchy**.

Now we will set the positions of each of the spots... the 12 position values are as follows:

- **(X:-5.2, Y:3.5, Z:0)**
- **(X:-2.2, Y:3.5, Z:0)**
- **(X:0.8, Y:3.5, Z:0)**
- **(X:3.8, Y:3.5, Z:0)**
- **(X:-3.8, Y:0.4, Z:0)**
- **(X:-0.8, Y:0.4, Z:0)**
- **(X:2.2, Y:0.4, Z:0)**
- **(X:5.2, Y:0.4, Z:0)**
- **(X:-5.2, Y:-3.0, Z:0)**
- **(X:-2.2, Y:-3.0, Z:0)**
- **(X:0.8, Y:-3.0, Z:0)**
- **(X:3.8, Y:-3.0, Z:0)**

You should now see 12 **Xs** spaced around the path on the map, 3 rows of 4.

### Placing monsters

Select **Openspot** in the prefabs folder, and in the inspector click **Add Component**. Choose **New script** and name it **PlaceMonster**. Open the script in Visual Studio. Add these two variables above `Start`:

```csharp
public GameObject monsterPrefab;
private GameObject monster;
```

The first is a reference to the monster prefab, and the second is a variable that holds a monster on this spot, when you have created one. 

And add the following method:

```csharp
private bool CanPlaceMonster()
{
  return monster == null;
}
```

This method checks if the `monster` variable is **null**. **Null** means no monster has been created here yet, so it is ok to create one. If `monster` is NOT null, then it means we have already created one, so this method will help us prevent duplicate placement on one spot.

Add the following method to create a monster on this spot:

```csharp
void OnMouseUp()
{
  if (CanPlaceMonster())
  {
    monster = (GameObject)Instantiate(monsterPrefab, transform.position, Quaternion.identity);

    AudioSource audioSource = gameObject.GetComponent<AudioSource>();
    audioSource.PlayOneShot(audioSource.clip);

    // TODO: Deduct gold
  }
}
```

This is a built-in Unity method called `OnMouseUp` which listens for a **mouseUp** event on the **Openspot** - basically that means the player clicked the spot. The method then calls `CanPlaceMonster()` to check if a monster has already been placed here or not. If the spot is clear, then a new monster is instantiated from the prefab. Let's take a look at this line more closely:

```csharp
monster = (GameObject)Instantiate(monsterPrefab, transform.position, Quaternion.identity);
```

First we have the `Instantiate` method that we've used before... this takes the `monsterPrefab` as its reference, the `transform.position` of the **Openspot** as its position (that is, place the monster at the same location), and no rotation (the default `Quarternion.identity`). 

The `(GameObject)` at the start is called a **cast**... If you remember your inheritance lessons from Programming 2, you'll know instances of classes can be cast as different types of that class based on inheritance rules: in this case, `Instantiate` creates an instance of the base class `Object`, but we want to access methods of the descendant class `GameObject`, so we cast it and store it *as* a `GameObject`. It's like how *dogs* are *animals*, and so you can think of *Snoopy* as either a *dog* OR an *animal* - it doesn't change what *Snoopy* is, just how you think of him.

Finally, we are assigning the instantiated object into the `monster` variable... now, if we try to click here again, `CanPlaceMonster()` would return NOT null, and thus this placement code would not run a second time.

The last two lines of the method are just getting the **AudioSource** of the **Openspot** and playing the placement sound effect.

Save the script and switch back to the editor. Select **Openspot** in the prefabs folder, and in the Inspector click the circle to the right of the **PlaceMonster** script's **Monster Prefab** field - select **Monster** from the dialog box that appears.

Run the scene and you should be able to place monsters on the **X** spots.
