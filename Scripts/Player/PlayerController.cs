using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Extensions;
using System.Linq;

public class PlayerController : MonoBehaviour
{
    [Header("Player Movement")]
    [SerializeField] private float speed;
    [SerializeField] private float maxSpeed;
    [SerializeField] private float jumpHeigt;
    [SerializeField] private float groundDistance;

    [Header("Grabbel System")]
    [SerializeField] private float ropePullSpeed;
    [SerializeField] private float maxGrabbelDistance;

    private bool grounded = false, jumped = false, hasPulledGrabbel = true;
    private Vector2 swingPoint;

    private Rigidbody2D _rigidbody;
    private DistanceJoint2D joint;
    private LineRenderer _renderer;


    // Start is called before the first frame update
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        joint = GetComponent<DistanceJoint2D>();
        _renderer = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
        Grabbel();
    }
    private void Movement()
    {
        //move

        float horizontal = Input.GetAxis("Horizontal");

        if (grounded 
            && Mathf.Abs(_rigidbody.velocity.x) > maxSpeed
            )
        {
            _rigidbody.velocity *= Vector2.up;
            _rigidbody.velocity += Vector2.right * maxSpeed;
        }
        else
        {
            _rigidbody.AddForce(Vector2.right * horizontal * speed * Time.deltaTime, ForceMode2D.Force);
        }
        //jump

        if (!jumped 
            && Physics2D.Raycast(transform.position, Vector2.down).distance <= groundDistance
            )
        {
            grounded = true;
        }


        if (grounded 
            && (Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
            )
        {
            grounded = false;
            jumped = true;
            StartCoroutine(Jump());
        }
    }

    private IEnumerator Jump()
    {
        _rigidbody.AddForce(Vector2.up * jumpHeigt, ForceMode2D.Impulse);
        yield return new WaitForSeconds(0.2f);
        jumped = false;
    }

    private void Grabbel()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);


        if (Input.GetMouseButtonDown(0) 
            && Physics2D.CircleCast(mousePos, .01f, Vector2.zero).collider != null 
            && Vector2.Distance(transform.position, mousePos) <= maxGrabbelDistance
            )
        {
            joint.connectedAnchor = mousePos;
            swingPoint = mousePos;
            _renderer.SetPosition(1, swingPoint);

            joint.enabled = true;
            _renderer.enabled = true;
        }
        if (Input.GetMouseButton(0) 
            && hasPulledGrabbel
            )
        {
            _renderer.positionCount = 2;
            _renderer.SetPosition(0, transform.position);

            if (Vector2.Distance(transform.position, swingPoint) > 2)
            {
                hasPulledGrabbel = false;
                StartCoroutine(PullGrabbel());
            }
        }
        if (Input.GetMouseButtonUp(0))
        {
            joint.enabled = false;
            _renderer.enabled = false;
        }
    }
    private IEnumerator PullGrabbel()
    {
        joint.distance = Vector2.Distance(transform.position, swingPoint) - (Time.deltaTime * ropePullSpeed);
        yield return new WaitForFixedUpdate();
        hasPulledGrabbel = true;
    }
}
