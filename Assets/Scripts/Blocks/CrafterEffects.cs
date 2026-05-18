using UnityEngine;

public class CrafterEffects : MonoBehaviour
{
    [SerializeField]
    private CrafterBlock crafter;

    [SerializeField]
    private ParticleSystem processingParticles;

    [SerializeField]
    private float particleInterval = 0.2f;

    private float particleTimer;

    
    private void Update()
    {
        bool isProcessing = (crafter.state == CrafterState.Processing);

        if (isProcessing)
        {
            particleTimer -= Time.deltaTime;

            if (particleTimer <= 0f)
            {
                processingParticles.Play();
                particleTimer = particleInterval;
            }
        }
    }
}