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
        public GameObject Panel;


        public Spin spin;

        public void Click()
        {
            if (CorrectAnswer)
            {
                Debug.Log("Acertou");
                Confetti.SetActive(true);
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
            spin.IgnoreDraw.Add(wheelPieceIndex);
            spin.DestroyPickerWheel();
            yield return new WaitForSeconds(3f);
            Confetti.SetActive(false);
            Panel.SetActive(false);
        }
    }
}
