using UnityEngine;

/// <summary>
/// Handles the visual effects for the CrafterBlock, specifically managing the particle system that plays during the processing state.
/// </summary>
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