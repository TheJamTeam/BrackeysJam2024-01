using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFootsteps : MonoBehaviour
{
    private PlayerMovementComponent _playerMovementComponent;
    private AudioComponent _playerAudioComponent;
    public float timeBetweenFootsteps;
    public Vector2 timeVariation;
    private float _timeToNextFootstep;
    private float _timeSinceLastFootstep;
    void Awake()
    {
        _playerMovementComponent = GetComponent<PlayerMovementComponent>();
        _playerAudioComponent = GetComponent<AudioComponent>();
    }

    // Update is called once per frame
    void Update()
    {
        FootstepUpdate();
    }

    void FootstepUpdate()
    {
        _timeSinceLastFootstep += Time.deltaTime;
        
        if (_playerMovementComponent.IsMoving())
        {
            if (_timeToNextFootstep > 0)
            {
                _timeToNextFootstep -= Time.deltaTime;
            }
            else if(_timeSinceLastFootstep > timeBetweenFootsteps + timeVariation.x)
            {
                _timeToNextFootstep = timeBetweenFootsteps + Random.Range(timeVariation.x, timeVariation.y);
                PlayFootstep();
            }
        }
        else if(_timeToNextFootstep > 0)
        {
            _timeToNextFootstep = 0f;
        }
    }

    void PlayFootstep()
    {
        _timeSinceLastFootstep = 0f;
        _playerAudioComponent.PlaySound("Footstep");
    }
}
