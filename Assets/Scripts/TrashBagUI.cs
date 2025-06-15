using UnityEngine;
using TMPro;

public class TrashHUDManager : MonoBehaviour
{
    public static TrashHUDManager Instance { get; private set; }

    [SerializeField] private GameObject trashHUD;
    [SerializeField] private TextMeshProUGUI trashCountText;

    

    private int trashCount = 0;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }

        trashHUD.SetActive(false); // Esconde no início
    }

    public void ShowTrashHUD(int initialCount)
    {
        trashCount = initialCount;
        UpdateTrashCountUI();
        trashHUD.SetActive(true);        
    }

    public void UseTrashBag()
    {
        if (trashCount > 0)
        {
            trashCount--;
            UpdateTrashCountUI();

            if (trashCount == 0)
                trashHUD.SetActive(false);
        }
    }
    public bool IsTrashEmpty()
{
    return trashCount <= 0;
}

    private void UpdateTrashCountUI()
    {
        trashCountText.text = "x" + trashCount.ToString();
    }
}
