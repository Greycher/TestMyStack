using System;
using System.Collections;
using System.Linq;
using Code.Model;
using Code.Others;
using UnityEngine;
using Newtonsoft.Json;
using UnityEngine.Networking;

[CreateAssetMenu(menuName = Constants.Models + "/" + nameof(StudentModel), fileName = nameof(StudentModel))]
public class StudentModel : ScriptableObject
{
    [SerializeField] private string studentDataGetURl = "https://ga1vqcu3o1.execute-api.us-east-1.amazonaws.com/Assessment/stack";
    
    private Grade[] _grades = new Grade[3];

    public Grade[] Grades => _grades;

    public IEnumerator GetStudentDataCoroutine(Action<StudentModel> onGetStudentDataSuccess, Action<UnityWebRequest.Result> onGetStudentDataFail)
    {
        using (var request = UnityWebRequest.Get(studentDataGetURl))
        {
            yield return request.SendWebRequest();
            
            switch (request.result)
            {
                case UnityWebRequest.Result.Success:
                    var topicArr = JsonConvert.DeserializeObject<Topic[]>(request.downloadHandler.text);
                    var topicsGroupedByGrade = topicArr.GroupBy(top => top.Grade);
                    var jaggedTopicArr = topicsGroupedByGrade.Select(group => group.ToArray()).ToArray();
                    for (int i = 0; i < _grades.Length; i++)
                    {
                        var topics = jaggedTopicArr[i].OrderBy(topic => topic.Domain)
                            .ThenBy(topic => topic.Cluster)
                            .ThenBy(topic => topic.Standardid)
                            .ToArray();
                        _grades[i] = new Grade(topics);
                    }
        
                    onGetStudentDataSuccess?.Invoke(this);
                    break;
                
                default:
                    onGetStudentDataFail?.Invoke(request.result);
                    break;
            }
        }
    }
}
