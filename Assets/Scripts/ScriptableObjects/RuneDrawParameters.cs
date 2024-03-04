using UnityEngine;

[CreateAssetMenu(fileName = "RuneDrawParameters", menuName = "ScriptableObjects/RuneDrawParameters")]
public class RuneDrawParameters : ScriptableObject
{
    [Header("changing this properties doesn't affects already created runes\nPlease recreate them to apply changes")]
    public float distanceBetweenPoints;
    public float acceptableError;
    public int heavyCheckStep;
}
  