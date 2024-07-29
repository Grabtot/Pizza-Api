using ErrorOr;
using Moq;
using PizzaApi.Application.Allergens.Commands.DeleteAllergen;
using PizzaApi.Application.Common.Interfaces.Repositories;
using PizzaApi.Domain.Ingredients.ValueObjects;

namespace PizzaApi.Tests.Application.Allergens
{
    public class DeleteAllergenCommandHandlerTests
    {
        private readonly Mock<IAllergenRepository> _mockAllergenRepository;
        private readonly DeleteAllergenCommandHandler _handler;

        public DeleteAllergenCommandHandlerTests()
        {
            _mockAllergenRepository = new Mock<IAllergenRepository>();
            _handler = new DeleteAllergenCommandHandler(_mockAllergenRepository.Object);
        }

        [Theory]
        [MemberData(nameof(ValidDeleteAllergenCommands))]
        public async Task HandleDeleteAllergenCommand_AllergenExists_ShouldDeleteAllergen(DeleteAllergenCommand command)
        {
            // Arrange
            Allergen existingAllergen = new(command.Name, "Existing description");
            _mockAllergenRepository.Setup(repo => repo.FindAsync(command.Name))
                .ReturnsAsync(existingAllergen);

            // Act
            ErrorOr<Success> result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result.IsError);
            Assert.Equal(Result.Success, result.Value);
            _mockAllergenRepository.Verify(repo => repo.Delete(existingAllergen), Times.Once);
        }

        [Theory]
        [MemberData(nameof(ValidDeleteAllergenCommands))]
        public async Task HandleDeleteAllergenCommand_AllergenDoesNotExist_ShouldReturnNotFound(DeleteAllergenCommand command)
        {
            // Arrange
            _mockAllergenRepository.Setup(repo => repo.FindAsync(command.Name))
                .ReturnsAsync((Allergen)null);

            // Act
            ErrorOr<Success> result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.IsError);
            Assert.Contains(result.Errors, e => e.Code == Error.NotFound().Code);
            _mockAllergenRepository.Verify(repo => repo.Delete(It.IsAny<Allergen>()), Times.Never);
        }

        public static IEnumerable<object[]> ValidDeleteAllergenCommands()
        {
            yield return new object[] { new DeleteAllergenCommand("Peanuts") };
            yield return new object[] { new DeleteAllergenCommand("Gluten") };
        }
    }
}
