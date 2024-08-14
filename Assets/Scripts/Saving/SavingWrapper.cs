using GameDevTV.Saving;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SavingWrapper : MonoBehaviour
{
    [SerializeField] private string defaultSaveFile = "Save";


    private void Awake()
    {
        //StartCoroutine(StartScene());   
    }

    private IEnumerator StartScene()
    {
        //Fader fader = FindObjectOfType<Fader>();
        //fader.FadeOutImmediately();
        yield return GetComponent<SavingSystem>().LoadLastScene(defaultSaveFile);
        //yield return fader.FadeIn(1f);
    }
    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.S))
        {
            Save();
        }

        if(Input.GetKeyUp(KeyCode.L))
        {
            Load();
        }
    }

    public void Save()
    {
        GetComponent<SavingSystem>().Save(defaultSaveFile);
    }

    public void Load()
    {
        StartCoroutine(GetComponent<SavingSystem>().LoadLastScene(defaultSaveFile));
    }
}
