# 01: Introduction to Unity Scripting

## Unity

**Unity** is a **game engine**. A game engine is a powerful piece of software that provides the tools necessary for developers, artists, musicians, designers, and all types of creatives to come together and make amazing video games!

There are many game engines out there, but we have chosen Unity because:
- It's very powerful
- It's extremely well documented
- There are an insane number of resources out there for it
- **$ It's free! $**

## Algorithms

As developers, we are in charge of making our games **interactive**. Without a developer, you might have some pretty art or nice soundtracks, but you won't have a game.

The main way we bring interactivity into a game is by writing **code.**

**What is code!?** Code is, fundamentally, just a **sequence of instructions** that we are telling the computer. These sequences of instructions are referred to as **algorithms**. 

Algorithms power every piece of **interactivity** in a game, from making a character move when you press a button, to deciding the response an NPC is going to give you in Baldur's Gate 3 after you robbed them blind.

## Game Objects

Everything that the player sees and interacts with in your Unity game is a **Game Object**. *Mario*? - Game Object. *Dark Souls boss*? - Game Object. The dialogue screen in *Hades*? - a bunch of Game Objects. 

A Game Object in its purest form is not very exciting, but we can add **code** to a Game Object to make it do pretty much anything. 

Needless to say, you're going to be seeing a lot of Game Objects in the next few weeks.

## Scripts

A **script** is a small collection of code that can be attached to a Game Object inside Unity to perform some functionality. Scripts can:

- Hold variables that can be changed by you or a designer
- Detect player input and react to it
- Use physics
- Change values over time
- Play sound effects
- Solve world hunger (maybe one day)

Unity scripts are written in **C#**. C# is an object-oriented programming language designed by Microsoft. A programming language is like a real language, but instead of using it to talk to people, we use it to talk to computers.

>**Resource:** <https://docs.unity3d.com/Manual/ScriptingSection.html>

## Starter project

Download the **starter code** and open it as a project in Unity.

### Assets

Imagine that you are the director of a film. If your Unity game is a film, the Assets folder is a big old warehouse filled with all of the movie props (assets) you plan to use on set. An **asset** is literally anything that you intend to use in your game; from 3D models to sound effects, and most importantly for us; **scripts**.

Our Asset folder already has a bunch of assets inside it. Here's a quick overview of how those assets are organized:

- **Materials** - A material is applied to a model to change its appearance e.g. a metallic material could make a wall look like its made of steel.
- **Models** - 3D models that are used in the game.
- **Music** - The music in the game.
- **Prefabs** - Pre-fabricated Game Objects that we can use straight out of the box. More on these later.
- **Scenes** - Back to the film director analogy: a scene is like a scene in your film, with the curtains lowered, ready to play. You can split your game into as many or as few scenes as you want. A main menu is an example of something that could be split into its own scene.
- **Scripts** - Where all the magic happens.
- **Sounds** - Sound effect files.
- **Sprites** - 2D image files. In 3D games, these are typically reserved for UI graphical elements.
- **Textures** - An image that can be applied to models via **materials**.

### Scene view

The **scene view** lets us set up our game before it is played. Think of it as a place to get all of the props, actors lights and cameras into position on the scene before we call "action!".

In the scenes folder, there is a scene called **Game**. Double click it to open it up.

Open the **Game** scene. You should notice some objects appear in the **scene view**. (By default this is in the middle of the screen.)

**If you can't see the scene:**
- Select Window -> Layout -> Default from the top dropdown menu.

In the **hierarchy tab** you can see all the scene's Game Objects organised into a tree-like hierarchy. The hierarchy tab lists out all of the objects in the current scene and is linked to the **scene view**. If you select an object in the hierarchy, it will also be selected in the scene view, and vice-versa.

>**Tip:**
- If you're looking for an object in your scene, its usually easier to search the hierarchy than trying to locate the obect in the scene view.

There are currently 5 **Game Objects** in the top level of our **hierarchy**:

- **Music** - An invisible object that plays the background music.
- **Main Camera** - The camera is what the player will see through when they are playing your game (if you want to see what the camera is seeing, check the **game view** tab).
- **Directional Light** - A single light source that illuminates the scene. Scenes in Unity need some sort of light source to be able to see anything when playing the game.
- **Scenery** - An empty Game Object to hold the ground and the windmills. Empty Game Gbjects can be super useful! This one is essentially acting like a 'folder' to keep the hierarchy organized.
- **Hay Machine** - This is the blue machine sitting on the rails. It is comprised of a few Game Objects to make it easy to customise later on.

Press the **play button** to run the game. You'll see that it's not a very exciting game. I guess we have a job to do.

Press the button again to stop the game and return to the scene view.

>**Tip:**
    Any changes you make to your project while the game is playing **WILL NOT BE SAVED. *ALWAYS*** stop playing your game before you make any changes that you intend to keep.

### Inspector View

The **inspector view** shows us all the important information about the Game Object we have currently selected. Let's inspect the windmill wheel.

