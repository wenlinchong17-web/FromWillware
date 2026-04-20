using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISaveable
{
    object CaptureState();        // 保存
    void RestoreState(object state); // 读取
    string GetUniqueID();         // 唯一ID
}
