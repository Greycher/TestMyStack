using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using Button = UnityEngine.UI.Button;

namespace Code.View
{
    public class ErrorPopup : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI errorLabel;
        [SerializeField] private Button retryBtn;

        public UnityEvent OnRetryRequested => retryBtn.onClick;
        
        public void Show(string errorMessage)
        {
            errorLabel.text = errorMessage;
            gameObject.SetActive(true);
        }

        private void OnEnable()
        {
            OnRetryRequested.AddListener(Close);
        }

        private void OnDisable()
        {
            OnRetryRequested.RemoveListener(Close);
        }

        private void OnDestroy()
        {
            OnRetryRequested.RemoveAllListeners();
        }
        
        private void Close()
        {
            gameObject.SetActive(false);
        }
    }
}