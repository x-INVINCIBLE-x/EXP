using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TeleportManager : MonoBehaviour, ISaveable
{
    public static TeleportManager instance;
    [SerializeField] private List<Teleporter> activeTeleporters = new();

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

    #region Teleportation logic
    public void TeleportToTeleportal(Destination finalDestination, Phase finalPhase, int buildIndex)
    {
        int currIndex = SceneManager.GetActiveScene().buildIndex;
        StartCoroutine(Transition(finalDestination, finalPhase, buildIndex));
    }

    public void TeleportToPortal(Destination finalDestination, Phase finalPhase, int buildIndex)
    {
        StartCoroutine(Transition(finalDestination, finalPhase, buildIndex, true));
    }

    IEnumerator Transition(Destination finalDestination, Phase finalPhase, int buildIndex, bool isPortal = false)
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

        UpdatePosition(finalDestination, finalPhase, isPortal);

        savingWrapper.Save();

        yield return new WaitForSeconds(timeBetweenFade);

        InputManager.Instance.EnableControls();

        yield return fader.FadeIn(fadeInTime);
        yield return new WaitForEndOfFrame();
    }

    private void UpdatePosition(Destination finalDestination, Phase finalPhase, bool isPortal)
    {
        if (isPortal)
        {
            Portal otherPortal = GetOtherPortal(finalDestination, finalPhase);
            if (otherPortal != null)
            {
                UpdatePlayer(otherPortal.Spawnpoint);
            }
        }
        else
        {
            PortalCore otherTeleporter = GetOtherTeleporter(finalDestination, finalPhase);
            if (otherTeleporter != null)
            {
                UpdatePlayer(otherTeleporter.SpawnPoint);
            }
        }
    }

    private PortalCore GetOtherTeleporter(Destination finalDestination, Phase finalPhase)
    {
        foreach (PortalCore portal in FindObjectsOfType<PortalCore>())
        {
            if (portal.Destination != finalDestination || portal.Phase != finalPhase) { continue; }
            Debug.Log(portal.Destination + "  " + portal.Phase);
            return portal;
        }

        return null;
    }

    private Portal GetOtherPortal(Destination finalDestination, Phase finalPhase)
    {
        Portal[] portals = FindObjectsOfType<Portal>();

        foreach (Portal portal in FindObjectsOfType<Portal>())
        {
            Debug.Log(portal.Destination + "  " + portal.Phase);
            if (portal.Destination != finalDestination || portal.Phase != finalPhase) { continue; }
            return portal;
        }

        return null;
    }

    private void UpdatePlayer(Transform spawnPoint)
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        player.transform.SetPositionAndRotation(spawnPoint.position, spawnPoint.rotation);
    }

    #endregion
    public List<Teleporter> GetTeleportrersFrom(Destination destination)
    {
        List<Teleporter> specificTeleporters = new();

        foreach (Teleporter teleporter in activeTeleporters)
        {
            if (teleporter.location == destination)
                specificTeleporters.Add(teleporter);
        }

        return specificTeleporters;
    }

    public List<Teleporter> GetActiveTeleporters() => activeTeleporters;

    public object CaptureState()
    {
        return activeTeleporters;
    }

    public void RestoreState(object state)
    {
        activeTeleporters = (List<Teleporter>)state;
    }
}

[System.Serializable]
public class Teleporter
{
    public string name;
    public Destination location;
    public Phase phase;
    public int buildIndex;
    //public Sprite sprite;

    public Teleporter(string name, Destination location, Phase phase, int buildIndex, Sprite sprite)
    {
        this.name = name;
        this.location = location;
        this.phase = phase;
        this.buildIndex = buildIndex;
        //this.sprite = sprite;
    }
}