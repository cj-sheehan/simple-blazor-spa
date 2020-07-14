using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NameApi.Controllers;
using NameApi.DataAccess.Models;
using NameApi.DataAccess.Repositories;
using NameApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace NameApi.Tests.Controllers
{
    public class NameControllerTests
    {
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<INameRepository> _mockNameRepository;
        private readonly NameController _nameController;

        public NameControllerTests()
        {
            _mockMapper = new Mock<IMapper>();
            _mockNameRepository = new Mock<INameRepository>();
            _nameController = new NameController(_mockNameRepository.Object, _mockMapper.Object);

            SetupMapper();
        }

        [Fact]
        public async Task GetAll_ShouldReturnAllNames()
        {
            // Given
            var repoResponse = new List<NameModel> {
                new NameModel { Id = 1, Name = "Joe Bloggs", DateCreated = DateTime.Now },
                new NameModel { Id = 2, Name = "Suzie Suggs", DateCreated = DateTime.Now }
            };
            _mockNameRepository.Setup(repo => repo.GetAll())
                .ReturnsAsync(repoResponse);

            // When
            var actionResult = await _nameController.GetAllAsync();

            // Then
            var okResult = actionResult.Result as OkObjectResult;
            Assert.NotNull(okResult);

            var expectedObject = okResult.Value as IEnumerable<NameResponseModel>;
            Assert.NotNull(expectedObject);
            Assert.Equal(repoResponse.Count, expectedObject.Count());
        }

        [Theory]
        [MemberData(nameof(GetNullOrEmptyNameModelLists))]
        public async Task GetAll_ShouldReturnNoContent(List<NameModel> repoResponse)
        {
            // Given
            _mockNameRepository.Setup(repo => repo.GetAll())
                .ReturnsAsync(repoResponse);

            // When
            var actionResult = await _nameController.GetAllAsync();

            // Then
            var noContentResult = actionResult.Result as NoContentResult;
            Assert.NotNull(noContentResult);
        }

        [Fact]
        public async Task GetById_ReturnsName()
        {
            // Given
            var repoResponse = new NameModel { Id = 1, Name = "Joe Bloggs", DateCreated = DateTime.Now };
            _mockNameRepository.Setup(repo => repo.GetNameByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(repoResponse);

            // When
            var actionResult = await _nameController.GetByIdAsync(1);

            // Then
            var okResult = actionResult.Result as OkObjectResult;
            Assert.NotNull(okResult);

            var expectedObject = okResult.Value as NameResponseModel;
            Assert.NotNull(expectedObject);
            Assert.Equal(repoResponse.Id, expectedObject.Id);
            Assert.Equal(repoResponse.Name, expectedObject.Name);
            Assert.Equal(repoResponse.DateCreated, expectedObject.DateCreated);
        }

        [Fact]
        public async Task GetById_ReturnsNotFound()
        {
            // Given
            NameModel repoResponse = null;
            _mockNameRepository.Setup(repo => repo.GetNameByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(repoResponse);

            // When
            var actionResult = await _nameController.GetByIdAsync(1);

            // Then
            var notFoundResult = actionResult.Result as NotFoundResult;
            Assert.NotNull(notFoundResult);
        }

        [Fact]
        public async Task Post_ReturnsCreatedAtRouteResult()
        {
            // Given
            var request = new NameCreateRequestModel { Name = "Joe Bloggs" };
            var id = 1;
            var repoResponse = new NameModel { Id = id, Name = request.Name, DateCreated = DateTime.Now };
            _mockNameRepository.Setup(repo => repo.AddNameAsync(It.IsAny<string>()))
                .ReturnsAsync(repoResponse);

            // When
            var actionResult = await _nameController.PostAsync(request);

            // Then
            var createdAtRouteResult = actionResult.Result as CreatedAtRouteResult;
            Assert.NotNull(createdAtRouteResult);

            Assert.Equal(nameof(_nameController.GetByIdAsync), createdAtRouteResult.RouteName);
            createdAtRouteResult.RouteValues.TryGetValue("Id", out var IdRouteValue);
            Assert.Equal(id, (int)IdRouteValue);
        }

        public static IEnumerable<object[]> GetNullOrEmptyNameModelLists()
        {
            yield return new object[] { new List<NameModel>() };
            yield return new object[] { null };
        }

        private void SetupMapper()
        {
            _mockMapper.Setup(mapper => mapper.Map<IEnumerable<NameResponseModel>>(It.IsAny<IEnumerable<NameModel>>()))
                .Returns((IEnumerable<NameModel> source) =>
                {
                    return source?.Select(nameModel => new NameResponseModel
                    {
                        Id = nameModel.Id.Value,
                        Name = nameModel.Name,
                        DateCreated = nameModel.DateCreated.Value
                    }).ToList();
                });

            _mockMapper.Setup(mapper => mapper.Map<NameResponseModel>(It.IsAny<NameModel>()))
                .Returns((NameModel source) =>
                {
                    return new NameResponseModel
                    {
                        Id = source.Id.Value,
                        Name = source.Name,
                        DateCreated = source.DateCreated.Value
                    };
                });
        }
    }
}
