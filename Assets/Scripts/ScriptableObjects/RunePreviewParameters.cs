using UnityEngine;

[CreateAssetMenu(fileName = "RunePreviewParameters", menuName = "ScriptableObjects/RunePreviewParameters")]
public class RunePreviewParameters : ScriptableObject
{
    [Header("changing this properties doesn't affects already created previews\nPlease recreate them to apply changes")]
    public int size;
    public int border;
    public int pointRadius;
    public Color pointColor;
}
