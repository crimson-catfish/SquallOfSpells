using System;
using System.Collections.Generic;
using UnityEngine;

public class RuneMaker : MonoBehaviour
{
    [SerializeField] private RuneStorage runeStorage;

    private Rune runeToEdit;
    private RuneDrawVariation drawVariation;
    private InputManager inputManager;
    private RuneDrawManager drawManager;
    private Texture2D defaultRunePreview;


    private void Awake()
    {
        inputManager = InputManager.instance;
        drawManager = RuneDrawManager.instance;
        defaultRunePreview = Resources.Load<Texture2D>("Textures/Runes/defaultRunePreview");
        runeToEdit = new Rune(defaultRunePreview);
    }

    private void OnEnable()
    {
        drawManager.OnNewDrawVariation += CasheNewRuneDrawVariation;
    }

    private void OnDisable()
    {
        drawManager.OnNewDrawVariation -= CasheNewRuneDrawVariation;
    }
    

    private void CasheNewRuneDrawVariation(RuneDrawVariation variation)
    {
        drawVariation = variation;
    }

    public void SaveRuneDrawVariation()
    {
        if (drawVariation == null)
        {
            print("Draw something to save firstly :)");
            return;
        }

        runeToEdit.AddNewRuneDrawVariation(drawVariation);
    }

    public void NewRune()
    {
        runeToEdit = new Rune(defaultRunePreview);
        Debug.Log(runeToEdit);
        Debug.Log(defaultRunePreview);
        runeStorage.runes.Add(runeToEdit);
    }

    public void DeleteCurrentRune()
    {
        runeStorage.runes.Remove(runeToEdit);
    }
}