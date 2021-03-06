﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    public Text shootText;
    public Text hitText;
    [Tooltip("射击次数")]
    public int shootAmount;
    [Tooltip("击中次数")]
    public int hitAmount;
    [Tooltip("Save/Load提示信息")]
    public Text recordMessage;

    private void Awake()
    {
        gameObject.transform.Find("txt_msg").gameObject.SetActive(false);
    }

    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
        shootText.text = 0.ToString();
        hitText.text = 0.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        shootText.text = shootAmount.ToString();
        hitText.text = hitAmount.ToString();
    }

    public void AddShootAmount() {
        shootAmount++;
    }

    public void AddHitAmount()
    {
        hitAmount++;
    }

    public void AnimEnd() {
        recordMessage.gameObject.GetComponent<Animator>().SetBool("IsActive", false);
    }

}
