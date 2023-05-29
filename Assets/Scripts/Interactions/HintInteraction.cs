using System;
using UnityEngine;

public class HintInteraction : MonoBehaviour
{
    private Collider _collider;
    private void Awake()
    {
        _collider = GetComponent<Collider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.CompareTag("Player")) return;
        DialogueManager.GetInstance().HintPopup.SetActive(true);
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.gameObject.CompareTag("Player")) return;
    }
}
