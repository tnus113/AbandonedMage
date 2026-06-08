using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    [Header("Scene Transition Settings")]
    [SerializeField] private string nextSceneName;

    [Header("Portal Elements")]
    [SerializeField] private GameObject portalVisual;
    [SerializeField] private Collider portalCollider;

    [Header("Audio")]
    [SerializeField] private string activationSoundName = "Checkpoint";

    private void Start()
    {
        if (portalCollider == null)
        {
            portalCollider = GetComponent<Collider>();
        }

        if (portalVisual != null)
        {
            portalVisual.SetActive(false);
        }
        if (portalCollider != null)
        {
            portalCollider.enabled = false;
        }

        HealthSystem.OnBossDefeated += ActivatePortal;
    }

    private void OnDestroy()
    {
        HealthSystem.OnBossDefeated -= ActivatePortal;
    }

    public void ActivatePortal()
    {
        if (portalVisual != null)
        {
            portalVisual.SetActive(true);
        }

        if (portalCollider != null)
        {
            portalCollider.enabled = true;
        }

        if (SoundManager.Instance != null && !string.IsNullOrEmpty(activationSoundName))
        {
            SoundManager.Instance.PlaySound3D(activationSoundName, transform.position);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (other.TryGetComponent<PlayerMovement>(out var player))
            {
                player.PrepareForSceneTransition();
            }

            if (LevelManager.Instance != null)
            {
                LevelManager.Instance.LoadScene(nextSceneName, "CrossFade");
            }
            else
            {
                UnityEngine.SceneManagement.SceneManager.LoadScene(nextSceneName);
            }
        }
    }
}
