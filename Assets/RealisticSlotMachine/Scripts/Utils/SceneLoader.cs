using System.Collections;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

/*
    10012019
    - add Action<float> progressDel
    - remove public  Action LoadingCallBack
    13032019
    - add method  ReLoadCurrentScene()
    11112019
     - use LoadGroupPrefab popup
    18.11.2019
     - add GetCurrentSceneName();
*/

namespace Mkey
{
    public class SceneLoader : MonoBehaviour
    {
        [SerializeField]
        private PopUpsController LoadGroupPrefab;

        private float loadProgress;

        public static SceneLoader Instance;

        #region temp vars
        private SlotGuiController MGUI { get { return SlotGuiController.Instance; } }
        private PopUpsController LoadGroup;
        private SimpleSlider simpleSlider;
        #endregion temp vars

        #region regular
        private void Awake()
        {
            if (Instance != null) { Destroy(gameObject); }
            else
            {
                Instance = this;
            }
        }
        #endregion regular

        public void LoadScene(int scene)
        {
            StartCoroutine(AsyncLoadBeaty(scene,null, null));
        }

        public void LoadScene(int scene, Action completeCallBack)
        {
            StartCoroutine(AsyncLoadBeaty(scene, null, completeCallBack));
        }

        public void LoadScene(int scene, Action<float> progresUpdate, Action completeCallBack)
        {
            StartCoroutine(AsyncLoadBeaty(scene, progresUpdate, completeCallBack));
        }

        public void LoadScene(string sceneName)
        {
            int scene = SceneManager.GetSceneByName(sceneName).buildIndex;
            StartCoroutine(AsyncLoadBeaty(scene,null, null));
        }

        public void ReLoadCurrentScene()
        {
            int scene = SceneManager.GetActiveScene().buildIndex;
            StartCoroutine(AsyncLoadBeaty(scene, null, null));
        }

        private IEnumerator AsyncLoadBeaty(int scene, Action <float> progresUpdate, Action completeCallBack)
        {
            float apprLoadTime = 0.5f;
            float steps = 100f;
            float loadTime = 0.0f;
            loadProgress = 0;
            bool fin = false;

            if (LoadGroupPrefab) LoadGroup = MGUI.ShowPopUp(LoadGroupPrefab);
            if (LoadGroup) simpleSlider = LoadGroup.GetComponent<SimpleSlider>();
            if (simpleSlider) simpleSlider.value = loadProgress;
            GuiFader_v2 gF = LoadGroup.GetComponent<GuiFader_v2>();
           
            if (gF)
            {
                gF.FadeIn(0, () => { fin = true; });
            }
            else
            {
                fin = true;
            }

            while (!fin)
            {
                yield return null;
            }

            AsyncOperation ao = SceneManager.LoadSceneAsync(scene);
            ao.allowSceneActivation = false;
            float lastTime = Time.time;
            while (!ao.isDone && loadProgress < 0.99f)
            {
                loadTime += (Time.time - lastTime);
                lastTime = Time.time;
                loadProgress = Mathf.Clamp01(loadProgress + 0.01f);
                if (simpleSlider) simpleSlider.value = loadProgress;

                if (loadTime >= 0.5f * apprLoadTime && (ao.progress < 0.5f))
                {
                    apprLoadTime *= 1.1f;
                }
                else if (loadTime >= 0.5f * apprLoadTime && (ao.progress > 0.5f))
                {
                    apprLoadTime /= 1.1f;
                }

                if (ao.progress >= 0.90f && !ao.allowSceneActivation && loadProgress >= 0.99f)
                {
                    ao.allowSceneActivation = true;
                }
                progresUpdate?.Invoke(loadProgress);
                // Debug.Log("waite scene: " + loadTime + "ao.progress : " + ao.progress);
                yield return new WaitForSeconds(apprLoadTime / steps); ;
            }
            if (LoadGroup) LoadGroup.CloseWindow();
            completeCallBack?.Invoke();
        }

        public static string GetCurrentSceneName()
        {
            return SceneManager.GetActiveScene().name;
        }
    }
}