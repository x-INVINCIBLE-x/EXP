using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TeleportManager : MonoBehaviour
{
    public static TeleportManager instance;
    private List<Teleporter> activeTeleporters = new();


    [SerializeField] private float fadeOutTime = 1f;
    [SerializeField] private float fadeInTime = 1f;
    [SerializeField] private float timeBetweenFade = 1f;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    public void AddTeleporter(string name, Destination location, Phase phase, int buildIndex, Sprite sprite)
    {
        Teleporter teleporter = new Teleporter(name, location, phase, buildIndex, sprite);
        activeTeleporters.Add(teleporter);
    }


    public void TeleportTo(Destination finalDestination, Phase finalPhase, int buildIndex)
    {
        int currIndex = SceneManager.GetActiveScene().buildIndex;
        StartCoroutine(Transition(finalDestination, finalPhase, buildIndex));
    }

    IEnumerator Transition(Destination finalDestination, Phase finalPhase, int buildIndex)
    {
        InputManager.Instance.DisableControls();

        Fader fader = FindObjectOfType<Fader>();

        SavingWrapper savingWrapper = FindObjectOfType<SavingWrapper>();

        yield return fader.FadeOut(fadeOutTime);

        savingWrapper.Save();

        yield return SceneManager.LoadSceneAsync(buildIndex);

        yield return new WaitForSeconds(0.1f);
        InputManager.Instance.DisableControls();
        savingWrapper.Load();

        PortalCore otherPortal = GetOtherPortal(finalDestination, finalPhase);
        if (otherPortal != null)
        {
            UpdatePlayer(otherPortal);
        }

        savingWrapper.Save();

        yield return new WaitForSeconds(timeBetweenFade);

        InputManager.Instance.EnableControls();

        yield return fader.FadeIn(fadeInTime);
        yield return new WaitForEndOfFrame();
    }

    private PortalCore GetOtherPortal(Destination finalDestination, Phase finalPhase)
    {
        foreach (PortalCore portal in FindObjectsOfType<PortalCore>())
        {
            if (portal.Destination != finalDestination && portal.Phase != finalPhase) { continue; }
            Debug.Log(portal.Destination + "  " + portal.Phase);
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

    public List<Teleporter> GetActiveTeleporters() => activeTeleporters;
}

public class Teleporter
{
    public Teleporter(string name, Destination location, Phase phase, int buildIndex, Sprite sprite)
    {
        this.name = name;
        this.location = location;
        this.phase = phase;
        this.buildIndex = buildIndex;
        this.sprite = sprite;
    }

    public string name;
    public Destination location;
    public Phase phase;
    public int buildIndex;
    public Sprite sprite;
}