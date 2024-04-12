using System;
using System.Collections.Generic;
using System.Xml.Linq;

// Class to represent an ingredient in the recipe
public class Ingredient
{
    public string Name { get; set; }
    public double Quantity { get; set; }
    public string Unit { get; set; }
    internal double OriginalQuantity { get; set; }

    // Constructor to initialize ingredient properties
    public Ingredient(string name, double quantity, string unit)
    {
        Name = name;
        Quantity = quantity;
        Unit = unit;
        OriginalQuantity = quantity;
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
    private object Name;

    // Constructor to initialize recipe with specified number of ingredients and steps
    public Recipe(int numIngredients, int numSteps)
    {
        ingredients = new List<Ingredient>(numIngredients);
        steps = new List<RecipeStep>(numSteps);
    }

    // Method to input details of the recipe
    public void InputRecipeDetails()
    {
 
        try
        {
            // Prompt user for number of ingredients and steps
            Console.Write("Enter the number of ingredients: ");
            int numIngredients = int.Parse(Console.ReadLine());

            Console.Write("Enter the number of steps: ");
            int numSteps = int.Parse(Console.ReadLine());

            // Input ingredients
            for (int i = 0; i < numIngredients; i++)
            {
                Console.WriteLine($"Enter details for ingredient {i + 1}:");
                Console.Write("Name: ");
                string name = Console.ReadLine();

                Console.Write("Quantity: ");
                double quantity = double.Parse(Console.ReadLine());

                Console.Write("Unit: ");
                string unit = Console.ReadLine();

                ingredients.Add(new Ingredient(name, quantity, unit));
            }

            // Input steps
            for (int i = 0; i < numSteps; i++)
            {
                Console.WriteLine($"Enter description for step {i + 1}:");
                string description = Console.ReadLine();
                steps.Add(new RecipeStep(description));
            }
        }
        catch (FormatException)
        {
            Console.WriteLine("Invalid input format. Please enter a valid number.");
        }
    }

    // Method to display the recipe
    public void DisplayRecipe()
    {
        Console.WriteLine();
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"Recipe: {Name}");
        Console.ResetColor();

        Console.WriteLine("Ingredients:");
        for (int i = 0; i < ingredients.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {ingredients[i].Name} - {ingredients[i].Quantity} {ingredients[i].Unit}");
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

   
    /// Scales the recipe by a specified factor.
    /// return the updated recipe with scaled ingredient quantities.
    public Recipe ScaleRecipe(Action<string> getUserInput)
    {
        getUserInput.Invoke("Enter the scale factor (e.g., half, double, triple, 0.5, 2): ");
        string scaleInput = Console.ReadLine();

        double factor = 1.0;

        switch (scaleInput.ToLower())
        {
            case "half":
                factor = 0.5;
                break;
            case "double":
                factor = 2.0;
                break;
            case "triple":
                factor = 3.0;
                break;
            default:
                if (!double.TryParse(scaleInput, out factor))
                {
                    Console.WriteLine("Invalid scale type. Recipe was not scaled.");
                    return this;
                }
                break;
        }

        foreach (Ingredient ingredient in ingredients)
        {
            ingredient.Quantity *= factor;
        }

        return this;
    }



    // Method to reset ingredient quantities to original values
    /// Reset ingredient quantities to their original values if the user scaled the recipe.
    public bool ResetQuantities()
    {
        Console.WriteLine("Would you like to reset the quantities? (Y/N)");
        string userInput = Console.ReadLine();

        if (userInput.ToLower() == "y")
        {
            List<Ingredient> originalQuantities = new List<Ingredient>(ingredients);
            foreach (Ingredient ingredient in ingredients)
            {
                ingredient.Quantity = ingredient.OriginalQuantity;
            }

            //output message
            Console.WriteLine("Quantities reset successfully.");

            // Print the reset recipe
            DisplayRecipe();

            return true;
        }
        else
        {
            Console.WriteLine("Reset quantities canceled.");
            return false;
        }
    }



    // Method to clear all data and start with a new recipe
    public void ClearData()
    {
        // Confirm with user before clearing data
        Console.Write("Are you sure you want to clear all data? (yes/no): ");
        string response = Console.ReadLine().ToLower();

        if (response == "yes")
        {
            ingredients.Clear();
            steps.Clear();
            Console.WriteLine("Data cleared successfully.");
        }
        else if (response != "no")
        {
            Console.WriteLine("Invalid response. Data was not cleared.");
        }
    }
}

// Main class to run the program
public class Program
{
    public static void Main(string[] args)
    {
        Recipe recipe = new Recipe(5, 10);
        recipe.InputRecipeDetails();
        recipe.DisplayRecipe();

        //prompt the user for input
        Action<string> getUserInput = (prompt) =>
        {
            Console.Write(prompt);
        };

        // Call ScaleRecipe method with a getUserInput function
        recipe.ScaleRecipe(getUserInput);
        recipe.DisplayRecipe();
        recipe.ResetQuantities();
        recipe.ClearData();
    }
}
