using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private float minDistance;

    private bool moved = false;

    // Update is called once per frame
    void Update()
    {

        Transform plrTransform = GameObject.FindGameObjectsWithTag("Player")[0].transform;

        if (!moved && Mathf.Abs(Vector3.Distance(transform.position, plrTransform.position)) > minDistance)
        {
            moved = true;
            StartCoroutine(move());
        }
    }
    IEnumerator move()
    {
        Transform plrTransform = GameObject.FindGameObjectsWithTag("Player")[0].transform;
        transform.position = Vector3.Lerp(transform.position, plrTransform.position, 10 / Vector3.Distance(transform.position, plrTransform.position));
        transform.position = new Vector3(transform.position.x, transform.position.y, -10);
        yield return new WaitForFixedUpdate();
        moved = false;
    }
}
