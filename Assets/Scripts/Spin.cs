using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PickerWheelUI;
using UnityEngine.UI;
using UnityEngine.Events;

public class Spin : MonoBehaviour
{
    public Button uiSpinButton;
    public PickerWheel pickerWheel;

    public delegate void SpinStopHandler(WheelPiece wheelPiece);
    public event SpinStopHandler SpinStop;

    public GameObject panel;


    public Transform PickerWheelPosition;
    public GameObject pickerWheelPrefab;
    public GameObject pickerWheelGameObject;

    public List<int> IgnoreDraw;
    // Update is called once per frame
    private void Awake()
    {
        IgnoreDraw = new List<int>();
        // IgnoreDraw.Add(1);
    }
    void Update()
    {
        //DestroyPickerWheel();
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
                    SpinStop(wheelPiece);
                }
            });
            pickerWheel.Spin();
        });
    }

    public void DestroyPickerWheel()
    {
        pickerWheelGameObject = pickerWheel.gameObject;
        Destroy(pickerWheelGameObject);
        pickerWheelGameObject = Instantiate(pickerWheelPrefab, PickerWheelPosition.position, Quaternion.identity, PickerWheelPosition);
        Vector3 scale = pickerWheel.transform.localScale;
        scale.x = 0.02f;
        scale.y = 0.02f;
        pickerWheelGameObject.transform.localScale = scale;

        pickerWheel = pickerWheelGameObject.GetComponent<PickerWheel>();
    }
}
