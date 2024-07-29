using ErrorOr;
using Moq;
using PizzaApi.Application.Allergens.Commands.UpdateAllergen;
using PizzaApi.Application.Common.Interfaces.Repositories;
using PizzaApi.Domain.Ingredients.ValueObjects;

namespace PizzaApi.Tests.Application.Allergens
{
    public class UpdateAllergenCommandHandlerTests
    {
        private readonly Mock<IAllergenRepository> _mockAllergenRepository;
        private readonly UpdateAllergenCommandHandler _handler;

        public UpdateAllergenCommandHandlerTests()
        {
            _mockAllergenRepository = new Mock<IAllergenRepository>();
            _handler = new UpdateAllergenCommandHandler(_mockAllergenRepository.Object);
        }

        [Theory]
        [MemberData(nameof(ValidUpdateAllergenCommands))]
        public async Task HandleUpdateAllergenCommand_AllergenExists_ShouldUpdateAllergen(UpdateAllergenCommand command)
        {
            // Arrange
            Allergen existingAllergen = new(command.CurrentName, "Existing description");
            _mockAllergenRepository.Setup(repo => repo.FindAsync(command.CurrentName))
                .ReturnsAsync(existingAllergen);
            _mockAllergenRepository.Setup(repo => repo.FindAsync(command.NewName))
                .ReturnsAsync((Allergen)null);

            // Act
            ErrorOr<Allergen> result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result.IsError);
            Assert.NotNull(result.Value);
            Assert.Equal(command.NewName ?? command.CurrentName, result.Value.Name);
            Assert.Equal(command.Description, result.Value.Description);
        }

        [Theory]
        [MemberData(nameof(ValidUpdateAllergenCommands))]
        public async Task HandleUpdateAllergenCommand_AllergenDoesNotExist_ShouldReturnNotFound(UpdateAllergenCommand command)
        {
            // Arrange
            _mockAllergenRepository.Setup(repo => repo.FindAsync(command.CurrentName))
                .ReturnsAsync((Allergen)null);

            // Act
            ErrorOr<Allergen> result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.IsError);
            Assert.Contains(result.Errors, e => e.Code == Error.NotFound().Code);
        }

        [Theory]
        [MemberData(nameof(InvalidUpdateAllergenCommands))]
        public async Task HandleUpdateAllergenCommand_NewNameAlreadyExists_ShouldReturnConflict(UpdateAllergenCommand command)
        {
            // Arrange
            Allergen existingAllergen = new(command.CurrentName, "Existing description");
            Allergen conflictingAllergen = new(command.NewName, "Conflicting description");
            _mockAllergenRepository.Setup(repo => repo.FindAsync(command.CurrentName))
                .ReturnsAsync(existingAllergen);
            _mockAllergenRepository.Setup(repo => repo.FindAsync(command.NewName))
                .ReturnsAsync(conflictingAllergen);

            // Act
            ErrorOr<Allergen> result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.IsError);
            Assert.Contains(result.Errors, e => e.Code == Error.Conflict().Code);
        }

        public static IEnumerable<object[]> ValidUpdateAllergenCommands()
        {
            yield return new object[] { new UpdateAllergenCommand("Peanuts", "Nuts", "Updated description") };
            yield return new object[] { new UpdateAllergenCommand("Peanuts", null, null) };
            yield return new object[] { new UpdateAllergenCommand("Peanuts", "Nuts", null) };
            yield return new object[] { new UpdateAllergenCommand("Gluten", null, "Updated gluten description") };
        }
        public static IEnumerable<object[]> InvalidUpdateAllergenCommands()
        {
            yield return new object[] { new UpdateAllergenCommand("Peanuts", "Nuts", "Updated description") };
            yield return new object[] { new UpdateAllergenCommand("Peanuts", "Nuts", null) };
        }
    }
}
