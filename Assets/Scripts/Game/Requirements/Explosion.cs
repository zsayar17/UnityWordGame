using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    ParticleSystem _particleSystem;
    void Awake()
    {
        _particleSystem = GetComponent<ParticleSystem>();
    }

    void Update()
    {
        if (!_particleSystem.isPlaying || UIManager.Instance.currentState != UIState.CelebrateScreen)
            Destroy(gameObject);

    }
}
