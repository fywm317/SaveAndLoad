using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunManager : MonoBehaviour
{
    public static GunManager Instance { get; private set; }

    private float maxYRotation = 120;
    private float minYRotation = 0;
    private float maxXRotation = 60;
    private float minXRotation = 0;

    [Tooltip("发射CD")]
    public float shootTime;
    [Tooltip("子弹初速度")]
    public int bulletSpeed;
    [Tooltip("计时器")]
    public float shootTimer = 0;

    public GameObject bulletObject;
    public Transform firePosition;

    private AudioSource fireSound;

    private void Start()
    {
        Instance = this;
        fireSound = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameManager.Instance.isPause) {
            shootTimer += Time.deltaTime;
            if (shootTimer >= shootTime)
            {
                if (Input.GetMouseButtonUp(0))
                {
                    GameObject currentBullet = Instantiate(bulletObject, firePosition.position, Quaternion.identity);
                    currentBullet.GetComponent<Rigidbody>().AddForce(transform.forward * bulletSpeed);
                    GetComponent<Animation>().Play(); //射击动画
                    shootTimer = 0;
                    fireSound.Play();
                    UIManager.Instance.AddShootAmount();
                }
            }
            // 控制手枪旋转
            float xPosPercent = Input.mousePosition.x / Screen.width;
            float yPosPercent = Input.mousePosition.y / Screen.height;
            float xAngle = -Mathf.Clamp(yPosPercent * maxXRotation, minXRotation, maxXRotation) + 15;
            float yAngle = Mathf.Clamp(xPosPercent * maxYRotation, minYRotation, maxYRotation) - 60;
            transform.eulerAngles = new Vector3(xAngle, yAngle, 0);
        }
    }
}
