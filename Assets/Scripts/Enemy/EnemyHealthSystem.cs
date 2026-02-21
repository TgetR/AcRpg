using UnityEngine;

public class EnemyHealthSystem : MonoBehaviour
{
    public int LocationNumber = 0;
    public int HP = 10;
    [SerializeField] private bool _IsArenaUnit = false;
    [SerializeField] private int xpReach = 50;
    [SerializeField] private int goldReach = 35;
    [SerializeField] private QuestChecker questChecker;
    private ArenaSpawner _arenaSpawner;

    private StatsManager _Smanager;
    private KeySystemController _KeySystem;
    private ArenaCounter _arenaCounter;
    
    void Start()
    {
        if (_IsArenaUnit)
        {
            _arenaCounter = GameObject.FindGameObjectWithTag("ArenaCounter").GetComponent<ArenaCounter>();
            _arenaSpawner = _arenaCounter.transform.parent.GetChild(1).GetComponent<ArenaSpawner>();
        } 

        _Smanager = GameObject.FindGameObjectWithTag("GameController").GetComponent<StatsManager>();
        _KeySystem = GameObject.FindGameObjectWithTag("KeyController").GetComponent<KeySystemController>();
    }

    void Update()
    {
        if (HP <= 0)
        {
            if (_KeySystem.DropNeedCheck()) Instantiate(Resources.Load("Prefabs/Key"), transform.position, Quaternion.identity);

            Destroy(this.gameObject);
            _Smanager.xpCount += xpReach;
            _Smanager.Gold += goldReach;
            if(_IsArenaUnit) _arenaCounter.AddKill(xpReach,goldReach);
            questChecker.AddEnemyKill();
        }
    }

    public void ChangeHealth(int heatlh)
    {
        HP += heatlh;
    }
}
