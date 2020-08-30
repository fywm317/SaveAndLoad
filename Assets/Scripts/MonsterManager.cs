using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterManager : MonoBehaviour
{
    public AnimationClip idleClip;
    public AnimationClip dieClip;
    private Animation anim;
    public AudioSource kickSound;
    [Tooltip("Monster类型")]
    public int monsterType;

    // Start is called before the first frame update
    void Awake()
    {
        anim = GetComponent<Animation>();
        anim.clip = idleClip;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// 碰撞检测
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Bullet"))
        {
            Destroy(collision.gameObject);
            anim.clip = dieClip;
            anim.Play();
            GetComponent<BoxCollider>().enabled = false;
            StartCoroutine("DeActivate");
            kickSound.Play();
            UIManager.Instance.AddHitAmount();
        }
    }

    private void OnDisable()
    {
        anim.clip = idleClip;
    }

    private IEnumerator DeActivate() {
        yield return new WaitForSeconds(2);
        gameObject.GetComponentInParent<TargetManager>().UpdateMonster();
    }
}
