using UnityEngine;

public class ArenaSpawner : MonoBehaviour
{
    public int EnemyCount = 0;
    public GameObject EnemyPrefab;
    public float SpawnDelay = 5;
    public float MaxumimSpawned = 10;
    public bool IsActive = false;
    //Spawn Range
    [SerializeField] private float centerX;
    [SerializeField] private float centerY;
    [SerializeField] private Vector2 MaxRadius;
    void Start()
    {
        InvokeRepeating("SpawnEnemy", SpawnDelay, SpawnDelay);
        centerX = transform.position.x;
        centerY = transform.position.y;
    }
    void SpawnEnemy()
    {
        Vector2 SpawnPos = new Vector2(centerX, centerY);
        if (EnemyCount < MaxumimSpawned && IsActive)
        {
            Instantiate(EnemyPrefab, SpawnPos, Quaternion.identity);
        }
    }

    public void ActivateSpawner()
    {
        IsActive = true;
    }
    public void DisableSpawner()
    {
        IsActive = false;
    }
}
