using Code.Model;
using TMPro;
using UnityEngine;

namespace Code.View
{
    public class BlockDescriptionView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI gradeLabel;
        [SerializeField] private string gradeLabelFormat = "{0}: {1}";
        [SerializeField] private TextMeshProUGUI clusterLevelLabel;
        [SerializeField] private TextMeshProUGUI standardLabel;
        [SerializeField] private string standardLabelFormat = "{0}: {1}";
        
        public void UpdateDescription(Topic topic)
        {
            gradeLabel.text = string.Format(gradeLabelFormat, topic.Grade, topic.Domain);
            clusterLevelLabel.text = topic.Cluster;
            standardLabel.text = string.Format(standardLabelFormat, topic.Standardid, topic.Standarddescription);
        }
    }
}