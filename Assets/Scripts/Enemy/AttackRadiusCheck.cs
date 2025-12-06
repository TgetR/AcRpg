using UnityEngine;

public class AttackRadiusCheck : MonoBehaviour
{
    [SerializeField] private WarriorController warrior;
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerController player = collision.GetComponent<PlayerController>();
            StartCoroutine(warrior.Attack(player));
        } 
    }
}
