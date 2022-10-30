using ToDoR.Common.Contracts;

namespace ToDoR.Api.Models
{
    public class TaslFilterModel
    {
        public List<TaskStatusEnum> Status { get; set; }
        public bool isDeleted { get; set; } = false;
    }
}
