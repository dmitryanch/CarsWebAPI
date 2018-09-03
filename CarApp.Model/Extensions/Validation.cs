namespace CarApp.Model.Extensions
{
    public static class Validation
    {
        public static void MergeWith(this Car car, Car newCar)
        {
            if (newCar == null) return;
            if (!string.IsNullOrEmpty(newCar.Name))
            {
                car.Name = newCar.Name;
            }
            if (!string.IsNullOrEmpty(newCar.Description))
            {
                car.Description = newCar.Description;
            }
        }
    }
}
