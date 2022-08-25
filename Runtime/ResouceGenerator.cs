using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace com.floxgames.IdleTycoonSDK {
    public class ResouceGenerator : MonoBehaviour
    {
        public float growthTime = 10.0f;
        public GeneratorState state;
        Vector3 initalScale;
        public bool isActivated = false;

        void Awake() => initalScale = transform.localScale;
        public void changeState(GeneratorState newState)
        {
            state = newState;
            switch (newState)
            {
                case GeneratorState.Empty:
                    onEmptyState();
                    break;
                case GeneratorState.Half:
                    onHalfState();
                    break;
                case GeneratorState.Full:
                    onFullState();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(state), newState, null);
            }
        }

        private void onFullState()
        {
            transform.localScale = initalScale;
            show();
        }

        private void onHalfState()
        {
            transform.localScale = initalScale * .5f;
            show();
            StartCoroutine(grow());
        }

        private void onEmptyState()
        {
            hide();
            StartCoroutine(grow());
        }

        void hide() => GetComponent<SpriteRenderer>().enabled = false;
        void show() => GetComponent<SpriteRenderer>().enabled = true;
        IEnumerator grow()
        {
            yield return new WaitForSeconds(growthTime);
            nextGrowthState();
        }

        public void nextGrowthState()
        {
            if (state == GeneratorState.Empty) changeState(GeneratorState.Half);
            else if (state == GeneratorState.Half) changeState(GeneratorState.Full);
        }
    }

    public enum GeneratorState
    {
        Empty,
        Half,
        Full
    }
}

