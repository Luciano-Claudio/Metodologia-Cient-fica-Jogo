using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PickerWheelUI;
using UnityEngine.UI;
using UnityEngine.Events;

public class Spin : MonoBehaviour
{
    [SerializeField] private Button uiSpinButton;
    [SerializeField] private PickerWheel pickerWheel;

    public delegate void SpinStopHandler(WheelPiece wheelPiece);
    public event SpinStopHandler SpinStop;

    public GameObject panel;
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
                panel.SetActive(true);

                if (SpinStop != null)
                {
                    Debug.Log("Entrou");
                    SpinStop(wheelPiece);
                }
            });
            pickerWheel.Spin();
        });
    }
}
