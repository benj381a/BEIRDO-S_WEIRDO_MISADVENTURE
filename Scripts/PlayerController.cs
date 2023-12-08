using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Extensions;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed, jumpHeigt, maxSpeed;

    private bool grounded = false;

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
        
        if (Mathf.Abs(_rigidbody.velocity.x) > maxSpeed)
        {
            _rigidbody.velocity *= Vector2.up;
            _rigidbody.velocity += Vector2.right * maxSpeed;
        }
        else
        {
            _rigidbody.AddForce(Vector2.right * horizontal * speed * Time.deltaTime);
        }


        if (grounded && (Input.GetKey(KeyCode.Space)))
        {

        }
    }
}
