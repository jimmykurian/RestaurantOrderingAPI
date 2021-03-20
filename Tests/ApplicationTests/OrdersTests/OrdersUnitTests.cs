using FluentValidation.TestHelper;
using System.Collections.Generic;
using Xunit;
using static Application.Orders.Queries.GetOrder;

namespace Tests.ApplicationTests.OrdersTests
{
    public class OrdersUnitTests
    {
        private QueryValidator _validator;

        public OrdersUnitTests()
        {
            _validator = new QueryValidator();
        }

        [Fact]
        public void Validate_NotEqualZero_Error()
        {
            // Arrange
            Query model = new Query()
            {
                MealType = "Breakfast",
                Orders = new List<int>() { 0, 1, 2 }
            };

            // Act
            var result = _validator.TestValidate(model);

            // Assert
            result.ShouldHaveValidationErrorFor(query => query.Orders);
        }

        [Fact]
        public void Validate_NotGreaterThanFour_Error()
        {
            // Arrange
            Query model = new Query()
            {
                MealType = "Breakfast",
                Orders = new List<int>() { 1, 2, 3, 5 }
            };

            // Act
            var result = _validator.TestValidate(model);

            // Assert
            result.ShouldHaveValidationErrorFor(query => query.Orders);
        }

        [Fact]
        public void Validate_MustHaveAMain_Error()
        {
            // Arrange
            Query model = new Query()
            {
                MealType = "Breakfast",
                Orders = new List<int>() { 2, 3 }
            };

            // Act
            var result = _validator.TestValidate(model);

            // Assert
            result.ShouldHaveValidationErrorFor(query => query.Orders);
        }

        [Fact]
        public void Validate_MustHaveASide_Error()
        {
            // Arrange
            Query model = new Query()
            {
                MealType = "Breakfast",
                Orders = new List<int>() { 1, 3 }
            };

            // Act
            var result = _validator.TestValidate(model);

            // Assert
            result.ShouldHaveValidationErrorFor(query => query.Orders);
        }

        [Fact]
        public void Validate_DessertWithDinnerOnlyAndRequired_Error()
        {
            // Arrange
            Query model = new Query()
            {
                MealType = "Breakfast",
                Orders = new List<int>() { 1, 2, 3, 4 }
            };

            // Act
            var result = _validator.TestValidate(model);

            // Assert
            result.ShouldHaveAnyValidationError();
        }

        [Fact]
        public void Validate_CannotOrderMultipleMains_Error()
        {
            // Arrange
            Query model = new Query()
            {
                MealType = "Breakfast",
                Orders = new List<int>() { 1, 1, 2, 3 }
            };

            // Act
            var result = _validator.TestValidate(model);

            // Assert
            result.ShouldHaveValidationErrorFor(query => query.Orders);
        }

        [Fact]
        public void Validate_CannotOrderMultipleSides_Error()
        {
            // Arrange
            Query model = new Query()
            {
                MealType = "Breakfast",
                Orders = new List<int>() { 1, 2, 2, 3 }
            };

            // Act
            var result = _validator.TestValidate(model);

            // Assert
            result.ShouldHaveAnyValidationError();
        }

        [Fact]
        public void Validate_CannotOrderMultipleDrinks_Error()
        {
            // Arrange
            Query model = new Query()
            {
                MealType = "Lunch",
                Orders = new List<int>() { 1, 2, 3, 3 }
            };

            // Act
            var result = _validator.TestValidate(model);

            // Assert
            result.ShouldHaveAnyValidationError();
        }

        [Fact]
        public void Validate_CannotOrderMultipleDesserts_Error()
        {
            // Arrange
            Query model = new Query()
            {
                MealType = "Dinner",
                Orders = new List<int>() { 1, 2, 3, 4, 4 }
            };

            // Act
            var result = _validator.TestValidate(model);

            // Assert
            result.ShouldHaveValidationErrorFor(query => query.Orders);
        }


        [Fact]
        public void Validate_NotNull_Error()
        {
            // Arrange
            Query model = new Query()
            {
                MealType = "Breakfast",
                Orders = new List<int>() { }
            };

            // Act
            var result = _validator.TestValidate(model);

            // Assert
            result.ShouldHaveValidationErrorFor(query => query.Orders);
        }

        [Fact]
        public void Validate_NotEmpty_Error()
        {
            // Arrange
            Query model = new Query()
            {
                MealType = "",
                Orders = new List<int>() { 1, 2, 3 }
            };

            // Act
            var result = _validator.TestValidate(model);

            // Assert
            result.ShouldHaveValidationErrorFor(query => query.MealType);
        }

        [Fact]
        public void Validate_IsEnumName_Error()
        {
            // Arrange
            Query model = new Query()
            {
                MealType = "TEST",
                Orders = new List<int>() { 1, 2, 3 }
            };

            // Act
            var result = _validator.TestValidate(model);

            // Assert
            result.ShouldHaveValidationErrorFor(query => query.MealType);
        }

        [Fact]
        public void Validate_Breakfast_Order()
        {
            // Arrange
            Query model = new Query()
            {
                MealType = "Breakfast",
                Orders = new List<int>() { 1, 2, 3 }
            };

            // Act
            var result = _validator.TestValidate(model);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }


        [Fact]
        public void Validate_Breakfast_MultipleDrinks_Order()
        {
            // Arrange
            Query model = new Query()
            {
                MealType = "Breakfast",
                Orders = new List<int>() { 1, 2, 3, 3}
            };

            // Act
            var result = _validator.TestValidate(model);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }

        [Fact]
        public void Validate_Breakfast_NoCoffee_Order()
        {
            // Arrange
            Query model = new Query()
            {
                MealType = "Breakfast",
                Orders = new List<int>() { 1, 2 }
            };

            // Act
            var result = _validator.TestValidate(model);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }

        [Fact]
        public void Validate_Lunch_Order()
        {
            // Arrange
            Query model = new Query()
            {
                MealType = "Lunch",
                Orders = new List<int>() { 1, 2, 3 }
            };

            // Act
            var result = _validator.TestValidate(model);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }

        [Fact]
        public void Validate_Lunch_MultipleSides_Order()
        {
            // Arrange
            Query model = new Query()
            {
                MealType = "Lunch",
                Orders = new List<int>() { 1, 2, 2, 3 }
            };

            // Act
            var result = _validator.TestValidate(model);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }

        [Fact]
        public void Validate_Lunch_NoDrinks_Order()
        {
            // Arrange
            Query model = new Query()
            {
                MealType = "Lunch",
                Orders = new List<int>() { 1, 2, 2 }
            };

            // Act
            var result = _validator.TestValidate(model);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }

        [Fact]
        public void Validate_Dinner_Order()
        {
            // Arrange
            Query model = new Query()
            {
                MealType = "Dinner",
                Orders = new List<int>() { 1, 2, 3, 4 }
            };

            // Act
            var result = _validator.TestValidate(model);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }

        [Fact]
        public void Validate_Dinner_NoWine_Order()
        {
            // Arrange
            Query model = new Query()
            {
                MealType = "Dinner",
                Orders = new List<int>() { 1, 2, 4 }
            };

            // Act
            var result = _validator.TestValidate(model);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}
