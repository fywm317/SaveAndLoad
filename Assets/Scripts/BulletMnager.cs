using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletMnager : MonoBehaviour
{
    [Tooltip("生命周期")]
    public float duration;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine("DestroySelf");
    }

    private IEnumerator DestroySelf() {
        yield return new WaitForSeconds(duration);
        Destroy(this.gameObject);
    }
}
