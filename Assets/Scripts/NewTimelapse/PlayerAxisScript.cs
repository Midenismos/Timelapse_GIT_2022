using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAxisScript : MonoBehaviour
{
    [SerializeField] private int[] _axis;
    [SerializeField] private int _currentAxis = 0;
    [SerializeField] private int _iCurrentAxis = 0;
    [SerializeField] private float _rotationLerp = 0;
    [SerializeField] private float _rotationCountdown = 1;
    [SerializeField] private float _rotationSpeed = 0.2f;
    private bool _isLerping = false;

    // Start is called before the first frame update
    void Start()
    {
        _iCurrentAxis = 0;
        _currentAxis = _axis[_iCurrentAxis];
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("d"))
        {
            if (!_isLerping)
            {
                _iCurrentAxis += 1;
                if (_iCurrentAxis < 0)
                    _iCurrentAxis = _axis.Length-1;
                else if (_iCurrentAxis > _axis.Length-1)
                    _iCurrentAxis = 0;
                _rotationCountdown = 1;
                _rotationLerp = 0;
                _isLerping = true;
            }
        }
        if (Input.GetKeyDown("q"))
        {
            if(!_isLerping)
            {
                _iCurrentAxis -= 1;
                if (_iCurrentAxis < 0)
                    _iCurrentAxis = _axis.Length - 1;
                else if (_iCurrentAxis > _axis.Length - 1)
                    _iCurrentAxis = 0;
                _rotationCountdown = 1;
                _rotationLerp = 0;
                _isLerping = true;
            }
        }

        //Gère le lerp des sons lors d'un changement temporel

        if (_isLerping)
        {
            _rotationCountdown = Mathf.Clamp(_rotationCountdown - Time.unscaledDeltaTime * _rotationSpeed, 0f, 1f);

            if (_rotationCountdown == 0)
            {
                _currentAxis = _axis[_iCurrentAxis];
                _isLerping = false;
            }

            transform.rotation = Quaternion.Slerp(Quaternion.Euler(0, _currentAxis, 0), Quaternion.Euler(0, _axis[_iCurrentAxis], 0), _rotationLerp);

            _rotationLerp = 1f - _rotationCountdown;
        }
    }
}
