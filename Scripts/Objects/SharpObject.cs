using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SharpObject : MonoBehaviour
{
    [SerializeField] private int damage;
    [SerializeField] private float force;

    private PlayerController plrController;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            plrController = collision.GetComponent<PlayerController>();
            plrController.health -= damage;

            Vector2 colTrans_Trans = collision.transform.position - transform.position;

            collision.GetComponent<Rigidbody2D>().AddForceAtPosition(colTrans_Trans * force, transform.position, ForceMode2D.Impulse);
            
            if (damage > 0)
            {
                if (plrController.health > 0 && !collision.GetComponent<Spin>())
                {
                    plrController.PlaySfx(plrController.hurt);
                }
                else if (plrController.health > 0 && collision.GetComponent<Spin>())
                {
                    plrController.PlaySfx(plrController.buzzSawImpact);
                }
                else
                {
                    plrController.PlaySfx(plrController.die);
                }
                plrController.dammage.Play();
                StartCoroutine(HurtAnim(collision));

            }

            if (collision.GetComponent<ParticleController>())
            {
                if (damage < 0)
                {
                    collision.GetComponent<ParticleController>().pickupParticle.Play();
                    plrController.PlaySfx(plrController.pickUpImpact);
                }
            }

            if (damage < 0)
            {
                RaycastHit2D hit = Physics2D.BoxCast(plrController.swingPoint, Vector2.one, 0, Vector2.zero);
                if (hit.collider != null)
                {
                    if (hit.collider.gameObject == gameObject)
                    {
                        plrController.Realease();
                    }
                }
                

                Destroy(gameObject);
            }

            if (GetComponent<DemonScissorController>())
            {
                plrController.PlaySfx(plrController.scissorImpact);
                GetComponent<DemonScissorController>().Stop_();
                GetComponent<DemonScissorController>().Start_();
            }
        }

    }
    IEnumerator HurtAnim(Collider2D collision)
    {
        yield return new WaitForSeconds(1f);
        plrController.animator.SetTrigger("Hurt");
    }
}
