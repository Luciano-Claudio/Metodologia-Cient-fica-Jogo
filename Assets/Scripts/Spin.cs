using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PickerWheelUI;
using UnityEngine.UI;

public class Spin : MonoBehaviour
{
    [SerializeField] private Button uiSpinButton;
    [SerializeField] private PickerWheel pickerWheel;

    // Update is called once per frame
    void Start()
    {
        uiSpinButton.onClick.AddListener(() =>
        {
            pickerWheel.OnSpinStart(() =>
            {
                Debug.Log("Spin Started..");
            });
            pickerWheel.OnSpinEnd(wheelPiece =>
            {
                Debug.Log("Spin end: Label:" + wheelPiece.Label);
            });
            pickerWheel.Spin();
        });
    }
}
