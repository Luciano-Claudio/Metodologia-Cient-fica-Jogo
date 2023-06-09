﻿using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.Events;
using System.Collections.Generic;

namespace PickerWheelUI
{

    public class PickerWheel : MonoBehaviour
    {
        [Header("New :")]
        public Button button;
        public Spin spin;
        public List<int> ignoreDraw;

        [Header("References :")]
        [SerializeField] private GameObject linePrefab;
        [SerializeField] private Transform linesParent;

        [Space]
        [SerializeField] private Transform PickerWheelTransform;
        [SerializeField] private Transform wheelCircle;
        [SerializeField] private GameObject wheelPiecePrefab;
        [SerializeField] private Transform wheelPiecesParent;

        [Space]
        [Header("Sounds :")]
        [SerializeField] private AudioSource audioSource;
        [SerializeField] private AudioClip tickAudioClip;
        [SerializeField] [Range(0f, 1f)] private float volume = .5f;
        [SerializeField] [Range(-3f, 3f)] private float pitch = 1f;

        [Space]
        [Header("Picker wheel settings :")]
        [Range(1, 20)] public int spinDuration = 8;
        [SerializeField] [Range(.2f, 2f)] private float wheelSize = 1f;

        [Space]
        [Header("Picker wheel pieces :")]
        public List<WheelPiece> wheelPieces;

        // Events
        private UnityAction onSpinStartEvent;
        private UnityAction<WheelPiece> onSpinEndEvent;


        private bool _isSpinning = false;

        public bool IsSpinning { get { return _isSpinning; } }


        private Vector2 pieceMinSize = new Vector2(81f, 146f);
        private Vector2 pieceMaxSize = new Vector2(144f, 213f);
        private int piecesMin = 1;
        private int piecesMax = 15;

        private float pieceAngle;
        private float halfPieceAngle;
        private float halfPieceAngleWithPaddings;


        private double accumulatedWeight;
        private System.Random rand = new System.Random();

        private List<int> nonZeroChancesIndices = new List<int>();
        private void Awake()
        {
            spin = GameObject.FindAnyObjectByType<Spin>().GetComponent<Spin>();
        }

        private void Start()
        {
            spin.pickerWheel = this;
            spin.uiSpinButton = button;
            ignoreDraw = spin.IgnoreDraw;
            for (int j = 0; j < ignoreDraw.Count; j++)
            {
                wheelPieces.RemoveAt(ignoreDraw[j]);
            }
            if (wheelPieces.Count == 0) Destroy(this);
            pieceAngle = 360 / wheelPieces.Count;
            halfPieceAngle = pieceAngle / 2f;
            halfPieceAngleWithPaddings = halfPieceAngle - (halfPieceAngle / 4f);

            Generate();

            CalculateWeightsAndIndices();
            if (nonZeroChancesIndices.Count == 0)
                Debug.LogError("You can't set all pieces chance to zero");


            SetupAudio();

        }

        private void SetupAudio()
        {
            audioSource.clip = tickAudioClip;
            audioSource.volume = volume;
            audioSource.pitch = pitch;
        }

        private void Generate()
        {
            wheelPiecePrefab = InstantiatePiece();

            RectTransform rt = wheelPiecePrefab.transform.GetChild(0).GetComponent<RectTransform>();
            float pieceWidth = Mathf.Lerp(pieceMinSize.x, pieceMaxSize.x, 1f - Mathf.InverseLerp(piecesMin, piecesMax, wheelPieces.Count));
            float pieceHeight = Mathf.Lerp(pieceMinSize.y, pieceMaxSize.y, 1f - Mathf.InverseLerp(piecesMin, piecesMax, wheelPieces.Count));
            rt.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, pieceWidth);
            rt.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, pieceHeight);

            for (int i = 0; i < wheelPieces.Count; i++)
            {
                DrawPiece(i);
            }

