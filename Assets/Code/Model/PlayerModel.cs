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
    
    private Grade[] _grades;

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
                    _grades = new Grade[jaggedBlockArr.Length];
                    for (int i = 0; i < jaggedBlockArr.Length; i++)
                    {
                        _grades[i] = new Grade(jaggedBlockArr[i]);
                    }
                    onPlayerDataGot?.Invoke(this);
                    break;
            }
        }
    }
}
