using UnityEngine;
using UnityEngine.SceneManagement;
public class GameOver : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void OnClickReintentar()
    {
        SceneManager.LoadScene("Nivel1");
    }

    public void OnClickMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
    public void OnClickIniciar()
    {
        SceneManager.LoadScene("Nivel1");
    }
}