            Destroy(wheelPiecePrefab);
        }

        private void DrawPiece(int index)
        {
            WheelPiece piece = wheelPieces[index];
            Transform pieceTrns = InstantiatePiece().transform.GetChild(0);

            pieceTrns.GetChild(0).GetComponent<Image>().sprite = piece.Icon;
            pieceTrns.GetChild(1).GetComponent<Text>().text = piece.Label;
            //pieceTrns.GetChild (2).GetComponent <Text> ().text = piece.Amount.ToString () ;

            //Line
            Transform lineTrns = Instantiate(linePrefab, linesParent.position, Quaternion.identity, linesParent).transform;
            lineTrns.RotateAround(wheelPiecesParent.position, Vector3.back, (pieceAngle * index) + halfPieceAngle);

            pieceTrns.RotateAround(wheelPiecesParent.position, Vector3.back, pieceAngle * index);
        }

        private GameObject InstantiatePiece()
        {
            return Instantiate(wheelPiecePrefab, wheelPiecesParent.position, Quaternion.identity, wheelPiecesParent);
        }


        public void Spin()
        {
            if (!_isSpinning)
            {
                _isSpinning = true;
                if (onSpinStartEvent != null)
                    onSpinStartEvent.Invoke();

                int index = GetRandomPieceIndex();
                WheelPiece piece = wheelPieces[index];

                if (piece.Chance == 0 && nonZeroChancesIndices.Count != 0)
                {
                    index = nonZeroChancesIndices[Random.Range(0, nonZeroChancesIndices.Count)];
                    piece = wheelPieces[index];
                }

                float angle = -(pieceAngle * index);

                float rightOffset = (angle - halfPieceAngleWithPaddings) % 360;
                float leftOffset = (angle + halfPieceAngleWithPaddings) % 360;

                float randomAngle = Random.Range(leftOffset, rightOffset);
                spinDuration = Random.Range(5, spinDuration);

                Vector3 targetRotation = Vector3.back * (randomAngle + 2 * 360 * spinDuration);

                //float prevAngle = wheelCircle.eulerAngles.z + halfPieceAngle ;
                float prevAngle, currentAngle;
                prevAngle = currentAngle = wheelCircle.eulerAngles.z;

                bool isIndicatorOnTheLine = false;

                wheelCircle
                .DORotate(targetRotation, spinDuration, RotateMode.FastBeyond360)
                .SetEase(Ease.InOutQuart)
                .OnUpdate(() =>
                {
                    float diff = Mathf.Abs(prevAngle - currentAngle);
                    if (diff >= halfPieceAngle)
                    {
                        if (isIndicatorOnTheLine)
                        {
                            audioSource.PlayOneShot(audioSource.clip);
                        }
                        prevAngle = currentAngle;
                        isIndicatorOnTheLine = !isIndicatorOnTheLine;
                    }
                    currentAngle = wheelCircle.eulerAngles.z;
                })
                .OnComplete(() =>
                {
                    _isSpinning = false;
                    if (onSpinEndEvent != null)
                        onSpinEndEvent.Invoke(piece);

                    onSpinStartEvent = null;
                    onSpinEndEvent = null;
                });

            }
        }

        private void FixedUpdate()
        {

        }

        public void OnSpinStart(UnityAction action)
        {
            onSpinStartEvent = action;
        }

        public void OnSpinEnd(UnityAction<WheelPiece> action)
        {
            onSpinEndEvent = action;
        }


        private int GetRandomPieceIndex()
        {
            double r = rand.NextDouble() * accumulatedWeight;

            for (int i = 0; i < wheelPieces.Count; i++)
                if (wheelPieces[i]._weight >= r)
                    return i;

            return 0;
        }

        private void CalculateWeightsAndIndices()
        {
            for (int i = 0; i < wheelPieces.Count; i++)
            {
                WheelPiece piece = wheelPieces[i];

                //add weights:
                accumulatedWeight += piece.Chance;
                piece._weight = accumulatedWeight;

                //add index :
                piece.Index = i;

                //save non zero chance indices:
                if (piece.Chance > 0)
                    nonZeroChancesIndices.Add(i);
            }
        }




    }
}