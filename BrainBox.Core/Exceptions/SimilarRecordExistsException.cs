namespace BrainBox.Core.Exceptions
{
    public class SimilarRecordExistsException : Exception
    {
        public SimilarRecordExistsException(string message) : base(message)
        {
        }
    }
}
