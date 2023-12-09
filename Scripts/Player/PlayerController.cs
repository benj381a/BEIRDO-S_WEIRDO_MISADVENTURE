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

    [Header("Death")]
    [SerializeField] private float waitTime;

    private bool grounded = false, jumped = false, hasPulledGrabbel = true, grabbeling = false;
    /*[HideInInspector]*/ public int health = 100; //out of 100
    private float width = .01f;
    private Vector2 swingPoint;
    private Vector2 startPosition;

    private Rigidbody2D _rigidbody;
    private DistanceJoint2D joint;
    private LineRenderer _renderer;


    // Start is called before the first frame update
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        joint = GetComponent<DistanceJoint2D>();
        _renderer = GetComponent<LineRenderer>();

        startPosition = transform.position;

        _renderer.startWidth = width;
        _renderer.endWidth = width;
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
        Grabbel();
        if(health <= 0)
        {
            StartCoroutine(Dead());
        }
    }
    private void Movement()
    {
        //move

        float horizontal = Input.GetAxis("Horizontal");

        if (grounded 
            && Mathf.Abs(_rigidbody.velocity.x) > maxSpeed
            && !grabbeling
            )
        {
            _rigidbody.velocity *= Vector2.up;
            _rigidbody.velocity += Vector2.right * maxSpeed;
        }
        else if (Mathf.Abs(_rigidbody.velocity.x) > (maxSpeed * .1f)
            && grabbeling)
        {

        }
        else if(!grabbeling)
        {
            _rigidbody.AddForce(Vector2.right * horizontal * speed * Time.deltaTime, ForceMode2D.Force);
        }
        else
        {
            _rigidbody.AddForce(Vector2.right * horizontal * (speed * .1f) * Time.deltaTime, ForceMode2D.Force);
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

            grabbeling = true;
        }
        if (Input.GetMouseButton(0) 
            && hasPulledGrabbel
            )
        {
            _renderer.positionCount = 2;
            _renderer.SetPosition(0, transform.position);
            _renderer.widthMultiplier = health;

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

            grabbeling = false;
        }
    }
    private IEnumerator PullGrabbel()
    {
        joint.distance = Vector2.Distance(transform.position, swingPoint) - (Time.deltaTime * ropePullSpeed);
        yield return new WaitForFixedUpdate();
        hasPulledGrabbel = true;
    }
    private IEnumerator Dead()
    {
        yield return new WaitForSeconds(waitTime);
        health = 100;
        transform.position = startPosition;
    }
}
