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

Save the script and return to the editor. Select **Quit Button**, add a **Quit Button** component to it and run the scene. Try clicking the **Quit** button and you'll notice nothing happens. This is because `Application.Quit` only works in an actual build of the game, not inside the Unity software.
