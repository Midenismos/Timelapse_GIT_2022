using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NebuleuseLight : MonoBehaviour
{
    private NewLoopManager _loopManager = null;
    public float _colorLerp = 0;
    public float _LerpCooldown = 1;
    public float _lerpSpeed = 0.1f;
    public bool _isLerping = false;
    private Color _colorA;
    private Color _colorB;
    private Light _light;
    private void Awake()
    {
        _light = GetComponent<Light>();
        _loopManager = GameObject.Find("LoopManager").GetComponent<NewLoopManager>();
        _loopManager.ReactedToNebuleuse += delegate (NebuleuseType NebuleuseType)
        {
            if (NebuleuseType == NebuleuseType.PURPLE1 || NebuleuseType == NebuleuseType.PURPLE2)
            {
                _colorA = Color.yellow;
                _colorB = new Color(0.90f, 0, 1, 1);
            }
            else if (NebuleuseType == NebuleuseType.YELLOW)
            {
                _colorA = new Color(0.90f, 0, 1, 1);
                _colorB = Color.yellow;
            }
            _LerpCooldown = 1;
            _colorLerp = 0;
            _isLerping = true;

        };
        _light.color = new Color(0.90f, 0, 1, 1);

    }

    private void Update()
    {
        if (_isLerping)
        {
            _LerpCooldown = Mathf.Clamp(_LerpCooldown - Time.unscaledDeltaTime * _lerpSpeed, 0f, 1f);

            if (_LerpCooldown == 0)
            {
                _isLerping = false;
            }

            _light.color = Color.Lerp(_colorA, _colorB, _colorLerp);
            _colorLerp = 1f - _LerpCooldown;
        }
    }
}
