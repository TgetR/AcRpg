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
    public void BuyHealTicketByGold(int cost)
    {
        statsManager.BuyHeal(0, cost);
        _ticktes = statsManager.HealTickets;
        ticktesCount.text = $"You have: {_ticktes} tickets.";

    }
    public void BuyHealTicketByXp(int cost)
    {
        statsManager.BuyHeal(1, cost);
        _ticktes = statsManager.HealTickets;
        ticktesCount.text = $"You have: {_ticktes} tickets.";
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
