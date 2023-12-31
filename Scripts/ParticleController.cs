using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleController : MonoBehaviour
{
    [SerializeField] public ParticleSystem movementParticle;
    [SerializeField] public ParticleSystem pickupParticle;

    [Range(0, 50)]
    [SerializeField] int occurAfterVelocity;

    [Range(0, 0.2f)]
    [SerializeField] float dustFormationPeriod;

    [SerializeField] Rigidbody2D playerRb;

    float prevVelocity = 0;

    float counter;

    // Update is called once per frame
    private void Update()
    {
        counter += Time.deltaTime;
        if(Mathf.Abs(playerRb.velocity.x-prevVelocity)<30)
        {
            Camera.main.orthographicSize = (Mathf.Abs(playerRb.velocity.x) + 10) * 1.5f;
        }
        prevVelocity = playerRb.velocity.x;
        if (Mathf.Abs(playerRb.velocity.x) > occurAfterVelocity)
        {
            if (counter > dustFormationPeriod)
            {
                movementParticle.Play();
                counter = 0;
            }
        }
    }
}