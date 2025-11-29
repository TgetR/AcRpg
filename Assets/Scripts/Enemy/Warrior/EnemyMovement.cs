using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public float speed = 1;
    public Transform player;
    public bool isAgressive;
    private Animator _animator;
    public bool moveRestrict = false;
    public int FacingDirection = 1; // 1 or -1  1-Right  -1-Left

    private Rigidbody2D _rb;


    void Start()
    {
        _animator = GetComponent<Animator>();
        _rb = gameObject.GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }

    void Update()
    {
        if (isAgressive && !moveRestrict)
        {
            _animator.SetBool("Move", true);
            if (player.position.x > transform.position.x && FacingDirection == 1 || player.position.x < transform.position.x && FacingDirection == -1)
            {
                Flip();
            }
            Vector2 direction = (player.position - transform.position).normalized;
            _rb.linearVelocity = direction * speed;
        }
        else
        {
            _rb.linearVelocity = Vector2.zero; 
            _animator.SetBool("Move",false);
        } 
    }

    void Flip()
    {
        FacingDirection *= -1;
        transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isAgressive = true;
        }
    }
    
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isAgressive = false;
        }
    }
}
