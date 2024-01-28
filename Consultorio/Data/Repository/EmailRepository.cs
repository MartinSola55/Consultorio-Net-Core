using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using Consultorio.Data.Repository.IRepository;
using Consultorio.Models;
using Microsoft.EntityFrameworkCore;
using System.Net.Mail;
using System.Net;

namespace Consultorio.Data.Repository
{
    public class EmailRepository(IConfiguration config, IWebHostEnvironment env) : IEmailRepository
    {
        private readonly IConfiguration _config = config;
        private readonly IWebHostEnvironment _env = env;

        public async Task SendConfirmTurno(Turno turno)
        {
            SmtpClient smtp = new("smtp.gmail.com", 587);

            string email = _config["EmailSender"] ?? throw new Exception("No se pudo enviar el email");
            string password = _config["EmailSenderPassword"] ?? throw new Exception("No se pudo enviar el email");

            smtp.Credentials = new NetworkCredential(email, password);
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtp.EnableSsl = true;

            MailMessage mail = new()
            {
                From = new MailAddress("turnos@fernandosolatraumatologia.com.ar", "Dr. Fernando Sola"),
                Subject = "Tu turno ha sido confirmado",
                IsBodyHtml = true,
                To = { new MailAddress(turno.Persona.Correo ?? throw new Exception("No se pudo enviar el email")) }
            };

            string path = Path.Combine(_env.ContentRootPath, "Views", "Emails", "Confirmation.html");
            string body = File.ReadAllText(path);
            body = body.Replace("[NOMBRE]", turno.Persona.Apellido + ", " + turno.Persona.Nombre);
            body = body.Replace("[DIA]", turno.DiaHorario.Dia.ToShortDateString());
            body = body.Replace("[HORA]", turno.DiaHorario.Horario.Hora.ToShortTimeString());
            body = body.Replace("[ANIO]", DateTime.Now.Year.ToString());
            mail.Body = body;

            await smtp.SendMailAsync(mail);
            smtp.Dispose();
        }
    }
}