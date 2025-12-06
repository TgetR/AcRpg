using UnityEngine;

public class EnemyChasing : MonoBehaviour
{
    public float speed = 1;
    public Transform player;
    public bool isAgressive;
    public bool moveRestrict = false;

    [SerializeField] private Rigidbody2D _rb;
    [SerializeField] private Animator _animator;
    [SerializeField] private WarriorController warrior;


    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }

    void Update()
    {
        if (isAgressive && !moveRestrict)
        {
            _animator.SetBool("Move", true);
            if (player.position.x > transform.position.x && warrior.FacingDirection == 1 || player.position.x < transform.position.x && warrior.FacingDirection == -1)
            {
                warrior.Flip();
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
