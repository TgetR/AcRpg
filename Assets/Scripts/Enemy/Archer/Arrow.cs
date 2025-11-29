using System.Collections;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    public float speed = 2.5f;
    public float lifetime = 2.5f;
    private Rigidbody2D _rb;
    private Transform _target;
    
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        
        _target = GameObject.FindWithTag("Player").transform;
        Vector2 direction = (_target.position - transform.position).normalized;
        _rb.AddForce(direction * speed);

        float angleRadians = Mathf.Atan2(direction.y, direction.x);
        float angleDegrees = angleRadians * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, angleDegrees);

        
        StartCoroutine(LifetimeTimer());
    }

    IEnumerator LifetimeTimer()
    {
        yield return new WaitForSeconds(lifetime);
        Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.gameObject.tag != "Archer")
        {
            if (collider.gameObject.tag == "Player") collider.gameObject.GetComponent<PlayerController>().ChangeHealth(-5);
            Destroy(gameObject);
        }
    }
}
