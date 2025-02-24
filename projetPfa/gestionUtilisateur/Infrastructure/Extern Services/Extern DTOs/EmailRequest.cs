namespace gestionUtilisateur.Infrastructure.Extern_Services.Extern_DTOs
{
    public class EmailRequest
    {
        public string ToEmail { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }

        public EmailRequest(string to, string password, string userName) 
        {
            ToEmail = to;
            Subject = "Password Recovery Request";
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
                    .recovery-key {{
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
                    <p class='header'>Password Recovery Request</p>
                    <p>Hello <b>{userName}</b>,</p>
                    <p>We received a request to reset your password. Use the recovery key below to proceed:</p>
                    <p class='recovery-key'>{password}</p>
                    <p>If you did not request this, you can ignore this email.</p>
                    <p>Best regards,<br><b>Dream Sports</b></p>
                </div>
            </body>
            </html>";


        }
    }
}
