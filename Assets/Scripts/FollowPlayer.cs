using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    [SerializeField] private Transform _playerTransform;
    [SerializeField] private float _smoothTime = .25f;
    private float _distance;
    private Vector3 _currentVelocity;
    private Transform _pivotTransform;

    private void Awake()
    {
        _pivotTransform = GetComponent<Transform>();
    }

    private void LateUpdate()
    {
        var position = _playerTransform.position;
        var target = position + (transform.position - position).normalized * _distance;
        _pivotTransform.position = Vector3.SmoothDamp(_pivotTransform.position, target,
            ref _currentVelocity, _smoothTime);
    }
}
