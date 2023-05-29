using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    [Header("Ink JSON File")] [SerializeField]
    private TextAsset _inkJSON;

    public bool Interacted { get; private set; }
    public AudioSource audioSourceIn;
    public AudioSource audioSourceOut;

    [SerializeField] private Canvas _visualCue;
    [Header("Must be unchecked if doesn't have animations")] [SerializeField] 
    private bool _hasAnimations;
    
    private Animator _animator;
    private bool _playerInRange;

    // All OneTimeNPC must have OneTimeNPC tag on object with DialogueTrigger component
    private bool _isOneTimeNPC;

    private void Awake()
    {
        _animator = GetComponentInParent<Animator>();
        _playerInRange = false;
        _isOneTimeNPC = gameObject.CompareTag("OneTimeNPC");
    }

    private void Start()
    {
        if (!_isOneTimeNPC) return;
            _visualCue.gameObject.SetActive(true);
    }

    private void Update()
    {
        if (_playerInRange && !DialogueManager.GetInstance().DialogueIsPlaying)
        {
            _visualCue.gameObject.SetActive(true);
            if (!InputManager.GetInstance().GetInteractPressed()) return;
            DialogueManager.GetInstance().EnterDialogueMode(_inkJSON);
            ToInteractedState();
            // For One Time Interact NPC only.
            /*if (!_isOneTimeNPC) return;
            _animator.SetBool(AnimatorParameterIdList.Interacted, true);*/
        }
        /*else
            _visualCue.gameObject.SetActive(false);*/
    }

    private void OnTriggerEnter(Collider other)
    {
        audioSourceIn.Play();
        DialogueManager.GetInstance().HintPopup.SetActive(true);
        if (!other.gameObject.CompareTag("Player")) return;
        _playerInRange = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.gameObject.CompareTag("Player")) return;
        audioSourceOut.Play();
        DialogueManager.GetInstance().HintPopup.SetActive(false);
        _playerInRange = false;
        if (_isOneTimeNPC) return;
        _visualCue.gameObject.SetActive(false);
    }

    private void ToInteractedState()
    {
        Interacted = true;
        _playerInRange = false;
        DialogueManager.GetInstance().HintPopup.SetActive(false);
        _visualCue.gameObject.SetActive(false);
        var colliders = GetComponents<Collider>();
        foreach (var col in colliders)
            col.enabled = false;
        if (!_isOneTimeNPC) return;
        if (_hasAnimations)
            _animator.SetBool(AnimatorParameterIdList.Interacted, true);
    }
}
