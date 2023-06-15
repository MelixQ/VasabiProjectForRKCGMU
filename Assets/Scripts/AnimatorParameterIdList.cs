using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorParameterIdList : MonoBehaviour
{
    public static readonly int Interacted = Animator.StringToHash("Interacted");
    public static readonly int IsWalking = Animator.StringToHash("IsWalking");
    public static readonly int Default = Animator.StringToHash("default");
    public static readonly int Big = Animator.StringToHash("big");
    public static readonly int Pointed = Animator.StringToHash("Pointed");
}
