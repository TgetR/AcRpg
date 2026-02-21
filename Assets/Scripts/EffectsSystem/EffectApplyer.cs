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
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ApplyEffect(0);
        }
    }

    public void ApplyEffect(int id)
    {
        Effect effect = availableEffects.FirstOrDefault(obj => obj.effectId == id);

        //if effect gives ussualy buffs or debuffs (use buff system)
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
        }

        // Special Effect appyling (without using buff system)
        else
        {
            StartCoroutine(EndEffectAfterDuration(effect.defaultDuration, effect.effectId));
            switch (effect.effectId)
            {
                case 1:
                    CF_Enter.ColdForestEnable();
                    break;
                case 2:
                    // Another effect
                    break;
                default:
                    Debug.LogWarning($"Effect with ID {effect.effectId} has no defined behavior!");
                    break;
            }
        }

        // Update UI for active effects
        switch (activeEffectsCount)
        {
            case 0:
                Debug.Log("0 active effects! Activationg first slot.");
                StartCoroutine(UISlotSetAndDuration(FirstSlot, effect.defaultDuration, effect.icon));
                break;
            case 1:
                Debug.Log("1 active effects! Activationg second slot.");
                StartCoroutine(UISlotSetAndDuration(SecondSlot, effect.defaultDuration, effect.icon));
                break;
            case 2:
                Debug.Log("2 active effects! Activationg third slot.");
                StartCoroutine(UISlotSetAndDuration(ThirdSlot, effect.defaultDuration, effect.icon));
                break;
            default:
                Debug.Log("EF: All slots are active! ACTIVE EFFECTS COUNT: " + activeEffectsCount);
                break;
        }
    }

    IEnumerator UISlotSetAndDuration(GameObject slot, int duration, Sprite icon)
    {
        slot.SetActive(true);
        slot.transform.GetChild(0).GetComponent<Image>().sprite = icon;
        yield return new WaitForSeconds(duration);
        slot.SetActive(false);
    }

    IEnumerator EndEffectAfterDuration(int duration, int deltaHealth, int deltaSpeed, int deltaDamage, float deltaXpMultiplier)
    {
        activeEffectsCount++;
        yield return new WaitForSeconds(duration);
        statsManager.AddStatBonus(StatsManager.StatType.Health, -deltaHealth);
        statsManager.AddStatBonus(StatsManager.StatType.Speed, -deltaSpeed);
        statsManager.AddStatBonus(StatsManager.StatType.Damage, -deltaDamage);
        statsManager.AddXpMultiplier(-deltaXpMultiplier);
        activeEffectsCount--;
    }
    IEnumerator EndEffectAfterDuration(int duration, int id)
    {
        activeEffectsCount++;
        yield return new WaitForSeconds(duration);
        activeEffectsCount--;
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
