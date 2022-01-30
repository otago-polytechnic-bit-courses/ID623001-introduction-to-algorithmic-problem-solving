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
