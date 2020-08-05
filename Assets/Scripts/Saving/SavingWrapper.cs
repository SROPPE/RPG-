using RPG.Saving;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace RPG.SceneManagment
{

    public class SavingWrapper : MonoBehaviour
    {
        [SerializeField] float loadScreenOutTime = 1f;
        const string defaultSaveFile = "save";
        private void Awake()
        {
            StartCoroutine(LoadLastScene());
        }
        private IEnumerator LoadLastScene()
        {
            yield return GetComponent<SavingSystem>().LoadLastScene(defaultSaveFile);
            var screen = FindObjectOfType<LoadScreen>();
            screen.LoadScreenImmediate();
            yield return screen.LoadScreenOut(loadScreenOutTime);
        }
    
        private void Update()
        {
            if(Input.GetKeyDown(KeyCode.L))
            {
                Load();
            }
            else if(Input.GetKeyDown(KeyCode.S))
            {
                Save();
            }
        }

        public void Save()
        {
            GetComponent<SavingSystem>().Save(defaultSaveFile);
        }

        public void Load()
        {
            GetComponent<SavingSystem>().Load(defaultSaveFile);
        }
    }
}