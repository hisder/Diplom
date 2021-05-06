using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DustRoom
{
    private DustCollector _ventilation;

    public List<DustSource> DustSources { get; private set; }
    public List<SuctionPoint> SuctionPoints { get; private set; }



    private AspirationSystem _aspirationSystem;

    private float[] _allParticle;
    private float[] _residualParticle;

    public DustRoom(DustCollector ventilation, AspirationSystem aspirationSystem)
    {
        _ventilation = ventilation;
        _ventilation.SetThroughput(1000);

        _aspirationSystem = aspirationSystem;

        DustSources = new List<DustSource>();
        SuctionPoints = new List<SuctionPoint>();

        _allParticle = new float[5];
        _residualParticle = new float[5];
    }

    public void Update()
    {
        int totalParticals = 0;
        int residualParticles = 0;

        bool haveAspirationSystem;

        haveAspirationSystem = SuctionPoints.Count != 0;

        foreach (var dustSource in DustSources)
        {
            totalParticals += dustSource.ParticlesPerSecondMax;
        }

        if (!haveAspirationSystem)
        {
            residualParticles = totalParticals - _ventilation.Throughput;

            residualParticles = residualParticles < 0 ? 0 : residualParticles;

            if (residualParticles > 0)
            {
                Debug.Log($"требуется доп очистка {residualParticles}");
            }
        }
        else
        {
            foreach (var suctionPoint in SuctionPoints)
            {
                suctionPoint.Suck(this);
            }
        }
    }

    public void Reset()
    {
        foreach (var dustSource in DustSources)
        {
            dustSource.Reset();
        }

        foreach (var suctionPoint in SuctionPoints)
        {
            suctionPoint.Reset();
        }
    }

    public void AddDustSource(DustSource newSource)
    {
        DustSources.Add(newSource);
    }

    public void AddSuctionPoint(SuctionPoint newSuctionPoint)
    {
        SuctionPoints.Add(newSuctionPoint);
    }
}
