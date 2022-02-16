# 03: Introduction to Unity Scripting Assessment Tasks

## Dealing with multiple scenes

### Title scene

Save the currently opened scene and open the **Title** scene from the scenes folder. Take a look around the scene, it's the same environment as the game itself, but with some props added together and a different camera view. Expand the **Main Camera** in the **hierarchy** and see the three children: **Title**, **Start Button** and **Quit Button**.

Start by selecting **Start Button** and **Quit Button** in the **hierarchy**. Add a **Box collider** to both and set their size to **(X:2.5, Y:1, Z:0.4)**.

Now add an **Event System** to the **hierarchy** by selecting **GameObject > UI > Event System** from the top menu. This will add a Game Object named **EventSystem** to the scene with an **Event System** and a **Standalone Input Module** component attached. The event system is used to handle all sorts of mouse, keyboard, gamepad and touch input and passes the data on to the game. It's an essential part of handling player input when it comes to user interfaces. There's one more component that's needed before interacting with these buttons will work.

Select **Main Camera** and add a **Physics Raycaster** component to it. This component, as its name implies, casts rays into the scene and passes events to the event system when a 3D collider is detected (like when hovering over a button).

Create a new folder in the scripts folder and name it **Title**. Now create a new C# script in there called **StartButton**. Add these `using` statements below the others:

```csharp
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
```

Now replace this line:

```csharp
public class StartButton : MonoBehaviour
```

With this:

```csharp
public class StartButton : MonoBehaviour, IPointerClickHandler
```

Inheriting from the `IPointerClickHandler` interface allows this class to receive **OnPointerClick** callbacks from the event system. In fact, it's required to use the method when inheriting from that interface, so you'll probably see some red squiggly lines showing up. Remove `Start` and `Update`. Add the following method to fix that:

```csharp
public void OnPointerClick(PointerEventData eventData) 
{
    SceneManager.LoadScene("Game"); 
}
```

This method needs to have a specific signature in order to work with the event system, you need a single **PointerEventData** parameter that holds all information you could wish for about the click event. We use the **SceneManager** to load the **Game** scene.

Save the script and return to the editor. Select **Start Button**, add a **Start Button** component to it and take a look at the bottom of the **Inspector**. It now shows an extra section named **Intercepted Events**, which lists all of the events that will be passed from the event system onto the selected Game Object. In this case, when you click this Game Object, `OnPointerClick` will be called on **StartButton**.

Play the scene and click on the **Start** button; the **Game** scene will load.

To get the quit button working, duplicate the **StartButton** script and name the copy **QuitButton**.

Replace this line:

```csharp
public class StartButton : MonoBehaviour, IPointerClickHandler
```

With this:

```chsarp
public class QuitButton : MonoBehaviour, IPointerClickHandler
```

Now remove this line inside of `OnPointerClick`:

```csharp
SceneManager.LoadScene("Game");
```

And replace it with:

```csharp
Application.Quit();
```

Save the script and return to the editor. Select **Quit Button**, add a **Quit Button** component to it and run the scene. Try clicking the **Quit** button and you'll notice nothing happens. This is because `Application.Quit` only works in an actual build of the game, not inside the Unity software. Try building the game to an executable by pressing **CTRL + B** and selecting a folder to deploy to. After the game has launched, try clicking the **Quit** button again, it will close the game as expected now.

There's one final adjustment to the buttons that will make it more clear to the player that they can be clicked, and that's by changing their colour when the mouse cursor hovers over them. Create a new C# script in the **Scripts/Title** folder and name it **ChangeColorOnMouseOver**. Add this `using` statement below the others:

```csharp
using UnityEngine.EventSystems;
```

And replace this line:

```csharp
public class ChangeColorOnMouseOver : MonoBehaviour
```

With this:

```csharp
public class ChangeColorOnMouseOver : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
```

These pointer event interfaces are used to react to events triggered by the event system. In this case, they supply methods for when the pointer enters and exits the GameObject. You will get some errors again about not implementing the interface members, but ignore those for now, they'll disappear once you add the methods.

