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
