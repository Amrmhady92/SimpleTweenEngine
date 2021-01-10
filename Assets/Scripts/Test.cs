using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    public SimpleTweenEngine.FloatField floatTest;
    public GameObject moveTestObject;
    public GameObject scaleTestObject;
    public GameObject actionOvertimeObject;
    public float time = 3f;
    public AnimationCurve moveCurve;
    public AnimationCurve scaleCurve;
    void Start()
    {

        TweenEngine.Move(moveTestObject,
            moveTestObject.transform.position/* + (Vector3.up * 4)*/,
            time,
            () => { Debug.Log("Move Object Started"); },
            () => { Debug.Log("Move Object Complete"); }, // will be overriden down below by set on complete
            () => { Debug.Log("Move Object Interrupted"); }
            ).SetCurve(moveCurve,TweenAxis.Y, 5).SetOnComplete(()=>{ Debug.Log("On Complete Completed"); });

        TweenEngine.Scale(scaleTestObject,
            moveTestObject.transform.localScale /*+ (Vector3.one * 2)*/,
            time).SetCurve(scaleCurve,TweenAxis.All, 2);

        floatTest.Value = 0;
        TweenEngine.FloatValue(floatTest,
            10,
            time);


        MeshRenderer actionRenderer = actionOvertimeObject.GetComponent<MeshRenderer>();
        TweenEngine.CallBackOverTime(() => { actionRenderer.material.color = new Color(Random.Range(0,1f), Random.Range(0,1f), Random.Range(0, 1f)); },
            time);


       // Invoke("Ended", time);
    }

    //void Ended()
    //{
    //    Debug.Log("Ended");
    //}
    
}
