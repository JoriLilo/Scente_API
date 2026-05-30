using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;

namespace Scente.API.Services;

// ============================================================
// EmailService — MailKit implementation of IEmailService
//
// Reads SMTP settings from configuration under the "Email"
// section. Locally that comes from appsettings.json; in
// production Jori sets them as environment variables, e.g.
//   Email__Host, Email__Port, Email__Username, Email__Password
// (ASP.NET maps the double underscore to the "Email:Key" path,
// so NOTHING secret is ever hardcoded here.)
// ============================================================
public class EmailService : IEmailService
{
    private readonly IConfiguration _config;
    private readonly ILogger<EmailService> _logger;

    public EmailService(IConfiguration config, ILogger<EmailService> logger)
    {
        _config = config;
        _logger = logger;
    }

    public async Task<bool> SendOrderConfirmationAsync(OrderEmailData data)
    {
        try
        {
            // ── Read settings (never hardcoded) ──────────────
            var host       = _config["Email:Host"];
            var portValue  = _config["Email:Port"];
            var username   = _config["Email:Username"];
            var password   = _config["Email:Password"];
            var fromEmail  = _config["Email:FromEmail"] ?? username;
            var fromName   = _config["Email:FromName"] ?? "Scenté";

            // If email isn't configured (e.g. a teammate hasn't set
            // it up locally), skip gracefully instead of crashing.
            if (string.IsNullOrWhiteSpace(host) ||
                string.IsNullOrWhiteSpace(username) ||
                string.IsNullOrWhiteSpace(password))
            {
                _logger.LogWarning(
                    "Email not configured — skipping confirmation for order {OrderNumber}.",
                    data.OrderNumber);
                return false;
            }

            var port = int.TryParse(portValue, out var p) ? p : 587;

            // ── Build the message ────────────────────────────
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(fromName, fromEmail));
            message.To.Add(new MailboxAddress(data.CustomerName, data.ToEmail));
            message.Subject = $"Your Scenté order {data.OrderNumber}";

            var builder = new BodyBuilder
            {
                HtmlBody = BuildHtmlBody(data),
                TextBody = BuildTextBody(data)
            };
            message.Body = builder.ToMessageBody();

            // ── Send ─────────────────────────────────────────
            using var client = new SmtpClient();
            await client.ConnectAsync(host, port, SecureSocketOptions.StartTls);
            await client.AuthenticateAsync(username, password);
            await client.SendAsync(message);
            await client.DisconnectAsync(true);

            _logger.LogInformation(
                "Confirmation email sent for order {OrderNumber}.",
                data.OrderNumber);
            return true;
        }
        catch (Exception ex)
        {
            // Never let an email failure bubble up — the order is
            // already saved. Just log and report failure.
            _logger.LogError(ex,
                "Failed to send confirmation email for order {OrderNumber}.",
                data.OrderNumber);
            return false;
        }
    }

    // ── HTML body (brand-styled, simple inline CSS) ──────────
    private static string BuildHtmlBody(OrderEmailData d)
    {
        var rows = string.Join("", d.Items.Select(i =>
            $@"<tr>
                 <td style='padding:8px 0;'>{i.ProductName} ({i.Size}) &times; {i.Quantity}</td>
                 <td style='padding:8px 0; text-align:right;'>${(i.Price * i.Quantity):F2}</td>
               </tr>"));

        var shippingText = d.ShippingCost == 0 ? "Free" : $"${d.ShippingCost:F2}";

        return $@"
        <div style='font-family:Arial,sans-serif; max-width:560px; margin:auto; color:#2d1b2e;'>
            <h1 style='color:#c4687e;'>Thank you for your order, {d.CustomerName}!</h1>
            <p>Your order <strong>{d.OrderNumber}</strong> has been received and is now being prepared.</p>
            <table style='width:100%; border-collapse:collapse; margin:24px 0;'>
                {rows}
                <tr><td style='padding-top:16px;'>Subtotal</td>
                    <td style='padding-top:16px; text-align:right;'>${d.Subtotal:F2}</td></tr>
                <tr><td>Shipping</td><td style='text-align:right;'>{shippingText}</td></tr>
                <tr><td style='font-weight:bold; border-top:1px solid #f0dde2; padding-top:8px;'>Total Paid</td>
                    <td style='font-weight:bold; border-top:1px solid #f0dde2; padding-top:8px; text-align:right;'>${d.TotalPaid:F2}</td></tr>
            </table>
            <p>Estimated delivery: <strong>{d.EstimatedDelivery}</strong></p>
            <p style='color:#888; font-size:12px;'>Scenté · This is an automated confirmation.</p>
        </div>";
    }

    // ── Plain-text fallback for email clients without HTML ───
    private static string BuildTextBody(OrderEmailData d)
    {
        var lines = string.Join("\n", d.Items.Select(i =>
            $"- {i.ProductName} ({i.Size}) x{i.Quantity}: ${(i.Price * i.Quantity):F2}"));

        var shippingText = d.ShippingCost == 0 ? "Free" : $"${d.ShippingCost:F2}";

        return
$@"Thank you for your order, {d.CustomerName}!

Order: {d.OrderNumber}

{lines}

Subtotal: ${d.Subtotal:F2}
Shipping: {shippingText}
Total Paid: ${d.TotalPaid:F2}

Estimated delivery: {d.EstimatedDelivery}

Scenté";
    }
}
