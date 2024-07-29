using ErrorOr;
using Moq;
using PizzaApi.Application.Allergens.Commands.CreateAllergen;
using PizzaApi.Application.Common.Interfaces.Repositories;
using PizzaApi.Domain.Ingredients.ValueObjects;

namespace PizzaApi.Tests.Application.Allergens
{
    public class CreateAllergenCommandHandlerTests
    {
        private readonly Mock<IAllergenRepository> _mockAllergenRepository;
        private readonly CreateAllergenCommandHandler _handler;

        public CreateAllergenCommandHandlerTests()
        {
            _mockAllergenRepository = new Mock<IAllergenRepository>();
            _handler = new CreateAllergenCommandHandler(_mockAllergenRepository.Object);
        }

        [Theory]
        [MemberData(nameof(ValidCreateAllergenCommands))]
        public async Task HandleCreateAllergenCommand_AllergenDoesNotExist_ShouldReturnAllergen(CreateAllergenCommand command)
        {
            // Arrange
            _mockAllergenRepository.Setup(repo => repo.FindAsync(command.Name))
                .ReturnsAsync((Allergen)null);

            // Act
            ErrorOr<Allergen> result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result.IsError);
            Assert.NotNull(result.Value);
            Assert.Equal(command.Name, result.Value.Name);
            Assert.Equal(command.Description, result.Value.Description);
            _mockAllergenRepository.Verify(repo => repo.AddAsync(It.IsAny<Allergen>()), Times.Once);
        }

        [Theory]
        [MemberData(nameof(InvalidCreateAllergenCommands))]
        public async Task HandleCreateAllergenCommand_AllergenAlreadyExists_ShouldReturnConflictError(CreateAllergenCommand command)
        {
            // Arrange
            Allergen existingAllergen = new(command.Name, "Existing description");
            _mockAllergenRepository.Setup(repo => repo.FindAsync(command.Name))
                .ReturnsAsync(existingAllergen);

            // Act
            ErrorOr<Allergen> result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.IsError);
            Assert.Contains(result.Errors, e => e.Code == Error.Conflict().Code);
            _mockAllergenRepository.Verify(repo => repo.AddAsync(It.IsAny<Allergen>()), Times.Never);
        }

        public static IEnumerable<object[]> ValidCreateAllergenCommands()
        {
            yield return new object[] { new CreateAllergenCommand("Peanuts", "Peanut allergen") };
            yield return new object[] { new CreateAllergenCommand("Gluten", "Gluten allergen") };
        }

        public static IEnumerable<object[]> InvalidCreateAllergenCommands()
        {
            yield return new object[] { new CreateAllergenCommand("Peanuts", "Duplicate allergen") };
            yield return new object[] { new CreateAllergenCommand("Gluten", "Duplicate allergen") };
        }
    }
}
