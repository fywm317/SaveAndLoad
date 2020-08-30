using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData
{
    [Tooltip("当前激活的Target集合")]
    public List<int> targetPosList = new List<int>();
    [Tooltip("当前激活的Target的Monster类型集合")]
    public List<int> monsterTypeList = new List<int>();
    [Tooltip("射击次数")]
    public int shootAmount;
    [Tooltip("击中次数")]
    public int hitAmount;
}