Drill down to the windmill wheel in the hierarchy view by clicking on the arrow next to Scenery. Go:
Scenery -> Windmill -> Windmill -> Wheel.

Click on the **Wheel** Game Object in the hierarchy and you will notice that the **inspector** window fills up with some information. There are 3 **Components** attached to the Wheel Game Object. These are:

- Transform
- Wheel (Mesh Filter)
- Mesh Renderer

We don't care too much about the second 2 just yet, but we are interested in the **Transform** component.

### Transforms

Every single Game Object has a **transform**. Transforms define crucial spatial coordinates for the Game Object in the scene. A transform is made up of:

- **Position** - Where the object is in 3D space, relative to its parent.
- **Rotation** - How the object has been rotated from its default rotation.
- **Scale** - How much bigger or smaller the object is from its default size. Objects can be scaled on different dimensions.

### Thinking like a developer

It's time for us to make something happen in this game. Let's do something extremely simple: rotate the blades on the windmill.

You'll notice that if you change the y-rotation in the Wheel's transform, the wheel rotates in the scene, as a real windmill wheel might rotate in the wind. This is kinda neat, but obviously we can't be manually rotating the windmill wheel for our players while they are playing our game.

So that begs the question: Could we write a script to perform this rotation for us while the game is playing?

The answer is: **Yes.**

Before we write any code, let's think about what we are trying to do:
- While the game is running, we want to change the y-rotation of the Wheel Game Object over time.

Unity allows us to write code that repeats every single frame of the game (~60 times per second). Could we come up with an instruction that runs 60 times per second and updates the y-rotation of the Wheel to make it smoothly rotate over time? What would that instruction look like?


# **Collaborative Problem Solving Session 1: Make a Wheel Rotate**


## Creating the script

After class discussion, let's test our algorithm.

Create a new script in the **Scripts** folder of your project. (if this folder doesn't exist, make it). You can create scripts by right clicking in the assets view and selecting Create -> C# script. Call it `Rotate`.

Assign the script to the Wheel Game Object by dragging and dropping it from the assets view on to the Wheel Game Object in the hierarchy view.

Double click the script to open it.

`Rotate.cs` will look like this:

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

This is how every new C# Unity script looks.

We're interested in the `Update()` function:

```csharp
void Update() { 

}
```

Whatever code we put inside the `{` curly braces will run every frame that the Game Object this script is attached to is active in the scene.

### Task

Based on the class discussion, add 1 line of code to the `Update()` function that will make the windmill wheel rotate.

The answer will utilize Unity's `Transform.Rotate` method:

>**Resource:** <https://docs.unity3d.com/ScriptReference/Transform.Rotate.html>

**Try to solve this for yourselves first.** Only look at the solution if you are really stuck. After ~10 minutes, I will walk everyone through the solution, so it's not a race.

<details>
<summary>Solution Example</summary>

```csharp
transform.Rotate(0, 50 * Time.deltaTime, 0);
```
</details>

### Variables

Our code works, but there is a problem with it that the untrained eye will likely miss:

This `50` is what programmers like to refer to as a **"magic number"**. What makes it magic? 
- It just appears out of thin air with no explanation.

If someone else was to read this code, they wouldn't know why we used the number `50`, nor what the `50` was supposed to represent (although an experienced programmer could work it out.) It's also difficult to change. If someone (such as a designer) wants to make the windmill wheels rotate faster or slower, they have to go into our code and change this specific number. A non-programmer is **never** going to do that.

Instead of using a magic number in our code, let's store the number in a **variable**.

Why use a variable?
- **Variables** allow you to assign names to values. This makes your code much easier to read.
- **Single source of truth** for values. If you wrote `50` for the windmill speed in multiple places across multiple scripts, you'd have many instances of the number that you need to update. If `50` is stored in a variable, it only needs to be updated in one place.
- **Mutability** - A fancy word for "something that can be changed". Putting the `50` in a variable allows it to be changed at runtime, meaning your windmill could speed up or slow down based on what's happening in the game.

We can declare a new variable on the line above `Start()`:

```csharp
public float RotationSpeed = 50f;
```

We'll break down exactly what each of these terms means later. For now, just know that we've created a variable called `RotationSpeed` with a default value of `50`.

Look at the inspector view with your **Wheel Game Object** selected. You will see that under the **Rotate Component**, there is a new editable text field called `Rotation Speed`. This is the variable we just created, and it's editable directly in Unity. Cool!

Back in `Rotate.cs`, change the line we wrote in `Update()` to use the `RotationSpeed` variable instead of a magic number.

Play around with different values in this field and see how they affect the windmill when the game is playing.

## Lesson takeaways

 - Unity is a powerful game engine
- The main views you will use while working are the scene, hierarchy, project, and inspector views. Get familiar with them.
- We can write scripts to change things in our scene while the game is running.
- We can use variables to make our code cleaner and our components usable for non-programmers.