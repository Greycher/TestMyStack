using System;
using System.Collections;
using System.Linq;
using System.Text.RegularExpressions;
using Code.Model;
using Code.Others;
using UnityEngine;
using Newtonsoft.Json;
using UnityEngine.Networking;
using UnityEngine.Serialization;

[CreateAssetMenu(menuName = Constants.Models + "/" + nameof(PlayerModel), fileName = nameof(PlayerModel))]
public class PlayerModel : ScriptableObject
{
    [SerializeField] private TextAsset playerDataTextAsset;
    [SerializeField] private string playerDataGetURl = "https://ga1vqcu3o1.execute-api.us-east-1.amazonaws.com/Assessment/stack";
    
    private Grade[] _grades = new Grade[3];

    public Grade[] Grades => _grades;

    public IEnumerator GetPlayerDataCoroutine(Action<PlayerModel> onPlayerDataGot)
    {
        using (var request = UnityWebRequest.Get(playerDataGetURl))
        {
            yield return request.SendWebRequest();
            
            switch (request.result)
            {
                case UnityWebRequest.Result.ConnectionError:
                case UnityWebRequest.Result.DataProcessingError:
                    Debug.LogError($"Error getting player data: {request.error}.");
                    break;
                case UnityWebRequest.Result.ProtocolError:
                    Debug.LogError($"Error getting player data: {request.error}.");
                    break;
                case UnityWebRequest.Result.Success:
                    var blockArr = JsonConvert.DeserializeObject<Block[]>(request.downloadHandler.text);
                    var blocksGroupedByGrade = blockArr.GroupBy(block => block.Grade);
                    var jaggedBlockArr = blocksGroupedByGrade.Select(group => group.ToArray()).ToArray();
                    for (int i = 0; i < _grades.Length; i++)
                    {
                        var blocks = jaggedBlockArr[i].OrderBy(block => block.Domain)
                            .ThenBy(block => block.Cluster)
                            .ThenBy(block => block.Standardid)
                            .ToArray();
                        _grades[i] = new Grade(blocks);
                    }
                    onPlayerDataGot?.Invoke(this);
                    break;
            }
        }
    }
}
