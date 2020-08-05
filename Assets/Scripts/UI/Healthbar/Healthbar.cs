using RPG.Combat;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace RPG.UI
{
    public class Healthbar : MonoBehaviour
    {
        [SerializeField] HealthSystem healthSystem;
        [SerializeField] RectTransform rectTransform;
        [SerializeField] Canvas canvas;
        private void Update()
        {
            if (healthSystem.GetNormilizedHP() == 0 || healthSystem.GetNormilizedHP() == 100)
            {
                canvas.enabled = false;
                return;
            }
            canvas.enabled = true;
            rectTransform.localScale = new Vector3((float)healthSystem.GetNormilizedHP() / 100, 1, 1);
        }
    }
}
