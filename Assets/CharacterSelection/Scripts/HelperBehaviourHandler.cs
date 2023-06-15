using System;
using Unity.VisualScripting;
using UnityEngine;

public class HelperBehaviourHandler : MonoBehaviour
{
    private Outline _outline;
    private Animator _animator;
    private Collider _collider;
    [SerializeField] private TextAsset _inkJSON;
    //[SerializeField] private GameObject _selectCanvas;

    private void Awake()
    {
        _collider = GetComponent<Collider>();
        _animator = GetComponent<Animator>();
        _outline = GetComponent<Outline>();
    }

    //private void OnEnable() => DialogueManager.Exited += EnableSelectCanvas;

    /*private void EnableSelectCanvas()
    {
        _collider.enabled = true;
        _selectCanvas.SetActive(true);
    }*/

    private void OnMouseEnter()
    {
        _outline.enabled = true;
        _animator.SetBool(AnimatorParameterIdList.Pointed, true);
    }

    private void OnMouseExit()
    {
        _outline.enabled = false;
        _animator.SetBool(AnimatorParameterIdList.Pointed, false);
    }

    private void OnMouseDown()
    {
        //_selectCanvas.SetActive(false);
        _collider.enabled = false;
        HelperManager.GetInstance().EnterDialogueMode(_inkJSON);
    }

    //private void OnDisable() => DialogueManager.Exited -= EnableSelectCanvas;
}
