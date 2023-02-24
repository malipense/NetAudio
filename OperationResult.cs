namespace NetAudio
{
    internal class OperationResult
    {
        public OperationResult(bool succeeded, string message)
        {
            Succeeded = succeeded;
            Message = message;
        }
        public bool Succeeded { get; set; }
        public string Message { get; set; }
    }
}
