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
    [SerializeField] private float missedPullSpeed;

    [Header("Death")]
    [SerializeField] private float waitTime;

    private bool grounded = false, hasPulledGrabbel = true, grabbeling = false, stop = false, pullingBack = false;
    /*[HideInInspector]*/ public int health = 100; //out of 100
    private float width = .01f;
    [HideInInspector] public Vector2 swingPoint;
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
        if (!stop)
        {
            Movement();
            Grabbel();
            if (health <= 0)
            {
                StartCoroutine(Dead());
            }
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
        else if (!grabbeling)
        {
            _rigidbody.AddForce(Vector2.right * horizontal * speed * Time.deltaTime, ForceMode2D.Force);
        }
        else
        {
            _rigidbody.AddForce(Vector2.right * horizontal * (speed * .1f) * Time.deltaTime, ForceMode2D.Force);
        }
        //jump
        grounded = Physics2D.Raycast(transform.position, Vector2.down).distance <= groundDistance;

        if (grounded
            && (Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
            )
        {
            if (_rigidbody.velocity.y == 0)
            {
                _rigidbody.AddForce(Vector2.up * jumpHeigt, ForceMode2D.Impulse);
            }
        }
    }

    private void Grabbel()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);


        if (Physics2D.Raycast(transform.position, (mousePos - transform.position.ToVector2()).normalized.normalized, maxGrabbelDistance).collider == null
            && (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
            )
        {
            swingPoint =  Vector2.MoveTowards(transform.position, mousePos, maxGrabbelDistance);

            _renderer.SetPosition(1, swingPoint);

            _renderer.enabled = true;

            StartCoroutine(PullGrabbelIn());
        }
        Debug.Log(mousePos);

        _renderer.SetPosition(0, transform.position);
        _renderer.widthMultiplier = health;

        if ((Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
            && Physics2D.Raycast(transform.position, (mousePos - transform.position.ToVector2()).normalized.normalized, maxGrabbelDistance).collider != null
            )
        {
            swingPoint = Physics2D.Raycast(transform.position, (mousePos - transform.position.ToVector2()).normalized.normalized, maxGrabbelDistance).point;
            joint.connectedAnchor = swingPoint;
            _renderer.SetPosition(1, swingPoint);

            joint.enabled = true;
            _renderer.enabled = true;

            grabbeling = true;
        }
        if ((Input.GetMouseButton(0) || Input.GetMouseButton(1))
            && hasPulledGrabbel
            )
        {
            _renderer.positionCount = 2;

            if (Vector2.Distance(transform.position, swingPoint) > 2)
            {
                hasPulledGrabbel = false;
                StartCoroutine(PullGrabbel());
            }
        }
        if ((Input.GetMouseButtonUp(0) || Input.GetMouseButtonUp(1))
            &&!pullingBack
            )
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
    private IEnumerator PullGrabbelIn()
    {
        pullingBack = true;
        while (Vector2.Distance(transform.position, swingPoint) > 1)
        {
            swingPoint = Vector2.MoveTowards(swingPoint, transform.position, missedPullSpeed * Time.deltaTime);
            _renderer.SetPosition(1, swingPoint);
            yield return new WaitForFixedUpdate();
        }
        _renderer.enabled = false;
        pullingBack = false;
    }

    private IEnumerator Dead()
    {
        stop = true;
        yield return new WaitForSeconds(waitTime);
        stop = false;
        health = 100;
        transform.position = startPosition;
    }
}
