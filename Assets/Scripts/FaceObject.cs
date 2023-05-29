using UnityEngine;

public class FaceObject : MonoBehaviour
{
    [SerializeField] private float _speed = 20f;
    private GameObject _player;

    private void Awake()
    {
        _player = GameObject.FindWithTag("Player");
    }

    private void FixedUpdate()
    {
        // TODO: Make rotation work correctly.
        var position = _player.transform.position;
        var relativePos = position - transform.position;
        relativePos.y = 0;
        var rotation = Quaternion.LookRotation(relativePos);
        transform.rotation = Quaternion.Lerp(transform.rotation, rotation, _speed * Time.deltaTime);
    }
}


