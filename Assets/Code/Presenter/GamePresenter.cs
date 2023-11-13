using System.Collections;
using Code.View;
using UnityEngine;
using UnityEngine.Networking;

namespace Code.Presenter
{
    public class GamePresenter : MonoBehaviour
    {
        [SerializeField] private StudentModel studentModel;
        [SerializeField] private StackPresenter stackPresenter;
        [SerializeField] private LoadingView loadingView;
        [SerializeField] private ErrorPopup errorPopup;
        [SerializeField] private float minLoadingDuration = 1f;
        
        private float _loadingStartTime;

        private void Awake()
        {
            _loadingStartTime = Time.realtimeSinceStartup;
            GetStudentData();
        }

        private void GetStudentData()
        {
            StartCoroutine(studentModel.GetStudentDataCoroutine(OnGetStudentDataSuccess, OnGetStudentDataFail));
        }

        private void OnGetStudentDataSuccess(StudentModel studentModel)
        {
            var elapsedTime = Time.realtimeSinceStartup - _loadingStartTime;
            if (elapsedTime < minLoadingDuration)
            {
                StartCoroutine(StartTheGameWithDelayCoroutine(studentModel, minLoadingDuration - elapsedTime));
            }
            else
            {
                StartTheGame(studentModel);
            }
        }

        private IEnumerator StartTheGameWithDelayCoroutine(StudentModel studentModel, float elapsedTime)
        {
            yield return new WaitForSeconds(elapsedTime);
            StartTheGame(studentModel);
        }

        private void StartTheGame(StudentModel studentModel)
        {
            loadingView.gameObject.SetActive(false);
            stackPresenter.OnGetStudenDataSuccess(studentModel);
        }

        private void OnGetStudentDataFail(UnityWebRequest.Result result)
        {
            loadingView.StopLoadingAnimation();
            errorPopup.OnRetryRequested.AddListener(OnRetryRequested);

            string errorMessage;
            switch (result)
            {
                case UnityWebRequest.Result.ConnectionError:
                    errorMessage = "There was a connection error while getting student data. " +
                                   "Make sure to have reliable and secure connection.";
                    break;
                    
                default: 
                    errorMessage = "There was a unkown error while getting student data.";
                    break;
            }
            
            errorPopup.Show(errorMessage);
        }

        private void OnRetryRequested()
        {
            errorPopup.OnRetryRequested.RemoveListener(OnRetryRequested);
            GetStudentData();
        }
    }
}