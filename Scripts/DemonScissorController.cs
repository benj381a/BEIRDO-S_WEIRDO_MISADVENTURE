using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Extensions;

public class DemonScissorController : MonoBehaviour
{
    [SerializeField] int startWait = 1;
    [SerializeField] float wait = 1;

    List<Vector2> plrPositions;
    Vector2 location;
    Transform plr;

    [HideInInspector] public DemonScissorController instance;
    [HideInInspector] public bool runLogger = false, runFollow = false;

    private void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        plr = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (runFollow)
        {
            transform.position = Vector2.MoveTowards(transform.position, location, plr.GetComponent<PlayerController>().speed * Time.deltaTime);
        }
    }
    public void Start_()
    {
        runLogger = true;
        StartCoroutine(StartLogger());
        StartCoroutine(_.SetTimeOut(startWait, () => {
            runFollow = true;
            StartCoroutine(StartFollow());
        }));
    }
    public void Stop_()
    {
        runLogger = false;
        runFollow = false;
        plrPositions.Clear();
    }

    private IEnumerator StartLogger()
    {
        while (runLogger)
        {
            plrPositions.Add(plr.position);
            yield return new WaitForSeconds(wait);
        }
    }
    private IEnumerator StartFollow()
    {
        while (runFollow)
        {
            location = plrPositions[0];
            plrPositions.RemoveAt(0);
            yield return new WaitForSeconds(wait);
        }
    }
}
