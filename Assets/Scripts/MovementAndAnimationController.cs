using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class MovementAndAnimationController : MonoBehaviour
{
    // TODO: Delete obsolete movement code based on InputAction _mouseClickAction
    private const float Gravity = -9.81f;
    
    [SerializeField] private float _speed = 7f;
    [SerializeField] private float _rotationSpeed = 4f;
    public AudioSource walkingAudio;

    private CharacterController _characterController;
    private Animator _animator;
    private Camera _mainCamera;
    private Coroutine _coroutine;

    private bool _isInputGiven; 
    private int _groundLayer;

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
        _animator = GetComponent<Animator>();
        _mainCamera = Camera.main;
        _groundLayer = LayerMask.NameToLayer("Ground");
    }
    
    /*private void OnEnable()
    {
        _mouseClickAction.Enable();
        _mouseClickAction.started += MovePlayer;
    }*/

    private void Update()
    {
        HandleAnimation();
        if (!Mouse.current.leftButton.isPressed) return;
        MovePlayer();
    }

    private void HandleAnimation()
    {
        var isWalking = _animator.GetBool(AnimatorParameterIdList.IsWalking);
        switch (_isInputGiven)
        {
            case true when !isWalking:
                walkingAudio.Play();
                _animator.SetBool(AnimatorParameterIdList.IsWalking, true);
                break;
            case false when isWalking:
                walkingAudio.Pause();
                _animator.SetBool(AnimatorParameterIdList.IsWalking, false);
                break;
        }
    }

    private void MovePlayer()
    {
        if (DialogueManager.GetInstance().DialogueIsPlaying) return;
        var ray = _mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());
        if (!Physics.Raycast(ray, out var hit) || 
            !hit.collider || 
            hit.collider.gameObject.layer.CompareTo(_groundLayer) != 0) return;
        if (_coroutine != null) StopCoroutine(_coroutine);
        _coroutine = StartCoroutine(MoveTowardsDestination(hit.point));
    }

    private IEnumerator MoveTowardsDestination(Vector3 destination)
    {
        var playerDistanceToFloor = transform.position.y - destination.y;
        destination.y += playerDistanceToFloor;
        _isInputGiven = true;
        while (Vector3.Distance(transform.position, destination) > .1f)
        {
            var direction = destination - transform.position;
            var movement = direction.normalized * (_speed * Time.deltaTime);
            // Gravity
            if (_characterController.isGrounded) movement.y = -.05f;
            else movement.y += Gravity * Time.deltaTime;
            // Rotation
            var playerTransform = transform;
            var positionToLookAt = new Vector3
            {
                x = direction.normalized.x,
                y = 0.0f,
                z = direction.normalized.z
            };
            var currentRotation = playerTransform.rotation;
            transform.rotation = Quaternion.Slerp(currentRotation,
                Quaternion.LookRotation(positionToLookAt),
                _rotationSpeed * Time.deltaTime);
            // Movement
            _characterController.Move(movement);

            yield return null;
        }

        _isInputGiven = false;
    }

    /*private void OnDisable()
    {
        _mouseClickAction.started -= MovePlayer;
        _mouseClickAction.Disable();
    }*/
}