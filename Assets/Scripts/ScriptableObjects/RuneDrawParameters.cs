using UnityEngine;

[CreateAssetMenu(fileName = "RuneDrawParameters", menuName = "ScriptableObjects/RuneDrawParameters")]
public class RuneDrawParameters : ScriptableObject
{
    public float requairedDistance;
    public float acceptableError;
    public int heavyCheckStep;
}
