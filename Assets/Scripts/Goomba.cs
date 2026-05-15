using UnityEngine;

public class Goomba : MonoBehaviour
{
    public Sprite flatSprite;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Player player = collision.gameObject.GetComponent<Player>();

            if (player.starpower) {
                Hit(player);
            } 
            else if (collision.transform.DotTest(transform, Vector2.down)) {
                Flatten(player);
            } 
            else {
                player.Hit();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Shell")) {
            Hit(null);
        }
    }

    private void Flatten(Player player)
    {
        // sonido enemigo derrotado
        if (player != null)
        {
            player.soundController.PlayEnemyKillSound();
        }

        // NUEVO - sumar score
        if (GameHUD.Instance != null)
        {
            GameHUD.Instance.AddScore(200);
        }

        GetComponent<Collider2D>().enabled = false;
        GetComponent<EntityMovement>().enabled = false;
        GetComponent<AnimatedSprite>().enabled = false;
        GetComponent<SpriteRenderer>().sprite = flatSprite;

        Destroy(gameObject, 0.5f);
    }

    private void Hit(Player player)
    {
        // sonido enemigo derrotado
        if (player != null)
        {
            player.soundController.PlayEnemyKillSound();
        }

        // NUEVO - sumar score
        if (GameHUD.Instance != null)
        {
            GameHUD.Instance.AddScore(200);
        }

        GetComponent<AnimatedSprite>().enabled = false;
        GetComponent<DeathAnimation>().enabled = true;

        Destroy(gameObject, 3f);
    }

}