using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelParticipleHadler : MonoBehaviour
{
    float particleEmissionRate = 0;


    TopDownCarController topDownCarController;

    ParticleSystem particleSystemSmoke;
    ParticleSystem.EmissionModule particleSystemEmissionModule;

    private void Awake()
    {
        topDownCarController = GetComponentInParent<TopDownCarController>();
        particleSystemSmoke = GetComponent<ParticleSystem>();
        particleSystemEmissionModule = particleSystemSmoke.emission;
        particleSystemEmissionModule.rateOverTime = 0;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        particleEmissionRate = Mathf.Lerp(particleEmissionRate, 0, Time.deltaTime * 5);
        particleSystemEmissionModule.rateOverTime = particleEmissionRate;

        if (topDownCarController.IsTireScreepching( out float lateralVelocity, out bool isBraking))
        {
            if (isBraking)
                particleEmissionRate = 30;
            else particleEmissionRate = Mathf.Abs(lateralVelocity) * 2;
        }
     

    }
}
