using FluentValidation;
using FQ.Application.Dtos.Posts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FQ.Application.Validators.Posts
{
    public class PostValidators : AbstractValidator<AddPostDto>
    {
        public PostValidators()
        {
            RuleFor(x => x.Titulo).NotNull().WithMessage("El campo titulo del post no puede ser nulo")
              .NotEmpty().WithMessage("El campo titulo carta no puede ser vacio")
              .MaximumLength(50).WithMessage("El titulo del post debe tener menos de 50 caracteres");

            RuleFor(x => x.Contenido).NotNull().WithMessage("El contenido de post no puede ser nulo")
             .NotEmpty().WithMessage("El contenido del post no puede ser vacio")
             .MaximumLength(200).WithMessage("El contenido del post debe tener menos de 200 caracteres");
        }
    }
}
