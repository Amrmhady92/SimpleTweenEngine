using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    public SimpleTweenEngine.FloatField floatTest;
    public GameObject moveTestObject;
    public GameObject moveTestObjectLocal;
    public GameObject scaleTestObject;
    public GameObject actionOvertimeObject;
    public float time = 3f;
    public AnimationCurve moveCurve;
    public AnimationCurve scaleCurve;
    void Start()
    {

        TweenEngine.Move(
            moveTestObject,
            moveTestObject.transform.position + (new Vector3(1, -1, 0)),
            time)
            .SetOnComplete(() =>
            {
                TweenEngine.MoveY(
                    moveTestObject,
                    0,
                    time)
                .SetOnComplete(() =>
                {
                    TweenEngine.MoveX(
                        moveTestObject,
                        0,
                        time)
                    .SetOnComplete(() =>
                    {
                        Debug.Log("On Complete Completed");
                    });
                });
            });

        TweenEngine.MoveLocal(
            moveTestObjectLocal,
            moveTestObjectLocal.transform.localPosition + (new Vector3(-2, 0, 0)),
            time)
            .SetOnComplete(() =>
            {
                TweenEngine.MoveLocalY(
                    moveTestObjectLocal,
                    -1,
                    time)
                .SetOnComplete(() =>
                {
                    TweenEngine.MoveLocalX(
                        moveTestObjectLocal,
                        1,
                        time)
                    .SetOnComplete(() =>
                    {
                        Debug.Log("On Complete Local Completed");
                    });
                });
            });

        TweenEngine.Scale(
            moveTestObject,
            moveTestObject.transform.localScale * 0.5f,
            time).
            SetOnComplete(() =>
            {
                TweenEngine.ScaleX(moveTestObject, 1, time)
                .SetOnComplete(() =>
                {
                    TweenEngine.ScaleY(moveTestObject, 1, time)
                    .SetOnComplete(() =>
                    {
                        TweenEngine.ScaleZ(moveTestObject, 1, time);
                    });
                });
            });

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
