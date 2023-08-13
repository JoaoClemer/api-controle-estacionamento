namespace ControleDeEstacionamento.ModelView
{
    public class ResultModel<T>
    {
        public ResultModel(T data, List<string> errors)
        {
            Data = data;
            Errors = errors;
        }
        public T Data { get; private set; }
        public List<string> Errors { get; private set; } = new();
        public ResultModel(T data)
        {
            Data = data;
        }
        public ResultModel(string error)
        {
            Errors.Add(error);
        }
        public ResultModel(List<string> errors)
        {
            Errors = errors;
        }
    }
}
