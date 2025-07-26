using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
public class VolumeSettings : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

        [SerializeField] private Volume gameVolume;
        [SerializeField] private float startingIntensity = 0.1f; // 10% per press
        // [SerializeField] private float currentLerp = 0f;
        [SerializeField] private ShadowsMidtonesHighlights smh;
        [SerializeField] private ColorAdjustments colorAdjust;
        [SerializeField] private SplitToning splitToning;
        [SerializeField] private float transitionSpeed = 1.0f;
        private float currentT = 0f; // This will gradually move toward targetT
        [SerializeField] private float targetT = 0f;
    private Vector4 targetShadows, targetMidtones, targetHighlights;
        private float targetShadowStart, targetShadowEnd;
        private float targetHighlightStart, targetHighlightEnd;
        private float targetExposure, targetHueShift, targetSaturation;
        private Color targetSplitShadow, targetSplitHighlight;
        private float targetSplitBalance;

        
        // NIGHT (purple tones)
        private readonly Vector4 nightShadows = new Vector4(0.6f, 0.4f, 0.9f, 0.5f); // purplish
        private readonly Vector4 nightHighlights = new Vector4(0.4f, 0.3f, 0.6f, 0.5f); // darker purples
        private readonly Color nightSplitShadow = new Color(0.3f, 0.1f, 0.4f); // deep purple
        
        private readonly Color nightSplitHighlight = new Color(0.4f, 0.3f, 0.6f); // lavender
        private readonly float nightBalance = -50f;
        private readonly float nightSaturation = -10f;
        private readonly float nightHueShift = -20f;
        private readonly float nightExposure = -0.5f;

        // DAY (warm pinkish tones)
        private readonly Vector4 dayShadows = new Vector4(1.0f, 0.7f, 0.8f, 0.5f); // pinkish
        private readonly Vector4 dayHighlights = new Vector4(1.0f, 0.9f, 0.8f, 0.5f); // warm highlights
        private readonly Color daySplitShadow = new Color(0.9f, 0.6f, 0.7f); // rosy
        private readonly Color daySplitHighlight = new Color(1.0f, 0.8f, 0.7f); // peachy
        private readonly float dayBalance = 20f;
        private readonly float daySaturation = 15f;
        private readonly float dayHueShift = 10f;
        private readonly float dayExposure = 0.2f;
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

        // targetExposure = colorAdjust.postExposure.value;
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

        // colorAdjust.postExposure.overrideState = true;
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
        float enemyRatio = (float)GetComponentInParent<Room>().GetAliveEnemies() / GetComponentInParent<Room>().GetTotalEnemies();
        targetT = ((1 - enemyRatio+0.01f) / 1.5f)+ startingIntensity;
        currentT = Mathf.MoveTowards(currentT, targetT, Time.deltaTime * transitionSpeed);
        // Increase interpolation progress by set percentage
        // currentLerp = Mathf.Clamp01(currentLerp + percentPerPress);
        SetLerpedValues(currentT);
        
    }

    private void SetLerpedValues(float t)
    {
        // Shadows/Midtones/Highlights
        smh.shadows.value = Vector4.Lerp(nightShadows, dayShadows, t);
        smh.highlights.value = Vector4.Lerp(nightHighlights, dayHighlights, t);

        // Color Adjustments
        colorAdjust.hueShift.value = Mathf.Lerp(nightHueShift, dayHueShift, t);
        colorAdjust.saturation.value = Mathf.Lerp(nightSaturation, daySaturation, t);
        // colorAdjust.postExposure.value = Mathf.Lerp(nightExposure, dayExposure, t); // optional if used

        // Split Toning
        splitToning.shadows.value = Color.Lerp(nightSplitShadow, daySplitShadow, t);
        splitToning.highlights.value = Color.Lerp(nightSplitHighlight, daySplitHighlight, t);
        splitToning.balance.value = Mathf.Lerp(nightBalance, dayBalance, t);
    }
}
