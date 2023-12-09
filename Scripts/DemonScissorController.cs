using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Extensions;

public class DemonScissorController : MonoBehaviour
{
    [SerializeField] int startWait = 1;
    [SerializeField] float wait = 1;

    List<Vector2> plrPositions = new List<Vector2>();
    Vector2 location;
    private float distance;
    [SerializeField] Transform plr;

    [HideInInspector] public DemonScissorController instance;
    [HideInInspector] public bool runLogger = false, runFollow = false;

    private void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (runFollow)
        {
            distance = Vector2.Distance(transform.position, location);
            Vector2 direction = location - transform.position.ToVector2();
            direction.Normalize();
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            transform.position = Vector2.MoveTowards(transform.position,location, (Vector2.Distance(transform.position,location)/wait) * Time.deltaTime);
            transform.rotation = Quaternion.Euler(Vector3.forward * angle);
        }
    }
    public void Start_()
    {
        runLogger = true;
        StartCoroutine(StartLogger());
        StartCoroutine(_.SetTimeOut(startWait , () => {
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
