using System.Collections;
using UnityEngine;

public class ArcherController : MonoBehaviour 
{
    public float KnockBackForce = 0.2f;
    public Transform player;
    public int FacingDirection = 1; // 1 or -1  1-Right  -1-Left

    private Animator _animator;
    private bool _isKnockedBack;
    private bool _ShootAllow = true;

    private void Start()
    {
        _animator = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }

    private void Update()
    {
        if (player.position.x > transform.position.x && FacingDirection == 1 || player.position.x < transform.position.x && FacingDirection == -1)
        {
            Flip();
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject == player && _ShootAllow)
        {
            StartCoroutine(Shoot());
        } 
    }
    void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject == player && _ShootAllow)
        {
            StartCoroutine(Shoot());
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
