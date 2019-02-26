using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetFalseMe : MonoBehaviour {

    public float time =2f;
	void OnEnable () {
        Invoke("SetFalse", time);
	}
	
    void SetFalse(){
        gameObject.SetActive(false);
    }

}
