# 02: Introduction to Unity Scripting - Prefabs and User Inputs

## Prefabs

An important concept to understand in Unity development is that of **Prefabs.**

A prefab is a **pre-fabricated Game Object.** Pre-fabricated means the Game Object is prepared *ahead of time*, and sits in your asset folder waiting to be added to a scene.

How is that helpful?

### A thought experiment

Imagine a scenario where you are developing your game and you are tasked with creating a forest. You've set up a scene and created a "tree" Game Object (maybe with a Mesh Renderer and a physics collider), and duplicated it 149 times throughout your scene. You've moved the various tree copies around to fill out an area that resembles where you want your forest to be. Great - we have a forest.

Now imagine that things change (as they inevitably do!). Suddenly, your design team wants all the trees to be animated to sway in the wind, and be twice as big.

You now have to go through **every tree in your scene** and add an animator to it, as well as tweak the scale. This might not be so bad if they're all in one place, but what happens when those trees have been copied across multiple scenes? You'd better remember every single place you've copied a tree into, otherwise the designers are not going to be happy. 

It becomes nigh-impossible to keep track of the trees throughout your project, and this only gets worse as your game grows. Imagine the effort every time you want to make a tiny change to something that is reused! You'll ragequit long before you ever complete your game. Thankfully, there is a better solution.

### A better solution

Prefabs allow you to avoid this situation entirely. A prefab acts as a **source of truth** for a Game Object that you want to reuse throughout your game. In our example from earlier; we could make a **tree prefab** that would sit inside our asset folder. This prefab could then be added as many times as we wanted into our forest scene. 

Each tree Game Object that is added to our scene is called a **prefab instance.** It is the same concept as instances in object-oriented programming. There is one prefab that acts as a "source of truth", or "blueprint", and many prefab instances that derive from it.

**One prefab; many prefab instances.**

Now we have 150 tree prefab instances in our forest scene. When the design team comes to us and asks us to add an animation and scale up the trees, no problem! We simply update the prefab in our project, and every single instance of that prefab across the **entire project** will **immediately** reflect the changes we made. Prefabs are *really powerful* and absolutely essential if you ever want to finish a moderately-sized project.

There are a couple of other things that make prefabs **essential**:
- Prefab instances can have different properties from the original prefab. These are called **overrides**, and they allow you to tweak individual properties of a prefab instance to make it unique while still reflecting any changes made to the original prefab.
- You can instantiate prefabs in your game **at runtime**. This allows you to do things like spawning an enemy at a certain time, or playing an effect when the player does a specific action (which is what we are going to do today).

If you're still lost, here is a useful video that explains the concept quite well:

>**Resource:** <https://www.youtube.com/watch?v=EyhRXMkW1ns>

### Live example of setting up prefabs

Collaboratively set up a heart prefab in class. If you missed class or need a refresher, check this video:

> Resource: <https://www.youtube.com/watch?v=IfcCXVXjLNM>

## Player Input

This may sound obvious, but in order for a game to be considered a "game", it needs to respond to the player. There's not currently any way for our game to respond to player inputs, so today we are going to remedy that.

Unity can **listen** for **input events** from the player (pressing a key, clicking the mouse etc.) and run code when those events happen. In our case, we want to listen for when the arrow keys are pressed, and move the hay machine in accordance with those inputs.

# **Collaborative Problem Solving Session 2: Make a Hay Machine Move**

## Creating the script

After class discussion, let's test our algorithm. 

Add a new script to the `Hay Machine` prefab, following the steps from last class.

## Task 1: Movement

In our `Update` method, we are going to write a short algorithm to listen for input, and then update the position of the `Hay Machine`'s transform based on that input we received. 

> **Hint:** We will need to use Unity's `Input` library and the `transform.Translate` method.

 You may find this documentation useful:
 
> Resource: <https://docs.unity3d.com/2019.1/Documentation/ScriptReference/Transform.Translate.html>

> Resource: <https://docs.unity3d.com/2019.1/Documentation/ScriptReference/Input.html>

Do your best to implement this algorithm yourself. **Only view the solution example once you've given it a proper attempt.**

<details>
    <summary>Solution Example</summary>

    ```csharp
    public float movementSpeed = 10f;

    void Update()
    {
        float horizontalInput = Input.GetAxisRaw("Horizontal"); 

        if (horizontalInput < 0) 
        {
            transform.Translate(transform.right * -movementSpeed * Time.deltaTime);
        }
        else if (horizontalInput > 0) 
        {
            transform.Translate(transform.right * movementSpeed * Time.deltaTime);
        }
    }
    ```
</details>

### Methods

In object-oriented programming, a **method** is just a **function** that belongs to a class. We can move our code out of `Update` and put it in its own method to keep our code clean and readable.

`Update` and `Start` are 2 methods you've already seen, so see if you can create your own method that runs the movement code. You should name it something that concisely conveys its purpose. You can now take the movement code out of `Update` and instead call your movement function in the `Update` loop. What we've just done is called **abstraction**, and it has made our code slightly easier to read!

**Optional:**
- Ask me if you want to talk in more detail about abstraction, or check out this article:

> Resource: <https://stackify.com/oop-concept-abstraction/>

### Task 2: Boundaries

Our code successfully moves the Hay Machine, but you may notice if you try to move too far in one direction, you can drive the Hay Machine out of bounds. Obviously, this is not the intended functionality :)

