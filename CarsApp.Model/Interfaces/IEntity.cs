namespace CarsApp.Model.Interfaces
{
    public interface IEntity<TIdentifier>
    {
        TIdentifier Id { get; }
    }
}
