using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyGameObject : MonoBehaviour
{ 
    private void DestroySelf() {
        gameObject.SetActive(false);
    }

    public void DisableObjectActive() {
        Invoke("DestroySelf", 2);
    }
}
