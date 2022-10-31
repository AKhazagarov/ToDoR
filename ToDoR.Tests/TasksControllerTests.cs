using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MockQueryable.Moq;
using Moq;
using ToDoR.Api.Controllers;
using ToDoR.Api.Models;
using ToDoR.Common.Contracts;
using ToDoR.DataAccess.Interfaces;

namespace ToDoR.Tests
{
    [TestClass]
    [TestCategory("UnitTests")]
    public class TasksControllerTests
    {
        private Mock<IDoTaskProvider> _providerMock;
        private TasksController _controller;

        [TestInitialize]
        public void Initialize()
        {
            _providerMock = new Mock<IDoTaskProvider>();
            _controller = new TasksController(_providerMock.Object);
        }

        [TestCleanup]
        public void Cleanup()
        {
            _providerMock.VerifyNoOtherCalls();
        }

        [TestMethod]
        public async Task TasksController_GetAll_ValidRequest_ListTaskModel()
        {
            _providerMock
                .Setup(r => r.Requests)
                .Returns(_doTasks.BuildMock());

            var result = await _controller.GetAll();

            result.Result.Should().BeAssignableTo<OkObjectResult>();

            var actual = (((OkObjectResult)result.Result)?.Value as IEnumerable<TaskModel>).ToList();

            _tasksModel.Should().BeEquivalentTo(actual);
            _providerMock.Verify(q => q.Requests, Times.Once);
        }

        [TestMethod]
        public async Task TasksController_GetByFilter_ValidRequest_ListTaskModel()
        {
            var status = new List<TaskStatusEnum> { TaskStatusEnum.New };

            _providerMock
                .Setup(r => r.Requests)
                .Returns(_doTasks.Where(t => status.Contains(t.Status)).BuildMock());

            var result = await _controller.GetByFilter(new TaskFilterModel() { Status = status });

            result.Result.Should().BeAssignableTo<OkObjectResult>();

            var actual = (((OkObjectResult)result.Result)?.Value as IEnumerable<TaskModel>).ToList();

            _tasksModel.Where(t => status.Contains(t.Status)).Should().BeEquivalentTo(actual);
            _providerMock.Verify(q => q.Requests, Times.Once);
        }

        [TestMethod]
        public async Task TasksController_Add_ValidRequest_Error()
        {
            var intent = new DoTaskAdd()
            {
                Name = "too short",
                Note = "kok",
                DueDate = null,
            };

            var result = await _controller.Add(intent);

            result.Should().BeOfType(typeof(BadRequestObjectResult));
        }

        [TestMethod]
        public async Task TasksController_Add_ValidRequest_Success()
        {
            var intent = new DoTaskAdd()
            {
                Name = "not so very short",
                Note = "kok",
                DueDate = null,
            };

            var result = await _controller.Add(intent);

            result.Should().BeAssignableTo<OkResult>();
            _providerMock.Verify(q => q.Add(It.IsAny<DoTaskAdd>()), Times.Once);
        }

        [TestMethod]
        public async Task TasksController_Edit_ValidRequest_Success()
        {
            var doTask = new List<DoTask> { _doTasks.First() };

            var intent = new TaskEditModel()
            {
                Id = doTask.First().Id,
                Name = "not so very short",
                Note = "kok",
                DueDate = null,
            };

            _providerMock
                .Setup(r => r.Requests)
                .Returns(doTask.BuildMock());

            _providerMock
                .Setup(q => q.Update(It.IsAny<DoTaskEdit>()))
                .Callback<object>(n =>
                {
                    var intent = n as DoTaskEdit;
                    intent.Should().NotBeNull();
                    intent.Id.Should().Be(intent.Id);
                    intent.Name.Should().Be(intent.Name);
                    intent.Note.Should().Be(intent.Note);
                    intent.DueDate.Should().Be(intent.DueDate);
                });

            var result = await _controller.Edit(intent);

            result.Should().BeAssignableTo<OkResult>();
            _providerMock.Verify(q => q.Requests, Times.Once);
            _providerMock.Verify(q => q.Update(It.IsAny<DoTaskEdit>()), Times.Once);
        }

