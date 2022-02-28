# 02: Game Mechanics - Enemy behaviour

So we have some basic enemy behaviour, but nothing really happens - in this section, we will implement more enemy behaviour, such as **enemy waves**, and **decreasing player health**.

## Rotating the enemies

First, we need to rotate the enemies as they follow the waypoints. Open the **MoveEnemy** script and add the following method:

```csharp
private void RotateIntoMoveDirection()
{
  Vector3 newStartPosition = waypoints [currentWaypoint].transform.position;
  Vector3 newEndPosition = waypoints [currentWaypoint + 1].transform.position;
  Vector3 newDirection = (newEndPosition - newStartPosition);

  float x = newDirection.x;
  float y = newDirection.y;
  float rotationAngle = Mathf.Atan2 (y, x) * 180 / Mathf.PI;

  GameObject sprite = gameObject.transform.Find("Sprite").gameObject;
  sprite.transform.rotation = Quaternion.AngleAxis(rotationAngle, Vector3.forward);
}
```

This method basically makes the enemy bugs "face forward" depending on the direction they are moving. It calculates the bug’s current movement direction by subtracting the current waypoint’s position from that of the next waypoint. It uses `Mathf.Atan2` to determine the angle toward which `newDirection` points in radians - **zero** points to the right. We then multiply this radian result by `180 / Mathf.PI` to convert the angle to degrees. We can use this degrees result to rotate the bug along the **z axis** (note: we actually rotate the **Sprite** (or graphic/image) of the bug, rather than the entire Game Object - this is because later we will implement a **health bar**, which we do not want to rotate.

In `Update` replace this:

```csharp
// TODO: Rotate into move direction
```

With this:

```csharp
RotateIntoMoveDirection();
```

Save the file and return to the editor. Play the scene and the bug should face the correct direction as it makes its way around the map.

## Enemy waves

