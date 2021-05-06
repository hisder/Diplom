using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SuctionPoint
{
    [SerializeField] private Vector2 _position;
    public Vector2 Position { get => _position; private set => _position = value; }

    [SerializeField] private float _suctionOptimalDistance;
    public float SuctionOptimalDistance { get => _suctionOptimalDistance; private set => _suctionOptimalDistance = value; }
    [SerializeField] private float _suctionMaxDistance;
    public float SuctionMaxDistance { get => _suctionMaxDistance; private set => _suctionMaxDistance = value; }


    [SerializeField] private float _throughput;
    public float Throughput { get => _throughput; private set => _throughput = value; }


    private float _currentLoad;
    public float CurrentLoad { get => _currentLoad; private set => _currentLoad = value; }


    public SuctionPoint(Vector2 position, float optimalDistance, float maxDistance, float throughput)
    {
        Position = position;
        SuctionOptimalDistance = optimalDistance;
        SuctionMaxDistance = maxDistance;

        Throughput = throughput;
    }

    public void SetPosition(Vector2 newPosition)
    {
        Position = newPosition;
    }

    public void Suck(DustRoom suckRoom)
    {
        float distance;

        Debug.Log(suckRoom.DustSources.Count);

        foreach (var dustSource in suckRoom.DustSources)
        {
            distance = Vector2.Distance(dustSource.Position, this.Position);

            if (TrySuck(dustSource, distance, SuctionOptimalDistance, 1f))
            {
                continue;
            }

            if (TrySuck(dustSource, distance, SuctionMaxDistance, 0.5f))
            {
                continue;
            }

        }

    }

    private bool TrySuck(DustSource dustSource, float currentDistance, float suckArea, float efficiencyMultiplier)
    {
        float sourceInfluence;

        if (_currentLoad >= Throughput)
        {
            Debug.Log($"пеереполнение пыли {_currentLoad} {dustSource.ParticlesPerSecondCurrent}");
            return false;
        }

        if (currentDistance < suckArea)
        {
            sourceInfluence = dustSource.ParticlesPerSecondCurrent * efficiencyMultiplier;

            if (_currentLoad + sourceInfluence >= Throughput)
            {
                dustSource.ParticlesPerSecondCurrent -= Throughput - _currentLoad;
                _currentLoad = Throughput;
            }
            else
            {
                _currentLoad += sourceInfluence;
                dustSource.ParticlesPerSecondCurrent -= sourceInfluence;
            }

            Debug.Log($"всасываю пыль сильно {_currentLoad} {dustSource.ParticlesPerSecondCurrent}");
            return true;
        }

        return false;
    }

    public void Reset()
    {
        _currentLoad = 0f;
    }
}
