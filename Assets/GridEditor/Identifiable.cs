namespace GridEditor
{
    public abstract class Identifiable
    {
        private static int _idCounter;
        public readonly int Id;
        public Identifiable()
        {
            Id = _idCounter++;
        }
    }
}