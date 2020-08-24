using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Tooltip("菜单")]
    public GameObject menuObject;
    [Tooltip("游戏暂停标识")]
    public bool isPause;
    public GameObject[] targets;
    public Toggle musicToggle;
    private AudioSource music;
    [Tooltip("背景音乐开关")]
    private bool musicSwitch;


    private void Awake()
    {
        Instance = this;
        music = GetComponent<AudioSource>();
        Pause();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape)) {
            Pause();
        }
        MusicSwitch();
    }

    /// <summary>
    /// 打开菜单，暂停游戏
    /// </summary>
    private void Pause() {
        isPause = true;
        menuObject.SetActive(true);
        Cursor.visible = true;  //鼠标可见
        Time.timeScale = 0; //设置游戏速度，0停止，小于1变慢，1正正常，大于1变快
    }

    /// <summary>
    /// 继续游戏
    /// </summary>
    private void UnPause()
    {
        isPause = false;
        menuObject.SetActive(false);
        Cursor.visible = false;
        Time.timeScale = 1;
    }


    public void ContinueGame() {
        //GunManager.Instance.shootTimer = 0;
        UnPause();
    }

    public void NewGame() {
        foreach (GameObject target in targets) {
            target.GetComponent<TargetManager>().UpdateMonster();
        }
        UIManager.Instance.shootAmount = 0;
        UIManager.Instance.hitAmount = 0;
        //GunManager.Instance.shootTimer = 0;
        UnPause();
    }

    public void QuitGame() {
        Application.Quit();
    }

    public void MusicSwitch()
    {
        music.enabled = musicToggle.isOn;
        musicSwitch = musicToggle.isOn;
    }
}
