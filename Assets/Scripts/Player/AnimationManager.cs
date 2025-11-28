using UnityEngine;
using System;

public class AnimationManager : MonoBehaviour
{
    [SerializeField] Animator Animator;

    public void SetAnimation(PlayerAnimation animation)
    {
        foreach (PlayerAnimation value in Enum.GetValues(typeof(PlayerAnimation)))
        {
            Animator.SetBool(value.ToString(), false);
        }

        Animator.SetBool(animation.ToString(), true);
    }
}

public enum PlayerAnimation
{
    Idle,
    Movement,
    Wings
}