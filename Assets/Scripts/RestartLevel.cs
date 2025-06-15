using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartLevel : MonoBehaviour
{
    public void ReiniciarFase()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
