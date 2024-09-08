using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportManager : MonoBehaviour
{
    public static TeleportManager instance;
    private List<Teleporter> activeTeleporters;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    public void AddTeleporter(Destination location, Phase phase, int buildIndex, Sprite sprite)
    {
        Teleporter teleporter = new Teleporter(location, phase, buildIndex, sprite);
        activeTeleporters.Add(teleporter);
    }

    public List<Teleporter> GetActiveTeleporters() => activeTeleporters;
}

public class Teleporter
{
    public Teleporter(Destination location, Phase phase, int buildIndex, Sprite sprite)
    {
        this.location = location;
        this.phase = phase;
        this.buildIndex = buildIndex;
        this.sprite = sprite;
    }

    public Destination location;
    public Phase phase;
    public int buildIndex;
    public Sprite sprite;
}