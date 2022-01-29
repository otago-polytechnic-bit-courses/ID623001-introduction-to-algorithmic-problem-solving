# 02: Introduction to Unity Scripting - User Inputs

## Player input

We are going to **listen** for input events from the player and respond with some actions in the game - in the first instance, listening for the movement keys being pressed and moving the hay machine accordingly.

### Movement

Create a new C# script like before, and name it **HayMachine**. Open it in Visual Studio. Add this variable right above the `Start` method:

```csharp
public float movementSpeed;
```

Like the `rotationSpeed` varialbe we used earlier, this will control the movement speed of the hay machine.

Add this method below `Update`:

```csharp
private void UpdateMovement()
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

The first line of this method captures any player input that is changing along the **horizontal** axis, or the **left and right** input buttons. You can go into the settings of this game and define the buttons that consistute the horizontal axis, but for now we'll just leave them as the defaults: the **right and left** arrow buttons, and also **A and D** as alternate inputs.

The input is 0 by default - no key is pressed. If the **left** arrow is pressed, the input will register as **less than zero**. If the **right** arrow is pressed, the input will register as **greater than zero**. That is what the **if condition** is checking for - less than or greater than zero on the horizontal input.

`transform.Translate` is used to manipulate the Game Object's position. `transform.right` is shorthand for accessing a Vector3 on the horizontal (x) axis only. Again, we multiply this vector by our speed variable (positive or negative, to move right or left respectively) and `Time.deltaTime` (for smoothing).

To call the function, add this code to the `Update` method:

```csharp
UpdateMovement();
```

Save the script and add it to the **HayMachine** prefab. Set the initial **Movement Speed** to 14 and play the scene. Use the arrow keys to move the machine left and right.

### Boundaries

If you move the **Hay machine** in one direction long enough, it will go off the edge of the world... this is because we haven't told it where it should stop moving. In the **HayMachine** script, add this variable below **movementSpeed**:

```csharp
public float horizontalBoundary = 22;
```

In this case, 22 happens to be the distance limit for the machine to move either direction. Now, let's tweak the `UpdateMovement` method to include the boundary checking:

```csharp
if (horizontalInput < 0 && transform.position.x > -horizontalBoundary) 
{
    transform.Translate(transform.right * -movementSpeed * Time.deltaTime);
}
else if (horizontalInput > 0 && transform.position.x < horizontalBoundary) 
{
    transform.Translate(transform.right * movementSpeed * Time.deltaTime);
}
```

Now, not only are we checking for **horizontal input** in either direction, but the **Hay machine** must **also** be within the boundary limits for the machine to move.

Save the script again, return to the editor, and play the scene. The **Hay machine** should now stop at the edges of the map.

### Creating and shooting projectiles

We are going to make our **Hay machine** launch bales of hay when the player presses the 'fire' button.

We need to create a new Game Object to hold the bale of hay. Right-click in the **hierarchy tab** and select **Create Empty** to create a new, empty Game Object. Change its name to **Hay bale** in the inspector, and right-click on its Transform and select **Reset**.

Drag the **hay bale** model from the models folder onto the **Hay Bale** Game Object in the **hierarchy tab**. With the model still selected, name it **Hay Bale Model** and reset its Transform.

Select the **Hay Bale** Game Object and add these components:

- **Box collider** - creates an invisible box around the object that can detect collisions with other objects.
- **Rigidbody** - makes the object respond to the Unity physics engine (gravity, drag, force, etc...).
- **Rotate** - our rotate script from earlier.

Check the **Is Trigger** checkbox on the **Box collider** - this will stop it from 'pushing' other objects around, and indicates it is only for registering a collision event (for us to write some code).

Check the **Is Kinematic** checkbox on the **Rigidbody** - this will actually prevent the physics from dictating the hay bale's movement (e.g. gravity, causing it to fall and stick to the ground), and rather let us control its movement through our script.

Set the **Rotation speed** to **(X:300, Y:50, Z:0)** - which will rotate it around the x axis, and give it a slight roll about the y axis (like it's bouncing).

To check that the hay is rolling, press the play button.

To make the hay bale move forward, we will create a new script called **Move**.

Add the following variables to the top of the class above the `Start` method:

```csharp
public Vector3 movementSpeed;
public Space space;
```

The `movementSpeed` variable holds a Vector3 again, so we can say how much the hay bale should move along the x, y and z axes.

The `Space` variable is a Unity-specific thing: we can set the 'space' of the movement to either **World** or **Self** - if we choose **Self**, then the rotation of the Game Object will be taken into consideration, otherwise with **World** the movement will only happen along the axes of the world space (ignoring the rotation of the object).

Add the following code to the `Update` method:

```csharp
transform.Translate(movementSpeed * Time.deltaTime, space);
```

The `Translate` method, like before with the **Hay machine** tells the hay bale to move along a particular set of axes by a particular amount (and in this case, also takes a second argument for the 'space' variable).

Save the script and return to the editor. Add the **Move** component to **Hay Bale**, set the **Movement Speed** to **(X:0, Y:0, Z:20)** and leave **Space** set to **World**.

Play the scene and you should see the hay bale flying across the screen!

Drag the **Hay Bale** Game Object from the **hierarchy** to the **Prefabs** folder - this will make it a prefab we can reference later. Then delete the original Object from the **hierarchy**.

To make the hay machine shoot out bales, open the **Hay Machine** script and add these variables above `Start`:

```csharp
public GameObject hayBalePrefab; 
public Transform haySpawnpoint; 
public float shootInterval; 
private float shootTimer; 
```

- The first is a reference to the **Hay Bale** prefab.
- Next is the point at which the bale will spawn when shot.
- The next is a variable to limit 'spamming' the shoot button - it is the smallest time allowable between shots.
- Finally, a timer that decreases steadily to determine whether the machine can shoot again or not.

Now add a `ShootHay` method below `UpdateMovement`:

```csharp
private void ShootHay()
{
    Instantiate(hayBalePrefab, haySpawnpoint.position, Quaternion.identity);
}
```

**Instantiate** creates an instance of a prefab and places it in the scene at the position, and rotation specified. The first argument is the **Hay Bale** prefab, the second is the spawn point, and the third is a sort of 'default' rotation (similar to setting a Vector3 to **(X:0, Y:0, Z:0)**.

To call the method, we will poll for input; add the following method above `ShootHay`:

```csharp
private void UpdateShooting()
{
    shootTimer -= Time.deltaTime; 

    if (shootTimer <= 0 && Input.GetKey(KeyCode.Space)) 
    {
        shootTimer = shootInterval; 
        ShootHay(); 
    }
}
```

The first line decrements the shooting timer by 1 every second - we want the timer to be 0 or less than 0 before we are allowed to shoot. IF we are equal to or less than 0, **and the space bar is pressed** then we will execute the shooting code. We first reset the timer to our static interval variable (and it starts counting down again), and we call `ShootHay();`

Finally, add this line of code to `Update`, right below `UpdateMovement();`

```csharp
UpdateShooting();
```

This calls the polling method every frame. Save the script and return to the editor. Select **Hay Machine** from the **hierarchy tab**. Drag the **Hay Bale** prefab from the folder onto the **Hay Machine's** Hay Bale variable in the inspector. Drag the **Hay Spawnpoint** found in the **Hay Machine** hierarchy onto the **Spawnpoint** variable. And set the **Shoot Interval** to **0.8**.

Save the scene and click play - you should be able to move the hay machine back and forth and shoot bales by pressing the space bar.
