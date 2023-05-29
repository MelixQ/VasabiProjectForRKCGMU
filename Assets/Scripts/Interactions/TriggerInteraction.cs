using UnityEngine;

public class TriggerInteraction : MonoBehaviour
{ 
    [SerializeField] private GameObject _interactableObject;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.CompareTag("Player")) return;
        
        if (_interactableObject.TryGetComponent<AudioInteraction>(out var audioInteraction)) audioInteraction.Interact();
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.gameObject.CompareTag("Player")) return;
        if (TryGetComponent<MeshCollider>(out var meshcoll)) meshcoll.enabled = false;
        if (TryGetComponent<Collider>(out var col)) col.enabled = false;
    }
}