using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using Unity.XR.CoreUtils;
using UnityEngine.XR.Interaction.Toolkit;

public class HandItem : MonoBehaviour
{
    public Transform controlleur;
    public Pioche pioche;
    public XRNode inputSource;

    private Pioche instance;
    bool grabInput;

    void Update()
    {
        InputDevice device = InputDevices.GetDeviceAtXRNode(inputSource);
        device.TryGetFeatureValue(CommonUsages.gripButton, out grabInput);
        if (grabInput)
        {
            if(instance == null) //pour avoir le transform attache pas trouver un autre moyen
                instance = Instantiate<Pioche>(pioche, controlleur.GetChild(0).GetChild(0).GetChild(0).GetChild(0).position, controlleur.transform.rotation, controlleur.GetChild(0).GetChild(0).GetChild(0));
        }
        else
        {
            
            if (instance != null)
                Destroy(instance);
        }
    }
}
