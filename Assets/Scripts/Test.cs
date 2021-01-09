using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleTweenEngine;

public class Test : MonoBehaviour
{
    public FloatField floatTest;
    public GameObject moveTestObject;
    public GameObject scaleTestObject;
    public GameObject actionOvertimeObject;
    public float time = 3f;
    void Start()
    {

        TweenEngine.Move(moveTestObject,
            moveTestObject.transform.position + (Vector3.up * 2),
            time//,
            //() => { Debug.Log("Move Object Started"); },
            //() => { Debug.Log("Move Object Complete"); },
            //() => { Debug.Log("Move Object Interrupted"); }
            );

        TweenEngine.Scale(scaleTestObject,
            moveTestObject.transform.localScale + (Vector3.one * 2),
            time);

        floatTest.Value = 0;
        TweenEngine.FloatValue(floatTest,
            10,
            time);


        MeshRenderer actionRenderer = actionOvertimeObject.GetComponent<MeshRenderer>();
        TweenEngine.CallBackOverTime(() => { actionRenderer.material.color = new Color(Random.Range(0,1f), Random.Range(0, 1f), Random.Range(0, 1f)); },
            time);

        Invoke("Ended", time);
    }

    void Ended()
    {
        Debug.Log("Ended");
    }
    
}
