using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace PickerWheelUI
{
    public class QuestionPanel : MonoBehaviour
    {
        [SerializeField] private Text question;
        [SerializeField] private Text[] toggles;
        public Spin spin;
        // Start is called before the first frame update
        void Awake()
        {
            spin.SpinStop += OnSpinStop;
        }

        private void OnSpinStop(WheelPiece wheelPiece)
        {
            Debug.Log("Entrou na função");
            question.text = wheelPiece.Question;

            for(int i = 0; i < toggles.Length; i++)
            {
                toggles[i].text = wheelPiece.Toggles[i];
            }
        }
    }
}
