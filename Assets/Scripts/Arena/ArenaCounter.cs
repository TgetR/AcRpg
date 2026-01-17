using TMPro;
using UnityEngine;

public class ArenaCounter : MonoBehaviour
{
    [SerializeField] TMP_Text killsCounter;
    [SerializeField] TMP_Text xpCounter;
    [SerializeField] TMP_Text goldCounter;
    [SerializeField] GameObject player;
    [SerializeField] ArenaSpawner ArenaSpawner;
    private Animator _animator;
    private int _kills = 0;
    private int _xpCount = 0;
    private int _goldCount = 0;
    private GameObject canvas;
    void Start()
    {
        _animator = GetComponent<Animator>();
        canvas = transform.GetChild(0).gameObject;
    }
    public void AddKill(int xp, int gold)
    {
        _kills += 1;
        _xpCount += xp;
        _goldCount += gold;
    }
    public void DisplayStatistic()
    {
        killsCounter.text = "Kills: " + _kills.ToString();
        xpCounter.text = "XP: " + _xpCount.ToString();
        goldCounter.text = "Gold: " + _goldCount.ToString();
        _animator.SetTrigger("Post");
        canvas.SetActive(true);
    }
    public void CloseMenu()
    {
        canvas.SetActive(false);
        player.GetComponent<PlayerController>().onArena = false;
    }
    public void Restart()
    {
        ArenaSpawner.ActivateSpawner();
        _kills = 0;
        _xpCount = 0;
        _goldCount = 0;
        canvas.SetActive(false);
        transform.parent.GetChild(0).GetComponent<EntryToArena>().ConfirmEntry(player);
    }
}
