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

### Sound Effects
