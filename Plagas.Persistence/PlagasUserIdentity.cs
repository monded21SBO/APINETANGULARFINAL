﻿using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace Plagas.Persistence
{
    public class PlagasUserIdentity : IdentityUser
    {
        [StringLength(100)]
        public string FirstName { get; set; } = default!;

        [StringLength(100)]
        public string LastName { get; set; } = default!;
        public int Age { get; set; }

        public DocumentTypeEnum DocumentType { get; set; }

        [StringLength(20)]
        public string DocumentNumber { get; set; } = default!;

    }

    public enum DocumentTypeEnum : short
    {
        Dni,
        Passport
    }



}
