using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    #region Setup
    public int FacingDirection = 1; // 1 or -1  1-Right  -1-Left

    [SerializeField] TMP_Text HpText;
    [SerializeField] TMP_Text GoldText;
    private Rigidbody2D _rb;
    private HashSet<EnemyHealthSystem> _enemiesInRange;
    private Animator _animator;
    private bool _AttackAllow = true;
    private bool _isInAttack = false;
    private bool _isKnockedBack = false;
    private bool _isKnockBackCooldownEnd = true;

    [SerializeField] private float attackRadius = 1.2f;
    private CircleCollider2D _attackRangeCollider;
    

    //Controlled by StatsManager
    private StatsManager _manager;
    private int _HP = 100;
    private int _maxHP = 100;
    private int _Gold = 100;
    private float _speed = 5;
    private int _damage = 5;

    void Start()
    {
        //StatsManager
        _manager = GameObject.FindGameObjectWithTag("GameController").GetComponent<StatsManager>();
        _HP = _manager.Health;

        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _enemiesInRange = new HashSet<EnemyHealthSystem>();

        SetupAttackRangeTrigger();
    }

    private void SetupAttackRangeTrigger()
    {
        _attackRangeCollider = gameObject.AddComponent<CircleCollider2D>();
        _attackRangeCollider.isTrigger = true;
        _attackRangeCollider.radius = attackRadius;
    }
    #endregion

    #region UpdateMethods
    private void Update()
    {
        //Stats Update
        _maxHP = _manager.MaxHealth;
        _speed = _manager.Speed;
        _damage = _manager.Damage;
        _Gold = _manager.Gold;

        UpdateAndCheckStats();
        AttackCheck();
    }

    void FixedUpdate()
    {
        if (!_isKnockedBack)
        {
            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");

            if (horizontal > 0 && transform.localScale.x < 0 || horizontal < 0 && transform.localScale.x > 0)
            {
                Flip();
            }

            _animator.SetFloat("Horizontal", Mathf.Abs(horizontal));
            _animator.SetFloat("Vertical", Mathf.Abs(vertical));

            _rb.linearVelocity = new Vector2(horizontal * _speed, vertical * _speed);

            //Give xp when walk
            if (horizontal != 0 || vertical != 0)
            {
                _manager.xpCount += 0.01f;
            }
        }
    }
    #endregion
    #region CheckMethods
    void UpdateAndCheckStats()
    {
        HpText.text = "HP: " + _HP + "/" + _maxHP;
        GoldText.text = "Gold: " + _Gold;
        if (_HP <= 0)
        {
            SceneManager.LoadScene("GameOver");
        }
    }

    void AttackCheck()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0) && !_isInAttack && _AttackAllow)
        {
            _animator.SetTrigger("Attack");
            _isInAttack = true;
            //Select attack version
            byte AttackVersion = (byte)Random.Range(0, 2); //0 - first version;  1- second version
            _animator.SetInteger("AttackV", AttackVersion);
            if (_enemiesInRange.Count > 0)
            {
                EnemyHealthSystem closestEnemy = GetClosestEnemy();
                if (closestEnemy != null)
                {
                StartCoroutine(Attack(closestEnemy));
                }
            }
            else
            {
                StartCoroutine(ResetAttackCooldown());
            } 
            
            _enemiesInRange.RemoveWhere(enemy => enemy == null); //Clean up null references
        }
    }
    #endregion

    #region TriggerMethods
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("Archer"))
        {
            EnemyHealthSystem enemy = collision.GetComponent<EnemyHealthSystem>();
            _enemiesInRange.Add(enemy);
            Debug.Log($"Enemy enter to radius. Total enemies in radius: {_enemiesInRange.Count}");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("Archer"))
        {
            EnemyHealthSystem enemy = collision.GetComponent<EnemyHealthSystem>();
            if (enemy != null)
            {
                _enemiesInRange.Remove(enemy);
                Debug.Log($"Enemy exit from radius. Total enemies: {_enemiesInRange.Count}");
            }
        }
    }

    private EnemyHealthSystem GetClosestEnemy()
    {
        EnemyHealthSystem closest = null;
        float minDistance = float.MaxValue;

        foreach (var enemy in _enemiesInRange)
        {
            if (enemy == null) continue;

            float distance = Vector2.Distance(transform.position, enemy.transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                closest = enemy;
            }
        }

        return closest;
    }
    #endregion

    
    #region EssentialMethods
    void Flip()
    {
        FacingDirection *= -1;
        transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
    }

    public void ChangeHealth(int heatlh)
    {
        _HP += heatlh;
    }
    #endregion

    #region CombatMethods
    IEnumerator Attack(EnemyHealthSystem target)
    {

        if (_AttackAllow && target != null)
        {
            _AttackAllow = false;
            target.transform.localScale = new Vector3(-FacingDirection, transform.localScale.y);

            yield return new WaitForSeconds(0.6f);
            if(target != null) target.ChangeHealth(-_damage);
        }
       yield return new WaitForSeconds(1);
        _isInAttack = false;
        _AttackAllow = true;
    }

    public void KnockBack(Transform enemy, float force, float stunTime)
    {   
        if(!_isKnockedBack && _isKnockBackCooldownEnd)
        {
           _isKnockedBack = true;
            Vector2 direction = (transform.position - enemy.position).normalized;
            _rb.linearVelocity = direction * force;
            StartCoroutine(KnockBackCounter(stunTime));
            StartCoroutine(KnockBackCooldown(5f));
        }
    }
    #endregion

    #region Coroutines
    IEnumerator KnockBackCounter(float time)
    {
        yield return new WaitForSeconds(time);
        _rb.linearVelocity = Vector2.zero;
        _isKnockedBack = false;
    }

    IEnumerator KnockBackCooldown(float time)
    {
        _isKnockBackCooldownEnd = false;
        yield return new WaitForSeconds(time);
        _isKnockBackCooldownEnd = true;
    }

        private IEnumerator ResetAttackCooldown()
    {
        yield return new WaitForSeconds(0.6f);
        _isInAttack = false;
    }
    #endregion

    #region Gizmos
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRadius);
        
        Gizmos.color = Color.yellow;
        if (_enemiesInRange != null)
        {
            foreach (var enemy in _enemiesInRange)
            {
                if (enemy != null)
                {
                    Gizmos.DrawLine(transform.position, enemy.transform.position);
                }
            }
        }
    }
    #endregion
}