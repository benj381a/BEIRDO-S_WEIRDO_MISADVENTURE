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

            Vector2 colTrans_Trans = collision.transform.position - transform.position;

            collision.GetComponent<Rigidbody2D>().AddForceAtPosition(colTrans_Trans * force, transform.position, ForceMode2D.Impulse);
            
            if (damage > 0)
            {
                collision.GetComponent<PlayerController>().dammage.Play();
            }
            
            if (damage < 0)
            {
                Destroy(gameObject);
            }

            if (GetComponent<DemonScissorController>())
            {
                GetComponent<DemonScissorController>().Stop_();
                GetComponent<DemonScissorController>().Start_();
            }
        }

        if (collision.GetComponent<ParticleController>())
        {
            if (damage < 0)
            {
                collision.GetComponent<ParticleController>().pickupParticle.Play();
            }
        }

    }
}
