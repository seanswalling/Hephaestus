namespace Hephaestus.Core.Building
{
    public class Copyright
    {
        private readonly string _copyright;

        public Copyright(string copyright)
        {
            _copyright = copyright;
        }

        public override string ToString()
        {
            return _copyright;
        }
    }
}