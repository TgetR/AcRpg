using System.Collections;
using TMPro;
using UnityEngine;

public class StatsManagerMenu : MonoBehaviour
{
    [SerializeField] private GameObject menu;

    [Header("Count text objects")]
    
    [SerializeField] private TMP_Text speedCountText;
    [SerializeField] private TMP_Text healthCountText;
    [SerializeField] private TMP_Text damageCountText;
    [SerializeField] private TMP_Text pointsCountText;

    [Header("Upgrade text objects")]
    [SerializeField] private TMP_Text speedUpgradeText;
    [SerializeField] private TMP_Text healthUpgradeText;
    [SerializeField] private TMP_Text damageUpgradeText;

    private StatsManager _statsManager;

    void Start()
    {
        _statsManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<StatsManager>();
        menu.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.Escape) && menu.activeSelf) Close();
    }

    public void Open()
    {
        speedCountText.text = "Speed - " + _statsManager.Speed;
        healthCountText.text = "Maximum health - " + _statsManager.MaxHealth;
        damageCountText.text = "Damage - " + _statsManager.Damage;
        pointsCountText.text = "Your upgrade points: " + _statsManager.GetUpgradePoints();

        menu.SetActive(true);
    }
    public void Close()
    {
        menu.SetActive(false);
    }

    public void UpgradeSpeed()
    {
        if (!_statsManager.LimitIsOk)
        {
            UnSucessfulUpgrade(speedUpgradeText, "Your reach points limit! No more upgrades!");
            return;
        }

        else if (_statsManager.GetUpgradePoints() > 0)
        {
            _statsManager.UpgradeStat(1);
            StartCoroutine(SucessfulUpgrade(speedUpgradeText));
            speedCountText.text = "Speed - " + _statsManager.Speed;
        }
        else StartCoroutine(UnSucessfulUpgrade(speedUpgradeText, "Not enought points!"));
    }

    public void UpgradeHealth()
    {
        if (!_statsManager.LimitIsOk)
        {
            UnSucessfulUpgrade(healthUpgradeText, "Your reach points limit! No more upgrades!");
            return;
        }

        else if (_statsManager.GetUpgradePoints() > 0)
        {
            _statsManager.UpgradeStat(0);
            StartCoroutine(SucessfulUpgrade(healthUpgradeText));
            healthCountText.text = "Maximum health - " + _statsManager.MaxHealth;
        }
        else
            StartCoroutine(UnSucessfulUpgrade(healthUpgradeText, "Not enought points!"));
    }

    public void UpdgradeDamage()
    {
        if (!_statsManager.LimitIsOk)
        {
            UnSucessfulUpgrade(damageUpgradeText, "Your reach points limit! No more upgrades!");
            return;
        }

        else if (_statsManager.GetUpgradePoints() > 0)
        {
            _statsManager.UpgradeStat(2);
            StartCoroutine(SucessfulUpgrade(damageUpgradeText));
            damageCountText.text = "Damage - " + _statsManager.Damage;
        }
        else StartCoroutine(UnSucessfulUpgrade(damageUpgradeText, "Not enought points!"));
    }



    private IEnumerator SucessfulUpgrade(TMP_Text text)
    {
        pointsCountText.text = "Your upgrade points: " + _statsManager.GetUpgradePoints();
        Color colorFirst = text.color;
        string firstText = text.text;

        text.color = Color.softGreen;
        text.text = "Sucessful upgraded!";

        yield return new WaitForSeconds(1);

        text.color = colorFirst;
        text.text = firstText;

    }
    private IEnumerator UnSucessfulUpgrade(TMP_Text text, string reason)
    {
        pointsCountText.text = "Your upgrade points: " + _statsManager.GetUpgradePoints();
        Color colorFirst = text.color;
        string firstText = text.text;

        text.color = Color.softRed;
        text.text = "Can't upgrade it! Reason: " + reason;

        yield return new WaitForSeconds(1);

        text.color = colorFirst;
        text.text = firstText;
    }
    

    int counter = 0;
    public void Debug_AddUP()
    {
        counter++;
        if(counter > 5)
        {
            _statsManager.AddUpgradePoints(1, "Cheat");
            pointsCountText.text = "Your upgrade points: " + _statsManager.GetUpgradePoints();
        }

    }
}
