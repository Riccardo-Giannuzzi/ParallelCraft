/*using UnityEngine;

public class TransporterBlock : IOBlock
{
    [Header("Impostazioni Trasporto")]
    public float timeToCross = 1.0f; 
    public float textureSpeed = 2.0f;

    private GameObject currentItem;
    private Vector3 startPos;
    private Vector3 centerPos;
    private Vector3 endPos;
    private float progress = 0f;

    [Header("Test Rapido")]
    public GameObject itemPartenza; // <-- AGGIUNTO: Lo slot per mettere la sfera iniziale

    protected override void Start()
    {
        base.Start(); // Questo richiama l'auto-saldatura che abbiamo messo in IOBlock
        blockID = "belt_basic"; 

        // <-- AGGIUNTO: Se hai assegnato un oggetto iniziale, lo fa partire subito!
        if (itemPartenza != null)
        {
            Transform puntoDiPartenza = inputs.Count > 0 ? inputs[0] : transform;
            ReceiveItem(itemPartenza, puntoDiPartenza);
        }
    }

    public override bool CanReceiveItem()
    {
        return currentItem == null;
    }

    public override void ReceiveItem(GameObject item, Transform entryPoint)
    {
        currentItem = item;
        
        startPos = entryPoint.position;
        endPos = output.position;
        
        centerPos = transform.position;
        
        centerPos.y = startPos.y; 
        
        progress = 0f;
    }

    void Update()
    {
        AnimateTexture();

        if (currentItem != null)
        {
            progress += (Time.deltaTime / timeToCross);

            if (progress <= 0.5f)
            {
                float t = progress * 2f;
                currentItem.transform.position = Vector3.Lerp(startPos, centerPos, t);
            }
            else if (progress <= 1.0f)
            {
                float t = (progress - 0.5f) * 2f;
                currentItem.transform.position = Vector3.Lerp(centerPos, endPos, t);
            }
            else
            {
                currentItem.transform.position = endPos;
                
                if (nextBlock != null && nextBlock.CanReceiveItem())
                {
                    // Passiamo DIRETTAMENTE il nostro output come punto di ingresso per il prossimo blocco!
                    nextBlock.ReceiveItem(currentItem, this.output); 
                    currentItem = null; 
                }
            }
        }
    }

    private void AnimateTexture()
    {
        Renderer rend = GetComponent<Renderer>();
        if (rend != null)
        {
            float offset = Time.time * textureSpeed * 0.25f;
            rend.material.mainTextureOffset = new Vector2(0, -offset);
        }
    }
}*/