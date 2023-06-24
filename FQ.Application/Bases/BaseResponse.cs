using FluentValidation.Results;
using Microsoft.Azure.CognitiveServices.ContentModerator.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FQ.Application.Bases
{
    public class BaseResponse<T>
    {
        public bool isSucces { get; set; }

        public T? Data { get; set; }

        public string? Message { get; set; }
        public IEnumerable<ValidationFailure>? Errors { get; set; }
        public Evaluate? moderationImageResult {get;set;}
    }
}
