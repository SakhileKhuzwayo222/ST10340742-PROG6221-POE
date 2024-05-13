using System;
using System.Collections.Generic;
using System.Linq;

namespace RecipeApp
{
    public class Ingredient
    {
        public string Name { get; set; }
        public double Quantity { get; set; }
        public string Unit { get; set; }
        public double Calories { get; set; }
        public string FoodGroup { get; set; }

        public Ingredient(string name, double quantity, string unit, double calories, string foodGroup)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentException("Ingredient name cannot be null or empty.");
            if (string.IsNullOrEmpty(unit))
                throw new ArgumentException("Ingredient unit cannot be null or empty.");
            if (string.IsNullOrEmpty(foodGroup))
                throw new ArgumentException("Ingredient food group cannot be null or empty.");

            Name = name;
            Quantity = quantity;
            Unit = unit;
            Calories = calories;
            FoodGroup = foodGroup;
        }
    }

    public class RecipeStep
    {
        public string Description { get; }

        public RecipeStep(string description)
        {
            if (string.IsNullOrEmpty(description))
                throw new ArgumentException("Description cannot be null or empty.");

            Description = description;
        }
    }

    public class Recipe
    {
        private readonly List<Ingredient> ingredients;
        private readonly List<RecipeStep> steps;
        public string Name { get; set; }

        public delegate void CaloriesExceedHandler(Recipe recipe, double totalCalories);
        public event CaloriesExceedHandler CaloriesExceeded;

        public Recipe(string name)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentException("Recipe name cannot be null or empty.");

            Name = name;
            ingredients = new List<Ingredient>();
            steps = new List<RecipeStep>();
            CaloriesExceeded += delegate { };
        }

        public static List<Recipe> InputRecipes()
        {
            List<Recipe> recipes = new List<Recipe>();

            while (true)
            {
                Console.WriteLine("Enter recipe details (or 'done' to finish):");
                Console.Write("Recipe Name: ");
                string recipeName = Console.ReadLine()?.Trim();
                if (string.IsNullOrWhiteSpace(recipeName))
                {
                    Console.WriteLine("Recipe name cannot be empty.");
                    continue;
                }

                Recipe recipe = new Recipe(recipeName);
                recipe.InputRecipeDetails();
                recipes.Add(recipe);

                Console.Write("Do you want to add another recipe? (yes/no): ");
                string addAnother = Console.ReadLine()?.Trim();
                if (addAnother?.ToLower() != "yes")
                    break;
            }

            return recipes.OrderBy(r => r.Name).ToList();
        }

        public void InputRecipeDetails()
        {
        // Prompt user for the number of ingredients
        Console.Write("Enter the number of ingredients: ");
        int numIngredients;
        while (!int.TryParse(Console.ReadLine(), out numIngredients) || numIngredients <= 0)
        {
            Console.WriteLine("Invalid input. Please enter a valid number greater than 0.");
            Console.Write("Enter the number of ingredients: ");
        }

        // Prompt user for ingredient details
        for (int i = 0; i < numIngredients; i++)
        {
            Console.WriteLine($"Enter details for ingredient {i + 1}:");

            Console.Write("Ingredient Name: ");
            string ingredientName = Console.ReadLine()?.Trim();

            // Check if the user wants to finish entering ingredients
            if (string.Equals(ingredientName, "done", StringComparison.OrdinalIgnoreCase))
                break;

            Console.Write("Ingredient Quantity: ");
            double quantity;
            while (!double.TryParse(Console.ReadLine(), out quantity))
            {
                Console.WriteLine("Invalid quantity. Please enter a valid number.");
                Console.Write("Ingredient Quantity: ");
            }

            Console.Write("Ingredient Unit: ");
            string unit = Console.ReadLine()?.Trim();

            Console.Write("Ingredient Food Group: ");
            string foodGroup = Console.ReadLine()?.Trim();

            Console.Write("Ingredient Calories: ");
            double calories;
            while (!double.TryParse(Console.ReadLine(), out calories))
            {
                Console.WriteLine("Invalid calories. Please enter a valid number.");
                Console.Write("Ingredient Calories: ");
            }

            Ingredient ingredient = new Ingredient(ingredientName, quantity, unit, calories, foodGroup);
            ingredients.Add(ingredient);

            Console.WriteLine("Ingredient added successfully.");
        }

        // Prompt user for the number of steps
        Console.Write("Enter the number of steps: ");
        int numSteps;
        while (!int.TryParse(Console.ReadLine(), out numSteps) || numSteps <= 0)
        {
            Console.WriteLine("Invalid input. Please enter a valid number greater than 0.");
            Console.Write("Enter the number of steps: ");
        }

        // Prompt user for step details
        for (int i = 0; i < numSteps; i++)
        {
            Console.WriteLine($"Enter details for step {i + 1}:");

            Console.Write("Step Description: ");
            string description = Console.ReadLine()?.Trim();

            // Check if the user wants to finish entering steps
            if (string.Equals(description, "done", StringComparison.OrdinalIgnoreCase))
                break;

            RecipeStep step = new RecipeStep(description);
            steps.Add(step);

            Console.WriteLine("Step added successfully.");
        }
    }


        public double CalculateTotalCalories()
        {
            double totalCalories = ingredients.Sum(i => i.Calories * i.Quantity);
            if (totalCalories > 300)
            {
                CaloriesExceeded?.Invoke(this, totalCalories);
            }
            return totalCalories;
        }

        
        public void DisplayRecipe()
        {
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"Recipe: {Name}");
            Console.ResetColor();

            if (ingredients.Any())
            {
                Console.WriteLine("Ingredients:");
                foreach (var ingredient in ingredients)
                {
                    Console.WriteLine($"{ingredient.Name} - {ingredient.Quantity} {ingredient.Unit} ({ingredient.FoodGroup})");
                }
            }
            else
            {
                Console.WriteLine("No ingredients added.");
            }

            if (steps.Any())
            {
                Console.WriteLine("Steps:");
                for (int i = 0; i < steps.Count; i++)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.Write($"{i + 1}. ");
                    Console.ResetColor();
                    Console.WriteLine(steps[i].Description);
                }
            }
            else
            {
                Console.WriteLine("No steps added.");
            }

            Console.WriteLine();
        }

        public void ClearDataWithConfirmation()
        {
            Console.WriteLine("Are you sure you want to clear all data? (yes/no)");
            string userInput = Console.ReadLine()?.Trim();

            if (string.Equals(userInput, "yes", StringComparison.OrdinalIgnoreCase))
            {
                ingredients.Clear();
                steps.Clear();
                Console.WriteLine("Data cleared.");
            }
            else
            {
                Console.WriteLine("Operation cancelled.");
            }
        }
    }

    public class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                List<Recipe> recipes = Recipe.InputRecipes();

                foreach (var recipe in recipes)
                {
                    recipe.CaloriesExceeded += Recipe_CaloriesExceeded;
                    recipe.DisplayRecipe();
                    double totalCalories = recipe.CalculateTotalCalories();
                    Console.WriteLine($"Total Calories: {totalCalories}");
                    recipe.ClearDataWithConfirmation();
                    recipe.CaloriesExceeded -= Recipe_CaloriesExceeded;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }

        public static void Recipe_CaloriesExceeded(Recipe recipe, double totalCalories)
        {
            Console.WriteLine($"Warning: Total calories of recipe '{recipe.Name}' exceed 300 ({totalCalories}).");
        }
    }
}

