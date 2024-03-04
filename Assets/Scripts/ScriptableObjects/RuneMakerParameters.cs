using UnityEngine;

[CreateAssetMenu(fileName = "RuneMakerParameters", menuName = "ScriptableObjects/RuneMakerParameters")]
public class RuneMakerParameters : ScriptableObject
{
    [Header("changing this properties doesn't affects already created previews\nPlease recreate them to apply changes")]
    public int previewSize;
    public int previewBorder;
    public int previewPointRadius;
}
  