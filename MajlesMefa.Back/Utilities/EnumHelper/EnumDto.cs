namespace MajlesMefa.Back.Utilities.EnumHelper
{
    public class EnumDto<T> where T : struct
    {
        public T Value { get; set; }
        public string Name { get; set; }
    }
}
