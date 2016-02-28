namespace SignalRConsole.Model
{
    public class ResultStatus<T>
    {
        public bool IsError { get; set; }
        public string ErrorMsg { get; set; }

        public T ResultModel { get; set; }
    }
}