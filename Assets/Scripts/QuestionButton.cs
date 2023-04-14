using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace PickerWheelUI
{
    public class QuestionButton : MonoBehaviour
    {
        public int wheelPieceIndex;
        public bool CorrectAnswer = false;
        public GameObject Confetti;
        public GameObject pickerWheel;
        public GameObject Panel;

        public Transform PickerWheelPosition;
        public GameObject pickerWheelPrefab;

        public Spin spin;

        public void Click()
        {
            if (CorrectAnswer)
            {
                Debug.Log("Acertou");
                Confetti.SetActive(true);
                spin.IgnoreDraw.Add(wheelPieceIndex);
                StartCoroutine(InCorrectAnswer());

            }
            else
            {
                Debug.Log("Errou");
                Panel.SetActive(false);
            }
        }


        IEnumerator InCorrectAnswer()
        {
            spin.DestroyPickerWheel();
            yield return new WaitForSeconds(3f);
            Confetti.SetActive(false);
            Panel.SetActive(false);
        }
    }
}
