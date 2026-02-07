using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class EffectApplyer : MonoBehaviour
{
    public List<Effect> availableEffects;

    [SerializeField] private StatsManager statsManager;
    [SerializeField] private ColdForestEnter CF_Enter;
    

    [SerializeField] private GameObject FirstSlot;
    [SerializeField] private GameObject SecondSlot;
    [SerializeField] private GameObject ThirdSlot;

    private int activeEffectsCount = 0;

    void Start()
    {
        availableEffects = Resources.LoadAll<Effect>("Effects/ScriptableObjects").ToList();
        Debug.Log($"Loaded {availableEffects.Count} effects.");
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            ApplyEffect(0);
        }
    }

    public void ApplyEffect(int id)
    {
        Effect effect = availableEffects.FirstOrDefault(obj => obj.effectId == id);

        if (effect.useBuffSystem)
        {
            int deltaHealth = effect.buffHealth - effect.debuffHealth;
            int deltaSpeed = effect.buffSpeed - effect.debuffSpeed;
            int deltaDamage = effect.buffDamage - effect.debuffDamage;
            float deltaXpMultiplier = effect.buffXpMultiplier - effect.debuffXpMultiplier;

            statsManager.AddStatBonus(StatsManager.StatType.Health, deltaHealth);
            statsManager.AddStatBonus(StatsManager.StatType.Speed, deltaSpeed);
            statsManager.AddStatBonus(StatsManager.StatType.Damage, deltaDamage);
            statsManager.AddXpMultiplier(effect.buffXpMultiplier - deltaXpMultiplier);

            StartCoroutine(EndEffectAfterDuration(effect.defaultDuration, deltaHealth, deltaSpeed, deltaDamage, deltaXpMultiplier));

            switch (activeEffectsCount)
            {
                case 0:
                    FirstSlot.SetActive(true);
                    FirstSlot.transform.GetChild(0).GetComponent<Image>().sprite = effect.icon;
                    break;
                case 1:
                    SecondSlot.SetActive(true);
                    SecondSlot.transform.GetChild(0).GetComponent<Image>().sprite = effect.icon;
                    break;
                case 2:
                    ThirdSlot.SetActive(true);
                    ThirdSlot.transform.GetChild(0).GetComponent<Image>().sprite = effect.icon;
                    break;
                default:
                    Debug.LogWarning("More than 3 active effects! UI may not display correctly.");
                    break;
            }
            activeEffectsCount++;
        }
        else
        {
            switch (effect.effectId)
            {
                case 1:
                    CF_Enter.ColdForestEnable();
                    StartCoroutine(EndEffectAfterDuration(effect.defaultDuration, effect.effectId));
                    break;
                case 2:
                    // Another effect
                    break;
                default:
                    Debug.LogWarning($"Effect with ID {effect.effectId} has no defined behavior!");
                    break;
            }
        }
    }

    IEnumerator EndEffectAfterDuration(int duration, int deltaHealth, int deltaSpeed, int deltaDamage, float deltaXpMultiplier)
    {
        yield return new WaitForSeconds(duration);
        statsManager.AddStatBonus(StatsManager.StatType.Health, -deltaHealth);
        statsManager.AddStatBonus(StatsManager.StatType.Speed, -deltaSpeed);
        statsManager.AddStatBonus(StatsManager.StatType.Damage, -deltaDamage);
        statsManager.AddXpMultiplier(-deltaXpMultiplier);
    }
    IEnumerator EndEffectAfterDuration(int duration, int id)
    {
        yield return new WaitForSeconds(duration);
        switch (id)
        {
            case 1:
                CF_Enter.ColdForestDisable();
                break;
            case 2:
                // Another effect
                break;
            default:
                Debug.LogWarning($"Effect with ID {id} has no defined behavior!");
                break;
        }
        
    }
}
