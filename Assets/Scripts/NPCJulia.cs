using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using UnityEngine.UI; // Necessário para o Toggle

public class NPCJulia : MonoBehaviour
{
    public Transform player;
    public float interactionDistance = 3f;      

    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private InputAction interactAction;

    private bool isPlayerNear;

    [TextArea(3, 10)]
    public string[] dialogAntesDaMissao;
    [TextArea(3, 10)]
    public string[] dialogEntregaItens;
    [TextArea(3, 10)]
    public string[] dialogPosEntrega;

    // Referência ao estado da missão (pode vir de outro script)
    public LeaderCommunity missionScript; // Referência ao script do líder
    private bool itemEntregue = false; // Controla se Júlia já deu os sacos recicláveis

     public GameObject objectivePanel;
    public Toggle objectiveToggle;
    public Image foto;

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
        if (player == null || missionScript == null) return;

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
                MostrarDialogoDeAcordoComEstado();
            }
        }
    }

    void MostrarDialogoDeAcordoComEstado()
    {
        if (!missionScript.missionCompleted)
        {
            // Jogador ainda não aceitou a missão do líder
            DialogUI.Instance.ShowDialog(new List<string>(dialogAntesDaMissao),"julia");            
        }
        else if (missionScript.missionCompleted && !itemEntregue)
        {

            DialogUI.Instance.ShowDialog(new List<string>(dialogEntregaItens), "julia");

            // Jogador aceitou a missão, mas ainda não recebeu os itens
            itemEntregue = true;
            //objectivePanel.SetActive(true);
            GameManager.Instance.AllowTrashCollection(); // Ativa a coleta

            

            // Exibe HUD do lixo com 3 unidades
            TrashHUDManager.Instance.ShowTrashHUD(3);
            // Aqui você pode instanciar os itens no inventário do jogador, etc.             
            
            objectiveToggle.isOn = true;
            //objectivePanel.SetActive(true);
        }
        else
        {
            // Jogador já recebeu os sacos
            DialogUI.Instance.ShowDialog(new List<string>(dialogPosEntrega),"julia");
        }
    }
}
