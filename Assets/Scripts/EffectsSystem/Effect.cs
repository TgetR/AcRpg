using UnityEngine;

[CreateAssetMenu(fileName = "Effect", menuName = "Scriptable Objects/Effect")]
public class Effect : ScriptableObject
{
    [Header("General")]
    public int effectId;
    public int defaultDuration;
    public bool useBuffSystem = true;
    
    [Header("BuffSystem")]
    public int buffSpeed;
    public int buffHealth;
    public int buffDamage;
    public float buffXpMultiplier;

    public int debuffSpeed;
    public int debuffHealth;
    public int debuffDamage;
    public float debuffXpMultiplier;

    [Header("UI")]
    public Sprite icon;

}
