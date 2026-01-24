using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatsManager : MonoBehaviour
{
    public float Speed = 5;
    public int Health = 100;
    public int MaxHealth = 100;
    public int Damage = 100;
    public int Gold = 100;
    public float xpCount = 0;
    public int HealTickets = 2;
    public enum StatType { Health, Speed, Damage }

    //XP
    private int _xpLvlDefault = 100; //How many xp do you need to fist lvl up;
    private float _xpLvlMultiplier = 1; //Multiplier for lvl up (if = 2 and _xpLvlDefault = 100 you need 200 xp to lvl up)
    private int _LvlUpXpNeed;
    private int _Level = 1;
    //Upgrages
    public bool LimitIsOk = true;
    private int _UpgradePointsAvailable = 0;
    private int _UpgradePointsUsed = 0;
    private int _UpgradePointsUsedLimit = 25;
    //Regular bonus
    private Dictionary<StatType, int> _bonuses = new Dictionary<StatType, int>() 
    {
        { StatType.Health, 30 },
        { StatType.Speed, 2 },
        { StatType.Damage, 5 }
    };

    private OnScreenNotify _notify;
    void Start()
    {
        _notify = GameObject.FindGameObjectWithTag("Notifyer").GetComponent<OnScreenNotify>();
        _LvlUpXpNeed = (int)(_xpLvlDefault * _xpLvlMultiplier * _Level);
    }

    void Update()
    { 
        if (xpCount >= _LvlUpXpNeed) LevelUp();
    }

    void LevelUp()
    {
        xpCount -= _LvlUpXpNeed;
        _Level++;
        _xpLvlMultiplier += 0.1f * _xpLvlMultiplier;
        _notify.Notify("Reached level up!", 1);
        _UpgradePointsAvailable++;
        _LvlUpXpNeed = (int)(_xpLvlDefault * _xpLvlMultiplier * _Level);
        HealTickets++; //give 1 Heal Ticket
    }

    public int GetUpgradePoints()
    {
        return _UpgradePointsAvailable;
    }

    public void AddUpgradePoints(int count, string reason)
    {
        _notify.Notify($"You received {count} Upgrade Point{(count > 1 ? "s" : "")}: {reason}!", 3);
        _UpgradePointsAvailable += count;
    }

    public void UpgradeStat(StatType type)
    {
            if (!TrySpendUpgradePoint()) return;

            int bonus = _bonuses[type];

            switch (type)
            {
                case StatType.Health: MaxHealth += bonus; Health += bonus; break;
                case StatType.Speed: Speed += bonus; break;
                case StatType.Damage: Damage += bonus; break;
            }
    }

    public void UpgradeStat(StatType type, int customBonus)
    {   
        if (!TrySpendUpgradePoint()) return;

        int bonus = customBonus;

        switch (type)
        {
            case StatType.Health: MaxHealth += customBonus; break;
            case StatType.Speed: Speed += customBonus; break;
            case StatType.Damage: Damage += customBonus; break;
        }
    }

    private bool TrySpendUpgradePoint()
    {
        if (!LimitIsOk) return false;
        if (_UpgradePointsAvailable <= 0) return false;

        _UpgradePointsAvailable--;
        _UpgradePointsUsed++;
        
        if (_UpgradePointsUsed >= _UpgradePointsUsedLimit)
            LimitIsOk = false;

        return true;
    }

    public void AddStatBonus(StatType type, int bonus)
    {
        switch (type)
        {
            case StatType.Health: MaxHealth += bonus; break;
            case StatType.Speed: Speed += bonus; break;
            case StatType.Damage: Damage += bonus; break;
        }
    }

    public void AddXpMultiplier(float multi)
    {
        _xpLvlMultiplier += multi;
    }
    public void BuyHeal(int MethodId, int cost)
    {
        switch (MethodId)
        {
            case 0: //Gold
                if (Gold >= cost)
                {
                    Gold -= cost;
                    HealTickets++;
                    _notify.Notify("You bought 1 Heal Ticket!", 1);
                }
                else
                {
                    _notify.Notify("Not enough money!", 4);
                }
                break;
            case 1: //XP
                if (xpCount >= cost)
                {
                    xpCount -= cost;
                    HealTickets++;
                    _notify.Notify("You bought 1 Heal Ticket!", 1);
                }
                else
                {
                    _notify.Notify("Not enough xp!", 4);
                }
                break;
        }
    }
}
