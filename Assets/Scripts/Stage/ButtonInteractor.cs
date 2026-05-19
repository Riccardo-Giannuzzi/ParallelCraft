using UnityEngine;

public class ButtonInteractor : MonoBehaviour
{
    [SerializeField]
    private InputManager inputManager;

    [SerializeField]
    private Camera sceneCamera;

    [SerializeField]
    private float interactDistance = 5f;

    private void Start()
    {
        inputManager.OnClicked += TryInteract;
    }

    private void TryInteract()
    {
        Ray ray = new Ray(
            sceneCamera.transform.position,
            sceneCamera.transform.forward
        );

        if (Physics.Raycast(
            ray,
            out RaycastHit hit,
            interactDistance))
        {
            IInteractable interactable =
                hit.collider.GetComponent<IInteractable>();

            if (interactable != null)
            {
                interactable.Interact();
            }
        }
    }
}