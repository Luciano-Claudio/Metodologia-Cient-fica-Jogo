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
        public GameObject ForcaPanel;
        public GameObject WrongPanel;


        public Spin spin;
        private Animator animPanel;

        private void Start()
        {
            animPanel = Panel.GetComponent<Animator>();
        }

        public void Click()
        {
            if (CorrectAnswer)
            {
                Confetti.SetActive(true);
                StartCoroutine(InCorrectAnswer());

            }
            else
            {
                StartCoroutine(InIncorrectAnswer());
            }
        }


        IEnumerator InCorrectAnswer()
        {
            spin.IgnoreDraw.Add(wheelPieceIndex);
            spin.DestroyPickerWheel();
            yield return new WaitForSeconds(3f);
            Confetti.SetActive(false);
            //Panel.SetActive(false);
            ForcaPanel.SetActive(true);
        }
        IEnumerator InIncorrectAnswer()
        {
            WrongPanel.SetActive(true);
            yield return new WaitForSeconds(1f);
            WrongPanel.SetActive(false);
            animPanel.SetBool("Disable", true);
        }
    }
}
