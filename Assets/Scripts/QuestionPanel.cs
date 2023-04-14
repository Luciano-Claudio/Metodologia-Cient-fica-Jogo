using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace PickerWheelUI
{
    public class QuestionPanel : MonoBehaviour
    {
        [SerializeField] private Text question;
        [SerializeField] private Text[] buttonsText;
        [SerializeField] private QuestionButton[] buttons;


        [SerializeField] private Spin spin;


        // Start is called before the first frame update
        void Awake()
        {
            spin.SpinStop += OnSpinStop;
        }

        private void OnSpinStop(WheelPiece wheelPiece)
        {
            question.text = wheelPiece.Question;

            for (int i = 0; i < buttonsText.Length; i++)
            {
                buttonsText[i].text = wheelPiece.Buttons[i];
                if (i == wheelPiece.CorrectIndex)
                {
                    Debug.Log(i);
                    buttons[i].CorrectAnswer = true;
                    buttons[i].wheelPieceIndex = wheelPiece.Index;
                }
            }
        }
    }
}
