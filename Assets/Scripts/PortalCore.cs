using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public enum Destination
{
    Level1, Level2, Level3, Level4, Level5, Level6
}

public enum Phase
{
    Phase1, Phase2, Phase3, Phase4, Phase5
}

public class PortalCore : MonoBehaviour, IInteractable
{
    [field: SerializeField] public Destination Destination { get; private set; }
    [field: SerializeField] public Phase Phase { get; private set; }

    [field: SerializeField] public Transform SpawnPoint { get; private set; } // position to Teleport to
    public int BuildIndex { get; private set; }
    public bool IsActivaed { get; private set; }

    private UI ui;

    private void Awake()
    {
        ui = UI.instance;
    }

    public void Interact()
    {
        ui.SetPortalUI(true);

    }

    private void OnTriggerEnter(Collider other)
    {
        if (!IsActivaed)
        {
            BuildIndex = SceneManager.GetActiveScene().buildIndex;
            return;
        }
    }

    public void TeleportTo(Destination finalDestination, Phase finalPhase, int buildIndex)
    {
        int currIndex = SceneManager.GetActiveScene().buildIndex;
        StartCoroutine(Transition(finalDestination, finalPhase, buildIndex));
    }

    //private void Teleport(int buildIndex)
    //{
    //    StartCoroutine(Transition(buildIndex));
    //}

    IEnumerator Transition(Destination finalDestination, Phase finalPhase, int buildIndex)
    {
        DontDestroyOnLoad(gameObject);

        //Todo: Disable player controiller

        //Fader fader = FindObjectOfType<Fader>();

        SavingWrapper savingWrapper = FindObjectOfType<SavingWrapper>();

        //yield return fader.FadeOut(fadeOutTime);

        savingWrapper.Save();

        yield return SceneManager.LoadSceneAsync(buildIndex);

        //Todo: Enable player controiller
        yield return new WaitForSeconds(0.1f);
        savingWrapper.Load();

        PortalCore otherPortal = GetOtherPortal(finalDestination, finalPhase);
        if (otherPortal != null)
        {
            UpdatePlayer(otherPortal);
        }

        savingWrapper.Save();

        //yield return new WaitForSeconds(timeBetweenFade);

// Todo:: Enable player control again

        //yield return fader.FadeIn(fadeInTime);
        Destroy(gameObject);
    }

    private PortalCore GetOtherPortal(Destination finalDestination, Phase finalPhase)
    {
        foreach (PortalCore portal in FindObjectsOfType<PortalCore>())
        {
            if (portal.Destination != finalDestination && portal.Phase != finalPhase) { continue; }
            Debug.Log(portal.Destination + "  " +  portal.Phase);
            return portal;
        }

        return null;
    }
    
    private void UpdatePlayer(PortalCore portal)
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        player.transform.position = portal.SpawnPoint.position;
        player.transform.rotation = portal.SpawnPoint.rotation;
    }
}
