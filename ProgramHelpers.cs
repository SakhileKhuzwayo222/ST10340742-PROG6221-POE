using RecipeApp;

internal static class ProgramHelpers
{
    public static void Main(string[])
    {
        try
        {
            List<Recipe> recipes = InputRecipes();
            foreach (var recipe in recipes)
            {
                recipe.CaloriesExceeded += Recipe_CaloriesExceeded;
                recipe.ResetQuantities();
                recipe.DisplayRecipe();
                Console.WriteLine($"Total Calories: {recipe.CalculateTotalCalories()}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }
    }
}