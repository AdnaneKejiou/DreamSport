using Microsoft.AspNetCore.Identity.Data;

namespace gestionEmployer.Infrastructure.ExternServices.ExternDTOs
{
    public class EmailRequest
    {
        public string ToEmail { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }

        public EmailRequest(string toEmail,string EmployeeName, string EmployeePassword)
        {
            string loginUrl = "url login";
            ToEmail = toEmail;
            Subject = "Employee validation";
            Body = $@"
            <!DOCTYPE html>
            <html>
            <head>
                <style>
                    .container {{
                        font-family: Arial, sans-serif;
                        line-height: 1.6;
                        color: #333;
                        max-width: 600px;
                        margin: auto;
                        padding: 20px;
                        border: 1px solid #ddd;
                        border-radius: 5px;
                        background-color: #f9f9f9;
                    }}
                    .header {{
                        font-size: 20px;
                        font-weight: bold;
                        color: #007bff;
                    }}
                    .account-details {{
                        font-size: 18px;
                        font-weight: bold;
                        color: #d9534f;
                        background-color: #f8d7da;
                        padding: 10px;
                        border-radius: 5px;
                        display: inline-block;
                    }}
                    .footer {{
                        font-size: 12px;
                        color: #777;
                        margin-top: 20px;
                    }}
                </style>
            </head>
            <body>
                <div class='container'>
                    <p class='header'>Welcome to DreamSports!</p>
                    <p>Hello <b>{EmployeeName}</b>,</p>
                    <p>We are excited to have you on board. Below are your account details:</p>
                    <p><b>👤 Username:</b> {toEmail}</p>
                    <p><b>🔑 Temporary Password:</b> <span class='account-details'>{EmployeePassword}</span></p>
                    <p>For security reasons, please change your password upon your first login.</p>
                    <p>You can access your account here: <a href='{loginUrl}'>Login to your account</a></p>
                    <p>If you have any questions, feel free to contact HR or IT support.</p>
                    <p>Best regards,<br><b>DreamSports Team</b></p>
                </div>
            </body>
            </html>";
        }
    }
}
