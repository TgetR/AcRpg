using System.Collections;
using UnityEngine;

public class WarriorController : MonoBehaviour
{

    public float KnockBackForce = 0.2f;
    public int FacingDirection = 1; // 1 or -1  1-Right  -1-Left

    private Animator _animator;
    private bool _isKnockedBack;
    private bool _AttackAllow = true;
    private EnemyChasing _chasingController;
    private Rigidbody2D _rb;

    private void Start()
    {
        _chasingController = GetComponent<EnemyChasing>();
        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
    }

    //KnockBack for enemy when it have damage...
    public void KnockBack(Transform player, float force, float stunTime)
    {
        _isKnockedBack = true;
        _chasingController.moveRestrict = true;
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
        _chasingController.moveRestrict = false;
    }


    public IEnumerator Attack(PlayerController target)
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
    public void Flip()
    {
        FacingDirection *= -1;
        transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
    }
}
