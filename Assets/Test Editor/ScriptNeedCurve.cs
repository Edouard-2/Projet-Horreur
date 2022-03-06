using UnityEngine;
using System.Collections;


public class ScriptNeedCurve : MonoBehaviour
{
    public AnimationCurve curveX;

    public void SetCurves(AnimationCurve xC)
    {
        curveX = xC;
    }

    void Update()
    {
        transform.position = new Vector3(curveX.Evaluate(Time.time), 0, 0);
    }
}