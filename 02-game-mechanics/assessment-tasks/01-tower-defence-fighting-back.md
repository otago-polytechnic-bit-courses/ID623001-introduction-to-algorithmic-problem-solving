# 01: Game Mechanics assessment tasks - Fighting back

In this section we will set up our defence monsters to fire at the bugs as they march towards the cookie.

## Enemy health bar

First, we need to add some sort of health indicator to the enemy bugs. Drag **Enemy** from the prefabs folder onto the scene; then drag from **Images\Objects\HealthBarBackground** onto the **Enemy** in the **hierarchy** to add it as a child. Set the position for the **HealthBarBackground** to **(X:0, Y:1, Z:-4)**.

Next, select **Images\Objects\HealthBar** and ensure its **Pivot** is set to **Left**. Then add it as a child to **Enemy** in the **hierarchy** and set its position to **(X:-0.63, Y:1, Z:-5)**. Set its **X Scale** to 125.

Create a new C# script called **HealthBar** and add it as a component to the **HealthBar** Game Object. Select **Enemy** in the **hierarchy** and change its position to **(X:20, Y:0, Z:0)**. Click on the **Overrides** near the top of the Inspector and click **Apply All** to update all the prefabs. Delete the **Enemy** from the **hierarchy**.

Run the scene - you should see all the enemy bugs now with health bars above them.

Open the **HealthBar** script and add the following variables:

```csharp
public float maxHealth = 100;
public float currentHealth = 100;
private float originalScale;
```

`maxHealth` stores the enemy's maximal health points, and `currentHealth` tracks how much health remains. Lastly, `originalScale` remembers the health bar's original size. In `Start` add this line to remember the health bar's initial size:

```csharp
originalScale = gameObject.transform.localScale.x;
```

In `Update` add the following lines of code:

```csharp
Vector3 tmpScale = gameObject.transform.localScale;
tmpScale.x = currentHealth / maxHealth * originalScale;
gameObject.transform.localScale = tmpScale;
```

This calculates the scale that the health bar should be based on the `currentHealth` / `maxHealth`.

## Targeting enemies

Select **Monster** in the prefabs folder and add a **Circle collider 2D** component to it. Check **Is Trigger** and set the collider's **Radius** to 2.5 - this is the monsters' firing range.

Finally, at the top of the Inspector, set Monster's **Layer** to **Ignore Raycast**. Click **Yes, change children** in the dialog. If you don't ignore raycasts, the collider reacts to click events, which is a problem because the **Monster** will block events meant for the **Openspot** below.

Select **Enemy** in the prefabs folder and add a **Rigidbody 2D** component to it. Set **Body Type** to **Kinematic**.

Add a **Circle collider 2D** with a **Radius** of 1.

Create a new C# script named **ShootEnemies** and add it to the **Monster** prefab. Add this variable:

```csharp
public List<GameObject> enemiesInRange;
```

This list will contain all the enemies currently in range to the monster. Initialise the list in `Start`:

```csharp
enemiesInRange = new List<GameObject>();
```

Add this code to the script to add and remove enemies to the list:

```csharp
void OnTriggerEnter2D (Collider2D other)
{
    if (other.gameObject.tag.Equals("Enemy"))
        enemiesInRange.Add(other.gameObject);
}

void OnTriggerExit2D (Collider2D other)
{
    if (other.gameObject.tag.Equals("Enemy"))
        enemiesInRange.Remove(other.gameObject);
}
```

### Target switching

If there are multiple enemies within range, your monster should focus on the one closest to the cookie... open the **MoveEnemy** script and add this new method to calculate the enemy's distance from the goal:

```csharp
public float DistanceToGoal()
{
  float distance = Vector2.Distance(gameObject.transform.position, waypoints[currentWaypoint + 1].transform.position);
  for (int i = currentWaypoint + 1; i < waypoints.Length - 1; i++)
  {
      Vector3 startPosition = waypoints[i].transform.position;
      Vector3 endPosition = waypoints[i + 1].transform.position;
      distance += Vector2.Distance(startPosition, endPosition);
  }
  return distance;
}
```

This code calculates the length of road not yet traveled by the enemy. It does so using `Vector2.Distance`, which calculates the difference between two vectors.

## Bullets

Drag and drop **Images\Objects\Bullet1** onto the scene. Set the **Z** position to -2; the **X** and **Y** don't matter, so leave them at whatever. We are setting the **Z** position so the bullet will appear behind the Monster firing it, but in front of enemies.

Add a **Circle collider 2D** and check **Is Trigger**.

Create a new C# script named **BulletBehaviour** and add it to the **Bullet1** Game Object. Add these variables to the script:

```csharp
public float speed = 10;
public int damage;
public GameObject target;
public Vector3 startPosition;
public Vector3 targetPosition;
private Vector3 normalizeDirection;
private GameManagerBehavior gameManager;
```

- `speed` determines how quickly the bullets fly.
- `damage` is how much health the bullet will take off an enemy.
- `target`, `startPosition`, and `targetPosition` determine the bullet's direction - we'll refer to an enemy as `target` and then use its position to determine the direction vector from the monster.
- `normalizeDirection` is used to standardise the vectors - if we didn't do this, than a bullet shot at a closer enemy would move faster than one further away. 
- `gameManager` will be used to increase the player's **Gold** when an enemy is destroyed.

Assign start values to these variables in `Start`:

```csharp
normalizeDirection = (targetPosition - startPosition).normalized;
GameObject gm = GameObject.Find("GameManager");
gameManager = gm.GetComponent<GameManagerBehaviour>();
```

