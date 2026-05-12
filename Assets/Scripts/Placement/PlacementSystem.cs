using System.Collections.Generic;
using UnityEngine;

public class PlacementSystem : MonoBehaviour
{
    [SerializeField]
    private GameObject indicator, cellIndicator;
    [SerializeField]
    private InputManager inputManager;
    [SerializeField]
    private Grid grid;
    [SerializeField]
    private PlayerInventory inventory;

    private Dictionary<Vector3Int, GameObject> placedObjects = new Dictionary<Vector3Int, GameObject>();

    private void Start()
    {
        inputManager.OnClicked += PlaceStructure;
    }

    private void Update()
    {
        if (inputManager.TryGetSelectedMapPosition(out Vector3 position))
        {
            Vector3Int gridPosition = grid.WorldToCell(position);

            indicator.SetActive(true);
            cellIndicator.SetActive(true);

            indicator.transform.position = position;
            cellIndicator.transform.position =
                grid.CellToWorld(gridPosition);
        }
        else
        {
            indicator.SetActive(false);
            cellIndicator.SetActive(false);
        }
    }

    
    private void UpdateGridConnections(Vector3Int pos, IOBlock nuovoIO)
{
    // Il blocco piazzato imposta il SUO prossimo
    Vector3Int forwardDir = Vector3Int.RoundToInt(nuovoIO.transform.forward);
    Vector3Int nextPos = pos + forwardDir;

    if (placedObjects.TryGetValue(nextPos, out GameObject targetObj))
    {
        IOBlock targetIO = targetObj.GetComponent<IOBlock>();
        if (targetIO != null) 
        {
            nuovoIO.ConnectTo(targetIO);
        }
    }

    // Controlliamo le 4 direzioni attorno a noi. 
    // Solo il vicino che "punta" verso di noi deve aggiornarsi.
    Vector3Int[] directions = { Vector3Int.forward, Vector3Int.back, Vector3Int.left, Vector3Int.right };
    
    foreach (var dir in directions)
    {
        Vector3Int neighborPos = pos + dir;
        if (placedObjects.TryGetValue(neighborPos, out GameObject neighborObj))
        {
            IOBlock neighborIO = neighborObj.GetComponent<IOBlock>();
            if (neighborIO != null)
            {
                Vector3Int neighborForward = Vector3Int.RoundToInt(neighborIO.transform.forward);
                
                if (neighborPos + neighborForward == pos)
                {
                    neighborIO.ConnectTo(nuovoIO);
                }
            }
        }
    }
}


    private void PlaceStructure()
    {
        ItemData currentItem = inventory.GetCurrentItem();

        if (currentItem == null)
            return;

        if (currentItem.itemType == ItemType.Tool)
        {
            BreakStructure();
            return;
        }

        if (currentItem.itemType == ItemType.Placeable)
        {
            PlaceBlock(currentItem);
        }
    }

    private void PlaceBlock(ItemData item)
    {
        if (!inputManager.TryGetSelectedMapPosition(out Vector3 position))
        {
            return;
        }

        Vector3Int gridPosition = grid.WorldToCell(position);

        if (placedObjects.ContainsKey(gridPosition))
        {
            return;
        }

        Vector3 worldPosition = grid.CellToWorld(gridPosition) + grid.cellSize / 2;


        /* Per ora tolto
        Quaternion currentRotation = indicator.transform.rotation;
        
        GameObject placedObject = Instantiate(
            item.placeablePrefab, 
            worldPosition, 
            currentRotation
        );*/

        // --- NUOVA LOGICA DI ROTAZIONE ---
        // Prendiamo la direzione in cui guarda la camera
        Vector3 playerForward = Camera.main.transform.forward;
        playerForward.y = 0; // Appiattiamo il vettore (niente rotazioni verso l'alto/basso)
        playerForward.Normalize();

        // Determiniamo la direzione cardinale più vicina
        Quaternion lookRotation;
        if (Mathf.Abs(playerForward.x) > Mathf.Abs(playerForward.z))
        {
            // Guarda a destra o sinistra (Est/Ovest)
            lookRotation = Quaternion.LookRotation(new Vector3(Mathf.Sign(playerForward.x), 0, 0));
        }
        else
        {
            // Guarda avanti o dietro (Nord/Sud)
            lookRotation = Quaternion.LookRotation(new Vector3(0, 0, Mathf.Sign(playerForward.z)));
        }
        // ---------------------------------

        GameObject placedObject = Instantiate(item.placeablePrefab, worldPosition, lookRotation);

        placedObject
            .GetComponent<PlaceableObject>()
            .GridPosition = gridPosition;

        placedObjects.Add(gridPosition, placedObject);

        // --- LOGICA DI CONNESSIONE ---
        IOBlock nuovoIO = placedObject.GetComponent<IOBlock>();
        if (nuovoIO != null)
        {
            UpdateGridConnections(gridPosition, nuovoIO);
        }
    }

    private void BreakStructure()
    {
        PlaceableObject target =
            inputManager.GetTargetedPlaceable();

        if (target == null)
            return;

        Vector3Int gridPosition = target.GridPosition;

        placedObjects.Remove(gridPosition);

        Destroy(target.gameObject);
    }
}





