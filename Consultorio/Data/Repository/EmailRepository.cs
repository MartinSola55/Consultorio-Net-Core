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
    public class EmailRepository(ApplicationDbContext db, IConfiguration config, IWebHostEnvironment env) : IEmailRepository
    {
        private readonly ApplicationDbContext _db = db;
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

            var file = _env.ContentRootFileProvider.GetDirectoryContents("Views/Emails").First(x => x.Name == "Confirmation.html");

            string body = File.ReadAllText(file.PhysicalPath ?? throw new Exception("No se pudo enviar el email"));
            body = body.Replace("[NOMBRE]", turno.Persona.Apellido + ", " + turno.Persona.Nombre);
            body = body.Replace("[DIA]", turno.DiaHorario.Dia.ToString("dd/MM/yyyy"));
            body = body.Replace("[HORA]", turno.DiaHorario.Horario.Hora.ToString("HH:mm"));
            body = body.Replace("[ANIO]", DateTime.UtcNow.AddHours(-3).Year.ToString());
            mail.Body = body;

            await smtp.SendMailAsync(mail);
            smtp.Dispose();
        }

        public async Task SendModifyTurno(long turnoID, long oldHorarioID)
        {
            var turno = await _db.Turno
                .Include(x => x.Persona)
                .Include(x => x.DiaHorario)
                    .ThenInclude(x => x.Horario)
                .FirstAsync(x => x.ID == turnoID);
            var oldHorario = await _db.DiaHorario
                .Include(x => x.Horario)
                .FirstAsync(x => x.ID == oldHorarioID);

            SmtpClient smtp = new("smtp.gmail.com", 587);

            string email = _config["EmailSender"] ?? throw new Exception("No se pudo enviar el email");
            string password = _config["EmailSenderPassword"] ?? throw new Exception("No se pudo enviar el email");

            smtp.Credentials = new NetworkCredential(email, password);
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtp.EnableSsl = true;

            MailMessage mail = new()
            {
                From = new MailAddress("turnos@fernandosolatraumatologia.com.ar", "Dr. Fernando Sola"),
                Subject = "Tu turno ha sido modificado",
                IsBodyHtml = true,
                To = { new MailAddress(turno.Persona.Correo ?? throw new Exception("No se pudo enviar el email")) }
            };

            var file = _env.ContentRootFileProvider.GetDirectoryContents("Views/Emails").First(x => x.Name == "Modification.html");

            string body = File.ReadAllText(file.PhysicalPath ?? throw new Exception("No se pudo enviar el email"));
            body = body.Replace("[NOMBRE]", turno.Persona.Apellido + ", " + turno.Persona.Nombre);
            body = body.Replace("[DIA_ANTERIOR]", oldHorario.Dia.ToString("dd/MM/yyyy"));
            body = body.Replace("[HORA_ANTERIOR]", oldHorario.Horario.Hora.ToString("HH:mm"));
            body = body.Replace("[DIA_NUEVO]", turno.DiaHorario.Dia.ToString("dd/MM/yyyy"));
            body = body.Replace("[HORA_NUEVO]", turno.DiaHorario.Horario.Hora.ToString("HH:mm"));
            body = body.Replace("[ANIO]", DateTime.UtcNow.AddHours(-3).Year.ToString());
            mail.Body = body;

            await smtp.SendMailAsync(mail);
            smtp.Dispose();
        }
    }
}