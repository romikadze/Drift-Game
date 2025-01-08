namespace Source.Scripts.Data
{
    public interface IDataSaver
    {
        public void Save<T>(T data, string path);
        
        public T Load<T>(string path);
    }
}