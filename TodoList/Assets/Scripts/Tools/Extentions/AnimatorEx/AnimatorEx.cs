using UnityEngine;

public static class AnimatorEx
{
    public static void SetAnimEventByPercentage(this Animator animator,string clipName,float percentage,string EventName)
    {
        var clips = animator.runtimeAnimatorController.animationClips;
        for (int i = 0; i < clips.Length; i++)
        {
            var clip = clips[i];
            if (clip.name == clipName)
            {
                AnimationEvent animationEvent = new AnimationEvent();
                animationEvent.functionName = EventName;
                animationEvent.time = percentage* clip.length;
                AddEvent(clip, animationEvent);
            }
        }
    }

    public static void SetAnimEventByTime(this Animator animator, string clipName, float time, string EventName)
    {
        var clips = animator.runtimeAnimatorController.animationClips;
        for (int i = 0; i < clips.Length; i++)
        {
            var clip = clips[i];
            if (clip.name == clipName)
            {
                AnimationEvent animationEvent = new AnimationEvent();
                animationEvent.functionName = EventName;
                animationEvent.time = time;
                AddEvent(clip, animationEvent);
            }
        }
    }

    static void AddEvent(AnimationClip clip, AnimationEvent animationEvent)
    {
        for (int i = 0; i < clip.events.Length; i++)
        {
            if (clip.events[i].functionName == animationEvent.functionName)
                return;
        }

        clip.AddEvent(animationEvent);
    }
}
