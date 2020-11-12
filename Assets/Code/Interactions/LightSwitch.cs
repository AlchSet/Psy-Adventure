using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightSwitch : InteractiveObject
{
    public Light[] lights;

    public bool isOn=true;

    private void Start()
    {
        isReady = true;
    }

    public override void ExitInteract()
    {
        throw new System.NotImplementedException();
    }

    public override void Interact()
    {
        isOn = !isOn;
        UpdateLights();
        deselectEvent.Invoke();
    }


    public void UpdateLights()
    {
        foreach(Light l in lights)
        {
            l.enabled = isOn;

        }
    }

}
