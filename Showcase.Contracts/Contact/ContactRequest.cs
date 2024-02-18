namespace Showcase.Contracts.Contact;

public record ContactRequest
(
    string FirstName,
    string LastName,
    string PhoneNumber,
    string Email,
    string Subject,
    string Message,
    string RecaptchaToken
);