using UnityEngine;

public static class AnimationEx
{
    #region 动画的特殊播放方式
    /// <summary>
    /// 动画正播
    /// </summary>
    /// <param name="anim"></param>
    /// <param name="clipName"></param>
    public static void PlayForward(this Animation anim, string clipName)
    {
        var animationState = anim[clipName];
        if (animationState != null)
        {
            PlayAnim(anim, animationState, clipName, 0, 1);
        }
        else
        {
            Debug.LogError("没有这个动画  ====  " + clipName);
        }
    }

    /// <summary>
    /// 动画倒播
    /// </summary>
    /// <param name="anim"></param>
    /// <param name="clipName"></param>
    public static void PlayBackward(this Animation anim, string clipName)
    {
        var animationState = anim[clipName];
        if (animationState != null)
        {
            PlayAnim(anim, animationState, clipName, animationState.clip.length, -1);
        }
        else
        {
            Debug.LogError("没有这个动画  ====  " + clipName);
        }
    }

    private static void PlayAnim(Animation anim, AnimationState animationState, string clipName, float animTime, float animSpeed)
    {
        if (anim.isPlaying)
        {
            anim.Stop();
        }

        animationState.time = animTime;
        anim.Sample();
        animationState.speed = animSpeed;
        anim.Play(clipName);
    }

    /// <summary>
    /// 动画是否正在正播  (一般用在动画事件中  根据正播还是倒播的状态控制动画事件的执行)
    /// </summary>
    /// <param name="anim"></param>
    /// <param name="clipName">动画名称</param>
    /// <returns></returns>
    public static bool IsAnimForwardPlaying(this Animation anim, string clipName)
    {
        var animationState = anim[clipName];
        if (animationState != null)
        {
            if (animationState.speed > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            Debug.LogError("没有这个动画  ====  " + clipName);
            return true;
        }
    }
    #endregion

    /// <summary>
    /// 设置动画倒特定的一帧
    /// </summary>
    /// <param name="anim"></param>
    /// <param name="clipName"></param>
    /// <param name="time"></param>
    public static void SetAnimFrame(this Animation anim, string clipName, float time)
    {
        var animationState = anim[clipName];
        if (animationState != null)
        {
            if (anim.isPlaying)
            {
                anim.Stop();
            }

            try
            {
                //解决 设置时间无效问题
                animationState.time = time;
                animationState.speed = 0;
                anim.Play(clipName);
                anim.Sample();
                animationState.speed = 1;
                anim.Stop(clipName);
            }
            catch (System.Exception e)
            {
                Debug.LogError(e.Message);
            }
        }
        else
        {
            Debug.LogError("没有这个动画  ====  " + clipName);
        }
    }

    #region 添加动画事件

    /// <summary>
    /// 检查是否已存在该动画事件
    /// </summary>
    /// <param name="animationState"></param>
    /// <param name="funcName"></param>
    /// <returns></returns>
    private static bool CheckAnimEventIsLegal(AnimationState animationState, string funcName, float triggerTime)
    {
        if (triggerTime > animationState.clip.length || triggerTime < 0)
        {
            Debug.LogError("输入的时间超出动画时长 或 小于0");
            return true;
        }

        var events = animationState.clip.events;
        for (var i = 0; i < events.Length; i++)
        {
            var animEvent = events[i];

            if (animEvent.functionName.Equals(funcName))
            {
                //Debug.LogError("重复添加动画事件    " + funcName);
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// 添加动画事件
    /// </summary>
    /// <param name="animationState"></param>
    /// <param name="clipName"></param>
    /// <param name="triggerTime"></param>
    /// <param name="funcName"></param>
    /// <param name="arg"></param>
    private static void AddAnimEvent(AnimationState animationState, string clipName, float triggerTime, string funcName, object arg)
    {

        if (CheckAnimEventIsLegal(animationState, funcName, triggerTime)) return;

        AnimationEvent animationEvent = new AnimationEvent();
        animationEvent.functionName = funcName;
        animationEvent.time = triggerTime;

        if (arg != null)
        {
            if (arg is int)
                animationEvent.intParameter = (int)arg;
            else if (arg is float)
                animationEvent.floatParameter = (float)arg;
            else if (arg is string)
                animationEvent.stringParameter = (string)arg;
            else if (arg is UnityEngine.Object)
                animationEvent.objectReferenceParameter = (UnityEngine.Object)arg;
        }

        animationState.clip.AddEvent(animationEvent);

    }

    /// <summary>
    /// 添加动画结束事件
    /// </summary>
    /// <param name="anim"></param>
    /// <param name="clipName">动画名称</param>
    /// <param name="funcName">函数名称</param>
    /// <param name="arg">传递的参数   如果是传递一个Object也就是一个类，那么这个类要继承ScriptableObject才能被传递</param>
    public static void AddAnimCallBackEnd(this Animation anim, string clipName, string funcName, object arg = null)
    {
        var animationState = anim[clipName];
        if (animationState != null)
        {
            AddAnimEvent(animationState, clipName, animationState.clip.length, funcName, arg);
        }
        else
        {
            Debug.LogError("没有这个动画  ====  " + clipName);
        }
    }

    /// <summary>
    /// 添加动画开始事件
    /// </summary>
    /// <param name="anim"></param>
    /// <param name="clipName">动画名称</param>
    /// <param name="funcName">函数名称</param>
    /// <param name="arg">传递的参数   如果是传递一个Object也就是一个类，那么这个类要继承ScriptableObject才能被传递</param>
    public static void AddAnimCallBackStart(this Animation anim, string clipName, string funcName, object arg = null)
    {
        var animationState = anim[clipName];
        if (animationState != null)
        {
            AddAnimEvent(animationState, clipName, 0, funcName, arg);
        }
        else
        {
            Debug.LogError("没有这个动画  ====  " + clipName);
        }
    }

    /// <summary>
    /// 在动画的某个时间添加事件
    /// </summary>
    /// <param name="anim"></param>
    /// <param name="clipName">动画名称</param>
    /// <param name="triggerTime">在动画播到哪个时间段调用</param>
    /// <param name="funcName">函数名称</param>
    /// <param name="arg">传递的参数   如果是传递一个Object也就是一个类，那么这个类要继承ScriptableObject才能被传递</param>
    public static void AddAnimCallBackInSomeTime(this Animation anim, string clipName, float triggerTime, string funcName, object arg = null)
    {
        var animationState = anim[clipName];
        if (animationState != null)
        {
            AddAnimEvent(animationState, clipName, triggerTime, funcName, arg);
        }
        else
        {
            Debug.LogError("没有这个动画  ====  " + clipName);
        }
    }
    #endregion
}
