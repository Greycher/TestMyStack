using System;
using System.Collections;
using System.Collections.Generic;
using Code.Model;
using Code.View;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Events;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Code.Presenter
{
    public class StackPresenter : MonoBehaviour
    {
        [SerializeField] private OrbitalCameraMovement orbitalCameraMovement;
        [SerializeField] private float distanceBetweenStacks = 5f;
        [SerializeField] private StackView stackViewPrefab;
        [SerializeField] private Button testMyStackBtn;
        [SerializeField] private float stackTestingDuration = 5f;
        [SerializeField] private Button nextStackBtn;
        [SerializeField] private Button prevStackBtn;

        private StackView[] _stackViews;
        private int _selectedStackIndex;
        private HashSet<int> _stackIndicesInTest = new ();
        private StudentModel _studentModel;

        public UnityEvent OnSelectedStackChangedEvent { get; set; } = new UnityEvent();

        private void OnEnable()
        {
            testMyStackBtn.onClick.AddListener(TestSelectedStack);
            nextStackBtn.onClick.AddListener(SelectNextStack);
            prevStackBtn.onClick.AddListener(SelectPrevStack);
        }

        private void OnDisable()
        {
            testMyStackBtn.onClick.RemoveListener(TestSelectedStack);
            nextStackBtn.onClick.RemoveListener(SelectNextStack);
            prevStackBtn.onClick.RemoveListener(SelectPrevStack);
        }

        private void OnDestroy()
        {
            OnSelectedStackChangedEvent.RemoveAllListeners();
        }

        public void OnGetStudenDataSuccess(StudentModel studentModel)
        {
            _studentModel = studentModel;
            BuildStacks(studentModel);
            UpdateSelectedStackSafe(0);
            orbitalCameraMovement.TeleportToTarget();
        }

        private void BuildStacks(StudentModel studentModel)
        {
            var grades = studentModel.Grades;
            var count = grades.Length;
            var x = -((count - 1) / 2 + (1 - count % 2) * 0.5f) * distanceBetweenStacks;
            _stackViews = new StackView[grades.Length];
            for (int i = 0; i < grades.Length; i++)
            {
                _stackViews[i] = Instantiate(stackViewPrefab, x * Vector3.right, Quaternion.identity);
                _stackViews[i].BuildStack(grades[i]);
                x += distanceBetweenStacks;
            }
        }
        
        private void TestSelectedStack()
        {
            StartCoroutine(TestStackCoroutine(_selectedStackIndex));
        }

        private IEnumerator TestStackCoroutine(int stackIndex)
        {
            Assert.IsFalse(_stackIndicesInTest.Contains(stackIndex));
            testMyStackBtn.interactable = false;
            _stackIndicesInTest.Add(stackIndex);
            _stackViews[stackIndex].TestTheStack();
            yield return new WaitForSeconds(stackTestingDuration);
            _stackViews[stackIndex].ResetStack();
            _stackIndicesInTest.Remove(stackIndex);
            UpdateTestMyStackBtnInteractable();
        }
        
        private void SelectPrevStack()
        {
            UpdateSelectedStackSafe(_selectedStackIndex - 1);
        }

        private void SelectNextStack()
        {
            UpdateSelectedStackSafe(_selectedStackIndex + 1);
        }

        private void UpdateSelectedStackSafe(int stackIndex)
        {
            stackIndex = Mathf.Clamp(stackIndex, 0, _stackViews.Length - 1);
            _selectedStackIndex = stackIndex;
            orbitalCameraMovement.ChangeTarget(_stackViews[stackIndex].transform);
            prevStackBtn.interactable = stackIndex > 0;
            nextStackBtn.interactable = stackIndex < _stackViews.Length - 1;
            UpdateTestMyStackBtnInteractable();
            OnSelectedStackChangedEvent.Invoke();
        }

        private void UpdateTestMyStackBtnInteractable()
        {
            testMyStackBtn.interactable = !_stackIndicesInTest.Contains(_selectedStackIndex);
        }

        public bool DoesBelongToSelectedStack(BlockView blockView, out int index)
        {
            index = _stackViews[_selectedStackIndex].FindBlockIndex(blockView);
            return index != -1;
        }

        public Topic GetTopicAtSelectedStack(int index)
        {
            return _studentModel.Grades[_selectedStackIndex].Topics[index];
        }
    }
}