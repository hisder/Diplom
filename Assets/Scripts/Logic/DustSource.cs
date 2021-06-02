using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DustSource
{
    [SerializeField] private int _particlesPerSecond;
    public int ParticlesPerSecondMax { get => _particlesPerSecond; private set => _particlesPerSecond = value; }
    public float ParticlesPerSecondCurrent { get; set; }
    [SerializeField] private Vector2 _position;
    public Vector2 Position { get => _position; private set => _position = value; }

    private float[] _createdParticalsProportion;

    public DustSource(int particlesPerSecond, float[] particalsProportion, Vector2 position)
    {
        ParticlesPerSecondMax = particlesPerSecond;
        _createdParticalsProportion = particalsProportion;
        Position = position;

        ParticlesPerSecondCurrent = ParticlesPerSecondMax;
    }

    public void Reset()
    {
        ParticlesPerSecondCurrent = ParticlesPerSecondMax;
    }

    public void SetPosition(Vector2 position)
    {
        Position = position;
    }

    public void ReleaseParticles(DustRoom dustRoom)
    {

    }
}
