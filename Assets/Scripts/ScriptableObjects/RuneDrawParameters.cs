using UnityEngine;

[CreateAssetMenu(fileName = "RuneDrawParametrs", menuName = "ScriptableObjects/RuneDrawParametrs")]
public class RuneDrawParameters : ScriptableObject
{
    public float requairedDistance;
    public float acceptableError;
    public int heavyCheckStep;
}
