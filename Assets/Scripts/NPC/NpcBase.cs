using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcBase : MonoBehaviour
{
    private static bool created = false;

    void Awake()
    {
        if (!created)
        {
            // 使 GameObject 横跨多个场景时不被销毁
            DontDestroyOnLoad(this.gameObject);
            created = true;
        }
        else
        {
            // 如果已经有一个相同的 GameObject 存在，则销毁这个新的实例
            Destroy(this.gameObject);
        }
    }

}
