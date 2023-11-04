using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CG
{
    public class UISeedHandler : MonoBehaviour
    {
        [SerializeField] private Button _regenerateBtn;
        [SerializeField] private TMP_InputField _seedText;

        private void Start()
        {
            _regenerateBtn.onClick.AddListener(Regenerate);
            int currentSeed = GameManager.Instance.WorldGenerator.Seed;
            _seedText.text = currentSeed.ToString();
        }

        private void Regenerate()
        {
            int newSeed = int.Parse(_seedText.text);
            if (newSeed < 0 || newSeed >= Int32.MaxValue)
            {
                Debug.LogWarning("Selected seed is not correct");
                return;
            }
            GameManager.Instance.WorldGenerator.Seed = newSeed;
        }
    }
}
