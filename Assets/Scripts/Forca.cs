using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Forca : MonoBehaviour
{
    [SerializeField] private GameObject panel;
    [SerializeField] private GameObject forcaPanel;
    [SerializeField] private GameObject wrongPanel;
    [SerializeField] private GameObject correctButton;
    [SerializeField] private GameObject Confetti;

    [SerializeField] private InputField input;
    [SerializeField] private GameObject[] cubes;
    [SerializeField] private Material material;

    [SerializeField] private List<MeshRenderer> cubesMesh;
    [SerializeField] private List<Animator> anim;
    [SerializeField] private List<Cube> cubesBool;
    [SerializeField] private Text wrongLetters;

    private HashSet<char> letters;

    bool Completou = false;

    private void Start()
    {
        letters = new HashSet<char>();
        wrongLetters.text = "";
        input.interactable = true;
        cubesMesh = new List<MeshRenderer>();
        foreach(GameObject g in cubes)
        {
            cubesMesh.Add(g.GetComponent<MeshRenderer>());
            anim.Add(g.GetComponent<Animator>());
            cubesBool.Add(g.GetComponent<Cube>());
        }
    }
    public void OnCorrect()
    {
        char[] str = input.text.ToUpper().ToCharArray();
        char[] palavra = "CLASSIFICAÇAO".ToCharArray();
        char[] palavraCerta = "CLASSIFICAÇÃO".ToCharArray();
        input.text = "";
        int qtd = 0;
        bool correta = false;

        if (str.Length == 1)
        {
            for (int i = 0; i < palavra.Length; i++)
            {
                for (int j = 0; j < str.Length; j++)
                {
                    if (str[j] == palavra[i])
                    {
                        qtd++;
                        cubesMesh[i].material = material;
                        cubesBool[i].active = true;
                        StartCoroutine(InCorrectAnswer(i));
                    }
                }
            }
            if (qtd == 0)
            {
                StartCoroutine(InIncorrectAnswer());
                foreach (char ch in str)
                {
                    letters.Add(ch);
                }
            }
            else
                StartCoroutine(CorrectAnswer());

        }
        else
        {
            correta = true;
            for (int i = 0; i < palavra.Length; i++)
            {
                if(palavraCerta[i] != str[i])
                {
                    StartCoroutine(InIncorrectAnswer());
                    correta = false;
                    break;
                }

            }
        }
        wrongLetters.text = "";
        foreach (char ch in letters)
        {
            wrongLetters.text += ch + " ";
        }


        int qtdCubes = 0;
        foreach(Cube c in cubesBool)
        {
            if (c.active) qtdCubes++;

        }

        if (qtdCubes++ >= 13 || correta)
        {
            StartCoroutine(InCorrectAnswer());
            Debug.Log("Completou");
            Completou = true;
            Confetti.SetActive(true);
        }

    }

    IEnumerator InCorrectAnswer(int index)
    {
        input.interactable = false;
        yield return new WaitForSeconds(1f);
        if (!anim[index].GetBool("Done"))
        {
            anim[index].SetBool("Done", true);
        }
    }

    IEnumerator InCorrectAnswer()
    {
        input.interactable = false;
        for (int i = 0; i < cubesMesh.Count; i++)
        {
            if (!anim[i].GetBool("Done"))
            {
                cubesMesh[i].material = material;
                cubesBool[i].active = true;
            }
        }
            yield return new WaitForSeconds(1f);
        for(int i = 0; i < anim.Count; i++)
        {
            if (!anim[i].GetBool("Done"))
            {
                anim[i].SetBool("Done", true);
            }

        }
    }
    IEnumerator CorrectAnswer()
    {
        yield return new WaitForSeconds(3f);
        if(!Completou) correctButton.SetActive(true);
    }

    public void CorrectButton()
    {
        input.interactable = true;
        correctButton.SetActive(false);
        forcaPanel.SetActive(false);
        panel.SetActive(false);
    }

    IEnumerator InIncorrectAnswer()
    {
        wrongPanel.SetActive(true);
        yield return new WaitForSeconds(1f);
        wrongPanel.SetActive(false);
        forcaPanel.SetActive(false);
        panel.SetActive(false);
    }
}
