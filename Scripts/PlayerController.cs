using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Extensions;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed, jumpHeigt, maxSpeed, groundDistance;

    private bool grounded = false, jumped = false;

    private Rigidbody2D _rigidbody;

    // Start is called before the first frame update
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        
        if (grounded && Mathf.Abs(_rigidbody.velocity.x) > maxSpeed)
        {
            _rigidbody.velocity *= Vector2.up;
            _rigidbody.velocity += Vector2.right * maxSpeed;
        }
        else
        {
            _rigidbody.AddForce(Vector2.right * horizontal * speed * Time.deltaTime, ForceMode2D.Force);
        }

        if(!jumped && Physics2D.Raycast(transform.position,Vector2.down).distance <= groundDistance)
        {
            grounded = true;
        }


        if (grounded && (Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)))
        {
            grounded = false;
            jumped = true;
            StartCoroutine(Jump());
        }
    }
    IEnumerator Jump()
    {
        _rigidbody.AddForce(Vector2.up * jumpHeigt, ForceMode2D.Impulse);
        yield return new WaitForSeconds(0.2f);
        jumped = false;
    }
}
