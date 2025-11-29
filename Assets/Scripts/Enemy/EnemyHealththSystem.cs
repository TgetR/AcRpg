using UnityEngine;

public class EnemyHealthSystem : MonoBehaviour
{
    public int LocationNumber = 0;
    public int HP = 10;
    private StatsManager _Smanager;
    private KeyDropController _KeySystem;
    
    void Start()
    {
        _Smanager = GameObject.FindGameObjectWithTag("GameController").GetComponent<StatsManager>();
        _KeySystem = GameObject.FindGameObjectWithTag("KeyController").GetComponent<KeyDropController>();
    }

    void Update()
    {
        if (HP <= 0)
        {
            if (_KeySystem.DropNeedCheck()) Instantiate(Resources.Load("Prefabs/Key"), transform.position, Quaternion.identity);

            Destroy(this.gameObject);
            _Smanager.xpCount += 50;
            _Smanager.Gold += 35;
        }
    }

    public void ChangeHealth(int heatlh)
    {
        HP += heatlh;
    }
}
