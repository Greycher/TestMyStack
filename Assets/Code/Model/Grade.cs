namespace Code.Model
{
    public class Grade
    {
        private Topic[] _topics;
        
        public Grade(Topic[] topics)
        {
            _topics = topics;
        }

        public Topic[] Topics => _topics;
        public string DisplayName => _topics[0].Grade;
    }
}