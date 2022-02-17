# 01: Game Mechanics - Tower defense introduction

## Starter project

Download the **starter code** and open it as a project in Unity.

## Tower defense

We are going to build a 2D tower defense game, and implement functionality such as **gaining and using resources**, **upgrading units**, and rudimentary **enemy wayfinding**.

Open the **GameScene** from the scenes folder and set the Game view aspect ratio to **4:3** - this will ensure the labels and background line up correctly. The object is to place monsters in defensive positions around the path to stop the bugs from reaching the cookie at the end. You will earn gold as you play and use it to ugrade the monsters, defeating multiple waves of enemy bugs!

### Placing units

The first step is to set up the functionality to place your defensive monsters. Monsters can only be places on the spots marked with an **X**. Drag and drop **Images\Objects\Openspot** into the Scene view. Select **Openspot** in the **hierarchy tab** and add a **Box Collider 2D** component to it. Next add an **Audio\Audio Source** component to **Openspot** and set the Audio Source's **AudioClip** to **Audio\tower_place**. Deactivate **Play On Awake**.

Create a prefab from what you just created: drag and drop **Openspot** from the **hierarchy** to the **prefabs** folder. You can now create more placement spots using the prefab. Drag 11 more spots into the scene (don't worry about their position now, we'll adjust those in a minute). You should have a total of 12 placement spots in the **hierarchy**.
