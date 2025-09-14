namespace MajlesMefa.Back.Dtos.Common
{
    public sealed class BankDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public static BankDto Create(Guid id, string name)
        {
            return new BankDto { Id = id, Name = name };
        }
    }
}