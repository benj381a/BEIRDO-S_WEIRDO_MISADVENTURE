using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spin : MonoBehaviour
{

    private void Update()
    {
        transform.Rotate(Vector3.forward * -10);
    }

}