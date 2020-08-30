using LitJson;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml;
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
    [Tooltip("背景音乐开关,Save/Load用")]
    private bool musicSwitch;
    [Tooltip("存档路径")]
    private string filePath;
    //[Tooltip("SAVE/LOAD提示信息")]
    //public Animator recordAnima;

    private void Awake()
    {
        Instance = this;
        music = GetComponent<AudioSource>();
        musicSwitch = PlayerPrefs.GetInt("musicSwitch", 1) == 1 ? true : false;
        music.enabled = musicSwitch;
        musicToggle.isOn = musicSwitch;
        filePath = Application.dataPath + "/StreamingFiles";
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
        UnPause();
    }

    public void NewGame() {
        foreach (GameObject target in targets) {
            target.GetComponent<TargetManager>().UpdateMonster();
        }
        UIManager.Instance.shootAmount = 0;
        UIManager.Instance.hitAmount = 0;
        UnPause();
    }

    public void QuitGame() {
        Application.Quit();
    }

    public void MusicSwitch()
    {
        music.enabled = musicToggle.isOn;
        musicSwitch = musicToggle.isOn;
        PlayerPrefs.SetInt("musicSwitch", musicToggle.isOn ? 1 : 0);
    }

    /// <summary>
    /// 创建存档对象
    /// </summary>
    /// <returns></returns>
    public SaveData CreateSaveData() {
        SaveData saveData = new SaveData();
        foreach (GameObject go in targets) {
            TargetManager tm = go.GetComponent<TargetManager>();
            if (tm.activeMonster != null) {
                saveData.targetPosList.Add(tm.tagetPos);
                saveData.monsterTypeList.Add(tm.activeMonster.GetComponent<MonsterManager>().monsterType);
            }
        }
        saveData.shootAmount = UIManager.Instance.shootAmount;
        saveData.hitAmount = UIManager.Instance.hitAmount;
        return saveData;
    }

    /// <summary>
    /// 拆解存档对象
    /// </summary>
    public void AnalyseSaveData(SaveData saveData) {
        if (saveData != null) {
            UIManager.Instance.shootAmount = saveData.shootAmount;
            UIManager.Instance.hitAmount = saveData.hitAmount;
            // 重置每一个Target
            foreach (GameObject targetGO in targets)
            {
                targetGO.GetComponent<TargetManager>().UpdateMonster();
            }
            for (int i = 0; i < saveData.targetPosList.Count; i++) {
                targets[saveData.targetPosList[i]].GetComponent<TargetManager>().LoadMonsterByType(saveData.monsterTypeList[i]);
            }
        }
    }

    /// <summary>
    /// 显示 SVAE/LOAD 信息
    /// </summary>
    /// <param name="msg"></param>
    /// <param name="show"></param>
    public void ShowRecordMessage(string msg) {
        Transform msgText = UIManager.Instance.gameObject.transform.Find("txt_msg");
        msgText.GetComponent<Text>().text = msg;
        msgText.gameObject.SetActive(true);
        msgText.GetComponent<DestroyGameObject>().DisableObjectActive();
    }

    public void SaveGame()
    {
        //SaveBinary();
        //SaveJson();
        SaveXML();
        UnPause();
    }

    public void LoadGame()
    {
        //LoadBinary();
        //LoadJson();
        LoadXML();
        UnPause();
    }

    #region 保存/读取二进制
    private void SaveBinary()
    {
        FileStream stream = null;
        try
        {
            SaveData saveData = CreateSaveData();
            BinaryFormatter formatter = new BinaryFormatter();
            stream = File.Create(filePath + "/binarySaveData.txt");
            formatter.Serialize(stream, saveData);
            if (File.Exists(filePath + "/binarySaveData.txt")) {
                ShowRecordMessage("保存成功");
            }
        }
        finally {
            if (stream != null) {
                stream.Close();
            }
        }
    }

    private void LoadBinary()
    {
        FileStream stream = null;
        try
        {
            BinaryFormatter formatter = new BinaryFormatter();
            if (File.Exists(filePath + "/binarySaveData.txt"))
            {
                stream = File.Open(filePath + "/binarySaveData.txt", FileMode.Open);
                SaveData saveData = (SaveData)formatter.Deserialize(stream);
                AnalyseSaveData(saveData);
                ShowRecordMessage("读取成功");
            }
            else {
                ShowRecordMessage("读取失败");
            }
        }
        finally
        {
            if (stream != null) {
                stream.Close();
            }
        }
    }
    #endregion

    #region 保存/读取JSON
    private void SaveJson()
    {
        StreamWriter sw = null;
        try
        {
            SaveData saveData = CreateSaveData();
            string strJson = JsonMapper.ToJson(saveData);
            sw = new StreamWriter(filePath + "/jsonSaveData.json");
            sw.Write(strJson);
            ShowRecordMessage("保存成功");
        }
        finally {
            if (sw != null) {
                sw.Close();
            }
        }
    }

    private void LoadJson()
    {
        StreamReader sr = null;
        try
        {
            if (File.Exists(filePath + "/jsonSaveData.json"))
            {
                sr = new StreamReader(filePath + "/jsonSaveData.json");
                string strJson = sr.ReadToEnd();
                SaveData saveData = JsonMapper.ToObject<SaveData>(strJson);
                AnalyseSaveData(saveData);
                ShowRecordMessage("读取成功");
            }
            else {
                ShowRecordMessage("读取失败");
            }
        }
        finally
        {
            if (sr != null)
            {
                sr.Close();
            }
        }
    }
    #endregion

    #region 保存/读取XML
    private void SaveXML()
    {
        SaveData saveData = CreateSaveData();
        string file = filePath + "/xmlSaveData.xml";
        XmlDocument xml = new XmlDocument();
        XmlElement root = xml.CreateElement("root");   // 创建根节点
        root.SetAttribute("name", "saveData");
        XmlElement target;
        XmlElement targetPos;
        XmlElement monsterType;
        for (int i = 0; i < saveData.targetPosList.Count; i++) {
            target = xml.CreateElement("target");
            targetPos = xml.CreateElement("targetPos");
            targetPos.InnerText = saveData.targetPosList[i].ToString();
            monsterType = xml.CreateElement("monsterType");
            monsterType.InnerText = saveData.monsterTypeList[i].ToString();
            target.AppendChild(targetPos);
            target.AppendChild(monsterType);
            root.AppendChild(target);
        }
        XmlElement shootAmount = xml.CreateElement("shootAmount");
        shootAmount.InnerText = saveData.shootAmount.ToString();
        XmlElement hitAmount = xml.CreateElement("hitAmount");
        hitAmount.InnerText = saveData.hitAmount.ToString();
        root.AppendChild(shootAmount);
        root.AppendChild(hitAmount);
        xml.AppendChild(root);
        xml.Save(file);
        ShowRecordMessage("保存成功");
    }

    private void LoadXML() {
        string file = filePath + "/xmlSaveData.xml";
        if (File.Exists(file))
        {
            SaveData saveData = new SaveData();
            XmlDocument xml = new XmlDocument();
            xml.Load(file);
            XmlNodeList targets = xml.GetElementsByTagName("target");
            if (targets.Count > 0) {
                foreach (XmlNode target in targets) {
                    XmlNode targetPos = target.ChildNodes[0];
                    saveData.targetPosList.Add(int.Parse(targetPos.InnerText));
                    XmlNode monsterType = target.ChildNodes[1];
                    saveData.monsterTypeList.Add(int.Parse(monsterType.InnerText));
                }
            }
            XmlNodeList shootAmounts = xml.GetElementsByTagName("shootAmount");
            saveData.shootAmount = int.Parse(shootAmounts[0].InnerText);
            XmlNodeList hitAmounts = xml.GetElementsByTagName("hitAmount");
            saveData.hitAmount = int.Parse(hitAmounts[0].InnerText);
            AnalyseSaveData(saveData);
            ShowRecordMessage("读取成功");
        }
        else {
            ShowRecordMessage("读取失败");
        }
    }
    #endregion

}
