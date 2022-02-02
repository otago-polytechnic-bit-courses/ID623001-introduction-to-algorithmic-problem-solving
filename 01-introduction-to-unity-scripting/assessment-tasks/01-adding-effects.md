# 01: Introduction to Unity Scripting Assessment Tasks

## Adding Effects

### Visuals

We are going to add a small heart model that appears when the sheep are hit by hay, to show how grateful they are to have been saved. The heart will:

- appear when the sheep is hit by hay
- move up
- rotate around
- scale down (get smaller)
- eventually disappear after a short time

First, we'll create the heart prefab. Create a new empty Game Object in the **hierarchy** and name it **Heart**. Set its position to **(X:0, Y:5, Z:0)**. Now add a model to it by dragging **Heart** from the models folder onto **Heart** in the **hierarchy**. Name the child **Heart Model** and reset its Transform and set its rotation to **(X:-90, Y:-45, Z:0)**.

You should now see a big heart floating above the ground in the scene.

Add a **Move** component to **Heart** and set its **Movement Speed** to **(X:0, Y:10, Z:0)** to make it move up.

Add a **Rotate** component and set its **Rotation Speed** to **(X:0, Y:180, Z:0)**.

Press play and the heart should float up and rotate.

Now create a new C# script and name it **TweenScale**. Add these variables above `Start`:

```csharp
public float targetScale; 
public float timeToReachTarget; 
private float startScale;  
private float percentScaled; 
```

This script will 'tween' (or animate) between two sizes, essentially scaling down the heart over time. The first varialbe, **targetScale** is the size we want the heart to be at the end of the tween. The second variable is the time it will take to reach the target scale. The **startScale** is how big the Object is when the script is activated. And **percentScaled** is a percentage (between 0.0 and 1.0) that will change over time, and is used in the calculations as we move from the start scale to the end scale.

Add this to `Start`:

```csharp
startScale = transform.localScale.x;
```

This simply sets the **startScale** variable to the current scale of the heart when the script is activated.

Now, add this code to `Update`:

```csharp
if (percentScaled < 1f) 
{
    percentScaled += Time.deltaTime / timeToReachTarget; 
    float scale = Mathf.Lerp(startScale, targetScale, percentScaled); 
    transform.localScale = new Vector3(scale, scale, scale); 
}
```

The **if statement** says while the percent is less than 1 (100%) - i.e. the scaling hasn't reached its target scale amount - then keep executing the scaling code. The next line adds an amount to the **percentScaled** variable: it takes the `Time.delatTime`, which is our 'smoothing' time value, and divides it by the **timeToReachTarget** - this produces a small amount to increment the **percentScaled** each frame, in order to take the specified time. The next line creates a new variable called **scale** using the `Mathf.Lerp` built-in function. `Mathf` is a library of math functions, and `Lerp` is used to find a value between two numbers based on a percent - so we give it a start value, an end value, and a percent along that line to see what value that would be (imagine plotting those points on a graph). As this method keeps getting called we move along that line from the starting value to the end value. The last line of this method sets the scale of the heart to the calculated value from the `Lerp`.

Now save the script and return to the editor. Select **Heart** and add a **Tween Scale** component. Set its **Target Scale** to 0.5 and change **Time To Reach Target** to 1.5. Play the scene and the heart should now shrink as it moves up.

Now we just need to add a little code to make the heart disappear after a little while.

Create a new C# script named **DestroyTimer** and add this line above `Start`:

```csharp
public float timeToDestroy;
```

And add this to `Start`:

```csharp
Destroy(gameObject, timeToDestroy);
```

This simply delays a `Destroy` call for the specified amount of time. Save the script and return to the editor. Add a **Destroy Timer** component to **Heart** and set its **Time To Destroy** to 1.5.

Now drag the **Heart** to the prefabs folder to turn it into a prefab. Finally, delete **Heart** from the **hierarchy** and open the **Sheep** script again.

Add the following variables below the others:

