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
  public Recipe(string name) {
    Name = name;
    ingredients = new List<Ingredient>();
    steps = new List<RecipeStep>();
    CaloriesExceeded = delegate { };
}

    // Method to input details of the recipe
public static List<Recipe> InputRecipes() {

    List<Recipe> recipes = new List<Recipe>();

    while (true)
    {
        Console.WriteLine("Enter details for a new recipe (or 'done' to finish):");
        Console.Write("Recipe Name: ");
        string recipeName = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(recipeName))
        {
            Console.WriteLine("Recipe name cannot be empty.");
            continue;
        }

        Recipe recipe = new Recipe(recipeName);
        recipe.InputRecipeDetails(); // Method to input ingredients and steps
        recipes.Add(recipe);

        Console.WriteLine("Recipe added successfully.");

        Console.Write("Do you want to add another recipe? (yes/no): ");
        string addAnother = Console.ReadLine();
        if (addAnother.ToLower() != "yes")
            break;
    }

    return recipes.OrderBy(r => r.Name).ToList();
}

private void InputRecipeDetails()
{
    while (true)
    {
        Console.WriteLine("Enter details for an ingredient (or 'done' to finish):");
        Console.Write("Ingredient Name: ");
        string? ingredientName = Console.ReadLine();
        if (ingredientName?.ToLower() == "done")
            break;

        Console.Write("Ingredient Quantity: ");
        string? quantityString = Console.ReadLine();
        double quantity;
        if (!double.TryParse(quantityString, out quantity))
        {
            Console.WriteLine("Invalid quantity. Please enter a valid number.");
            continue;
        }

        Console.Write("Ingredient Unit: ");
        string? unit = Console.ReadLine();

        Console.Write("Ingredient Calories: ");
        string? caloriesString = Console.ReadLine();
        double calories;
        if (!double.TryParse(caloriesString, out calories))
        {
            Console.WriteLine("Invalid calories. Please enter a valid number.");
            continue;
        }

        Console.Write("Ingredient Food Group: ");
        string? foodGroup = Console.ReadLine();

        Ingredient ingredient = new Ingredient(ingredientName ?? "", quantity, unit ?? "", calories, foodGroup ?? "");
        ingredients.Add(ingredient);

        Console.WriteLine("Ingredient added successfully.");
    }

    while (true)
    {
        Console.WriteLine("Enter details for a step (or 'done' to finish):");
        Console.Write("Step Description: ");
        string? description = Console.ReadLine();
        if (description?.ToLower() == "done")
            break;

        RecipeStep step = new RecipeStep(description ?? "");
        steps.Add(step);

        Console.WriteLine("Step added successfully.");
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


public void ClearDataWithConfirmation()
{
    Console.WriteLine("Are you sure you want to clear all data? (yes/no)");
    string? userInput = Console.ReadLine();

    if(userInput?.ToLower() == "yes") {
        // Clear data
        ClearData();
        Console.WriteLine("Data cleared.");
    } else {
        Console.WriteLine("Operation cancelled.");
    }
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
        recipe.ClearDataWithConfirmation();

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


