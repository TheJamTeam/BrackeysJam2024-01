using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFootsteps : MonoBehaviour
{
    private PlayerMovementComponent _playerMovementComponent;
    private AudioComponent _playerAudioComponent;
    public float timeBetweenFootsteps;
    private float _timeToNextFootstep;
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
        if (_playerMovementComponent.IsMoving())
        {
            if (_timeToNextFootstep > 0)
            {
                _timeToNextFootstep -= Time.deltaTime;
            }
            else
            {
                _timeToNextFootstep = timeBetweenFootsteps;
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
        _playerAudioComponent.PlaySound("Footstep");
    }
}
