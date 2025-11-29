using System.Collections;
using UnityEngine;

public class ArcherController : MonoBehaviour 
{
    public int HP = 10;
    public float KnockBackForce = 0.2f;
    public Transform player;
    public int FacingDirection = 1; // 1 or -1  1-Right  -1-Left

    private Animator _animator;
    private bool _isKnockedBack;
    private bool _ShootAllow = true;
    private StatsManager _Smanager;
    private Rigidbody2D _rb;

    private void Start()
    {
        _Smanager = GameObject.FindGameObjectWithTag("GameController").GetComponent<StatsManager>();
        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }

    private void Update()
    {
        if (HP <= 0)
        {
            Destroy(this.gameObject);
            _Smanager.xpCount += 50;
        }

        if (player.position.x > transform.position.x && FacingDirection == 1 || player.position.x < transform.position.x && FacingDirection == -1)
        {
            Flip();
        }

        //Shoot all players (?) in circle radius
        Collider2D[] rawResults;
        rawResults = Physics2D.OverlapCircleAll(transform.position, 5f);
        foreach (Collider2D collider in rawResults)
        {
            if (collider.gameObject.CompareTag("Player") && _ShootAllow)
            {
                PlayerController player = collider.GetComponent<PlayerController>();
                StartCoroutine(Shoot());
            }
        }
    }

    IEnumerator Shoot()
    {
        if (_ShootAllow && !_isKnockedBack)
        {
            _ShootAllow = false;
            _animator.SetTrigger("Shoot");
            Instantiate(Resources.Load("Prefabs/Arrow"), transform.position, Quaternion.identity);
        }
        yield return new WaitForSeconds(1.5f);
        _ShootAllow = true;
    }

    void Flip()
    {
        FacingDirection *= -1;
        transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
    }
        
}
