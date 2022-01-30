# 03: Introduction to Unity Scripting - Tags and Physics

## Tags

Tags are used to identify and reference Game Objects - like "Player" or "Collectible" - so you can find them and differentiate them easily in code.

Creating tags is pretty straightforward: select **Edit > Project Settings** in the top menu bar, and open **Tags and Layers** in the settings window. Expand the **Tags** list and you'll see two have already been created for you: **DestroyHay** and **DropSheep**. Click the `+` button below the **Tags** list, name the new tag **Hay** and click save. Close the project settings.

Select **Hay Bale** in the Prefabs folder and select **Hay** in the **Tag** dropdown field. Now we can easily differentiate between hay bales and other Game Objects. We will now create an area that will destroy the hay bales if they collide with it.

### Trigger area

Add a new GameObject in the **hierarchy tab** and name it **Triggers**. Reset its Transform and add an empty GameObject as its child - name this child **Hay Destroyer**. Select **Hay Destroyer**, set its position to **(X:0, Y:4, Z:68)** and add a **Box Collider** to it. Check the **Is Trigger** checkbox on the collider and set its **Size** to **(X:60, Y:8, Z:12)**. Next, change the tag of **Hay Destroyer** to **DestroyHay**. In the scene view, you should be able to see the outline of the collider at the far end of the field.

Create a new C# script and name it **DestroyOnTrigger**. Remove the `Start` and `Update` methods entirely and add this variable instead:

```csharp
public string tagFilter;
```

This string will allow you to enter the name of any tag that will destroy this GameObject. Now add this method below the variable:

```csharp
private void OnTriggerEnter(Collider other) 
{
    if (other.CompareTag(tagFilter)) 
    {
        Destroy(gameObject); 
    }
}
```

`OnTriggerEnter` is a special Unity method that gets called when a GameObject with a **Rigidbody** and **Collider** (like our hay bales!) enters the trigger area.

The argument for `OnTriggerEnter` is the GameObject that has entered the area; the **if statement** here is checking if that object has the tag name specified in our **tagFilter** variable. If the tag of the object matches the one we are looking for, destroy the object.

Save the script and return to the editor. Select **Hay Bale** in the prefabs folder, add **Destroy On Trigger** as a component and change **Tag Filter** to **DestroyHay**.

Press the play button and shoot some hay bales - they should get destroyed when they reach the trigger area at the end of the field.

## Sheep

The object of this game is to stop the sheep from running off the edge of the field by launching hay bales at them. So, it's time to instantiate some sheep.

First, create a new empty GameObject in the **hierarchy** and name it **Sheep**. Reset its Transform, set its **Y rotation** to 180 and add both a **Box collider** and a **Rigidbody**. Check the **Is Trigger** checkbox of the **Box collider** and change its **Center** to **(X:0, Y:1.4, Z:-0.3)** and its **Size** to **(X:2.5, Y:2, Z:4)**. Finally, check the **Is Kinematic** checkbox on the **Rigidbody**.

Now, drag the Sheep model from the models folder onto **Sheep**, name the GameObject you just added **Sheep Model**, reset its Transform, and set its **X rotation** to -90 so its head comes out of the ground.

Create a new C# script named **Sheep**, and add the following variable declarations right above `Start`:

```csharp
public float runSpeed; 
public float gotHayDestroyDelay; 
private bool hitByHay; 
```

**runSpeed** is the speed in meters per second that the sheep will run. The second variable is the delay in seconds before the sheep gets destroyed after it is hit by hay. The last is a boolean that is set to true once the sheep is hit by hay.

Add this code to `Update`:

```csharp
transform.Translate(Vector3.forward * runSpeed * Time.deltaTime);
```

This makes the sheep run towards its forward vector at the speed set in the **runSpeed** variable.

Next add this method below `Update`:

```csharp
private void HitByHay()
{
    hitByHay = true; 
    runSpeed = 0;

    Destroy(gameObject, gotHayDestroyDelay);
}
```

When this method is called, the **hitByHay** boolean is set to true, the **runSpeed** is set to 0 (to stop the sheep from moving anymore) and the **gotHayDestroyDelay** is used in the `Destroy` method to delay the destruction of the sheep slightly. (`gameObject` here is the sheep in question)

Add the following method:

```csharp
private void OnTriggerEnter(Collider other) 
{
    if (other.CompareTag("Hay") && !hitByHay) 
    {
        Destroy(other.gameObject); 
        HitByHay(); 
    }
}
```

Similar to the last time we used this method, the sheep will compare the tag of anything that collides with it, and if it matches "Hay" it will go into the destroy functionality. Here, there is a second condition in the **if statement** which is checking that the **hitByHay** boolean is not yet set to true (which it will be as soon as we call the `HitByHay` method) - this stops the method from being repeatedly called during the 'delay' period.

If both conditions are met, we first destroy the hay bale ('other' in this case) and then call `HitByHay();` to start the sheep self-destruct sequence.

Save the script and return to the editor. Select **Sheep* in the **hierarchy** and add a **Sheep** component. Set its **Run Speed** to 10 and **Got Hay Destroy Delay** to 1. Play the scene and try it out - shooting sheep should now destroy them.

But we still need to set up a zone at the back of the machine for any sheep that manage to get through without being shot with hay. Create a new empty GameObject as a child of **Triggers**, anme it **Sheep Dropper** and reset its Transform. Set its position to **(X:0, Y:4, Z:-54)** and add a **Box collider** with **Is Trigger** checked and a **Size** of **(X:60, Y:8, Z:12)**. Change its **Tag** to **DropSheep** and we have a trigger area set up behind the hay machine.

When a sheep hit this trigger zone, it will fall and eventually be destroyed off screen. Open the **Sheep** script again and add these variables below the existing ones:

```csharp
public float dropDestroyDelay; 
private Collider myCollider; 
private Rigidbody myRigidbody; 
```

The first is the delay when the sheep starts dropping before it is destroyed. The second is a reference to the sheep's **Collider** and the third is a reference to the sheep's **Rigidbody**. We can assign these in the `Start` method by adding these lines:

```csharp
myCollider = GetComponent<Collider>();
myRigidbody = GetComponent<Rigidbody>();
```

This finds and caches the sheep's collider and rigidbody for later use. Next, we want to add a method that will 'turn on' gravity for the sheep once they hit the trigger zone:

```csharp
private void Drop()
{
    myRigidbody.isKinematic = false; 
    myCollider.isTrigger = false; 
    Destroy(gameObject, dropDestroyDelay); 
}
```

By turning **Is Kinematic** off, the sheep will start to fall, affected by Unity's gravity. We also turn the **Is Trigger** off to stop it being affected by the trigger zone. Finally, we start the self-destruct sequence with the **dropDestroyDelay**.

Now, we need to tweak the `OnTriggerEnter` method by adding this as an **else condition** below the existing **if condition**:

```csharp
else if (other.CompareTag("DropSheep"))
{
    Drop();
}
```

Here, if the sheep was hit by something other than a hay bale, this checks if it was something with the **DropSheep** tag, and if so, calls `Drop();`. Save the script and return to the editor. Select **Sheep** and change **Drop Destroy Delay** to 4. Play the scene and watch the sheep run past the machine and fall off the edge of the world!

Now that we're done setting up the sheep, drag it from the **hierarchy** into the Prefabs folder and delete the original from the **hierarchy**. Now we can instantiate many sheep to run at the machine!
