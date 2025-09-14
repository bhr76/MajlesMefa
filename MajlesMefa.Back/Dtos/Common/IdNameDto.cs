namespace MajlesMefa.Back.Dtos.Common
{
    public sealed class IdNameDto
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public static IdNameDto Create(int id, string name)
        {
            return new IdNameDto { Id = id, Name = name.Trim() };
        }
    }
}