Add these variables above `Start`:

```csharp
public MeshRenderer model; 
public Color normalColor; 
public Color hoverColor; 
```

Here's what these are used for:

- a reference to the mesh renderer that needs its colour changed.
- the default colour of the model.
- the colour that should be applied on the model when the pointer is hovering over it.

Now add this to `Start`:

```csharp
model.material.color = normalColor;
```

This changes the model's colour to the normal one.

Now, remove `Update` and add these methods in its place:

```csharp
public void OnPointerEnter(PointerEventData eventData) 
{
    model.material.color = hoverColor;
}

public void OnPointerExit(PointerEventData eventData) 
{
    model.material.color = normalColor;
}
```

These methods will be called when a pointer enters and exits the attached Game Object. Save the script and return to the editor. Select both buttons and add a **Change Color On Mouse Over** component to each.

Set the alpha value of **Normal Color** to 100 and change its hexadecimal value to **CCCCCC** (a light grey) on both buttons.

Now, do the same for **Hover Color**, but set its hexadecimal colour value to pure white, **FFFFFF**. Next, select just the **Start Button** and drag its child onto the **Model** slot. Do the same for **Quit Button** and play the scene. Hover over the buttons with your cursor and you'll notice they change colour.

### Passing data between scenes

The ability to pass data between scenes can be useful for passing things like game settings from an options menu to the game. For this game, you'll implement the option to change the colour of the hay machine from the title screen.

To start, you need something to click on to make the colour change happen. Create an empty Game Object in the root of the **hierarchy**, name it **Hay Machines** and set its position to **(X:16.5, Y:-0.5, Z:-20)**. This will act as a container for the differently coloured hay machines.

Select all of the hay machines in the **Prefabs/Hay Machine Models** folder and drag them to **Hay Machines**.

Next, with the three hay machines selected in the **hierarchy**, set their **Rotation** to **(X:-90, Y:0, Z:0)**. The hay machines are now upright.

Right now, all of the machines are overlapping so a random gets visibly rendered. The default machine color is blue, so disable the other two by clicking the little checkbox next to their names.

To make the hay machines clickable, they need a collider. Add a **Box collider** to **Hay Machines**, set its **Center** to **(X:0, Y:3.5, Z:0)** and its **Size** to **(X:5, Y:6, Z:8)**.

Create a new folder in the scripts folder, name it **Shared** and create a new C# script inside of it named **HayMachineColor**. Strip out all the `using` statement and both methods. Replace this line:

```csharp
public class HayMachineColor : MonoBehaviour
```

With this:

```csharp
public enum HayMachineColor
```

This script isn't meant to be used as a component, it's actually a simple **enum** that will be used by other scripts. **Enums** are actually just constant integers under the hood (like 0, 1, 2, etc.) but using them makes for a much more approachable and readable way of accessing variables.

Add this inside the `{ }` of the enum:

```csharp
Blue, Yellow, Red
```

Save this file and return to the edtior. Create another C# script inside the **Shared** folder and name it **GameSettings**. Strip out the `using` statements and the methods again. Replace this line:

```csharp
public class GameSettings : MonoBehaviour
```

With this:

```csharp
public static class GameSettings
```

This turns this script into a static class that can hold data for other scripts to use, even across scenes. Any static variables added to this class can be set from the title screen and then read by the game screen for example.

Add this variable declaration to the body of the class:

```csharp
public static HayMachineColor hayMachineColor = HayMachineColor.Blue;
```

This variable will be used to set and get the colour of the hay machine. It uses the **enum** you just created for readability.

Save the script and return to the editor. Create *another* C# script in shared and name it **HayMachineSwitcher**.

Add these `using` statements below the others:

```csharp
using UnityEngine.EventSystems;
using System;
```

And replace this line:

```csharp
public class HayMachineSwitcher : MonoBehaviour
```

With this:

```csharp
public class HayMachineSwitcher : MonoBehaviour, IPointerClickHandler
```

Now add these variables:

```csharp
public GameObject blueHayMachine;
public GameObject yellowHayMachine;
public GameObject redHayMachine;

private int selectedIndex;
```

