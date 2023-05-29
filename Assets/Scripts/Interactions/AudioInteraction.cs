using System;
using UnityEngine;

public class AudioInteraction : InteractionBase
{
    private AudioSource _audioSource;
    private DialogueTrigger _dialogueTrigger;
    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        _dialogueTrigger = GetComponent<DialogueTrigger>();
    }

    private void Update()
    {
        if (!_dialogueTrigger.Interacted) return;
        _audioSource.Stop();
    }

    public override void Interact() => _audioSource.Play();
}
