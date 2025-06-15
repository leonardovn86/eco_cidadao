using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public bool canCollectTrash = false;
    [SerializeField] private GameObject endScreen;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);  // Importante: Não destruir o objeto errado!
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);  // Faz o GameManager sobreviver entre cenas
        }

        if (endScreen != null)
            endScreen.SetActive(false);
    }

    public void ShowEndScreen()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        if (endScreen != null)
            endScreen.SetActive(true);

        Time.timeScale = 0;
    }

    public void AllowTrashCollection()
    {
        canCollectTrash = true;
    }

    public void RestartLevel()
    {
        Time.timeScale = 1;  // Volta o tempo ao normal (caso tenha pausado)

        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
    }
}
