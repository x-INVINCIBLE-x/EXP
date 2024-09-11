using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TeleportManager : MonoBehaviour, ISaveable
{
    public static TeleportManager instance;
    [SerializeField] private SerializableDictionary<Destination, List<Teleporter>> currentTeleporters = new();
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

        if (!currentTeleporters.ContainsKey(location))
            currentTeleporters.Add(location, new List<Teleporter>());

        currentTeleporters[location].Add(teleporter);
    }

    #region Teleportation logic
    public void TeleportToTeleportal(Destination finalDestination, Phase finalPhase, int buildIndex)
    {
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
                UpdatePlayer(otherPortal.SpawnPoint);
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
        GameObject teleporterParent = GameObject.FindWithTag("Teleporter");

        foreach (PortalCore portal in teleporterParent.GetComponentsInChildren<PortalCore>())
        {
            if (portal.Destination != finalDestination || portal.Phase != finalPhase) { continue; }
            Debug.Log(portal.Destination + "  " + portal.Phase);
            return portal;
        }

        return null;
    }

    private Portal GetOtherPortal(Destination finalDestination, Phase finalPhase)
    {
        GameObject portalParent = GameObject.FindWithTag("Portal");

        foreach (Portal portal in portalParent.GetComponentsInChildren<Portal>())
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

        if (!currentTeleporters.ContainsKey(destination))
            return null;

        return currentTeleporters[destination];
    }

    public object CaptureState()
    {
        return currentTeleporters;
    }

    public void RestoreState(object state)
    {
        currentTeleporters = (SerializableDictionary<Destination, List<Teleporter>>)state;
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