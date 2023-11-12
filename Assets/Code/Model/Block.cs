using UnityEngine;

namespace Code.Model
{
    public class Block
    {
        private int _id;
        private string _subject;
        private string _grade;
        private BlockType _blockType;
        private string _domainid;
        private string _domain;
        private string _cluster;
        private string _standardid;
        private string _standarddescription;
        
        public int ID => _id;
        public string Subject => _subject;
        public string Grade => _grade;
        public BlockType BlockType => _blockType;
        public string Domainid => _domainid;
        public string Domain => _domain;
        public string Cluster => _cluster;
        public string Standardid => _standardid;
        public string Standarddescription => _standarddescription;

        public Block(int id, string subject, string grade, int mastery, string domainid, string domain, string cluster, string standardid, string standarddescription)
        {
            this._id = id;
            this._subject = subject;
            this._grade = grade;
            _blockType = (BlockType)mastery;
            this._domainid = domainid;
            this._domain = domain;
            this._cluster = cluster;
            this._standardid = standardid;
            this._standarddescription = standarddescription;
        }
    }
}