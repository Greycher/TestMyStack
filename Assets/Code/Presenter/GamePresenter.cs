using Code.View;
using UnityEngine;

namespace Code.Presenter
{
    public class GamePresenter : MonoBehaviour
    {
        [SerializeField] private PlayerModel playerModel;
        [SerializeField] private float distanceBetweenJengas = 5f;
        [SerializeField] private JengaView jengaViewPrefab;

        private void Awake()
        {
            StartCoroutine(playerModel.GetPlayerDataCoroutine(OnPlayerDataFetched));
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
            for (int i = 0; i < grades.Length; i++)
            {
                Instantiate(jengaViewPrefab, x * Vector3.right, Quaternion.identity)
                    .BuildJenga(grades[i]);
                ;
                x += distanceBetweenJengas;
            }
        }
    }
}