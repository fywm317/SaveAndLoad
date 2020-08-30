using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetManager : MonoBehaviour
{
    [Tooltip("4种怪物数组")]
    public GameObject[] monsters;
    [Tooltip("当前激活的monster")]
    public GameObject activeMonster = null;
    [Tooltip("Target编号，0-9,Save/Load用")]
    public int tagetPos;

    private void Awake()
    {

    }

    // Start is called before the first frame update
    void Start()
    {
        foreach (GameObject monster in monsters) {
            monster.GetComponent<BoxCollider>().enabled = false;
            monster.SetActive(false);
        }
        StartCoroutine("AliveTimer");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// 随机激活monster
    /// </summary>
    private void ActivateMonster() {
        int index = Random.Range(0, monsters.Length);
        activeMonster = monsters[index];
        activeMonster.SetActive(true);
        activeMonster.GetComponent<BoxCollider>().enabled = true;
        StartCoroutine("DeathTimer");
    }

    /// <summary>
    /// 协程，随机等待1-5秒，激活monster
    /// </summary>
    /// <returns></returns>
    private IEnumerator AliveTimer() {
        yield return new WaitForSeconds(Random.Range(3, 5));
        ActivateMonster();
    }

    /// <summary>
    /// 关闭激活
    /// </summary>
    private void DeActivateMonster() {
        if (activeMonster != null) {
            activeMonster.GetComponent<BoxCollider>().enabled = false;
            activeMonster.SetActive(false);
            activeMonster = null;
        }
        StartCoroutine("AliveTimer");
    }

    private IEnumerator DeathTimer()
    {
        yield return new WaitForSeconds(Random.Range(4, 10));
        DeActivateMonster();
    }

    /// <summary>
    /// 重新生成monster
    /// </summary>
    public void UpdateMonster() {
        StopAllCoroutines(); //停止所有协程
        if (activeMonster != null) {
            activeMonster.GetComponent<BoxCollider>().enabled = false;
            activeMonster.SetActive(false);
            activeMonster = null;
        }
        StartCoroutine("AliveTimer"); //重新开始协程，生成monster
    }

    /// <summary>
    /// 读取游戏时根据存档记录初始化Monster
    /// </summary>
    /// <param name="type"></param>
    public void LoadMonsterByType(int type) {
        StopAllCoroutines(); //停止所有协程
        if (activeMonster != null)
        {
            activeMonster.GetComponent<BoxCollider>().enabled = false;
            activeMonster.SetActive(false);
            activeMonster = null;
        }
        activeMonster = monsters[type];
        activeMonster.SetActive(true);
        activeMonster.GetComponent<BoxCollider>().enabled = true;
        StartCoroutine("DeathTimer"); //重新开始协程循环
    }

}
