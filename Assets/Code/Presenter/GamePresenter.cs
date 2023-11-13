using System;
using System.Collections;
using Code.View;
using UnityEngine;
using UnityEngine.UI;

namespace Code.Presenter
{
    public class GamePresenter : MonoBehaviour
    {
        [SerializeField] private PlayerModel playerModel;
        [SerializeField] private float distanceBetweenJengas = 5f;
        [SerializeField] private JengaView jengaViewPrefab;
        [SerializeField] private Button testMyStackBtn;
        [SerializeField] private float jengaTestingDuration = 5f;

        private JengaView[] _jengaViews;
        [SerializeField] private int _selectedJengaIndex; 

        private void Awake()
        {
            StartCoroutine(playerModel.GetPlayerDataCoroutine(OnPlayerDataFetched));
        }

        private void OnEnable()
        {
            testMyStackBtn.onClick.AddListener(TestSelectedJenga);
        }

        private void OnDisable()
        {
            testMyStackBtn.onClick.RemoveListener(TestSelectedJenga);
        }

        private void OnPlayerDataFetched(PlayerModel playerModel)
        {
            foreach (var grade in playerModel.Grades)
            {
                Debug.Log($"{grade.DisplayName} has {grade.Blocks.Length} blocks.");
            }

            BuildJengas(playerModel);
        }

        private void BuildJengas(PlayerModel playerModel)
        {
            var grades = playerModel.Grades;
            var count = grades.Length;
            var x = -((count - 1) / 2 + (1 - count % 2) * 0.5f) * distanceBetweenJengas;
            _jengaViews = new JengaView[grades.Length];
            for (int i = 0; i < grades.Length; i++)
            {
                _jengaViews[i] = Instantiate(jengaViewPrefab, x * Vector3.right, Quaternion.identity);
                _jengaViews[i].BuildJenga(grades[i]);
                x += distanceBetweenJengas;
            }
        }
        
        private void TestSelectedJenga()
        {
            StartCoroutine(TestJengaCoroutine(_selectedJengaIndex));
        }

        private IEnumerator TestJengaCoroutine(int jengaIndex)
        {
            testMyStackBtn.interactable = false;
            _jengaViews[jengaIndex].TestTheStack();
            yield return new WaitForSeconds(jengaTestingDuration);
            _jengaViews[jengaIndex].ResetStack();
            testMyStackBtn.interactable = true;
        }
    }
}