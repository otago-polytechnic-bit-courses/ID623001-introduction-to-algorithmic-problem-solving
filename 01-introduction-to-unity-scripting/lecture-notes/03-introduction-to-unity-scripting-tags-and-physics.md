# 03: Introduction to Unity Scripting - Physics and Interactivity

## Components

A **Component** is a piece of **functionality** attached to a Game Object. In the last class we created our own **components** with the `Rotate` script, but there are many **components** built into Unity for us to use.

**Components** are what give Game Objects their **functionality**, and a Game Object can have as many or as few **components** as it needs. It's all up to you as the developer how you want to structure your Game Objects and components!

We're going to look at some of Unity's **built in** components that are related to the **physics system:**

### Colliders

A collider is a piece of invisible geometry that defines the area of influence for a physical object.
- Can be **solid**, which means it will **not move through other colliders.**
- Can be a **trigger**, which **other colliders can move through.**

Both have their uses:
- **Solid colliders** are great for things like walls that characters or projectiles shouldn't be able to move through.
- **Triggers** are essential for adding more complex and interesting interactions to your game. An invisible trigger in a horror game could make a door slam shut when the player walks into it.

**Colliders** also come with several preset shapes, such as **box collider**, **sphere collider** and **mesh collider**. Which collider shape is best for your Game Object depends on your situation.

> **Tip:** Keep collider geometry as simple as you can. Often a capsule or cube is better than a complicated mesh.

### Rigidbodies

A **rigidbody** defines how a Game Object is affected by the **physics system.**

They come in 3 flavours:
- **Dynamic:** the Game Object is affected by gravity and responds to forces. Useful for characters, projectiles or vehicles.
- **Kinematic:** the Game Object still acts upon other objects but has to be moved in code. Useful for moving platforms.
- **Static:** The Game Object can’t move or have forces applied to it. Great for static geometry in levels, such as walls that cannot be moved.

> **Note:** in Unity 2019.x, there is no such thing as a **static** rigidbody. Instead, rigidbodies are **dynamic** by default, and can be made **kinematic** by ticking the `isKinematic` checkbox. This is different in later Unity versions.

### Tags

**Tags** are not components, but rather **strings** that are attached to Game Objects.

**Tags** are useful in physics, because we can check them in code to see what kind of object we are dealing with.

e.g.

```csharp
private void OnTriggerEnter(Collider other) {
    if (other.tag == "Player") {
        // We know that "other" is the player's game object.
        // Do something to the player.
    }
}

```
A Game Object's tag can be set via the "Tag" **dropdown menu** at the top of the **inspector view**.

### Physics Layers

**Physics layers** define which objects can collide with each other.

You can set the **physics layer** of a Game Object via the **"Layer"** **dropdown menu** at the top of the **inspector view.**

From here you can also add new **physics layers** to your project.

Navigating to Edit -> Project Settings -> Physics will allow you to edit the **physics collision matrix.** This matrix allows you to define which **physics layers** are allowed to interact with each other.


## Task 1: Destroy the Hay

Your first task is to create an end zone for the hay. Using the concepts we have discussed, you must create an invisible area at the end of the map that will destroy hay bales when they collide with it.

> **Hint:** The solution will involve the `OnTriggerEnter` Unity function:

> Resource: <https://docs.unity3d.com/ScriptReference/Collider.OnTriggerEnter.html>

**Do not look at the solution example until you have implemented your own solution!**


<details>
<summary>Solution Example</summary>

Create a new Game Object with a `box collider` that spans the entire length of the field. The `box collider` should have `IsTrigger` ticked. Place it at the end of the field.

Assuming we assigned a tag "Hay" to the hay bale prefab, we would add a script to the end zone object that contained this code:

```csharp
public string tagFilter;

private void OnTriggerEnter(Collider other) 
{
    if (other.CompareTag(tagFilter)) 
    {
        Destroy(gameObject); 
    }
}
```

Where `tagFilter` would be assigned the value `Hay` in editor.

</details>


## Task 2: Save the sheep

We will do a collaborative problem solving session for this task.

Create a new prefab that will act as a sheep. This sheep should be able to:
- Run towards the near side of the field.
- Collide with the hay bales that are shot out of the Hay Machine.
- Disappear once they have been hit by the hay.
- Drop off the world if they reach the end without being hit by a hay bale.


<details>
<summary>Solution Example</summary>

Create a new `Sheep` Game Object in the scene. Make it a prefab. Edit the prefab and give it the sheep model as a child.

Add a new tag "Sheep" and assign it to `Sheep`
Add a new tag "Hay" and assign it to the `Hay` prefab from last lesson.
Add a trigger `box collider` to the `Hay` prefab.

Add a script to `Sheep` that looks something like this:

```csharp
public float runSpeed; 
public float gotHayDestroyDelay; 
public float dropDestroyDelay; 
private bool hitByHay;
private bool dropped;
private Collider myCollider; 
private Rigidbody myRigidbody; 

private void Update() {
    // Move forwards
    transform.Translate(Vector3.forward * runSpeed * Time.deltaTime);
}

private void HitByHay()
{
    // Stop moving and then destroy after a delay.
    hitByHay = true; 
    runSpeed = 0;

    Destroy(gameObject, gotHayDestroyDelay);
}

private void Drop()
{
    // Make gravity start affecting us, then destroy after a delay.
    dropped = true;
    myRigidbody.isKinematic = false; 
    myCollider.isTrigger = false; 
    Destroy(gameObject, dropDestroyDelay); 
}

private void OnTriggerEnter(Collider other) 
{
    // If we collided with hay:
    if (other.CompareTag("Hay") && !hitByHay) 
    {
        Destroy(other.gameObject); 
        HitByHay(); 
    }
    // If we collided with the edge of the map:
    else if (other.CompareTag("DropSheep") && !dropped)
    {
        Drop();
    }
}
```
</details>

## Task 3: Sheep invasion

Now that our sheep is working, do your own research and figure out how to spawn sheep over time (remember `GameObject.Instantiate`)