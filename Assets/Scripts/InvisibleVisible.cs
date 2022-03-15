using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName= "InvisibleOrVisible", menuName="Vision/InvisibleOrVisible",order=1)]
public class InvisibleVisible : ScriptableObject
{
    public Material m_material;
    
}

//Si Compétence activer
//  Shader1 , Shader2 .StepStrenght = Curve.Evaluate;

//   Si Début   => désactiveobj?.invoke();
//              => activeobj?.invoke();

//  Si finit    => désactiveobj?.invoke();
//              => activeobj?.invoke();