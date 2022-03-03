# 01: Game Mechanics assessment tasks - Fighting back

In this section we will set up our defence monsters to fire at the bugs as they march towards the cookie.

## Enemy health bar

First, we need to add some sort of health indicator to the enemy bugs. Drag **Enemy** from the prefabs folder onto the scene; then drag from **Images\Objects\HealthBarBackground** onto the **Enemy** in the **hierarchy** to add it as a child. Set the position for the **HealthBarBackground** to **(X:0, Y:1, Z:-4)**.

Next, select **Images\Objects\HealthBar** and ensure its **Pivot** is set to **Left**. Then add it as a child to **Enemy** in the **hierarchy** and set its position to **(X:-0.63, Y:1, Z:-5)**. Set its **X Scale** to 125.

Create a new C# script called **HealthBar** and add it as a component to the **HealthBar** Game Object.
