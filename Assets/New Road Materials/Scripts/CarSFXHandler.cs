using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
public class CarSFXHandler : MonoBehaviour
{
    [Header("Mixers")]
    public AudioMixer audioMixer;
    [Header("Audio sources")]
    public AudioSource tiresScreeachingAudioSource;
    public AudioSource engineAudiosource;
    public AudioSource carHitAudioSource;
    float desiredEnginePitch = 0.5f;
    float tireScreechPitch = 0.5f;
    TopDownCarController topDownCarController;

    void Awake()
    {
        topDownCarController = GetComponentInParent<TopDownCarController>();
    }
    // Start is called before the first frame update
    void Start()
    {
        audioMixer.SetFloat("SFXVolume", 0.5f);
    }

    // Update is called once per frame
    void Update()
    {
        UpdateEngineSFX();
        UpdateTiresScreechingSFX();
    }

    void UpdateEngineSFX()
    {
        float velocityMagnitude = topDownCarController.GetVelocityMagnitude();
        float desiredEngineVolume = velocityMagnitude * 0.05f;
        desiredEngineVolume = Mathf.Clamp(desiredEngineVolume, 0.2f, 1.0f);

        engineAudiosource.volume = Mathf.Lerp(engineAudiosource.volume, desiredEngineVolume, Time.deltaTime * 10);
        desiredEnginePitch = velocityMagnitude * 0.2f;
        desiredEnginePitch = Mathf.Clamp(desiredEnginePitch, 0.5f, 2f);
        engineAudiosource.pitch = Mathf.Lerp(engineAudiosource.pitch, desiredEnginePitch, Time.deltaTime * 1.5f);
    }
    void UpdateTiresScreechingSFX()
    {
        if (topDownCarController.IsTireScreepching(out float lateralVelocity, out bool isBraking))
        {
            if (!isBraking)
            {
                tiresScreeachingAudioSource.volume = Mathf.Lerp(tiresScreeachingAudioSource.volume, 1.0f, Time.deltaTime * 10);
                tireScreechPitch = Mathf.Lerp(tireScreechPitch, 0.5f, Time.deltaTime * 10);
            }
            else
            {
                tiresScreeachingAudioSource.volume = Mathf.Abs(lateralVelocity) * 0.05f;
                tireScreechPitch = Mathf.Abs(lateralVelocity) * 0.1f;
            }
        }
        else tiresScreeachingAudioSource.volume = Mathf.Lerp(tiresScreeachingAudioSource.volume, 0, Time.deltaTime * 10);
    }
    private void OnCollisionEnter2D(Collision2D collision2D)
    {
        float relativeVelocity = collision2D.relativeVelocity.magnitude;
        float volume = relativeVelocity * 0.1f;
        carHitAudioSource.pitch = Random.Range(0.95f, 1.05f);
        carHitAudioSource.volume = volume;
        if (!carHitAudioSource.isPlaying)
        {
            carHitAudioSource.Play();
        }
    }
}
