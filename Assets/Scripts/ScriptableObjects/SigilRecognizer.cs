using System;
using SquallOfSpells.SigilSystem;
using Unity.Mathematics;
using UnityEngine;
using Logger = SquallOfSpells.Plugins.Logger;

namespace SquallOfSpells
{
    [CreateAssetMenu(fileName = "Sigil recognizer", menuName = "Scriptable objects/sigil recognizer")]
    public class SigilRecognizer : ScriptableObject
    {
        [SerializeField] private SigilStorage storage;

        [SerializeField] private bool log;

        [Header("Recognition settings")]
        [SerializeField] private float acceptableHeightDiff = 0.2f;
        [SerializeField] private float acceptableMassCenterDiff = 0.2f;
        [SerializeField] private float heightPow                = 1.4f;
        [SerializeField] private float acceptableMismatch       = 1f;

        private Logger logger;

        private void OnEnable()
        {
            logger = new Logger(this, log);
        }

        // ReSharper disable once IdentifierTypo
        public event Action<Sigil> OnRecognized;

        public void Recognize(Sigil sigilToRecognize)
        {
            Sigil bestGuess = null;
            float minMismatch = float.PositiveInfinity;

            foreach (Sigil sigilCandidate in storage.sigils)
            {
                if (math.abs(sigilCandidate.height / sigilToRecognize.height - 1) > acceptableHeightDiff)
                {
                    logger.Log(sigilCandidate.name + " height mismatch");

                    continue;
                }

                if (math.abs(sigilCandidate.massCenter.x - sigilToRecognize.massCenter.x) > acceptableMassCenterDiff)
                {
                    logger.Log(sigilCandidate.name + " mass center X mismatch");

                    continue;
                }

                if (
                    math.abs(sigilCandidate.massCenter.y - sigilToRecognize.massCenter.y) /
                    sigilToRecognize.height >
                    acceptableMassCenterDiff
                )
                {
                    logger.Log(sigilCandidate.name + " mass center Y mismatch");

                    continue;
                }


                float mismatch = CompareSigils(sigilCandidate, sigilToRecognize);

                logger.Log(sigilCandidate.name + " | mismatch: " + mismatch);

                if (mismatch < minMismatch)
                {
                    minMismatch = mismatch;
                    bestGuess = sigilCandidate;
                }
            }

            logger.Log("minimal mismatch is " + minMismatch + " acceptable mismatch is " + acceptableMismatch);

            if (minMismatch > acceptableMismatch)
            {
                OnRecognized?.Invoke(null);
            }
            else
            {
                OnRecognized?.Invoke(bestGuess);

                if (bestGuess != null)
                    logger.Log("Recognized as " + bestGuess.name);
            }
        }


        private float CompareSigils(Sigil s1, Sigil s2)
        {
            float rawMismatch = CompareBaseAndMask(s1, s2) + CompareBaseAndMask(s2, s1);

            return rawMismatch / math.pow(s1.height, heightPow);
        }

        private static float CompareBaseAndMask(Sigil baseSigil, Sigil maskSigil)
        {
            float totalMismatch = 0;

            foreach (Vector2 basePoint in baseSigil.points)
            {
                float minSqrDistance = Mathf.Infinity;

                foreach (Vector2 maskPoint in maskSigil.points)
                {
                    float sqrDistance = (maskPoint - basePoint).sqrMagnitude;

                    if (sqrDistance < minSqrDistance)
                        minSqrDistance = sqrDistance;
                }

                totalMismatch += minSqrDistance;
            }

            return totalMismatch;
        }
    }
}