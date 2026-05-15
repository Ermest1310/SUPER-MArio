using TMPro;
using UnityEngine;

public class LifeSceneManager : MonoBehaviour
{
    public TMP_Text livesText;

    private void Start()
    {
        // mostrar vidas
        livesText.text = "MARIO\n\nx " + GameManager.Instance.lives;

        // esperar 2 segundos
        Invoke(nameof(StartLevel), 2f);
    }

    private void StartLevel()
    {
        GameManager.Instance.LoadCurrentLevel();
    }
}