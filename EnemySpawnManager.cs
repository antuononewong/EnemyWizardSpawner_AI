using UnityEngine;

public class EnemySpawnManager : MonoBehaviour
{
    // Spawn points
    public GameObject[] leftSpawnPoints;
    public GameObject[] rightSpawnPoints;

    // Enemy sprites
    public GameObject[] enemyPrefab;

    public GameObject target;
    
    // Enemy/projectile attributes
    private int spawnPointCount;
    private int attackPatternCount = 3;
    private int projectilePrefabCount;
    private float projectileSpeed = 5f;

    // Enemy spawn rate
    private float maxSpawnTimer = 1.5f;
    private float spawnTimer;

    void Start()
    {
        spawnPointCount = leftSpawnPoints.Length;
        projectilePrefabCount = enemyPrefab.Length;
    }

    void FixedUpdate()
    {
        spawnTimer -= Time.deltaTime;

        if (spawnTimer < 0)
        {
            SpawnEnemy();
            spawnTimer = maxSpawnTimer;
        }
    }

    // Randomly generate the location, attack type, and projectile type of an enemy wizard. Creates
    // said enemy with the randomized traits.
    private void SpawnEnemy()
    {
        int side = Random.Range(0, 2);
        int spawnPoint = Random.Range(0, spawnPointCount);
        int attackPattern = Random.Range(1, attackPatternCount + 1);
        int food = Random.Range(0, projectilePrefabCount);

        GameObject enemy = Instantiate<GameObject>(enemyPrefab[food]);
        enemy.GetComponent<WizardController>().SetBaseAttributes(target, projectileSpeed, attackPattern, food, side);

        if (side == 0)
        {
            enemy.transform.position = leftSpawnPoints[spawnPoint].transform.position;
        }
        else
        {
            enemy.transform.position = rightSpawnPoints[spawnPoint].transform.position;
        }
        
    }
}
