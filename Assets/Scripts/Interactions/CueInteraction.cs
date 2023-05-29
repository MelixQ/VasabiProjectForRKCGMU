using UnityEngine;

public class CueInteraction : InteractionBase
{
    private Canvas _visualCue;

    private void Awake()
    {
        _visualCue = GetComponentInChildren<Canvas>();
        _visualCue.gameObject.SetActive(false);
    }

    public override void Interact() => _visualCue.gameObject.SetActive(true);
}
