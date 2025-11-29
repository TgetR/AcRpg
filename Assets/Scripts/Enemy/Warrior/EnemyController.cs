using System.Collections;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public int HP = 10;
    public float KnockBackForce = 0.2f;

    private Animator _animator;
    private bool _isKnockedBack;
    private bool _AttackAllow = true;
    private EnemyMovement _movementController;
    private StatsManager _Smanager;
    private Rigidbody2D _rb;

    private void Start()
    {
        _Smanager = GameObject.FindGameObjectWithTag("GameController").GetComponent<StatsManager>();
        _movementController = GetComponent<EnemyMovement>();
        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        //Attack all players (?) in circle radius
        Collider2D[] rawResults;
        rawResults = Physics2D.OverlapCircleAll(transform.position, 1f);
        foreach (Collider2D collider in rawResults)
        {
            if (collider.gameObject.CompareTag("Player") && _AttackAllow)
            {
                PlayerController player = collider.GetComponent<PlayerController>();
                StartCoroutine(Attack(player));
            } 
        }
    }

    //KnockBack for enemy when it have damage...
    public void KnockBack(Transform player, float force, float stunTime)
    {
        _isKnockedBack = true;
        _movementController.moveRestrict = true;
        Vector2 direction = (transform.position - player.position).normalized;
        _rb.linearVelocity = direction * force;
        Debug.Log("KnockBack Enemy");
        StartCoroutine(KnockBackCounter(stunTime));
    }
    IEnumerator KnockBackCounter(float time)
    {
        yield return new WaitForSeconds(time);
        _rb.linearVelocity = Vector2.zero;
        _isKnockedBack = false;
        _movementController.moveRestrict = false;
    }


    IEnumerator Attack(PlayerController target)
    {
        yield return new WaitForSeconds(0.5f);
        if (_AttackAllow && !_isKnockedBack)
        {
            _AttackAllow = false;
            _animator.SetTrigger("Attack");
            target.ChangeHealth(-10);
            target.KnockBack(transform, KnockBackForce, 0.5f);
        }
        yield return new WaitForSeconds(1);
        _AttackAllow = true;
    }
}
