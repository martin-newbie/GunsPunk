using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;

public class SpineAnimationBehaviour : StateMachineBehaviour
{

    public AnimationClip motion;
    string animationClip;
    bool loop;

    [Header("Spine Motion Layer")]
    public int layer = 0;
    public float timeScale = 1.0f;
    public SkeletonAnimation skeletonAnimation;
    public Spine.AnimationState spineAnimationState;
    public Spine.TrackEntry trackEntry;

    void Start()
    {
        if(motion != null)
        animationClip = motion.name;

        Debug.Log(animationClip);
    }

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(skeletonAnimation == null)
        {
            skeletonAnimation = animator.GetComponentInChildren<SkeletonAnimation>();
            spineAnimationState = skeletonAnimation.state;
        }

        if(animationClip != null)
        {
            loop = stateInfo.loop;
            trackEntry = spineAnimationState.SetAnimation(layer, animationClip, loop);
            trackEntry.TimeScale = timeScale;
        }
    }

}
