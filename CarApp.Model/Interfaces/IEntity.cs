namespace CarApp.Model.Interfaces
{
    public interface IEntity<T>
    {
        T Id { get; set; }
    }
}