```csharp
public float heartOffset; 
public GameObject heartPrefab; 
```

The first variable is an offset on the y-axis where the heart will spawn, and the second is a reference to the **Heart** prefab you just made.

Next add this line to `HitByHay`:

```csharp
Instantiate(heartPrefab, transform.position + new Vector3(0, heartOffset, 0), Quaternion.identity);
```

This line instantiates a new heart and positions it above the sheep, offset by **heartOffset** on the y-axis.

We can also add some visuals to the sheep when it gets hit, and we can do this dynamically in the code. Add the following lines below the last one you just added:

```csharp
TweenScale tweenScale = gameObject.AddComponent<TweenScale>();
tweenScale.targetScale = 0; 
tweenScale.timeToReachTarget = gotHayDestroyDelay;
```

This adds a **Tween Scale** component dynamically to the sheep (rather than us adding one in the editor). We set the **targetScale** to 0 so the sheep completely shrinks away. Set the **timeToReachTarget** variable to the same time that the sheep takes to get destroyed (**gotHayDestroyDelay**).

Save the script and return to the editor. Select **Sheep** in the prefabs folder, set **Heart Offset** to 4 and drag **Heart** from the folder to the **Heart Prefab** slot.

Now play the scene and try shooting a sheep!

### Sound Effects

We're going to create a script that uses the **singleton** pattern, so it can be called from other scripts without needing a reference.

Create a new folder in the scripts folder called **Managers**. Create a new C# script in that folder and name it **SoundManager**. Add the following variables above `Start`:

```csharp
public static SoundManager Instance; 

public AudioClip shootClip; 
public AudioClip sheepHitClip; 
public AudioClip sheepDroppedClip; 

private Vector3 cameraPosition; 
```

The **SoundManager** instance is a `public static` variable, so it can be accessed from any other script. It stores a reference to a **SoundManager** component. The next three variables are references to various sound effect clips. The final variable holds a cached position of the camera.

Now, replace `Start` with `Awake` and add this code to the method:

```csharp
Instance = this; 
cameraPosition = Camera.main.transform.position; 
```

We changed the `Start` method to an `Awake` method because in Unity, in the order of code execution, `Awake` fires before `Start`... this means, if you try to reference the **SoundManager** from another script in *its* `Start` method, you know it was already initialised.

The two lines of code above: (a) cache this script in the variable called **Instance**; and (b) cache the position of the main camera in **cameraPosition**.

Next add this method:

```csharp
private void PlaySound(AudioClip clip)
{
    AudioSource.PlayClipAtPoint(clip, cameraPosition);
}
```

This method takes an audio clip and plays it at the position of the camera.

Finally, add these three methods to actually trigger the sound effects.

```csharp
public void PlayShootClip()
{
    PlaySound(shootClip);
}

public void PlaySheepHitClip()
{
    PlaySound(sheepHitClip);
}

public void PlaySheepDroppedClip()
{
    PlaySound(sheepDroppedClip);
}
```

Now, that the audio manager is ready, you'll need to add some code to other scripts to make use of its audio playing capabilities. Save this script, open the **HayMachine** script and add this line to `ShootHay`:

```csharp
SoundManager.Instance.PlayShootClip();
```

Now, save the **HayMachine** script and open up the **Sheep** script. Add this line to `HitByHay`:

```csharp
SoundManager.Instance.PlaySheepHitClip();
```

Next, add this line to `Drop`:

```csharp
SoundManager.Instance.PlaySheepDroppedClip();
```

Save this script and return to the editor. The sound manager needs to be added to a Game Object in order to work correctly, so create a new empty **GameObject** in the root of the **hierarchy** and name it **Managers**. Now create another empty **GameObject**, name it **Sound Manager** and make it a child of **Managers**.

Select **Sound Manager** and add a **Sound Manager** component to it. Now drag the audio clip from the sounds folder to their corresponding slots on **Sound Manager**.

Now play the scene and shoot some sheep, and you'll hear the sound effects playing!
