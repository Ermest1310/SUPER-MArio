using UnityEngine;

public class PowerUp : MonoBehaviour
{
    public enum Type
    {
        Coin,
        ExtraLife,
        MagicMushroom,
        Starpower,
    }

    public Type type;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) {
            Collect(other.gameObject);
        }
    }

    private void Collect(GameObject playerObject)
    {
        Player player = playerObject.GetComponent<Player>();

        switch (type)
        {
            case Type.Coin:

                // NUEVO - sonido moneda
                player.soundController.PlayCoinSound();

                GameManager.Instance.AddCoin();
                break;

            case Type.ExtraLife:

                // NUEVO - sonido vida
                player.soundController.PlayVidasSound();

                GameManager.Instance.AddLife();
                break;

            case Type.MagicMushroom:

                // Grow() ya reproduce sonido
                player.Grow();
                break;

            case Type.Starpower:

                // NUEVO - sonido power up
                player.soundController.PlayPowerUpSound();

                player.Starpower();
                break;
        }

        Destroy(gameObject);
    }

}