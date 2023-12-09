using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SharpObject : MonoBehaviour
{
    [SerializeField] private int damage;
    [SerializeField] private float force;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<PlayerController>().health -= damage;
            collision.GetComponent<Rigidbody2D>().AddForceAtPosition(Vector2.one * force, transform.position, ForceMode2D.Impulse);
        }
    }
}
