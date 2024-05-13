using System;
using System.Collections.Generic;
using System.Linq;

// Class to represent an ingredient in the recipe
public class Ingredient
{
    public string Name { get; set; }
    public double Quantity { get; set; }
    public string Unit { get; set; }
    public double Calories { get; set; }
    public string FoodGroup { get; set; }

    // Constructor to initialize ingredient properties
    public Ingredient(string name, double quantity, string unit, double calories, string foodGroup)
    {
        Name = name;
        Quantity = quantity;
        Unit = unit;
        Calories = calories;
        FoodGroup = foodGroup;
    }
}

// Class to represent a step in the recipe
public class RecipeStep
{
    public string Description { get; set; }

    // Constructor to initialize step description
    public RecipeStep(string description)
    {
        Description = description;
    }
}

// Class to represent a recipe
public class Recipe
{
    private List<Ingredient> ingredients;
    private List<RecipeStep> steps;
    public string Name { get; set; }

    // Event delegate for notifying when total calories exceed 300
    public delegate void CaloriesExceedHandler(Recipe recipe, double totalCalories);
    public event CaloriesExceedHandler CaloriesExceeded;

    // Constructor to initialize recipe
    public Recipe(string name)
    {
        Name = name;
        ingredients = new List<Ingredient>();
        steps = new List<RecipeStep>();
    }

    // Method to input details of the recipe
    public void InputRecipeDetails()
    {
        try
        {
            // Input ingredients
            Console.WriteLine($"Enter details for recipe '{Name}':");
            while (true)
            {
                Console.Write("Name (or 'done' to finish): ");
                string name = Console.ReadLine();
                if (name.ToLower() == "done")
                    break;

                Console.Write("Quantity: ");
                double quantity = double.Parse(Console.ReadLine());

                Console.Write("Unit: ");
                string unit = Console.ReadLine();

                Console.Write("Calories: ");
                double calories = double.Parse(Console.ReadLine());

                Console.Write("Food Group: ");
                string foodGroup = Console.ReadLine();

                ingredients.Add(new Ingredient(name, quantity, unit, calories, foodGroup));
            }

            // Input steps
            Console.WriteLine($"Enter steps for recipe '{Name}':");
            for (int i = 0; i < steps.Count; i++)
            {
                Console.Write($"Step {i + 1}: ");
                string description = Console.ReadLine();
                steps.Add(new RecipeStep(description));
            }
        }
        catch (FormatException)
        {
            Console.WriteLine("Invalid input format. Please enter a valid number.");
        }
    }

    // Method to calculate the total calories of the recipe
    public double CalculateTotalCalories()
    {
        double totalCalories = ingredients.Sum(i => i.Calories * i.Quantity);
        if (totalCalories > 300)
        {
            CaloriesExceeded?.Invoke(this, totalCalories);
        }
        return totalCalories;
    }

    // Method to display the recipe
    public void DisplayRecipe()
    {
        Console.WriteLine();
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"Recipe: {Name}");
        Console.ResetColor();

        Console.WriteLine("Ingredients:");
        foreach (var ingredient in ingredients)
        {
            Console.WriteLine($"{ingredient.Name} - {ingredient.Quantity} {ingredient.Unit} ({ingredient.FoodGroup})");
        }

        Console.WriteLine("Steps:");
        for (int i = 0; i < steps.Count; i++)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write($"{i + 1}. ");
            Console.ResetColor();
            Console.WriteLine(steps[i].Description);
        }
        Console.WriteLine();
    }
/*

public void ClearDataWithConfirmation()
{
    Console.WriteLine("Are you sure you want to clear all data? (yes/no)");
    string userInput = Console.ReadLine();

    if(userInput.ToLower() == "yes") {
        // Clear data
        recipe.ClearData();
        Console.WriteLine("Data cleared.");
    } else {
        Console.WriteLine("Operation cancelled.");
    }
}
*/
    // Method to clear all data and start with a new recipe
    public void ClearData()
    {
        ingredients.Clear();
        steps.Clear();
        Console.WriteLine("Data cleared successfully.");
    }
}

// Main class to run the program
public class Program
{
    public static void Main(string[] args)
{
    try
    {
        // Create a recipe
        Recipe recipe = CreateRecipe();

        // Subscribe to the event for notifying when total calories exceed 300
        recipe.CaloriesExceeded += Recipe_CaloriesExceeded;

        // Display recipe details
        recipe.DisplayRecipe();

        // Calculate and display total calories
        double totalCalories = recipe.CalculateTotalCalories();
        Console.WriteLine($"Total Calories: {totalCalories}");

        // Ask for user confirmation and clear data
        ClearDataWithConfirmation(recipe);

        // Unsubscribe from the event
        recipe.CaloriesExceeded -= Recipe_CaloriesExceeded;
    }
    catch (Exception ex)
    {
        Console.WriteLine($"An error occurred: {ex.Message}");
    }
}

    // Method to create a recipe by taking user input
    public static Recipe CreateRecipe()
    {
        Console.Write("Enter the recipe name: ");
        string name = Console.ReadLine();

        Recipe recipe = new Recipe(name);
        recipe.InputRecipeDetails();

        return recipe;
    }

    // Event handler for when total calories exceed 300
    private static void Recipe_CaloriesExceeded(Recipe recipe, double totalCalories)
    {
        Console.WriteLine($"Warning: Total calories of recipe '{recipe.Name}' exceed 300 ({totalCalories}).");
    }
}


// Output:
// Enter the number of ingredients: 2
// Enter the number of steps: 3
// Enter details for ingredient 1:
// Name: Apples
// Quantity: 2
// Unit: lbs
// Calories: 52
// Food Group: Fruit
// Enter description for step 1:
// Cut the apples
// Enter description for step 2:
// Peel the apples
// Enter description for step 3:
// Cut the apples in half

// Recipe: Apples
// Ingredients:
// Apples - 2 lbs (Fruit)
// Steps:
// 1. Cut the apples
// 2. Peel the apples
// 3. Cut the apples in half

// Enter the scale factor (e.g., 0.5 for half, 2 for double): 0.5
// Recipe: Apples
// Ingredients:
// Apples - 1.0 lbs (Fruit)
// Steps:
// 1. Cut the apples
// 2. Peel the apples
// 3. Cut the apples in half

// Enter the scale factor (e.g., 0.5 for half, 2 for double): 2
// Recipe: Apples
// Ingredients:
// Apples - 2.0 lbs (Fruit)
// Steps:
// 1. Cut the apples
// 2. Peel the apples
// 3. Cut the apples in half

// Quantities reset successfully.
// Recipe: Apples
// Ingredients:
// Apples - 2.0 lbs (Fruit)
// Steps:
// 1. Cut the apples
// 2. Peel the apples
// 3. Cut the apples in half

// Data cleared successfully.
// Press any key to continue . . .


