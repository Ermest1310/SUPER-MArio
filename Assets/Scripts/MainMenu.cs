using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public void PlayGame()
    {
        // iniciar juego
        GameManager.Instance.NewGame();
    }

    public void ExitGame()
    {
        // salir del juego
        Application.Quit();

        // para probar en Unity
        Debug.Log("Salir del juego");
    }
}