When a new **bullet** is instantiated, normalize the difference between `targetPosition` and `startPosition` to get a standard 'direction' vector. Also, grab a reference to the `GameManagerBehaviour`.

Add this code to `Update`:

```csharp
transform.position += normalizeDirection * speed * Time.deltaTime; 
```

This updates the bullet's position along the normalized vector, according to the `speed` variable.

Now add this method to the script:

```csharp
void OnTriggerEnter2D(Collider2D other)
{
    target = other.gameObject;
    if(target.tag.Equals("Enemy"))
    {
        Transform healthBarTransform = target.transform.Find("HealthBar");
        HealthBar healthBar = healthBarTransform.gameObject.GetComponent<HealthBar>();
        healthBar.currentHealth -= damage;

        if (healthBar.currentHealth <= 0)
        {
            Destroy(target);
            AudioSource audioSource = target.GetComponent<AudioSource>();
            AudioSource.PlayClipAtPoint(audioSource.clip, transform.position);
            gameManager.Gold += 50;
        }  
        Destroy(gameObject);
    }
}
```

This checks for a collision with an enemy (note, with the way this code is currently set up, the enemy hit by the bullet does not have to be the same as was fired at initially - this code will hit any enemy - you may or may not want this behaviour in an actual game, and you would adapt the code accordingly).

Find the health bar in the hit enemy, and reduce the health by the damage amount. 

If the health is less than or equal to 0, destroy the enemy. Also play a sound effect and increase the player's gold by 50.

Finally, destroy the bullet.

Save the script and return to the editor.

### Bigger bullets

Drag and drop **Bullet1** from the **hierarchy** to the prefabs folder to create a new prefab. Delete the **Bullet1** from the scene.

Create two copies of the **Bullet1** prefab and rename them **Bullet2** and **Bullet3**. Select **Bullet2** and in the inspector, set the **Sprite Renderer** component's **Sprite** field to **Images\Objects\Bullet2**. Repeat the process for **Bullet3**.

Now change the damage done by each bullet by selecting each one, and changing the **Damage** property in the **BulletBehaviour** component:

- **Bullet1** : 10
- **Bullet2** : 15
- **Bullet3** : 20

Open the **MonsterData** script and add these variables to `MonsterLevel`:

```csharp
public GameObject bullet;
public float fireRate;
```

Select the **Monster** prefab and in the inspector, expand **Levels** in the **Monster Data** component. Set **Fire rate** to 1 for each element, and set the **Bullet** property to **Bullet1**, **Bullet2** and **Bullet3** for each element respectively.

Open the **ShootEnemies** script and add these variables:

```csharp
private float lastShotTime;
private MonsterData monsterData;
```

`lastShotTime` keeps track of when the last bullet fired (for ensuring Monsters don't fire too quickly/slowly), and `MonsterData` is a reference to th bullet type, fire rate, etc...

Add this to `Start` to assign the starting values:

```csharp
lastShotTime = Time.time;
monsterData = gameObject.GetComponentInChildren<MonsterData>();
```

Add the following method:

```csharp
void Shoot(Collider2D target)
{
    GameObject bulletPrefab = monsterData.CurrentLevel.bullet;
    Vector3 startPosition = gameObject.transform.position;
    Vector3 targetPosition = target.transform.position;
    startPosition.z = bulletPrefab.transform.position.z;
    targetPosition.z = bulletPrefab.transform.position.z;

    GameObject newBullet = (GameObject)Instantiate(bulletPrefab);
    newBullet.transform.position = startPosition;
    BulletBehaviour bulletComp = newBullet.GetComponent<BulletBehaviour>();
    bulletComp.target = target.gameObject;
    bulletComp.startPosition = startPosition;
    bulletComp.targetPosition = targetPosition;

    Animator animator = monsterData.CurrentLevel.visualization.GetComponent<Animator>();
    animator.SetTrigger("fireShot");
    AudioSource audioSource = gameObject.GetComponent<AudioSource>();
    audioSource.PlayOneShot(audioSource.clip);
}
```

Get the start and target positions of the bullet. Set the **z-Position** to that of the `bulletPrefab`. Instantiate a new bullet using the `bulletPrefab` for `MonsterLevel`. Assign the `startPosition` and `targetPosition` of the bullet. Run a shoot animation and play a laser sound whenever the monster shoots.

Finally, add this code to `Update`:

```csharp
GameObject target = null;
float minimalEnemyDistance = float.MaxValue;
foreach (GameObject enemy in enemiesInRange)
{
    float distanceToGoal = enemy.GetComponent<MoveEnemy>().DistanceToGoal();
    if (distanceToGoal < minimalEnemyDistance)
    {
        target = enemy;
        minimalEnemyDistance = distanceToGoal;
    }
}

if (target != null)
{
    if (Time.time - lastShotTime > monsterData.CurrentLevel.fireRate)
    {
        Shoot(target.GetComponent<Collider2D>());
        lastShotTime = Time.time;
    }
    Vector3 direction = gameObject.transform.position - target.transform.position;
    gameObject.transform.rotation = Quaternion.AngleAxis(Mathf.Atan2 (direction.y, direction.x) * 180 / Mathf.PI, new Vector3 (0, 0, 1));
}
```

- Determine the target of the monster. Start with the maximum possible distance in the `minimalEnemyDistance`. Iterate over all enemies in range and make an enemy the new target if its distance to the cookie is smaller than the current minimum.
- Call `Shoot` if the time passed is greater than the fire rate of your monster and set `lastShotTime` to the current time.
- Calculate the rotation angle between the monster and its target. You set the rotation of the monster to this angle. Now it always faces the target.

Save the script and return to the editor. Play the scene and place some Monsters - they should attack the enemies as they make their way to the cookie!
