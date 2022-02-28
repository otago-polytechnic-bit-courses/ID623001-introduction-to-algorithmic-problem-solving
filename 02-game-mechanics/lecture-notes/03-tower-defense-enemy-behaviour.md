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

Before launching a wave of enemies at the player, we are going to provide a heads up that a wave is coming. Open the **GameManagerBehaviour** script and add these two variables:

```csharp
public Text waveLabel;
public GameObject[] nextWaveLabels;
```

The `waveLabel` stores a reference to the wave readout at the top right corner of the screen. 

`nextWaveLabels` stores two halves of an animated message to the player - they will slide in from opposite sides of the screen and form a message in the middle.

Save the file and return to the editor. Select **GameManager** in the **hierarchy**. Click on the small circle to the right of **Wave Label** and in the **Select Text** dialog select **WaveLabel**. Now set the **Size** of **Next Wave Labels** to 2 and assign **Element 0** to **NextWaveBottomLabel** and **Element 1** to **NextWaveTopLabel** in the same way as you just did for **Wave Label**.

Switch back to the **GameManagerBehaviour** script and add another variable:

```csharp
public bool gameOver = false;
```

This variable stores whether the player has lost the game. Add the following code to keep track of the waves:

```csharp
private int wave;
public int Wave
{
  get { return wave; }
  set
  {
    wave = value;
    if (!gameOver)
    {
      for (int i = 0; i < nextWaveLabels.Length; i++)
      {
          nextWaveLabels[i].GetComponent<Animator>().SetTrigger("nextWave");
      }
    }
    waveLabel.text = "WAVE: " + (wave + 1);
  }
}
```

This is similar to the **getters** and **setters** we've written before. Here, when we set the wave to a new value, we also check if the game is over: if not, we trigger the "Next Wave" animation, and update the player's UI. The `(wave + 1)` is because we start at 0 in our code, but the player should think of the first wave as wave 1, not 0.

In `Start` set the value:

```csharp
Wave = 0;
```

Save the file and return to the editor. If you play the scene now, you should see the player UI update with wave information.

### Spawning multiple enemies in a wave

Select the **Enemy** prefab and at the top of the Inspector click on the **Tag** dropdown and select **Add Tag**. Create a new tag called **Enemy**. Select the **Enemy** prefab again and set its **Tag** to **Enemy**.

To define a wave of enemies, open the **SpawnEnemy** script and add the following class before `SpawnEnemy`:

```csharp
[System.Serializable]
public class Wave
{
  public GameObject enemyPrefab;
  public float spawnInterval = 2;
  public int maxEnemies = 20;
}
```

`Wave` holds a reference to `enemyPrefab`, the basis for instantiating all enemies in that wave, a `spawnInterval` which is the time between enemies in the wave in seconds, and the `maxEnemies`, which is the quantity of enemies spawning in that wave.

This class is also marked `[System.Serializable]`, which means you can change the values in the Inspector.

Add the following variables to the `SpawnEnemy` class:

```csharp
public Wave[] waves;
public int timeBetweenWaves = 5;

private GameManagerBehaviour gameManager;

private float lastSpawnTime;
private int enemiesSpawned = 0;
```

- `waves` is an array of multiple `Wave` instances.
- `timeBetweenWaves` is the time in seconds before the next wave spawns.
- `lastSpawnTime` will be used to track when the last wave spawned, so we can count down to the next wave.
- `enemiesSpawned` will be used to track how many enemies spawned in this wave.

Replace the contents of `Start` with:

```csharp
lastSpawnTime = Time.time;
gameManager = GameObject.Find("GameManager").GetComponent<GameManagerBehaviour>();
```

We set `lastSpawnTime` to the current time that the script initialises, and find the `GameManagerBehaviour` so we can use it later.

Add this code to `Update`:

```csharp
int currentWave = gameManager.Wave;
if (currentWave < waves.Length)
{
    float timeInterval = Time.time - lastSpawnTime;
    float spawnInterval = waves[currentWave].spawnInterval;
    if (((enemiesSpawned == 0 && timeInterval > timeBetweenWaves) || (timeInterval > spawnInterval)) && 
    (enemiesSpawned < waves[currentWave].maxEnemies))
    {
        lastSpawnTime = Time.time;
        GameObject newEnemy = (GameObject)Instantiate(waves[currentWave].enemyPrefab);
        newEnemy.GetComponent<MoveEnemy>().waypoints = waypoints;
        enemiesSpawned++;
    }
    if (enemiesSpawned == waves[currentWave].maxEnemies && GameObject.FindGameObjectWithTag("Enemy") == null)
    {
        gameManager.Wave++;
        gameManager.Gold = Mathf.RoundToInt(gameManager.Gold * 1.1f);
        enemiesSpawned = 0;
        lastSpawnTime = Time.time;
    }
}
else
{
    gameManager.gameOver = true;
    GameObject gameOverText = GameObject.FindGameObjectWithTag("GameWon");
    gameOverText.GetComponent<Animator>().SetBool("gameOver", true);
}
```

- The first two lines get the current wave and check if it is the last wave or not - if not, the next code block will fire.
- Next, we calculate the time passed from the `lastSpawnTime` and now - we then use that to determine if it is time to spawn a new enemy or not.
- The first half of the `if` statement is: If `enemiesSpawned == 0` **AND** the time passed is greater than the `timeBetweenWaves`, that means it's time for a new wave! **OR** if the time passed is greater than the `spawnInterval` (here, because the first condition failed, it is implied the `enemiesSpawned != 0`) that means it is time to spawn a new enemy in this wave.
- The second half says you **must also** have fewer than the `maxEnemies` spawned for this wave. So, basically, if we haven't spawned all the enemies, and the right time has passed between spawns (or waves, for a new wave), spawn an enemy!
- We first update `lastSpawnTime` to the current time (so the countdown can start for the next spawn)... Then `Instantiate` a new enemy from this wave's info, set its waypoints dynamically, and increment the `enemiesSpawned` count.
- The next `if` statement will check if the wave is 'over' - for this to happen, two conditions need to be met: first, that the number of `enemiesSpawned` matches the `maxEnemies` for this wave (i.e. we've spawned all our enemies) **AND** all the enemies in the game have been destroyed (that all **Enemy** GameObjects `== null`).
- If this is the case, increment the `Wave` count, increase the player's `Gold` (here, by multiplying their current total by 1.1), reset the `enemiesSpawned` count, and set the `lastSpawnTime` for the next 'countdown'.
- The final `else` here is from the first condition: that the current wave **is not less than** (i.e. equals) the final wave count... in this case, the game is over. Set the `gameOver` flag to `true`, show the Game Over text to the player, and set a flag on the animator.

