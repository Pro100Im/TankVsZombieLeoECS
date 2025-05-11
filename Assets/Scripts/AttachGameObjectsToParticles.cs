using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class AttachGameObjectsToParticles : MonoBehaviour
{
    public GameObject prefab;

    private ParticleSystem _particleSystem;
    private List<GameObject> _instances = new List<GameObject>();
    private ParticleSystem.Particle[] _particles;


    private void Start()
    {
        _particleSystem = GetComponent<ParticleSystem>();
        _particles = new ParticleSystem.Particle[_particleSystem.main.maxParticles];
    }

    private void LateUpdate()
    {
        int count = _particleSystem.GetParticles(_particles);

        while (_instances.Count < count)
            _instances.Add(Instantiate(prefab, _particleSystem.transform));

        bool worldSpace = (_particleSystem.main.simulationSpace == ParticleSystemSimulationSpace.World);

        for (int i = 0; i < _instances.Count; i++)
        {
            if (i < count)
            {
                if (worldSpace)
                    _instances[i].transform.position = _particles[i].position;
                else
                    _instances[i].transform.localPosition = _particles[i].position;
                _instances[i].SetActive(true);
            }
            else
                _instances[i].SetActive(false);
        }
    }
}
