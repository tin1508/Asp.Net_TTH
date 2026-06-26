using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace StationeryStore.Mvc.HealthChecks;

public static class HealthCheckResponse
{
    private const string Css = """
        body { font-family: sans-serif; padding: 2rem; background: #f8fafc; }
        h1 { font-size: 1.8rem; font-weight: 700; margin-bottom: 1.5rem; }
        table { border-collapse: collapse; width: 100%; background: white; border-radius: 8px; overflow: hidden; box-shadow: 0 1px 4px rgba(0,0,0,0.08); }
        th { background: #1e293b; color: white; padding: 12px 16px; text-align: left; }
        td { padding: 12px 16px; border-bottom: 1px solid #e2e8f0; }
        .pill { padding: 4px 12px; border-radius: 999px; font-size: 0.85rem; font-weight: 600; }
        .healthy { background: #22c55e; color: white; }
        .unhealthy { background: #ef4444; color: white; }
    """;

    public static async Task WriteHtmlResponse(HttpContext context, HealthReport report)
    {
        context.Response.ContentType = "text/html; charset=utf-8";

        var rows = string.Join("\n", report.Entries.Select(e => $"""
            <tr>
                <td>{e.Key}</td>
                <td><span class="pill {(e.Value.Status == HealthStatus.Healthy ? "healthy" : "unhealthy")}">{e.Value.Status}</span></td>
                <td>{e.Value.Description}</td>
            </tr>
        """));

        var overallClass = report.Status == HealthStatus.Healthy ? "healthy" : "unhealthy";
        var overallDesc = report.Status == HealthStatus.Healthy ? "All checks are healthy." : "Some checks are unhealthy.";

        var html = $"""
            <!DOCTYPE html>
            <html>
            <head>
                <title>Health Check</title>
                <style>{Css}</style>
            </head>
            <body>
                <h1>Health Check - /health/ready</h1>
                <table>
                    <thead>
                        <tr><th>Check</th><th>Status</th><th>Description</th></tr>
                    </thead>
                    <tbody>
                        {rows}
                        <tr>
                            <td><strong>Overall Status</strong></td>
                            <td><span class="pill {overallClass}">{report.Status}</span></td>
                            <td>{overallDesc}</td>
                        </tr>
                    </tbody>
                </table>
            </body>
            </html>
        """;

        await context.Response.WriteAsync(html);
    }
}