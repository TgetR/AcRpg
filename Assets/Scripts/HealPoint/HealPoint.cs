using TMPro;
using UnityEngine;

public class HealPoint : MonoBehaviour
{
    [SerializeField] GameObject healPointMenu;
    [SerializeField] StatsManager statsManager;
    [SerializeField] TMP_Text ticktesCount;
    [SerializeField] OnScreenNotify notifier;
    private int _ticktes;

    void Start()
    {
        _ticktes = statsManager.HealTickets;
        ticktesCount.text = $"You have: {_ticktes} tickets.";
    }
    public void HealByGold(int cost)
    {
        if(statsManager.Gold >= cost)
        {
            statsManager.Gold -= cost;
            statsManager.Health = statsManager.MaxHealth;
            notifier.Notify("You health now full!", 1);
            _ticktes = statsManager.HealTickets;
            ticktesCount.text = $"You have: {_ticktes} tickets.";
        }
        else
        {
            notifier.Notify("Not enought money!", 4);
        }

    }
    public void HealByXp(int cost)
    {
        if(statsManager.xpCount >= cost)
        {
            statsManager.xpCount = statsManager.xpCount - cost;
            statsManager.Health = statsManager.MaxHealth;
            notifier.Notify("You health now full!", 1);
            _ticktes = statsManager.HealTickets;
            ticktesCount.text = $"You have: {_ticktes} tickets.";
        }
        else
        {
            notifier.Notify("Not enought xp!", 4);
        }
    }
    public void HealByTicket()
    {
        if(statsManager.HealTickets > 0)
        {
            statsManager.HealTickets--;
            statsManager.Health = statsManager.MaxHealth;
            notifier.Notify("You health now full!", 1);
            _ticktes = statsManager.HealTickets;
            ticktesCount.text = $"You have: {_ticktes} tickets.";
        }
        else
        {
            notifier.Notify("You don't have Heal Tickets!", 4);
            _ticktes = statsManager.HealTickets;
            ticktesCount.text = $"You have: {_ticktes} tickets.";
        }
    }
    public void Close()
    {
        _ticktes = statsManager.HealTickets;
        ticktesCount.text = $"You have: {_ticktes} tickets.";
        healPointMenu.SetActive(false);
        Time.timeScale = 1;
    }
}
