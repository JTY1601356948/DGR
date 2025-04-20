using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationTriggers : MonoBehaviour
{
    private Player player=>GetComponentInParent<Player>();      //从父节点Player中调用对象player
    //  =>相当于{get;}设置了只读属性并赋值

    private void AnimationTrigger()
    {
        player.AnimationTrigger();      //使其可调用至player
        // 动画组件通常是依附于一个特定的游戏对象（这里是Animator），并且用于控制该对象上的动画
        //然后动画事件又只能调用必须位于与Animator组件相同的GameObject上的函数
        //所以他新创建了一个脚本，然后Player player ，然后在这个脚本里调用trigger
        //然后把这个脚本放到要触发的地方， 触发这个trigger
    }
}
