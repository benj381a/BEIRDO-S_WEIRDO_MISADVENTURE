using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Extensions;
using System.Linq;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed, jumpHeigt, maxSpeed, groundDistance, ropePullSpeed;

    private bool grounded = false, jumped = false;

    private Rigidbody2D _rigidbody;
    private Vector2 swingPoint;

    [SerializeField] private DistanceJoint2D joint;
    [SerializeField] private LineRenderer _renderer;


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
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (Input.GetMouseButtonDown(0))
        {
            joint.connectedAnchor = mousePos;
            swingPoint = mousePos;
        }
        RaycastHit hit;
        Physics.Raycast(Camera.main.ScreenPointToRay(swingPoint), out hit,100);
        if (Input.GetMouseButton(0))
        {
            _renderer.positionCount = 2;
            _renderer.SetPosition(0, transform.position);
            _renderer.SetPosition(1, swingPoint);

            joint.enabled = true;
            _renderer.enabled = true;

            if (Vector2.Distance(transform.position, swingPoint) > 2)
            {
                joint.distance = Vector2.Distance(transform.position, swingPoint) - (Time.deltaTime * ropePullSpeed);
            }
        }
        if (Input.GetMouseButtonUp(0))
        {
            joint.enabled = false;
            _renderer.enabled = false;
        }



    }
    IEnumerator Jump()
    {
        _rigidbody.AddForce(Vector2.up * jumpHeigt, ForceMode2D.Impulse);
        yield return new WaitForSeconds(0.2f);
        jumped = false;
    }


}
