using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class ScreenShake : MonoBehaviour
{

    private static ScreenShake s_Instance;

    private static bool _enabled = true;

    public static void Shake(float duration, float amplitude, float fadeIn = .1f, float fadeOut = .1f, float frequency = 1f)
    {
        if (!_enabled) return;
        s_Instance.StartShake(duration, amplitude, fadeIn, fadeOut, frequency);
    }

    public static void Stop()
    {
        s_Instance.StopShake();
    }

    public static void Enable()
    {
        _enabled = true;
    }

    public static void Disable()
    {
        _enabled = false;
        Stop();
    }

    // Cinemachine Shake
    private CinemachineVirtualCamera virtualCamera;
    private CinemachineBasicMultiChannelPerlin virtualCameraNoise;

    private const float fadeRate = 0.05f;
    private bool isShaking = false;
    private Coroutine currentShakeCoroutine;

    private void Awake()
    {
        s_Instance = this;
        virtualCamera = GetComponentInChildren<CinemachineVirtualCamera>();
        virtualCameraNoise = virtualCamera.GetCinemachineComponent<Cinemachine.CinemachineBasicMultiChannelPerlin>();
        virtualCameraNoise.m_AmplitudeGain = 0f;
    }
    

    public void StartShake(float duration, float amplitude, float fadeIn = .1f, float fadeOut = .1f, float frequency = 1f)
    {
        if (isShaking) return;
        isShaking = true;
        currentShakeCoroutine = StartCoroutine(ShakeCoroutine(duration, amplitude, fadeIn, fadeOut, frequency));
    }

    public void StopShake()
    {
        if (currentShakeCoroutine != null)
        {
            StopCoroutine(currentShakeCoroutine);
            currentShakeCoroutine = null;
        }

        virtualCameraNoise.m_AmplitudeGain = 0f;
        isShaking = false;
    }

    IEnumerator ShakeCoroutine(float duration, float amplitude, float fadeIn, float fadeOut, float frequency)
    {
        float startTime = Time.time;
        if (fadeIn > 0f)        
        {
            yield return ShakeFade(fadeIn, amplitude, frequency);
        }

        // Set Cinemachine Camera Noise parameters
        virtualCameraNoise.m_AmplitudeGain = amplitude;
        virtualCameraNoise.m_FrequencyGain = frequency;
        
        yield return new WaitForSeconds(duration);
        startTime = Time.time;
        if (fadeOut > 0f)            
        {
            yield return ShakeFade(fadeOut, 0f, 0f);
        }

        StopShake();

    }

    IEnumerator ShakeFade(float duration, float amplitude, float frequency)
    {
        float perc = 0f;
        float durationRate = 1f / duration;
        while (perc < 1f)
        {
            virtualCameraNoise.m_AmplitudeGain = Mathf.Lerp(virtualCameraNoise.m_AmplitudeGain, amplitude, perc);
            virtualCameraNoise.m_FrequencyGain = Mathf.Lerp(virtualCameraNoise.m_FrequencyGain, frequency, perc);
            perc += fadeRate * durationRate;
            yield return new WaitForSeconds(fadeRate);
        }
    }

}
