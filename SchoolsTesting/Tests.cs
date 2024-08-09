using Microsoft.CodeAnalysis.Elfie.Serialization;
using Microsoft.Playwright;
using System.Diagnostics;
using System.IO;

[Parallelizable(ParallelScope.Self)]
[TestFixture]
public class Tests
{
    // Main page
    const string mainPage = "http://localhost:5209/";
    const string schoolsPage = "http://localhost:5209/Schools/Schools";
    const string addSchoolPage = "http://localhost:5209/Schools/SchoolAdd";

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

        // path for log
        string path = @"C:\Users\UserPC\Desktop\Test.txt";

        // Get to add page
        for (int i = start; i <= end; i++)
        {
            // Get to page that adds schools
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

            // Time submit request
            var watch = Stopwatch.StartNew();

            await page.GetByRole(AriaRole.Button, new() { Name = "Submit" }).ClickAsync();

            // Get to page that adds schools
            await page.GotoAsync(addSchoolPage);
            
            // Log time
            watch.Stop();
            var elapsedMs = watch.Elapsed.TotalMilliseconds;
            using (StreamWriter sw = File.AppendText(path))
                sw.WriteLine(elapsedMs.ToString());
        }
    }

    public async Task ReadSchools(int times)
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

        // path for log
        string path = @"C:\Users\UserPC\Desktop\Test.txt";
        
        for (int i = 1; i <= times; i++)
        {
            // Got to page
            await page.GotoAsync(schoolsPage);

            // Time read request
            var watch = new Stopwatch();
            watch.Start();

            // Wait till you see value School-2 (Which will be seen when all rows are read)
            await page.GetByRole(AriaRole.Cell, new() { Name = "School-2" }).ClickAsync();

            // Log time
            watch.Stop();
            var elapsedMs = watch.Elapsed.TotalMilliseconds;
            using (StreamWriter sw = File.AppendText(path))
                sw.WriteLine(elapsedMs.ToString());
        }
    }

    [Test]
    public async Task TestLoadSchools()
    {
        await LoadSchools(1, 10);
    }
}