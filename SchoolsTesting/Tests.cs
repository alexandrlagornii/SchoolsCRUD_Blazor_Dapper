using Microsoft.CodeAnalysis.Elfie.Serialization;
using Microsoft.Playwright;

[Parallelizable(ParallelScope.Self)]
[TestFixture]
public class Tests
{
    // Main page
    const string mainPage = "http://localhost:5209/";
    const string addSchoolPage = "http://localhost:5209/school-add";

    // Admin login and password
    const string adminLogin = "admin@admin.com";
    const string adminPassword = "utOFlURzbYZPv9o@";

    public async Task LoadSchools(int start, int end)
    {
        // Playwright
        using var playwright = await Playwright.CreateAsync();

        // Browser
        await using var browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
        {
            //Headless = false
        });

        // Get main page
        var page = await browser.NewPageAsync();
        await page.GotoAsync(mainPage);

        // Get login page
        await page.GetByRole(AriaRole.Link, new() { Name = "Login" }).ClickAsync();

        // Log in
        await page.GetByPlaceholder("name@example.com").FillAsync(adminLogin);
        await page.GetByPlaceholder("password").FillAsync(adminPassword);

        await page.GetByRole(AriaRole.Button, new() { Name = "Log in" }).ClickAsync();


        // Get to add page
        for (int i = start; i <= end; i++)
        {
            // Check how much time it took to write into database

            await page.GotoAsync(addSchoolPage);

            // Fill values
            // Name
            await page.GetByLabel("Name").ClickAsync();
            await page.GetByLabel("Name").FillAsync($"School-{i}");

            // Address
            await page.GetByLabel("Address").ClickAsync();
            await page.GetByLabel("Address").FillAsync($"Address-{i}");

            // Country
            await page.GetByPlaceholder("Search").First.ClickAsync();
            await page.GetByPlaceholder("Search").First.FillAsync("Moldova");
            await page.GetByPlaceholder("Search").First.PressAsync("Enter");
            await page.GetByRole(AriaRole.Cell, new() { Name = "Moldova" }).ClickAsync();

            // City
            await page.GetByPlaceholder("Search").Nth(1).ClickAsync();
            await page.GetByPlaceholder("Search").Nth(1).FillAsync("Chisinau");
            await page.GetByPlaceholder("Search").Nth(1).PressAsync("Enter");
            await page.GetByRole(AriaRole.Cell, new() { Name = "Chisinau" }).ClickAsync();

            await page.GetByRole(AriaRole.Button, new() { Name = "Submit" }).ClickAsync();
        }
    }

    [Test]
    public async Task TestLoadSchools()
    {
        // Set up multiple clients
        var tasks = new List<Task>();
        int start = 1, end = 1000;
        for (int i = 1; i <= 25; i++)
        {
            tasks.Add(LoadSchools(start, end));
            start += 1000;
            end += 1000;
        }

        // Time Test
        string path = @"C:\Users\UserPC\Desktop\Test.txt";
        var watch = System.Diagnostics.Stopwatch.StartNew();

        await Task.WhenAll(tasks);

        watch.Stop();
        var elapsedMs = watch.Elapsed.Milliseconds;
        File.WriteAllText(path, elapsedMs.ToString());
    }
}