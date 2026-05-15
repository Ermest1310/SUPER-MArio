using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSoundController : MonoBehaviour
{
    public AudioSource audioSource;

    [Header("Sonidos Player")]
    public AudioClip vidasClip;
    public AudioClip saltoPequenoClip;
    public AudioClip saltoGrandeClip;
    public AudioClip muerteClip;
    public AudioClip banderaClip;
    public AudioClip gameOverClip;
    public AudioClip powerUpClip;
    public AudioClip powerDownClip;

    [Header("Sonidos Extras")]
    public AudioClip coinClip;
    public AudioClip enemyKillClip;

    public void PlayVidasSound()
    {
        audioSource.PlayOneShot(vidasClip);
    }

    public void PlaySaltoPequenoSound()
    {
        audioSource.PlayOneShot(saltoPequenoClip);
    }

    public void PlaySaltoGrandeSound()
    {
        audioSource.PlayOneShot(saltoGrandeClip);
    }

    public void PlayMuerteSound()
    {
        audioSource.PlayOneShot(muerteClip);
    }

    public void PlayBanderaSound()
    {
        audioSource.PlayOneShot(banderaClip);
    }

    public void PlayGameOverSound()
    {
        audioSource.PlayOneShot(gameOverClip);
    }

    public void PlayPowerUpSound()
    {
        audioSource.PlayOneShot(powerUpClip);
    }

    public void PlayPowerDownSound()
    {
        audioSource.PlayOneShot(powerDownClip);
    }

    // NUEVO
    public void PlayCoinSound()
    {
        audioSource.PlayOneShot(coinClip);
    }

    // NUEVO
    public void PlayEnemyKillSound()
    {
        audioSource.PlayOneShot(enemyKillClip);
    }
}