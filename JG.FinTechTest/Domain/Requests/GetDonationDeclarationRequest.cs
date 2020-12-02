﻿using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;

namespace JG.FinTechTest.Domain.Requests
{
    public class GetDonationDeclarationRequest
    {
        [BindRequired]
        [StringLength(24, MinimumLength = 24, ErrorMessage = "Declaration id is defined by 24 character long string")]
        public string Id { get; set; }
    }
}
