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
    public int HealTickets = 2;

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
    private int _RegularBonusSpeed = 2;
    private int _RegularBonusHealth = 30;
    private int _RegularBonusDamage = 5;
    

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

    public void UpgradeStat(int statID)
    {
        int bonus = statID switch
        {
            0 => _RegularBonusHealth,
            1 => _RegularBonusSpeed,
            2 => _RegularBonusDamage,
            _ => 0
        };
        UpgradeStat(statID, bonus);
    }

    public void UpgradeStat(int statID, int customBonus)
    {
        if (!TrySpendUpgradePoint()) return;

        switch (statID)
        {
            case 0: MaxHealth += customBonus; break;
            case 1: Speed += customBonus; break;
            case 2: Damage += customBonus; break;
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

    public void AddStatBonus(int statID, int bonus)
    {
        switch (statID)
        {
            case 0: MaxHealth += bonus; break;
            case 1: Speed += bonus; break;
            case 2: Damage += bonus; break;
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
