using System;
using System.Collections;
using UnityEngine;

public class StatsManager : MonoBehaviour
{
    public float Speed = 5;
    public int Health = 100;
    public int MaxHealth = 100;
    public int Damage = 100;
    public int Gold = 100;
    public float xpCount = 0;

    //XP
    private int _xpLvlDefault = 100; //How many xp do you need to fist lvl up;
    private float _xpLvlMultiplier = 1; //Multiplier for lvl up (if = 2 and _xpLvlDefault = 100 you need 200 xp to lvl up)
    private int _LvlUpXpNeed;
    private int _Level;
    //Upgrages
    public bool LimitIsOk = true;
    private int _UpgradePointsAvailable = 0;
    private int _UpgradePointsUsed = 0;
    private int _UpgradePointsUsedLimit = 25;
    //Multipliers
    private float _UpgradeMultiPlier = 1f;
    

    private OnScreenNotify _notify;
    void Start()
    {
        _notify = GameObject.FindGameObjectWithTag("Notifyer").GetComponent<OnScreenNotify>();
    }

    void Update()
    {
        _LvlUpXpNeed = (int)(_xpLvlDefault * _xpLvlMultiplier * _Level);
        if (xpCount >= _LvlUpXpNeed)
        {
            xpCount -= _LvlUpXpNeed;
            _Level++;
            _xpLvlMultiplier += 0.1f * _xpLvlMultiplier;
            _notify.Notify("Reached level up!", 1);
            _UpgradePointsAvailable++;
        }
    }
    public int GetUpgradePoints()
    {
        return _UpgradePointsAvailable;
    }

    public void AddUpgradePoints(int count, string reason)
    {
        _notify.Notify("You reached a " + count + " Updagrade Points: " + reason +"!", 3);
        _UpgradePointsAvailable += count;
    }

    //For menu
    public void UpdgradeHealth(int count)
    {
        _UpgradePointsAvailable -= count;
        _UpgradePointsUsed += count;
        if(_UpgradePointsUsed >= _UpgradePointsUsedLimit) LimitIsOk = false;

        for (int i = 0; i < count; i++) MaxHealth = MaxHealth += (int)(50 * _UpgradeMultiPlier);

    }

    public void UpdgradeSpeed(int count)
    {
        _UpgradeMultiPlier *= _UpgradeMultiPlier;
        _UpgradePointsAvailable -= count;
        _UpgradePointsUsed += count;
        if(_UpgradePointsUsed >= _UpgradePointsUsedLimit) LimitIsOk = false;

        for (int i = 0; i < count; i++) Speed += 1 * _UpgradeMultiPlier;
    }
    public void UpdgradeDamage(int count)
    {
        _UpgradeMultiPlier *= _UpgradeMultiPlier;
        _UpgradePointsAvailable -= count;
        _UpgradePointsUsed += count;
        if(_UpgradePointsUsed >= _UpgradePointsUsedLimit) LimitIsOk = false;

        for (int i = 0; i < count; i++) Damage += (int)(20 * _UpgradeMultiPlier);
    }

    public void AddXpMultiplier(float multi)
    {
        _xpLvlMultiplier += multi;
    }
}
