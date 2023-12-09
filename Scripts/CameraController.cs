using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private float minDistance;

    // Update is called once per frame
    void Update()
    {
        Transform plrTransform = GameObject.FindGameObjectsWithTag("Player")[0].transform;

        if (Vector3.Distance(transform.position, plrTransform.position) > minDistance)
        {
            transform.position = Vector3.Lerp(transform.position, plrTransform.position, 1 / Vector3.Distance(transform.position, plrTransform.position));
            transform.position = new Vector3(transform.position.x, transform.position.y, -10);
        }
    }
}