        [TestMethod]
        public async Task TasksController_MarkComplete_ValidRequest_Success()
        {
            var doTask = new List<DoTask> { _doTasks.First() };

            _providerMock
                .Setup(r => r.Requests)
                .Returns(doTask.BuildMock());

            _providerMock
                .Setup(q => q.ChangeStatus(doTask.First().Id, TaskStatusEnum.Done));

            var result = await _controller.MarkComplete(doTask.First().Id);

            result.Should().BeAssignableTo<OkResult>();

            _providerMock.Verify(q => q.Requests, Times.Once);
            _providerMock.Verify(q => q.ChangeStatus(doTask.First().Id, TaskStatusEnum.Done), Times.Once);
        }

        [TestMethod]
        public async Task TasksController_MarkComplete_WrongId_Error()
        {
            _providerMock
                .Setup(r => r.Requests)
                .Returns(_doTasks.BuildMock());

            var result = await _controller.MarkComplete(Guid.NewGuid());

            result.Should().BeAssignableTo<BadRequestResult>();
            _providerMock.Verify(q => q.Requests, Times.Once);
        }

        [TestMethod]
        public async Task TasksController_MarkNotComplete_ValidRequest_Success()
        {
            var doTask = new List<DoTask> { _doTasks.First() };

            _providerMock
                .Setup(r => r.Requests)
                .Returns(doTask.BuildMock());

            _providerMock
                .Setup(q => q.ChangeStatus(doTask.First().Id, TaskStatusEnum.New));

            var result = await _controller.MarkNotComplete(doTask.First().Id);

            result.Should().BeAssignableTo<OkResult>();

            _providerMock.Verify(q => q.Requests, Times.Once);
            _providerMock.Verify(q => q.ChangeStatus(doTask.First().Id, TaskStatusEnum.New), Times.Once);
        }

        [TestMethod]
        public async Task TasksController_MarkNotComplete_WrongId_Error()
        {
            _providerMock
                .Setup(r => r.Requests)
                .Returns(_doTasks.BuildMock());

            var result = await _controller.MarkNotComplete(Guid.NewGuid());

            result.Should().BeAssignableTo<BadRequestResult>();

            _providerMock.Verify(q => q.Requests, Times.Once);
        }

        [TestMethod]
        public async Task TasksController_Delete_ValidRequest_Success()
        {
            var id = _doTasks.First().Id;
            var result = await _controller.Delete(id);

            result.Should().BeAssignableTo<OkResult>();

            _providerMock.Verify(q => q.Delete(id), Times.Once);
        }

        [TestMethod]
        public async Task TasksController_Delete_WrongId_Error()
        {
            var result = await _controller.Delete(Guid.Empty);
            result.Should().BeAssignableTo<BadRequestResult>();
        }

        static List<TaskModel>  _tasksModel = new List<TaskModel>()
        {
            new TaskModel()
            {
                Id = Guid.Parse("798f7a9a-152a-472c-af23-5257c9e46e31"),
                DueDate = null,
                Name = "ToFo",
                Note = "None",
                Status = TaskStatusEnum.New,
                CreatedAt = new DateTime(2022, 11, 11),
            },
            new TaskModel()
            {
                Id = Guid.Parse("798f7a9a-152a-472c-af23-5257c9e46e32"),
                Name = "ToFo",
                Note = "None",
                Status = TaskStatusEnum.Done,
                DueDate = new DateTime(2022, 1, 3),
                CreatedAt = new DateTime(2022, 1, 1),
            },
        };

        static List<DoTask> _doTasks = new List<DoTask> ()
        {
            new DoTask()
            {
                Id = Guid.Parse("798f7a9a-152a-472c-af23-5257c9e46e31"),
                DueDate = null,
                Name = "ToFo",
                Note = "None",
                Status = TaskStatusEnum.New,
                CreatedAt = new DateTime(2022, 11, 11),
            },
            new DoTask()
            {
                Id = Guid.Parse("798f7a9a-152a-472c-af23-5257c9e46e32"),
                Name = "ToFo",
                Note = "None",
                Status = TaskStatusEnum.Done,
                DueDate = new DateTime(2022, 1, 3),
                CreatedAt = new DateTime(2022, 1, 1),
            },
        };
    }
}