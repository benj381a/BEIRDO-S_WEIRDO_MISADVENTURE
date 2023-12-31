using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Extensions;
using System.Linq;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    [Header("Player Movement")]
    [SerializeField] public float speed;
    [SerializeField] private float maxSpeed;
    [SerializeField] private float jumpHeigt;
    [SerializeField] private float groundDistance;

    [Header("Grabbel System")]
    [SerializeField] private float ropePullSpeed;
    [SerializeField] private float maxGrabbelDistance;
    [SerializeField] private float missedPullSpeed;
    [SerializeField] private float minDistance;
    [SerializeField] private Transform platform;

    [Header("Death")]
    [SerializeField] private float waitTime;
    [Tooltip("out of 100")]
    public int health = 100;

    [Header("Particals")]
    public ParticleSystem dammage;

    [Header("Music")]
    [SerializeField] public List<AudioSource> audioSources = new List<AudioSource>();

    [SerializeField] private AudioClip intro;
    [SerializeField] private AudioClip loop;

    [SerializeField] public AudioClip walk;
    [SerializeField] public AudioClip run;
    [SerializeField] public AudioClip hurt;
    [SerializeField] public AudioClip die;
    [SerializeField] public AudioClip jump;
    [SerializeField] public AudioClip grabbel;
    [SerializeField] public AudioClip buzzSawImpact;
    [SerializeField] public AudioClip pickUpImpact;
    [SerializeField] public AudioClip scissorImpact;

    private bool grounded = false, hasPulledGrabbel = true, grabbeling = false, stop = false, pullingBack = false;
    private float width = .01f, prevVelocity = 0;
    [HideInInspector] public Vector2 swingPoint;
    private Vector2 startPosition;

    [HideInInspector] public Animator animator;
    private Rigidbody2D _rigidbody;
    private DistanceJoint2D joint;
    private LineRenderer _renderer;


    // Start is called before the first frame update
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        joint = GetComponent<DistanceJoint2D>();
        _renderer = GetComponent<LineRenderer>();

        audioSources = GameObject.Find("Music Manager").GetComponents<AudioSource>().ToList();

        startPosition = transform.position;

        _renderer.startWidth = width;
        _renderer.endWidth = width;

        StartCoroutine(Music());
        animator = GetComponent<Animator>();
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
        if (Mathf.Abs(_rigidbody.velocity.x-prevVelocity) > 5)
        {
            //SmallBeardoManager.Instance.animator.SetTrigger("Hurt");
        }
        prevVelocity = _rigidbody.velocity.x;
        if (Input.GetKeyDown(KeyCode.R))
        {
            //SmallBeardoManager.Instance.animator.SetTrigger("Restart");
            SceneManager.LoadScene(SceneManager.GetActiveScene().name, LoadSceneMode.Single);
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            //SmallBeardoManager.Instance.animator.SetTrigger("Restart");
            SceneManager.LoadSceneAsync(0);
        }
    }
    private void Movement()
    {
        //move

        float horizontal = Input.GetAxis("Horizontal");

        animator.SetInteger("walkDir", Mathf.RoundToInt(horizontal));
        animator.SetFloat("Velocity",_rigidbody.velocity.x);
        //SmallBeardoManager.Instance.animator.SetFloat("Velocity", Mathf.Abs(_rigidbody.velocity.x));

        if (grounded
            && Mathf.Abs(_rigidbody.velocity.x) > maxSpeed
            && !grabbeling
            )
        {
            _rigidbody.velocity *= Vector2.up;
            _rigidbody.velocity += Vector2.right * maxSpeed;

        }
        else if (!grabbeling)
        {
            _rigidbody.AddForce(Vector2.right * horizontal * speed * Time.deltaTime, ForceMode2D.Force);
            if (_rigidbody.velocity.x > 18)
            {
                PlaySfx(run);
            }
            else
            {
                PlaySfx(walk);
            }
        }
        else
        {
            _rigidbody.AddForce(Vector2.right * horizontal * (speed * 1f) * Time.deltaTime, ForceMode2D.Force);
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
                animator.SetTrigger("Jump");
                PlaySfx(jump);
            }
        }
    }

    private void Grabbel()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);


        if (platform.parent != transform)
        {
            swingPoint = platform.position;
            joint.connectedAnchor = swingPoint;
            _renderer.SetPosition(1, swingPoint);
        }

        if (Physics2D.Raycast(transform.position, (mousePos - transform.position.ToVector2()).normalized.normalized, maxGrabbelDistance).collider == null
            && (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
            )
        {
            swingPoint = Vector2.MoveTowards(transform.position, mousePos, maxGrabbelDistance);

            _renderer.SetPosition(1, swingPoint);

            _renderer.enabled = true;


            StartCoroutine(PullGrabbelIn());
        }

        _renderer.SetPosition(0, transform.position);
        _renderer.widthMultiplier = health;

        if ((Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
            && Physics2D.Raycast(transform.position, (mousePos - transform.position.ToVector2()).normalized.normalized, maxGrabbelDistance).collider != null
            )
        {
            swingPoint = Physics2D.Raycast(transform.position, (mousePos - transform.position.ToVector2()).normalized.normalized, maxGrabbelDistance).point;
            Transform trans = Physics2D.Raycast(transform.position, (mousePos - transform.position.ToVector2()).normalized.normalized, maxGrabbelDistance).transform;

            //PlaySfx(grabbel);

            if (trans.GetComponentInChildren<MovingPlatform>())
            {
                platform.position = swingPoint;
                platform.SetParent(trans);
                transform.SetParent(trans);
            }
            joint.connectedAnchor = swingPoint;
            _renderer.SetPosition(1, swingPoint);

            joint.enabled = true;
            _renderer.enabled = true;

            grabbeling = true;
        }
        if (platform != null)
        {
            if (platform.parent.GetComponent<MovingPlatform>())
            {
                swingPoint = platform.position;
                transform.parent = platform.parent;
            }
        }
        if ((Input.GetMouseButton(0) || Input.GetMouseButton(1))
            && hasPulledGrabbel
            && !pullingBack
            )
        {
            var dir = swingPoint - transform.position.ToVector2();
            var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);

            if (Vector2.Distance(transform.position, swingPoint) > minDistance && Input.GetMouseButton(1))
            {
                hasPulledGrabbel = false;

                StartCoroutine(PullGrabbel());
            }
        }
        if ((Input.GetMouseButtonUp(0) || Input.GetMouseButtonUp(1))
            && !pullingBack
            )
        {
            Realease();
        }
        transform.localScale = Vector3.one;
    }
    public void Realease()
    {
        joint.enabled = false;
        _renderer.enabled = false;

        grabbeling = false;
        platform.parent = transform;
        transform.parent = null;
        joint.autoConfigureDistance = true;

        transform.rotation = new Quaternion();
    }
    private IEnumerator PullGrabbel()
    {
        joint.autoConfigureDistance = false;

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
        Realease();
        GetComponent<ParticleSystem>().Play();
        yield return new WaitForSeconds(waitTime);
        //SmallBeardoManager.Instance.animator.SetTrigger("Restart");
        SceneManager.LoadScene(SceneManager.GetActiveScene().name, LoadSceneMode.Single);
    }
    private IEnumerator Music()
    {
        audioSources[0].clip = intro;
        audioSources[1].clip = loop;
        audioSources[1].loop = true;
        audioSources[0].Play();

        yield return new WaitForSeconds(intro.length); audioSources[1].Play();
    }
    public void PlaySfx(AudioClip sfx)
    {
        if (sfx == walk || sfx == run)
        {
            if (!audioSources[2].isPlaying)
            {
                audioSources[2].clip = sfx;
                audioSources[2].Play();
            }
        }
        else
        {
            if (!audioSources[3].isPlaying)
            {
                audioSources[3].clip = sfx;
                audioSources[3].Play();
            }
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Finish"))
        {
            //SmallBeardoManager.Instance.animator.SetTrigger("Restart");
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }
}

