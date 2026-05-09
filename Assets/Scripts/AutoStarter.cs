using UnityEngine;

public class AutoStarter : MonoBehaviour
{
    [Header("I tuoi blocchi")]
    public TransporterBlock blocco1;
    public TransporterBlock blocco2;
    public TransporterBlock blocco3;

    [Header("La sfera nella scena")]
    public GameObject sferaDaTrasportare;

    void Start()
    {
        // 1. Saldiamo i blocchi tra loro (così si passano gli input in automatico)
        if (blocco1 != null && blocco2 != null) blocco1.ConnectTo(blocco2);
        if (blocco2 != null && blocco3 != null) blocco2.ConnectTo(blocco3);

        // 2. Prendiamo la sfera e la buttiamo sul primo blocco
        if (blocco1 != null && sferaDaTrasportare != null && blocco1.CanReceiveItem())
        {
            // Troviamo da dove farla partire (dal suo InputPoint se c'è, altrimenti dal centro del blocco)
            Transform puntoDiPartenza = blocco1.inputs.Count > 0 ? blocco1.inputs[0] : blocco1.transform;
            
            // Diciamo al blocco di iniziare a trasportare la sfera
            blocco1.ReceiveItem(sferaDaTrasportare, puntoDiPartenza);
        }
    }
}