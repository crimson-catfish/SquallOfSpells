using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RuneMaker : MonoBehaviour
{
    public RuneStorage runeStorage;

    [HideInInspector] public int runeToEditIndex = 0;

    [SerializeField] private float requairedDistanceBetweenPoints;
    [SerializeField] private GameObject drawPoint;
    
    private Rune runeToEdit;
    private InputManager inputManager;
    private RuneDrawManager drawManager;
    private SpriteRenderer spriteRenderer;
    private List<Vector2> drawPointPositions = new();
    private Vector2 lastPointPosition;
    private int mass = 0;
    private Vector2 momentSum = Vector2.zero;
    private float[] drawFrame;


    private void Awake()
    {
        inputManager = InputManager.instance;
        drawManager = RuneDrawManager.instance;
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnEnable()
    {
        inputManager.OnDrawEnd += HandleDrawEnd;
        inputManager.OnCast += SaveRune;
    }

    private void OnDisable()
    {
        inputManager.OnDrawEnd -= HandleDrawEnd;
        inputManager.OnCast -= SaveRune;
    }

    private void HandleDrawEnd()
    {
    }

    public void NewRune()
    {

    }

    private void SaveRune()
    {
        Vector2 massCenter = momentSum / mass;
        float ratio = (drawFrame[2] - drawFrame[0]) / (drawFrame[3] - drawFrame[1]);
        
    }
}