The Game Objects are each of the hay machines you added earlier. The next is an index that will be incremented every time the hay machine selector is clicked.

Remove the `Start` and `Update` methods and add this to the script:

```csharp
public void OnPointerClick(PointerEventData eventData) 
{
    selectedIndex++; 
    selectedIndex %= Enum.GetValues(typeof(HayMachineColor)).Length; 

    GameSettings.hayMachineColor = (HayMachineColor)selectedIndex; 

    // 5
    switch (GameSettings.hayMachineColor)
    {
        case HayMachineColor.Blue:
            blueHayMachine.SetActive(true);
            yellowHayMachine.SetActive(false);
            redHayMachine.SetActive(false);
        break;

        case HayMachineColor.Yellow:
            blueHayMachine.SetActive(false);
            yellowHayMachine.SetActive(true);
            redHayMachine.SetActive(false);
        break;

        case HayMachineColor.Red:
            blueHayMachine.SetActive(false);
            yellowHayMachine.SetActive(false);
            redHayMachine.SetActive(true);
        break;
    }
}
```

This method first increments **selectedIndex**, and then performs a calculation using the `%` (or **modulo**) operator. This is like a **division** (`/`), but instead of returning the **quotient**, the modulo returns the **remainder** of the operation. It is used all the time in programming to create a 'loop' of numbers. Think about a really simply division pattern using 2. 

```
1 / 2 = 0 (remainder of 1)
2 / 2 = 1 (remainder of 0)
3 / 2 = 1 (remainder of 1)
4 / 2 = 2 (remainder of 0)
5 / 2 = 2 (remainder of 1)
6 / 2 = 3 (remainder of 0)
```

Notice how the remainder ping-pongs between 1 and 0? We can infinitely increment the number on the left and always return either a 1 or 0 by focussing on the remainder (modulo).

This is a very common practice for accessing items sequentially in a List, and returning to the start of the List once you've overshot the end.

Once we have this value (our index) we can evaluate it in a switch statement against our enum values "Blue", "Yellow" and "Red". Remember, enums are just 'labels' for numbers... so each colour name (e.g. "Blue") is actually a number (0, 1 or 2). Using enums means we don't need to try to remember which colour relates to which number. Depending on the enum value, we will enable the appropriately coloured hay machine, and disable the other two!

Save the script and return to the editor. Select **Hay Machines**, add a **Hay Machine Switcher** component and drag the hay machine children onto their matching slots.

Play the scene and click the hay machine a few times - it should switch colours!

We still need to edit the **HayMachine** script to remember which colour we select at the title screen to use in-game. Open the **HayMachine** script and add the following variables below the others:

```csharp
public Transform modelParent; 

public GameObject blueModelPrefab;
public GameObject yellowModelPrefab;
public GameObject redModelPrefab;
```

And add the following method below `Start`:

```csharp
private void LoadModel()
{
    Destroy(modelParent.GetChild(0).gameObject); 

    switch (GameSettings.hayMachineColor) 
    {
        case HayMachineColor.Blue:
            Instantiate(blueModelPrefab, modelParent);
        break;

        case HayMachineColor.Yellow:
            Instantiate(yellowModelPrefab, modelParent);
        break;

        case HayMachineColor.Red:
            Instantiate(redModelPrefab, modelParent);
        break;
    }
}
```

This replaces the default model with a new one depending on the machine colour saved in the game settings. First, the current model is destroyed, and then a new hay machine is instantiated. 

Finally, add this line to `Start`:

```csharp
LoadModel();
```

Now, save the script and return to the editor. Make sure the **Title** scene is saved. Open the **Game** scene, select **Hay Machine** and expand it in the **hierarchy**. The **Hay Machine** component has some extra fields to fill in now: drag **Model Parent** to the **Model Parent** slot and drag the prefabs found in **Prefabs/Hay Machine Models** to their corresponding slots.

Save the **Game** scene. Open the **Title** scene and press play. Switch the colour of the hay machine and press the **Start** button - the hay machine should be a different colour in-game!
