using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class NebulaPostProcessing : MonoBehaviour
{
    private NewLoopManager _loopManager = null;
    public float _colorLerp = 0;
    public float _LerpCooldown = 1;
    public float _lerpSpeed = 0.1f;
    public bool _isLerping = false;
    private Color _colorA;
    private Color _colorB;

    [SerializeField] private Color yellowNebulaColor = Color.yellow;
    [SerializeField] private Color purpleNebulaColor = Color.magenta;
    [SerializeField] private PostProcessVolume volume = null;

    private ChromaticAberration chromaticAberration;
    private ColorGrading colorGrading;

    private void Awake()
    {

        _loopManager = GameObject.Find("LoopManager").GetComponent<NewLoopManager>();
        _loopManager.ReactedToNebuleuse += delegate (NebuleuseType NebuleuseType)
        {
            if (NebuleuseType == NebuleuseType.PURPLE1 || NebuleuseType == NebuleuseType.PURPLE2)
            {
                _colorA = yellowNebulaColor;
                _colorB = purpleNebulaColor;
            }
            else if (NebuleuseType == NebuleuseType.YELLOW)
            {
                _colorA = purpleNebulaColor;
                _colorB = yellowNebulaColor;
            }
            _LerpCooldown = 1;
            _colorLerp = 0;
            _isLerping = true;

        };
        //volume.profile.TryGetSettings(out colorGrading);
        //colorGrading.colorFilter.value = Color.Lerp(_colorA, _colorB, _colorLerp);

    }

    private void Start()
    {
        volume.profile.TryGetSettings(out colorGrading);
        Debug.Log(_colorB);
        colorGrading.colorFilter.value = purpleNebulaColor;
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

            volume.profile.TryGetSettings(out colorGrading);
            colorGrading.colorFilter.value = Color.Lerp(_colorA, _colorB, _colorLerp);
            //_light.color = Color.Lerp(_colorA, _colorB, _colorLerp);
            _colorLerp = 1f - _LerpCooldown;
        }
    }
}
