using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using System.Collections.Generic;

public class TrashItem : MonoBehaviour
{
    public float interactionDistance = 3f;
    private Transform player;
    private PlayerInput playerInput;
    private InputAction interactAction;
    private bool isCollected = false;
    private bool isPlayerNear = false;
    public Toggle objectiveToggle;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        playerInput = GameObject.FindWithTag("Player")?.GetComponent<PlayerInput>();

        if (playerInput != null)
        {
            interactAction = playerInput.actions["Interact"];
            interactAction.performed += OnInteract;
            interactAction.Enable();
        }
    }

    private void OnDestroy()
    {
        if (interactAction != null)
            interactAction.performed -= OnInteract;
    }

    private void Update()
    {
        if (isCollected || player == null) return;

        float distance = Vector3.Distance(transform.position, player.position);
        isPlayerNear = distance <= interactionDistance;
    }

    private void OnInteract(InputAction.CallbackContext context)
    {
        if (isCollected || !isPlayerNear) return;

        if (GameManager.Instance.canCollectTrash)
        {
            CollectTrash();
        }
        else
        {
            if (!DialogUI.Instance.IsShowing)
            {
                DialogUI.Instance.ShowDialog(new List<string> {
                    "Quanta sujeira! Precisamos dar um jeito nisso."
                }, "player");
            }
            else
            {
                DialogUI.Instance.ShowNextLine();
            }
        }
    }

    void CollectTrash()
    {
        isCollected = true;
        Debug.Log("Coletando lixo...");

        TrashHUDManager.Instance.UseTrashBag();
        Destroy(gameObject);

        if (TrashHUDManager.Instance.IsTrashEmpty())
        {            
            objectiveToggle.isOn = true;            
            GameManager.Instance.ShowEndScreen();
        }
    }
}
