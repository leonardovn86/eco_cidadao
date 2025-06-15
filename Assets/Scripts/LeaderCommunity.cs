using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using UnityEngine.UI; // Necessário para o Toggle

public class LeaderCommunity : MonoBehaviour
{
    public Transform player;
    public float interactionDistance = 3f;

    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private InputAction interactAction;

    private bool isPlayerNear;

    [TextArea(3, 10)]
    public string[] dialogLines;

    public GameObject objectivePanel;
    public Toggle objectiveToggle;

    public bool missionCompleted = false;


    private void Awake()
    {
        playerInput = GameObject.FindWithTag("Player")?.GetComponent<PlayerInput>();
        if (playerInput != null)
        {
            interactAction = playerInput.actions["Interact"];
        }
    }

    private void OnEnable() => interactAction?.Enable();
    private void OnDisable() => interactAction?.Disable();

   void Update()
{
    if (player == null) return;

    float distance = Vector3.Distance(transform.position, player.position);
    isPlayerNear = distance <= interactionDistance;

    if (isPlayerNear && interactAction.triggered)
{
    if (DialogUI.Instance.IsShowing)
    {
        DialogUI.Instance.ShowNextLine();
    }
    else
    {
        if (missionCompleted)
        {
            DialogUI.Instance.ShowDialog(
                new List<string> { "Bom trabalho, continue ajudando a comunidade!" },"lider"
            );
        }
        else
        {
            DialogUI.Instance.ShowDialog(
                new List<string>(dialogLines),
                "lider",
                (_) => ShowChoicePanel() // Chama painel de escolha ao fim do diálogo
            );
        }
    }
}

}


void ShowChoicePanel()
{
    EnableChoiceMode();

    DialogUI.Instance.ShowChoice(
        onYes: () =>
        {
            DisableChoiceMode();
            OnDialogComplete(true);
        },
        onNo: () =>
        {
            DisableChoiceMode();
            OnDialogComplete(false);
        }
    );  
}

    void EnableChoiceMode()
{
    Cursor.lockState = CursorLockMode.None;
    Cursor.visible = true;
    if (playerInput != null)
        playerInput.DeactivateInput(); // Impede movimentação
}

void DisableChoiceMode()
{
    Cursor.lockState = CursorLockMode.Locked;
    Cursor.visible = false;
    if (playerInput != null)
        playerInput.ActivateInput(); // Restaura o controle
}
private void OnDialogComplete(bool? playerChoice)
{
        if (playerChoice == true) // Jogador escolheu "Sim"
        {
            // Diálogo de agradecimento
            DialogUI.Instance.ShowDialog(new List<string> { "Obrigado por ajudar! Procure a Júlia para pegar os sacos de lixo." }, "lider");
            //objectivePanel.SetActive(true);            
            objectiveToggle.isOn = true;
            missionCompleted = true;
    }
        else if (playerChoice == false) // Jogador escolheu "Não"
        {
            DialogUI.Instance.ShowDialog(new List<string> { "Tudo bem, nos avise se mudar de ideia." },"ider");
        }
        else
        {
            // O diálogo terminou sem escolha (pular/fechar), não faz nada
        }
}


    void OnYesChosen()
    {
        Debug.Log("Jogador escolheu SIM");
        // Aqui você pode iniciar uma missão, abrir inventário, etc.
    }

    void OnNoChosen()
    {
        Debug.Log("Jogador escolheu NÃO");
        // Pode apenas continuar o jogo
    }
}
