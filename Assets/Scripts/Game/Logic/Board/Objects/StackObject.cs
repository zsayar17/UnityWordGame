using UnityEngine;
public class StackObject : BaseObject
{
    ParticleSystem _particleSystem;

    protected override void Awake()
    {
        base.Awake();

        _particleSystem = GetComponentInChildren<ParticleSystem>();
    }

    public override void TurnOffObject()
    {
        gameObject.SetActive(false);
    }

    public void TurnOnObject(Vector3 position)
    {
        gameObject.SetActive(true);

        transform.position = position;
    }

    public void PlayParticle() => _particleSystem.Play();

    public bool IsBusy() => _particleSystem.isPlaying;
}
