using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.SceneManagment
{
    public class LoadScreen : MonoBehaviour
    {
       
        CanvasGroup canvasGroup;
        Coroutine currentRoutine;
        private void Awake()
        {
            canvasGroup = GetComponent<CanvasGroup>();
            DontDestroyOnLoad(this);
        }
        public void LoadScreenImmediate()
        {
            canvasGroup.alpha = 1;
        }
        public IEnumerator LoadScreenIn(float time)
        {
            return Load(time, 1);
        }
        public IEnumerator LoadScreenOut(float time)
        {
            return Load(time, 0);
        }
        private IEnumerator Load(float time, float to)
        {
            if (currentRoutine != null)
            {
                StopCoroutine(currentRoutine);
            }
            currentRoutine = StartCoroutine(LoadRoutine(time, to));
            yield return currentRoutine;
        }
        private IEnumerator LoadRoutine(float time, float to)
        {
            while (!Mathf.Approximately(canvasGroup.alpha,to))
            {
                canvasGroup.alpha = Mathf.MoveTowards(canvasGroup.alpha, to, Time.deltaTime / time);
                yield return null;
            }
        }
    }
}
