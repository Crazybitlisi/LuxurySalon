using FluentValidation;
using LuxurySalon.Application.DTOs;
using System;

namespace LuxurySalon.Application.Validators
{
    public class CreateAppointmentValidator : AbstractValidator<CreateAppointmentRequest>
    {
        public CreateAppointmentValidator()
        {
            RuleFor(x => x.ServiceId).GreaterThan(0);
            RuleFor(x => x.StylistId).GreaterThan(0);
            RuleFor(x => x.StartTime)
                .NotEmpty()
                .Must(start => start > DateTime.UtcNow)
                .WithMessage("Appointment must be in the future.");
        }
    }
}
