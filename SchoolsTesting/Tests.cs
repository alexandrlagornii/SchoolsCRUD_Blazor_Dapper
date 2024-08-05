using Microsoft.Playwright;

[Parallelizable(ParallelScope.Self)]
[TestFixture]
public class Tests
{
    // Main page
    const string mainPage = "https://localhost:7221/";
    const string addSchoolPage = "https://localhost:7221/school-add";

    // Admin login and password
    const string adminLogin = "admin@admin.com";
    const string adminPassword = "utOFlURzbYZPv9o@";

    [Test]
    public async Task TestLoadSchools()
    {
        // Playwright
        using var playwright = await Playwright.CreateAsync();

        // Browser
        await using var browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
        {
            Headless = false
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
        for (int i = 426; i <= 25000; i++)
        {
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

            // Click add
            await page.GetByRole(AriaRole.Button, new() { Name = "Submit" }).ClickAsync();
        }

    }
}