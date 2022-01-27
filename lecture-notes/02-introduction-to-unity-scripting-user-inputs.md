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
    float horizontalInput = Input.GetAxisRaw("Horizontal"); // 1

    if (horizontalInput < 0) // 2
    {
        transform.Translate(transform.right * -movementSpeed * Time.deltaTime);
    }
    else if (horizontalInput > 0) // 3
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

### Scene view

Open the **Game** scene. You can see all the Game Objects currently in the scene.

In the **hierarchy tab** you can see all the same Game Objects (and a few others), organised into a tree-like hierarchy.

- **Music** - an audio source playing the background music loop.
- **Main Camera** - the camera that points down at the scene to render everything to the player. To see anything upon running the game, you must have a camera.
- **Directional Light** - a single light source that illuminates the scene. Scenes in Unity need some sort of light source to be able to see anything when playing the game.
- **Scenery** - an empty Game Object to hold the ground and the windmills. Acts like a 'folder'.
- **Hay Machine** - this is the blue machine sitting on the rails. It is comprised of a few Game Objects to make it easy to customise later on.

Press the **play button** to run the game. Press the button again to stop the game and return to the scene view.

### Creating a script

We will now create a script to rotate the windmill blades. Right-click the **scripts** folder and select **Create > C# Script**. Name the script **Rotate**.

In the **hierarchy tab**, expand Scenery and double-click the Windmill Game Object to open the Prefab editor. We are now editing the **Windmill prefab** which is a collection of Game Objects. Click on the **Wheel** Game Object. In the **inspector tab** click the **Add Component** button and start typing "Rotate"... when the script appears in the list, select it.

To edit the script, you can double-click the script field of the Wheel component (where it now says Rotate). The script should open in Visual Studio. It should look like this:

```csharp
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
    
    }
    
    //Update is called once per frame
    void Update()
    {
    
    }
}
```

This script is a **class** that derives from **MonoBehaviour**, Unity's base class for components. By default, Unity components have a `Start` and `Update` method. The `Start` method is called the first time the component is initialised, and you would use it to set initial values, initial state, etc... The `Update` method is called **every frame** of the game, and is used to perform actions or update values as the game is running.

Add the following code inside of the `Update` method:

```csharp
transform.Rotate(0, 50 * Time.deltaTime, 0);
```

Let's break this line of code down.

- `transform` refers to a Game Object's **position**, **rotation** and **scale** - the different ways we can transform the object. We manipulate it's properties to move objects, rotate them and make them bigger and smaller. Usually the transform is a property coming off a Game Object; since we didn't specify anything in front of it (and just went straight in with `transform`, we are referring to **this** component (i.e. the transform of whatever Game Object this script happens to be attached to; in this case, the **Wheel**).
- `Rotate` is a public method of the Transform class, and takes 3 values - the **x-axis**, **y-axis** and **z-axis** rotations. Since we only want to rotate the wheel in one direction, we pass 0 to the x and z axes, and only change the y-axis.
- The value passed to the y-axis is `50 * Time.deltaTime`. If we had just 50, this would say increase the rotation by 50 degrees every frame. But, as we well know when playing games, framerate is a variable thing, based on the processing power of the machine. So, to make sure our rotation is stable, we are using `Time.deltaTime` in the calculation, which is the time in seconds between the last and the current frame. This will be calculated each frame, and provide a smoother, stable amount to rotate by based on how fast the machine is rendering the frames. Essentially, in the end, we are saying increase the rotation by 50 degrees per second.

Save the script and return to the Unity editor. Exit the Prefab editor and save your changes. Play the scene - the wheel of the windmill should now rotate!

### Variables

Varaibles store data. We can modify them and reuse them throughout our code, so they are more useful than simply defining a random `50` in our Rotate method. Let's add a variable to hold that rotation speed. Right above the `Start` method, add this code:

```csharp
public Vector3 rotationSpeed;
```

The `public` defines the access this variable has from outside this script - in most programming, you would use a `private` or `protected` variable and access it through setter and getter methods, but in Unity you use a lot more `public` variables. This is because marking a variable as public exposes it in the **Editor**.

The `Vector3` is the type that this variable holds. Our rotation is made up of 3 data points: x, y, and z. These can be expressed as a single **vector**.

Now replace this line in the `Update` method:

```csharp
transform.Rotate(0, 50 * Time.deltaTime, 0);
```

With this:

```csharp
transform.Rotate(rotationSpeed * Time.deltaTime);
```

As well as accepting individual x, y and z values, `Rotate` also has an overload method signature that accepts a Vector3. When we **multiply** a Vector3 by a number, each of the 3 values gets multiplied by that number. By default, the vcector values will get set to 0, so the `Time.deltaTime` will essentially be cancelled out on any axis we don't set a value for.

Save the script and return to the Unity editor. Open the **Windmill** prefab editor again. You should see in the **inspector tab** the Rotate component now has a **Rotation Speed** field with 3 adjustable values: x, y and z. Change the y value to something like 120 and exit the prefab editor.

Play the scene, and the wheel should be spinning faster than before. You can change the values in the editor in real-time while the game is playing to see the effect. **NOTE: any values you change while playing the scene will be reset when you stop the game.** 

If you want to save the values changed during play, right-click on the component and select **Copy Component**. Stop the play, right-click on the component again and select **Paste Component Values**. If you've done this to a single instance and want to apply it to **all** the instances of this Game Object, select **Overrides > Apply All** on the Prefab, and it will change the values of all the instances in the game.
