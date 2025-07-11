using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
public class VolumeSettings : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

        [SerializeField] private Volume gameVolume;
        [SerializeField] private float percentPerPress = 0.1f; // 10% per press
        [SerializeField] private float currentLerp = 0f;
        [SerializeField] private ShadowsMidtonesHighlights smh;
        [SerializeField] private ColorAdjustments colorAdjust;
        [SerializeField] private SplitToning splitToning;

        private Vector4 targetShadows, targetMidtones, targetHighlights;
        private float targetShadowStart, targetShadowEnd;
        private float targetHighlightStart, targetHighlightEnd;
        private float targetExposure, targetHueShift, targetSaturation;
        private Color targetSplitShadow, targetSplitHighlight;
        private float targetSplitBalance;

        private void Start()
        {
            // Copies the volume bc I really dont need this stuff all changing for every room
            gameVolume.profile = Instantiate(gameVolume.profile);

            gameVolume.profile.TryGet(out smh);
            gameVolume.profile.TryGet(out colorAdjust);
            gameVolume.profile.TryGet(out splitToning);

            
            targetShadows = smh.shadows.value;
            targetMidtones = smh.midtones.value;
            targetHighlights = smh.highlights.value;
            targetShadowStart = smh.shadowsStart.value;
            targetShadowEnd = smh.shadowsEnd.value;
            targetHighlightStart = smh.highlightsStart.value;
            targetHighlightEnd = smh.highlightsEnd.value;

            targetExposure = colorAdjust.postExposure.value;
            targetHueShift = colorAdjust.hueShift.value;
            targetSaturation = colorAdjust.saturation.value;

            targetSplitShadow = splitToning.shadows.value;
            targetSplitHighlight = splitToning.highlights.value;
            targetSplitBalance = splitToning.balance.value;

            // Activating all of the overrides
            smh.shadows.overrideState = true;
            smh.midtones.overrideState = true;
            smh.highlights.overrideState = true;
            smh.shadowsStart.overrideState = true;
            smh.shadowsEnd.overrideState = true;
            smh.highlightsStart.overrideState = true;
            smh.highlightsEnd.overrideState = true;

            colorAdjust.postExposure.overrideState = true;
            colorAdjust.hueShift.overrideState = true;
            colorAdjust.saturation.overrideState = true;

            splitToning.shadows.overrideState = true;
            splitToning.highlights.overrideState = true;
            splitToning.balance.overrideState = true;

            // Setting these to 0 so I can lerp later
           SetLerpedValues(0f);
    }

     private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            // Increase interpolation progress by set percentage
            currentLerp = Mathf.Clamp01(currentLerp + percentPerPress);
            SetLerpedValues(currentLerp);
        }
    }

    private void SetLerpedValues(float t)
    {
        smh.shadows.value = Vector4.Lerp(Vector4.zero, targetShadows, t);
        smh.midtones.value = Vector4.Lerp(Vector4.zero, targetMidtones, t);
        smh.highlights.value = Vector4.Lerp(Vector4.zero, targetHighlights, t);

        smh.shadowsStart.value = Mathf.Lerp(0f, targetShadowStart, t);
        smh.shadowsEnd.value = Mathf.Lerp(0f, targetShadowEnd, t);
        smh.highlightsStart.value = Mathf.Lerp(0f, targetHighlightStart, t);
        smh.highlightsEnd.value = Mathf.Lerp(0f, targetHighlightEnd, t);

        colorAdjust.postExposure.value = Mathf.Lerp(0f, targetExposure, t);
        colorAdjust.hueShift.value = Mathf.Lerp(0f, targetHueShift, t);
        colorAdjust.saturation.value = Mathf.Lerp(0f, targetSaturation, t);

        splitToning.shadows.value = Color.Lerp(Color.black, targetSplitShadow, t);
        splitToning.highlights.value = Color.Lerp(Color.black, targetSplitHighlight, t);
        splitToning.balance.value = Mathf.Lerp(0f, targetSplitBalance, t);
    }
}
