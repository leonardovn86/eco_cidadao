using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DialogUI : MonoBehaviour
{
    public static DialogUI Instance { get; private set; }

    [SerializeField] public GameObject dialogPanel;
    [SerializeField] public TextMeshProUGUI dialogText;
    [SerializeField] public GameObject choicePanel;
    public Button yesButton;
    public Button noButton;
    public Image playerFoto;
    public Image liderFoto;
    public Image juliaFoto;

    private Queue<string> lines = new Queue<string>();
    private Action<bool?> onDialogEnd;


    public bool IsShowing { get; private set; } = false;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }

        dialogPanel.SetActive(false);
        choicePanel.SetActive(false);
    }

    public void SelecionarFoto(String personagem)
    {
        playerFoto.gameObject.SetActive(false);
        juliaFoto.gameObject.SetActive(false);

        liderFoto.gameObject.SetActive(false);

        switch (personagem)
        {
            case "julia":
                juliaFoto.gameObject.SetActive(true);
                break;
            case "lider":
                liderFoto.gameObject.SetActive(true);
                break;
            case "player":
                playerFoto.gameObject.SetActive(true);
                break;
        }
    }

    public void ShowDialog(List<string> dialogLines, String personagem, Action<bool?> onEnd = null)
    {
        SelecionarFoto(personagem);
        if (dialogLines == null || dialogLines.Count == 0) return;

        dialogPanel.SetActive(true);
        IsShowing = true;

        lines.Clear();
        foreach (var line in dialogLines)
        {
            lines.Enqueue(line);
        }

        onDialogEnd = onEnd; // <-- Armazena ação que será chamada no final


        if (lines.Count == 1)
        {
            dialogText.text = lines.Dequeue(); // Mostra a única linha
        }
        else
        {
            ShowNextLine();
        }
    }

    public void ShowNextLine()
    {

        if (!IsShowing) return;

        if (lines.Count == 0)
        {
            EndDialog();
            return;
        }

        string line = lines.Dequeue();
        dialogText.text = line;
    }

    public void EndDialog()
    {
        dialogPanel.SetActive(false);
        IsShowing = false;

        onDialogEnd?.Invoke(null); // ou apenas onDialogEnd?.Invoke(true) se for fixo
        onDialogEnd = null;
    }

    public void OnPlayerChoice(bool choice)
    {
        dialogPanel.SetActive(false);
        IsShowing = false;
        onDialogEnd?.Invoke(choice);
    }




    public void ShowChoice(Action onYes, Action onNo)
    {
        choicePanel.SetActive(true);

        // Limpa listeners anteriores
        yesButton.onClick.RemoveAllListeners();
        noButton.onClick.RemoveAllListeners();

        // Atribui novos
        yesButton.onClick.AddListener(() =>
        {
            choicePanel.SetActive(false);
            onYes?.Invoke();
        });

        noButton.onClick.AddListener(() =>
        {
            choicePanel.SetActive(false);
            onNo?.Invoke();
        });
    }

}
