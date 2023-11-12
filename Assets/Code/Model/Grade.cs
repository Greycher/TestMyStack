namespace Code.Model
{
    public class Grade
    {
        private Block[] _blocks;
        
        public Grade(Block[] blocks)
        {
            _blocks = blocks;
        }

        public Block[] Blocks => _blocks;
        public string DisplayName => _blocks[0].Grade;
    }
}