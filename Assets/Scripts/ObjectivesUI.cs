using UnityEngine;
using UnityEngine.InputSystem;

public class ObjectivesUI : MonoBehaviour
{
    [SerializeField] private GameObject objectivesPanel;
    private PlayerInput playerInput;
    private InputAction toggleObjectivesAction;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        toggleObjectivesAction = playerInput.actions["ToggleObjectives"];
        toggleObjectivesAction.performed += ToggleObjectives;
    }

    private void OnEnable()
    {
        toggleObjectivesAction?.Enable();
    }

    private void OnDisable()
    {
        toggleObjectivesAction?.Disable();
    }

    private void ToggleObjectives(InputAction.CallbackContext context)
    {
        objectivesPanel.SetActive(!objectivesPanel.activeSelf);
        // Se quiser pausar o jogo:
        // Time.timeScale = objectivesPanel.activeSelf ? 0 : 1;
        // Cursor.lockState = objectivesPanel.activeSelf ? CursorLockMode.None : CursorLockMode.Locked;
        // Cursor.visible = objectivesPanel.activeSelf;
    }
}
