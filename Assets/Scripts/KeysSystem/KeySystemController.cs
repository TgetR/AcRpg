using System.Collections.Generic;
using UnityEngine;

public class KeySystemController : MonoBehaviour
{
    public bool KeysCanUse = false;
    public bool KeyDropEnabled = true;

    [SerializeField] int NumberOfLocations = 1; //How many locations. Starts from 0, but 0 its 1
    [SerializeField] int KillsMinimumForKeys = 2;
    [SerializeField] int KillsMaximumForKeys = 6;
    [SerializeField] float LocationMultiplier = 1.5f;

    private int kills = 0;
    private List<List<EnemyHealthSystem>> _EnemyListsByLocation = new List<List<EnemyHealthSystem>>();
    public bool DropNeedCheck()
    {
        if (KeyDropEnabled)
        {
            kills++;
            if (kills >= KillsMaximumForKeys)
            {
                kills = 0;
                return true; //Drop
            }
            else if (kills < KillsMinimumForKeys)
            {
                return false; //Don't drop
            }
            else
            {
                int number1 = Random.Range(0, 5);
                int number2 = Random.Range(0, 5);
                if (number1 == number2)
                {
                    kills = 0;
                    return true; //Drop
                }
            }
        }
        return false; //Defaut is false   
    }
}
