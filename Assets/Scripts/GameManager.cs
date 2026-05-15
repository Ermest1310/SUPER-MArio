using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public int world { get; private set; }
    public int stage { get; private set; }
    public int lives { get; private set; }
    public int coins { get; private set; }

    private void Awake()
    {
        if (Instance != null)
        {
            DestroyImmediate(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }

    private void Start()
    {
        Application.targetFrameRate = 60;

        // IMPORTANTE:
        // Ya NO iniciamos automáticamente el juego
        // porque ahora existe MainMenu
    }

    // INICIAR NUEVA PARTIDA
    public void NewGame()
    {
        lives = 3;
        coins = 0;

        LoadLevel(1, 1);
    }

    // GAME OVER
    public void GameOver()
    {
        // futuro: pantalla game over

        SceneManager.LoadScene("MainMenu");
    }

    // CARGAR NIVEL
    public void LoadLevel(int world, int stage)
    {
        this.world = world;
        this.stage = stage;

        // primero mostrar pantalla de vidas
        SceneManager.LoadScene("LifeScene");
    }

    // CARGAR NIVEL REAL
    public void LoadCurrentLevel()
    {
        // música
        if (MusicManager.Instance != null)
        {
            MusicManager.Instance.PlayMusic();
        }

        SceneManager.LoadScene($"{world}-{stage}");
    }

    // SIGUIENTE NIVEL
    public void NextLevel()
    {
        LoadLevel(world, stage + 1);
    }

    // REINICIAR NIVEL
    public void ResetLevel(float delay)
    {
        CancelInvoke(nameof(ResetLevel));

        // detener música
        if (MusicManager.Instance != null)
        {
            MusicManager.Instance.StopMusic();
        }

        Invoke(nameof(ResetLevel), delay);
    }

    // REINICIO REAL
    public void ResetLevel()
    {
        lives--;

        if (lives > 0)
        {
            LoadLevel(world, stage);
        }
        else
        {
            GameOver();
        }
    }

    // MONEDAS
    public void AddCoin()
    {
        coins++;

        // actualizar HUD
        if (GameHUD.Instance != null)
        {
            GameHUD.Instance.UpdateCoins();
            GameHUD.Instance.AddScore(100);
        }

        // 100 monedas = vida extra
        if (coins >= 100)
        {
            coins = 0;
            AddLife();
        }
    }

    // VIDA EXTRA
    public void AddLife()
    {
        lives++;
    }
}