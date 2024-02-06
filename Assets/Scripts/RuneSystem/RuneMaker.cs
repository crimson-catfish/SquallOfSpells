using System;
using System.Collections.Generic;
using UnityEngine;

public class RuneMaker : MonoBehaviour
{
    public RuneStorage runeStorage;

    [HideInInspector] public int runeToEditIndex = 0;

    private Rune runeToEdit;
    private RuneDrawVariation drawVariation;
    private InputManager inputManager;
    private RuneDrawManager drawManager;
    private SpriteRenderer spriteRenderer;


    private void Awake()
    {
        inputManager = InputManager.instance;
        drawManager = RuneDrawManager.instance;
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnEnable()
    {
        drawManager.OnNewDrawVariation += CasheNewRuneDrawVariation;
        inputManager.OnCast += SaveRune;
    }

    private void OnDisable()
    {
        drawManager.OnNewDrawVariation -= CasheNewRuneDrawVariation;
        inputManager.OnCast -= SaveRune;
    }

    
    private void CasheNewRuneDrawVariation(RuneDrawVariation variation)
    {
        drawVariation = variation;
    }

    private void SaveRune()
    {
        runeStorage.runes[runeToEditIndex].AddNewRuneDrawVariation(drawVariation);
    }

    public void NewRune()
    {
        runeStorage.runes.Add(new Rune());
    }
}