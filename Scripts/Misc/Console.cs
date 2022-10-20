using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Console : MonoBehaviour
{
    InputField consoleInput;
    GunHandler gunHandler;

    void Start()
    {
        gunHandler = GameObject.FindWithTag("Gun").GetComponent<GunHandler>();
        consoleInput = this.gameObject.GetComponent<InputField>();
    }

    public void CommitNewGun()
    {
        string desiredGunName = consoleInput.text;
        Gun desiredGun = Resources.Load<Gun>(desiredGunName);
        Debug.Log("Loading " + desiredGun._gunName);
        if (desiredGun != null)
        {
            gunHandler.currentGun = desiredGun;
        }
    }
}