We've just found our first **bug!** As your game gets more complicated with more systems interacting, it becomes harder and harder to predict and detect bugs. This is why there's an entire discipline called Quality Assurance (or QA). Sadly we do not have our own QA testers - but you could always get a friend or family member to test your game for free.

When writing our code, we should always think about how it could have unintended consequences, and prepare to address those consequences. This is something that typically comes with practice, so expect to write a lot of code that doesn't work how you wanted the first time!

Try to **set boundaries** for the Hay Machine yourself. Think about what **conditions** we need to look out for to detect whether or not the Hay Machine is allowed to move, and what to do when those conditions are met.

Again, view the example solution **only once you have tried to implement the code yourself.**

<details>
<summary>Example Solution</summary>

```csharp
public float horizontalBoundary = 22;

void PerformMovement() {
    if (horizontalInput < 0 && transform.position.x > -horizontalBoundary) 
    {
        transform.Translate(transform.right * -movementSpeed * Time.deltaTime);
    }
    else if (horizontalInput > 0 && transform.position.x < horizontalBoundary) 
    {
        transform.Translate(transform.right * movementSpeed * Time.deltaTime);
    }
}
```

</details>

### Task 3: Make hay while the sun shines

Time to combine **prefabs** and **code.**

We're going to create a prefab for hay bales which we can use to spawn **hay bale instances** while the game is playing. These hay bales will be spawned when the player presses the **space bar**, and fly across the screen. 

### 3-1: Set up a hay bale prefab 

Using what you've learned, set up a prefab for a Hay Bale using the assets provided in the project. This prefab should contain a model for the Hay Bale as its **child**.

<details>
<summary>Childing and Parenting Game Objects</summary>

Imagine we have 2 Game Objects:
- `Head`
- `Crown`

If we dragged `Crown` onto `Head` in our hierarchy view, `Crown` would be placed underneath `Head`. This is called **childing** a Game Object. In our example, `Crown` would be the **child** of `Head`, and `Head` would be the **parent** of `Crown`.

A child's transform becomes relative to its parent. This means if we move `Head`, `Crown` moves with it. Same goes for scaling or rotating. `Crown` can still move, rotate and scale independently of `Head`, but it will always be relative to its parent. Play with this in editor to get a feeling for how parents and children interact.
</details>

The Hay Bale also needs 2 components:
- **Box collider** - Creates an invisible box around the object that can detect collisions with other objects. Make sure the **Is Trigger** option is checked.
- **Rigidbody** - Makes the object respond to the Unity physics engine (gravity, drag, force, etc.) Make sure the **Is Kinematic** option is checked.

You may also add the **Rotate** component from last class if you want the Hay Bale to spin.

> **Tip:**
> - When setting a Game Object as the child of another Game Object, be conscious of the child's transform values. Our Hay Bale model should be at  the position `(0, 0, 0 )` in relation to its parent, rotated at `(0, 0, 0)` and scaled to `(1, 1, 1)`. We can quickly achieve this by **right clicking** on the transform component's title and selecting **Reset.**

### 3-2: Making the Hay Bale Move

Create a new script called `Movement` and be sure to assign it to the prefab.

Write some code in the `Movement` update loop to make the hay bale fly across the screen.

### 3-3: Shooting hay bales

Add to the Hay Machine script that we wrote earlier. Listen for input on the **space bar** and spawn the hay bale prefab when the space bar is pressed.

> **Hint:** You will need to use Unity's `GameObject.Instantiate` method.

Some considerations:
- Where in the world should the hay bales be spawned?
- Should there be a limit on how quickly we can shoot? How would we enforce that?
- How will the Hay Machine know which prefab it's supposed to instantiate? Maybe a variable?

<details>
<summary>Example Solutions</summary>

`Movement.cs`:
```csharp
public class Movement : MonoBehaviour
{
    public Vector3 MovementSpeed = new Vector3(0f, 0f, 10f);

    private void Update()
    {
        transform.Translate(MovementSpeed * Time.deltaTime);
    }
}
```

`HayMachine.cs`:
```csharp
public class HayMachine : MonoBehaviour
{
    public float movementSpeed = 4f;
    public GameObject hayBalePrefab;
    public Transform haySpawnpoint;
    public float shootInterval;
    public float horizontalBoundary = 22;
    private float shootTimer;

    void Update()
    {
        PerformMovement();
        HandleShooting();
    }

    void PerformMovement()
    {
        float horizontalInput = Input.GetAxisRaw("Horizontal");

            if (horizontalInput < 0 && transform.position.x > -horizontalBoundary) 
            {
                transform.Translate(transform.right * -movementSpeed * Time.deltaTime);
            }
            else if (horizontalInput > 0 && transform.position.x < horizontalBoundary) 
            {
                transform.Translate(transform.right * movementSpeed * Time.deltaTime);
            }
    }

    void HandleShooting()
    {
        shootTimer -= Time.deltaTime;

        if (shootTimer <= 0 && Input.GetKey(KeyCode.Space))
        {
            shootTimer = shootInterval;
            ShootHay();
        }
    }

    private void ShootHay()
    {
        Instantiate(hayBalePrefab, haySpawnpoint.position, Quaternion.identity);
    }
}
```

</details>
